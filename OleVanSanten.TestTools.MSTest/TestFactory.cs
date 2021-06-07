using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OleVanSanten.TestTools.Structure;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.MSTest
{
    public class TestFactory
    {
        public TypeVisitor TypeVisitor { get; set; }

        public IStructureService StructureService { get; set; }

        private TestFactory()
        {
        }

        public TestFactory(string fromNamespace, string toNamespace)
            : this(new RuntimeNamespaceDescription(fromNamespace), new RuntimeNamespaceDescription(toNamespace))
        {
        }

        public TestFactory(NamespaceDescription fromNamespace, NamespaceDescription toNamespace)
        {
            StructureService = new StructureService(fromNamespace, toNamespace)
            {
                StructureVerifier = new VerifierService()
            };

            TypeVisitor = new TypeVisitor(StructureService);
        }

        public UnitTest CreateTest()
        {
            return new UnitTest(StructureService) { TypeVisitor = TypeVisitor };
        }

        public StructureTest CreateStructureTest()
        {
            return new StructureTest(StructureService);
        }

        public static TestFactory CreateFromConfigurationFile(string pathToConfigFile)
        {
            string configFileContent = File.ReadAllText(pathToConfigFile);
            var configuration = new XMLConfiguration(configFileContent);

            var structureService = ConfigureStructureService(configuration);
            var typeVisitor = ConfigureTypeRewriter(structureService, configuration);

            return new TestFactory()
            {
                StructureService = structureService,
                TypeVisitor = typeVisitor
            };
        }

        private static IStructureService ConfigureStructureService(XMLConfiguration config)
        {
            var fromNamespace = new RuntimeNamespaceDescription(config.GetFromNamespaceName());
            var toNamespace = new RuntimeNamespaceDescription(config.GetToNamespaceName());

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

        private static TypeVisitor ConfigureTypeRewriter(IStructureService structureService, XMLConfiguration config)
        {
            var typeVerifiers = config.GetTypeVerifiers();
            var memberVerifiers = config.GetMemberVerifiers();

            var typeVisitor = new TypeVisitor(structureService);

            if (typeVerifiers != null)
                typeVisitor.TypeVerifiers = typeVerifiers;

            if (memberVerifiers != null)
                typeVisitor.MemberVerifiers = memberVerifiers;

            return typeVisitor;
        }
    }
}
