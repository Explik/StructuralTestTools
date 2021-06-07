using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using OleVanSanten.TestTools;
using OleVanSanten.TestTools.MSTest;

namespace Lecture_6_Tests
{
    public static class TestHelper
    {
        static TestHelper()
        {
            // To force the inclusion of the Lecture 6 Exercises assembly
            // a class from this assembly is referenced 
            Lecture_6.Program.Main(new string[0]);
        }

        public static TestFactory Factory { get; } = TestFactory.CreateFromConfigurationFile("./../../../TestToolsConfig.xml");
    }
}
