using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using System.Linq.Expressions;

namespace Lecture_4_Tests
{
    public static class TestHelper
    {
        static TestHelper()
        {
            // To force the inclusion of the Lecture 4 Exercises assembly
            // a class from this assembly is referenced 
            Lecture_4.Program.Main(new string[0]);
        }

        public static TestFactory Factory { get; } = TestFactory.CreateFromConfigurationFile("./../../../TestToolsConfig.xml");
    }
}
