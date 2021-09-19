using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using Explik.StructuralTestTools;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 3)
                throw new ArgumentException();

            TemplateUnitTests(args[0], args[1], args[2]);
        }

        public static void TemplateUnitTests(string solutionPath, string projectName, string configPath)
        {
            try
            {
                MSBuildLocator.RegisterDefaults();
            }
            catch(Exception ex)
            {
                Log(ex.Message);
            }
            RunTemplateUnitTests(solutionPath, projectName, configPath);
        }
        
        private static void RunTemplateUnitTests(string solutionPath, string projectName, string configPath)
        {
            var configFile = new FileInfo(configPath);
            var configDirectory = configFile.Directory;

            using (var workspace = MSBuildWorkspace.Create())
            {
                workspace.WorkspaceFailed += (sender, args) => Log(args.Diagnostic.Message);
                var solution = workspace.OpenSolutionAsync(solutionPath).Result;
                var compilation = CompileProject(solution, projectName);

                var configContent = File.ReadAllText(configPath);
                var globalNamespace = new CompileTimeNamespaceDescription(compilation, compilation.GlobalNamespace);
                var configuration = Configuration.CreateFromXMLWithDefaults(globalNamespace, configContent);

                var syntaxResolver = new CompileTimeDescriptionResolver(compilation);
                var structureService = new StructureService(configuration) { StructureVerifier = new VerifierService() };
                var typeRewriter = new TypeRewriter(syntaxResolver, structureService);
                var templateRewriter = new TemplateRewriter(syntaxResolver, typeRewriter);
                var nodes = compilation.SyntaxTrees.SelectMany(t => RetreiveClassDeclarations(t));
                foreach (var node in nodes)
                {
                    if (!syntaxResolver.HasTemplatedAttribute(node))
                        continue;

                    var rewrittenNode = templateRewriter.Visit(node.SyntaxTree.GetRoot());
                    var rewrittenClassName = RetreiveClassDeclarations(rewrittenNode.SyntaxTree).First().Identifier;
                    var rewrittenSource = SourceText.From(rewrittenNode.NormalizeWhitespace().ToFullString(), Encoding.UTF8);

                    var fileName = $"{rewrittenClassName}.g.cs";
                    var filePath = configDirectory + "\\" + fileName; 
                    if(File.Exists(filePath)) File.SetAttributes(filePath, FileAttributes.Normal);
                    File.WriteAllText(filePath, rewrittenSource.ToString());
                    File.SetAttributes(filePath, FileAttributes.ReadOnly);
                }
            }
        }

        private static Compilation CompileProject(Solution solution, string projectName)
        {
            Compilation compilation = null;

            var projectTree = solution.GetProjectDependencyGraph();
            foreach(var projectId in projectTree.GetTopologicallySortedProjects())
            {
                var project = solution.GetProject(projectId);
                var projectCompilation = CompileProject(project, project.Name == projectName);
                if (project.Name == projectName) compilation = projectCompilation;
            }

            if (compilation == null)
                throw new ArgumentException("Solution does not contain project with name: " + projectName);

            return compilation;
        }

        private static Compilation CompileProject(Project project, bool skipGeneratedFiles)
        {
            // Some project formats do not include files to compile, so the .cs files contained in the project
            // directory are included by default.
            if (project.DocumentIds.Count == 0)
            {
                var projectFile = new FileInfo(project.FilePath);
                var csFiles = projectFile.Directory.GetFiles("*.cs", SearchOption.AllDirectories);
                
                foreach(var csFile in csFiles)
                {
                    project = project.AddDocument(csFile.FullName, File.ReadAllText(csFile.FullName)).Project;
                }
            }

            // Removing generated files from earlier run as these might cause the compilation to fail.
            if (skipGeneratedFiles)
            {
                var generatedDocuments = project.Documents.Where(d => d.Name.EndsWith(".g.cs"));

                foreach (var document in generatedDocuments)
                {
                    project = project.RemoveDocument(document.Id);
                }
            }

            // Compile project
            var compilation = project.GetCompilationAsync().Result;
            var errors = compilation.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error);

            if(errors.Any())
            {
                Log("COMPILATION ERROR: {0}: {1} compilation errors:");
                foreach (var error in errors)
                    Log(error.ToString());
            }
            return compilation;
        }

        private static IEnumerable<ClassDeclarationSyntax> RetreiveClassDeclarations(SyntaxTree syntaxTree)
        {
            var root = syntaxTree.GetRoot();
            var visitor = new ClassVirtualizationVisitor();
            visitor.Visit(root);
            return visitor.Classes;
        }

        // Only logs lines in debug mode, so it does not show up when using the Explik.StructuralTestTools.MSBuild package. 
        private static void Log(string message)
        {
#if DEBUG
            Console.WriteLine(message);
#endif
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
