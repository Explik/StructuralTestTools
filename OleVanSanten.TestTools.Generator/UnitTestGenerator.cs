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
                AssemblyName name = new(args.Name);
                Assembly loadedAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().FullName == name.FullName);
                if (loadedAssembly != null)
                {
                    return loadedAssembly;
                }

                string resourceName = $"TestTools.Generator.{name.Name}.dll";

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
            var syntaxResolver = new CompileTimeDescriptionResolver(context.Compilation);
            var structureService = ConfigureStructureService(context.Compilation, configuration);
            var typeRewriter = ConfigureTypeRewriter(syntaxResolver, structureService, configuration);
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

        private IStructureService ConfigureStructureService(Compilation compilation, XMLConfiguration config)
        {
            var globalNamespace = new CompileTimeNamespaceDescription(compilation, compilation.GlobalNamespace);
            var fromNamespace = config.GetFromNamespace(globalNamespace);
            var toNamespace = config.GetToNamespace(globalNamespace);

            var typeTranslator = config.GetTypeTranslator();
            var memberTranslator = config.GetMemberTranslator();

            var structureService = new StructureService(fromNamespace, toNamespace)
            {
                StructureVerifier = new VerifierService()
            };

            if (typeTranslator != null)
                structureService.TypeTranslator = typeTranslator;

            if (memberTranslator != null)
                structureService.MemberTranslator = memberTranslator;

            return structureService;
        }

        private TypeRewriter ConfigureTypeRewriter(ICompileTimeDescriptionResolver syntaxResolver, IStructureService structureService,  XMLConfiguration config)
        {
            var typeVerifiers = config.GetTypeVerifiers();
            var memberVerifiers = config.GetMemberVerifiers();

            var typeRewritter = new TypeRewriter(syntaxResolver, structureService);

            if (typeVerifiers != null)
                typeRewritter.TypeVerifiers = typeVerifiers;

            if (memberVerifiers != null)
                typeRewritter.MemberVerifiers = memberVerifiers;

            return typeRewritter;
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