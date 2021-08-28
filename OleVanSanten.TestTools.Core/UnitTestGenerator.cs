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

namespace OleVanSanten.TestTools
{
    [Generator]
    public class UnitTestGenerator : ISourceGenerator
    {
        static UnitTestGenerator()
        {
            // Based on https://stackoverflow.com/questions/67071355/source-generators-dependencies-not-loaded-in-visual-studio
            AppDomain.CurrentDomain.AssemblyResolve += (_, args) =>
            {
                AssemblyName name = new AssemblyName(args.Name);
                Assembly loadedAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().FullName == name.FullName);
                if (loadedAssembly != null)
                {
                    return loadedAssembly;
                }

                string resourceName = $"TestTools.Generator.{name.Name}.dll";

                using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)) {

                    if (resourceStream == null)
                    {
                        return null;
                    }

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        resourceStream.CopyTo(memoryStream);

                        return Assembly.Load(memoryStream.ToArray());
                    }
                }
            };
        }

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
             Debugger.Launch();
#endif
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            SyntaxReceiver syntaxReceiver = context.SyntaxReceiver as SyntaxReceiver;

            if (syntaxReceiver == null)
                return;

            var configuration = GetConfiguration(context);
            configuration.GlobalNamespace = new CompileTimeNamespaceDescription(context.Compilation, context.Compilation.GlobalNamespace);

            var syntaxResolver = new CompileTimeDescriptionResolver(context.Compilation);
            var structureService = new StructureService(configuration) { StructureVerifier = new VerifierService() };
            var typeRewriter = new TypeRewriter(syntaxResolver, structureService);
            var templateRewriter = new TemplateRewriter(syntaxResolver, typeRewriter);

            foreach (var node in syntaxReceiver.CandidateSyntax)
            {
                if (syntaxResolver.GetTemplatedAttribute(node) == null)
                    continue;

                var fileName = $"{node.Identifier}.g.cs";
                var rewrittenNode = templateRewriter.Visit(node.SyntaxTree.GetRoot(context.CancellationToken));
                var source = SourceText.From(rewrittenNode.NormalizeWhitespace().ToFullString(), Encoding.UTF8);
                context.AddSource(fileName, source);
            }
        }

        private XMLConfiguration GetConfiguration(GeneratorExecutionContext context)
        {
            var xmlFiles = context.AdditionalFiles.Where(at => at.Path.EndsWith(".xml"));
            var rawConfig = xmlFiles.FirstOrDefault()?.GetText()?.ToString() ?? throw new ArgumentException("Configuration file is missing");
            return new XMLConfiguration(rawConfig);
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