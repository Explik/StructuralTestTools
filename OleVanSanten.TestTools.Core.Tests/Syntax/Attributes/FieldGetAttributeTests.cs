using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using OleVanSanten.TestTools.Syntax;
using static TestTools_Tests.TestHelper;

namespace TestTools_Tests.Syntax
{
    [TestClass]
    public class FieldGetAttributeTests
    {
        class Fixture
        {
            public int Field;

            public int GetField() => throw new NotImplementedException();

            public int GetNonExistentField() => throw new NotImplementedException();
        }

        readonly MethodInfo FixtureGetField = typeof(Fixture).GetMethod("GetField", new Type[0]);
        readonly MethodInfo FixtureGetNonExistentField = typeof(Fixture).GetMethod("GetNonExistentField", new Type[0]);

        [TestMethod("Transform replaces method-call expression with field expression")]
        public void Transform_ReplacesMethodCallExpressionWithAddAssignExpression_ForFieldEvents()
        {
            // Creating the expression "instance.GetField()"
            ParameterExpression instance = Expression.Parameter(typeof(Fixture), "instance");
            Expression input = Expression.Call(instance, FixtureGetField);

            // Validating that FieldGetAttribute creates expression reading Field
            Expression output = new FieldGetAttribute("Field").Transform(input);
            Func<Fixture, int> getField = Expression.Lambda<Func<Fixture, int>>(output, new[] { instance }).Compile();

            Fixture fixture = new Fixture() { Field = 5 };
            Assert.AreEqual(5, getField(fixture));
        }

        [TestMethod("Transform throws ArgumentException if there is no field with name")]
        public void Transform_ThrowsArgumentException_IfThereIsNoFieldOrPropertyWithName()
        {
            // Creating the expression "instance.GetNonExistentField()"
            Expression instance = Expression.Parameter(typeof(Fixture), "instance");
            Expression input = Expression.Call(instance, FixtureGetNonExistentField);

            // Validating that FieldGetAttribute fails on non-existent field
            FieldGetAttribute attribute = new FieldGetAttribute("NonExistentField");
            Assert.ThrowsException<ArgumentException>(() => attribute.Transform(input));
        }
    }
}
