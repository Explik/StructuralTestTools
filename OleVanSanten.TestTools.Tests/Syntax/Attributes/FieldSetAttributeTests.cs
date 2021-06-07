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
    public class FieldSetAttributeTests
    {
        class Fixture
        {
            public int Field;

            public void SetField(int value) => throw new NotImplementedException();

            public void GetNonExistentField(int value) => throw new NotImplementedException();
        }

        readonly MethodInfo FixtureSetField = typeof(Fixture).GetMethod("SetField", new Type[] { typeof(int) });
        readonly MethodInfo FixutreSetNonExistentField = typeof(Fixture).GetMethod("GetNonExistentField", new Type[] { typeof(int) });

        [TestMethod("Transform replaces method-call expression with field expression")]
        public void Transform_ReplacesMethodCallExpressionWithAddAssignExpression_ForFieldEvents()
        {
            // Creating the expression "instance.GetField(value)"
            ParameterExpression instance = Expression.Parameter(typeof(Fixture), "instance");
            ParameterExpression value = Expression.Parameter(typeof(int), "value");
            Expression input = Expression.Call(instance, FixtureSetField, value);

            // Validating that FieldSetAttribute creates expression assigning Field
            Expression output = new FieldSetAttribute("Field").Transform(input);
            Func<Fixture, int, int> setField = Expression.Lambda<Func<Fixture, int, int>>(output, new[] { instance, value }).Compile();

            Fixture fixture = new Fixture();
            setField(fixture, 5);
            Assert.AreEqual(5, fixture.Field);
        }

        [TestMethod("Transform throws ArgumentException if there is no field with name")]
        public void Transform_ThrowsArgumentException_IfThereIsNoFieldOrPropertyWithName()
        {
            // Creating the expression "instance.SetNonExistentField(5)"
            Expression instance = Expression.Parameter(typeof(Fixture), "instance");
            Expression input = Expression.Call(instance, FixutreSetNonExistentField, Expression.Constant(5));

            // Validating that FieldSetAttribute fails on non-existent field
            FieldSetAttribute attribute = new FieldSetAttribute("NonExistentField");
            Assert.ThrowsException<ArgumentException>(() => attribute.Transform(input));
        }
    }
}
