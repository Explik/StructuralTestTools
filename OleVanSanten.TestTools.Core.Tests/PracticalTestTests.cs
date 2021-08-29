using Microsoft.VisualStudio.TestTools.UnitTesting;
using OleVanSanten.TestTools;
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
            string solutionPath = "..\\..\\..\\..\\OleVanSanten.TestTools.PracticalTests/OleVanSanten.TestTools.PracticalTests.sln";
            string configPath = "..\\..\\..\\..\\OleVanSanten.TestTools.PracticalTests/TestToolsConfig.xml";
            UnitTestTemplator.TemplateUnitTests(solutionPath, "OleVanSanten.TestTools.PracticalTests",configPath);
        }
    }
}
