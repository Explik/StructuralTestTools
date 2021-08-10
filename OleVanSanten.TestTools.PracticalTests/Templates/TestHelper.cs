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
        }

        public static TestFactory Factory { get; } = TestFactory.CreateFromConfigurationFile("./../../../TestToolsConfig.xml");
    }
}
