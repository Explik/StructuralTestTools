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

        public UnitTest CreateTest()
        {
            return new UnitTest(StructureService) { TypeVisitor = TypeVisitor };
        }

        public StructureTest CreateStructureTest()
        {
            return new StructureTest(StructureService, new VerifierService());
        }

        public static TestFactory CreateFromConfigurationFile(string pathToConfigFile)
        {
            string configFileContent = File.ReadAllText(pathToConfigFile);
            var configuration = new XMLConfiguration(configFileContent);
            configuration.GlobalNamespace = new RuntimeNamespaceDescription("");
            var structureService = new StructureService(configuration) { StructureVerifier = new VerifierService() };
            var typeVisitor = new TypeVisitor(structureService);

            return new TestFactory()
            {
                StructureService = structureService,
                TypeVisitor = typeVisitor
            };
        }
    }
}
