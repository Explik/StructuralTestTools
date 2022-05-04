using Explik.StructuralTestTools.TypeSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explik.StructuralTestTools.MSBuild
{
    public class TemplateService: ITemplateService
    {
        private readonly IFileService _fileService;
        private readonly ILogService _logService;

        public TemplateService(IFileService fileService): this(fileService, null)
        {
        }

        public TemplateService(IFileService fileService, ILogService logService)
        {
            _fileService = fileService;
            _logService = logService;
        }

        public void TemplateUnitTests(Compilation compilation, IConfiguration configuration)
        {
            var syntaxResolver = new CompileTimeDescriptionResolver(compilation);
            var structureService = new StructureService(configuration) { StructureVerifier = new VerifierService() };
            var typeRewriter = new TypeRewriter(syntaxResolver, structureService);
            var templateRewriter = new TemplateRewriter(syntaxResolver, typeRewriter, configuration.FromNamespace);

            foreach (var syntaxTree in compilation.SyntaxTrees)
            {
                if (!HasTemplatedAttribute(syntaxTree, syntaxResolver))
                    continue;

                var originalFileName = new FileInfo(syntaxTree.FilePath).Name;
                var newFileName = originalFileName.Replace("_Template.cs", ".g.cs");

                var rewrittenRoot = templateRewriter.Visit(syntaxTree.GetRoot());
                var fileContent = rewrittenRoot.ToString();

                _fileService.CreateGeneratedFile(newFileName, fileContent);
            }
        }

        private bool HasTemplatedAttribute(SyntaxTree syntaxTree, ICompileTimeDescriptionResolver resolver)
        {
            var root = syntaxTree.GetRoot();
            var visitor = new ClassVirtualizationVisitor();
            visitor.Visit(root);

            return visitor.Classes.Any(resolver.HasTemplatedAttribute);
        }

        class ClassVirtualizationVisitor : CSharpSyntaxRewriter
        {
            public List<ClassDeclarationSyntax> Classes = new List<ClassDeclarationSyntax>();

            public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
            {
                Classes.Add(node);
                return (ClassDeclarationSyntax)base.VisitClassDeclaration(node);
            }
        }
    }
}
