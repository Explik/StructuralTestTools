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
using OleVanSanten.TestTools.Structure;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools
{
    public static class UnitTestTemplator
    {
        public static void TemplateUnitTests(string solutionPath, string projectName, string configPath)
        {
            var instance = MSBuildLocator.RegisterDefaults();
            RunTemplateUnitTests(solutionPath, projectName, configPath);
        }

        private static void RunTemplateUnitTests(string solutionPath, string projectName, string configPath)
        {
            var solutionFile = new FileInfo(solutionPath);
            var solutionDirectory = solutionFile.Directory;

            var solution = GetSolution(solutionPath);
            var project = GetProject(solution, projectName);
            var compilation = CompileProject(project);

            var configuration = GetConfiguration(configPath);
            configuration.GlobalNamespace = new CompileTimeNamespaceDescription(compilation, compilation.GlobalNamespace);
            
            var syntaxResolver = new CompileTimeDescriptionResolver(compilation);
            var structureService = new StructureService(configuration) { StructureVerifier = new VerifierService() };
            var typeRewriter = new TypeRewriter(syntaxResolver, structureService);
            var templateRewriter = new TemplateRewriter(syntaxResolver, typeRewriter);
            var nodes = compilation.SyntaxTrees.SelectMany(t => RetreiveClassDeclarations(t));
            foreach (var node in nodes)
            {
                if (!HasTemplatedAttribute(node))
                    continue;

                var fileName = $"{node.Identifier}.g.cs";
                var rewrittenNode = templateRewriter.Visit(node.SyntaxTree.GetRoot());
                var source = SourceText.From(rewrittenNode.NormalizeWhitespace().ToFullString(), Encoding.UTF8);
                File.WriteAllText(solutionDirectory + "\\" + fileName, source.ToString());
            }
        }

        private static Solution GetSolution(string solutionPath)
        {
            var workspace = MSBuildWorkspace.Create();
            workspace.WorkspaceFailed += (sender, args) => Console.WriteLine(args.Diagnostic.Message);
            
            return workspace.OpenSolutionAsync(solutionPath).Result;
        }

        private static Project GetProject(Solution solution, string projectName)
        {
            var projectGraph = solution.GetProjectDependencyGraph();
            var projectIds = projectGraph.GetTopologicallySortedProjects();
            var projects = projectIds.Select(projectId => solution.GetProject(projectId));
            var project = projects.SingleOrDefault(p => p.Name == projectName);

            if (project == null)
                throw new ArgumentException("Solution does not contain project with name: " + projectName);

            return project;
        }

        private static Compilation CompileProject(Project project)
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
            return project.GetCompilationAsync().Result;
        }

        private static XMLConfiguration GetConfiguration(string configPath)
        {
            var content = File.ReadAllText(configPath);
            return new XMLConfiguration(content);
        }

        private static IEnumerable<ClassDeclarationSyntax> RetreiveClassDeclarations(SyntaxTree syntaxTree)
        {
            var root = syntaxTree.GetRoot();
            var visitor = new ClassVirtualizationVisitor();
            visitor.Visit(root);
            return visitor.Classes;
        }

        private static bool HasTemplatedAttribute(ClassDeclarationSyntax node)
        {
            var attributes = node.AttributeLists.SelectMany(l => l.Attributes);
            return attributes.Any(a => a.Name.ToString().Contains("Templated"));
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
