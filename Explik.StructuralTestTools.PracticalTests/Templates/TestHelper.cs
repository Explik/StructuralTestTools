using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Explik.StructuralTestTools;
using Explik.StructuralTestTools.MSTest;

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
