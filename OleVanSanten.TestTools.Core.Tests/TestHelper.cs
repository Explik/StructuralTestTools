using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace TestTools_Tests
{
    public static class TestHelper
    {
        public static void AssertAreEqualExpressions(Expression expected, Expression actual)
        {
            // TODO develop a more robust comparison method
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}
