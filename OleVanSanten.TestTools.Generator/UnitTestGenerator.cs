using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using OleVanSanten.TestTools.Structure;
using OleVanSanten.TestTools.TypeSystem;
using System.Threading.Tasks;

namespace OleVanSanten.TestTools
{
    [Generator]
    public class UnitTestGenerator : ISourceGenerator
    {
        private XMLConfiguration _configuration;
        private CompileTimeDescriptionResolver _syntaxResolver;
        private StructureService _structureService;
        private TypeRewriter _typeRewriter;
        private TemplateRewriter _templateRewriter;

        static UnitTestGenerator()
        {
            // Based on https://stackoverflow.com/questions/67071355/source-generators-dependencies-not-loaded-in-visual-studio
            AppDomain.CurrentDomain.AssemblyResolve += (_, args) =>
            {
                AssemblyName name = new(args.Name);
                Assembly loadedAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().FullName == name.FullName);
                if (loadedAssembly != null)
                {
                    return loadedAssembly;
                }

                string resourceName = $"{name.Name}.dll";
                using Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
                if (resourceStream == null)
                {
                    return null;
                }

                using MemoryStream memoryStream = new MemoryStream();
                resourceStream.CopyTo(memoryStream);

                return Assembly.Load(memoryStream.ToArray());
            };
        }

        public void Initialize(GeneratorInitializationContext context)
        {
//#if DEBUG
//             Debugger.Launch();
//#endif
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            SyntaxReceiver syntaxReceiver = context.SyntaxReceiver as SyntaxReceiver;

            if (syntaxReceiver == null)
                return;

            _configuration ??= GetConfiguration(context);
            _syntaxResolver ??= new CompileTimeDescriptionResolver(context.Compilation);
            _structureService ??= new StructureService(_configuration) { StructureVerifier = new VerifierService() };
            _typeRewriter ??= new TypeRewriter(_syntaxResolver, _structureService);
            _templateRewriter ??= new TemplateRewriter(_syntaxResolver, _typeRewriter);

            bool IsTemplated(ClassDeclarationSyntax n) => _syntaxResolver.GetTemplatedAttribute(n) == null;
            Parallel.ForEach(syntaxReceiver.CandidateSyntax.Where(IsTemplated), node => {
                var fileName = $"{node.Identifier}.g.cs";
                var rewrittenNode = _templateRewriter.Visit(node.SyntaxTree.GetRoot(context.CancellationToken));
                var source = SourceText.From(rewrittenNode.NormalizeWhitespace().ToFullString(), Encoding.UTF8);
                context.AddSource(fileName, source);
            });
        }

        private XMLConfiguration GetConfiguration(GeneratorExecutionContext context)
        {
            var xmlFiles = context.AdditionalFiles.Where(at => at.Path.EndsWith(".xml"));
            var rawConfig = xmlFiles.FirstOrDefault()?.GetText()?.ToString() ?? throw new ArgumentException("Configuration file is missing");
            var config = new XMLConfiguration(rawConfig);
            config.GlobalNamespace = new CompileTimeNamespaceDescription(context.Compilation, context.Compilation.GlobalNamespace);
            
            return config;
            //foreach (var xmlFile in xmlFiles)
            //{
            //    if (context.AnalyzerConfigOptions.GetOptions(xmlFile).TryGetValue("build_metadata.AdditionalFiles.UnitTestGenerator_IsConfig", out var isConfig))
            //    {
            //        if(isConfig.Equals("true", StringComparison.OrdinalIgnoreCase))
            //            return xmlFile.GetText()?.ToString();
            //    }
            //}
            //return null;
        }

        private class SyntaxReceiver : ISyntaxReceiver
        {
            private readonly List<ClassDeclarationSyntax> _candidateSyntax = new List<ClassDeclarationSyntax>();

            public IEnumerable<ClassDeclarationSyntax> CandidateSyntax => _candidateSyntax;

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
                    _candidateSyntax.Add(classDeclarationSyntax);
            }
        }
    }
}