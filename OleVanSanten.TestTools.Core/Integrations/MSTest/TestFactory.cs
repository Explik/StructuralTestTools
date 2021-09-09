﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OleVanSanten.TestTools;
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

        public StructureTest CreateStructureTest()
        {
            VerifierService verifierService = new VerifierService()
            {
                ExceptionType = RuntimeTypeDescription.Create("Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException").Type
            };
            return new StructureTest(StructureService, verifierService);
        }

        public static TestFactory CreateFromConfigurationFile(string pathToConfigFile)
        {
            string configFileContent = File.ReadAllText(pathToConfigFile);
            var configuration = new XMLConfiguration(configFileContent)
            {
                GlobalNamespace = new RuntimeNamespaceDescription("")
            };
            VerifierService verifierService = new VerifierService()
            {
                ExceptionType = RuntimeTypeDescription.Create("Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException").Type
            };
            var structureService = new StructureService(configuration) { StructureVerifier = verifierService };
            var typeVisitor = new TypeVisitor(structureService);

            return new TestFactory()
            {
                StructureService = structureService,
                TypeVisitor = typeVisitor
            };
        }
    }
}