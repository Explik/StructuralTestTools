using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Syntax;
using System.Linq.Expressions;
using System.Reflection;
using static TestTools_Tests.TestHelper;

namespace TestTools_Tests.Syntax
{
    [TestClass]
    public class ConstructorCallAttributeTests
    {
        class Fixture
        {
            public int Value { get; }

            public Fixture() 
            {
                Value = 1;
            }

            public Fixture(int value) 
            {
                Value = value;
            }

            public static Fixture CreateTestClass() => throw new NotImplementedException();

            public static Fixture CreateTestClass(int value) => throw new NotImplementedException();

            public static Fixture CreateTestClass(int value1, int value2) => throw new NotImplementedException();
        }

        MethodInfo FixtureCreateTestClass1 = typeof(Fixture).GetMethod("CreateTestClass", new Type[0]);
        MethodInfo FixtureCreateTestClass2 = typeof(Fixture).GetMethod("CreateTestClass", new Type[] { typeof(int) });
        MethodInfo FixtureCreateTestClass3 = typeof(Fixture).GetMethod("CreateTestClass", new Type[] { typeof(int), typeof(int) });

        [TestMethod("Transform replaces method call expression without arguments with new expression")]
        public void Transform_ReplacesMethodCallWithoutArgumentsExpressionWithNewExpression()
        {
            // Creating the expression "Fixture.CreateTestClass()"
            Expression input = Expression.Call(FixtureCreateTestClass1);

            // Validating that ConstructorCallAttribute creates expression calling Fixture()
            Expression output = new ConstructorCallAttribute().Transform(input);
            Func<Fixture> func = Expression.Lambda<Func<Fixture>>(output).Compile();
            Fixture fixture = func();

            Assert.AreEqual(fixture.Value, 1);
        }

        [TestMethod("Transform replaces method call expression with arguments with new expression")]
        public void Transform_ReplacesMethodCallExpressionWithArgumentsWithNewExpression()
        {
            // Creating the expression "Fixture.CreateTestClass(5)"
            Expression input = Expression.Call(FixtureCreateTestClass2, Expression.Constant(5));

            // Validating that ConstructorCallAttribute creates expression calling Fixture(5)
            Expression output = new ConstructorCallAttribute().Transform(input);
            Func<Fixture> createFixture = Expression.Lambda<Func<Fixture>>(output).Compile();
            Fixture fixture = createFixture();

            Assert.AreEqual(fixture.Value, 5);
        }

        [TestMethod("Transform throws ArgumentException if no constructor with equavilent parameter list is found")]
        public void Transform_ThrowsArgumentException_IfNoConstructorWithEquavilentParameterListIsFound()
        {
            // Creating the expression "Fixture.CreateTestClass(5, 5)"
            Expression input = Expression.Call(FixtureCreateTestClass3, Expression.Constant(5), Expression.Constant(5));

            // Validating that ConstructorCallAttribute fails on non-existent delegate
            ConstructorCallAttribute attribute = new ConstructorCallAttribute();
            Assert.ThrowsException<ArgumentException>(() => attribute.Transform(input));
        }
    }
}
