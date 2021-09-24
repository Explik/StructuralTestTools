using Microsoft.VisualStudio.TestTools.UnitTesting;
using Explik.StructuralTestTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestTools_Tests
{
    [TestClass]
    public class PracticalTestTests
    {
        [TestMethod]
        public void UnitTestTemplator_Debug()
        {
            string msbuildPath = "3.1.412";
            string solutionPath = "..\\..\\..\\..\\Explik.StructuralTestTools.PracticalTests/Explik.StructuralTestTools.PracticalTests.sln";
            string configPath = "..\\..\\..\\..\\Explik.StructuralTestTools.PracticalTests/TestToolsConfig.xml";
            Program.TemplateUnitTests(solutionPath, "Explik.StructuralTestTools.PracticalTests", msbuildPath, configPath);
        }
    }
}
