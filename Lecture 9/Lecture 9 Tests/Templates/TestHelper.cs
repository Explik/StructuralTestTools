using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools;
using OleVanSanten.TestTools.MSTest;
using System.Linq.Expressions;

namespace Lecture_9_Tests
{
    public static class TestHelper
    {
        public static TestFactory Factory { get; } = TestFactory.CreateFromConfigurationFile("./../../../TestToolsConfig.xml");
    }
}
