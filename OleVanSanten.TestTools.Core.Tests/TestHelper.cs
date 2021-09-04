using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using OleVanSanten.TestTools.Expressions;

namespace TestTools_Tests
{
    public static class TestHelper
    {
        public static void AssertAreEqualExpressions(Expression expected, Expression actual)
        {
            // TODO develop a more robust comparison method
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        public static void AssertAreEqualExpressions(TestExpression expected, TestExpression actual)
        {
            AssertAreEqualExpressions(expected.Expression, actual.Expression);
        }

        public static void AssertAreEqualExpressions<T>(TestExpression<T> expected, TestExpression<T> actual)
        {
            AssertAreEqualExpressions(expected.Expression, actual.Expression);
        }
    }
}
