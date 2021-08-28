using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using NSubstitute;
using OleVanSanten.TestTools.Syntax;
using System.Reflection;
using static TestTools_Tests.TestHelper;

namespace TestTools_Tests.Syntax
{
    [TestClass]
    public class SyntaxVisitorTests
    {
        class TestAttribute : Attribute, ISyntaxTransformer
        {
            public Expression Transform(Expression expression)
            {
                return Expression.Constant(5);
            }
        }

        class Fixture
        {
            [TestAttribute]
            public static int Field;

            [TestAttribute]
            public static void Method() { }
        }

        FieldInfo FixureField = typeof(Fixture).GetField("Field");
        MethodInfo FixtureMethod = typeof(Fixture).GetMethod("Method");

        [TestMethod("Visit applies tranform on MemberExpression")]
        public void Visit_AppliesTransformOnMemberExpression()
        {
            Expression input = Expression.Field(null, FixureField);
            Expression expected = Expression.Constant(5);
            SyntaxVisitor visitor = new SyntaxVisitor();

            Expression actual = visitor.Visit(input);

            AssertAreEqualExpressions(expected, actual);           
        }

        [TestMethod("Visit applies tranform on MethodCallExpression")]
        public void Visit_AppliesTransformOnMethodCallExpression()
        {
            Expression input = Expression.Call(null, FixtureMethod);
            Expression expected = Expression.Constant(5);
            SyntaxVisitor visitor = new SyntaxVisitor();

            Expression actual = visitor.Visit(input);

            AssertAreEqualExpressions(expected, actual);
        }
    }
}
