using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using OleVanSanten.TestTools;
using OleVanSanten.TestTools.MSTest;

namespace Lecture_2_Tests
{
    public static class TestHelper
    {
        static TestHelper()
        {
            // To force the inclusion of the Lecture 2 Exercises assembly
            // a class from this assembly is referenced 
            Lecture_2.Program.Main(new string[0]);
        }

        public static TestFactory Factory { get; } = TestFactory.CreateFromConfigurationFile("./../../../TestToolsConfig.xml");
    }
}
