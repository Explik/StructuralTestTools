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
    public class PropertySetAttributeTests
    {
        class Fixture
        {
            public int Property { get; set; }
            
            public int ReadonlyProperty {
                get => throw new NotImplementedException();
            }

            public int SetProperty(int value) => throw new NotImplementedException();

            public int SetNonExistentProperty(int value) => throw new NotImplementedException();

            public int SetReadonlyProperty(int value) => throw new NotImplementedException();
        }

        readonly MethodInfo FixtureSetProperty = typeof(Fixture).GetMethod("SetProperty", new Type[] { typeof(int) });
        readonly MethodInfo FixtureSetNonExistentProperty = typeof(Fixture).GetMethod("SetNonExistentProperty", new Type[] { typeof(int) });
        readonly MethodInfo FixtureReadonlyProperty = typeof(Fixture).GetMethod("SetReadonlyProperty", new Type[] { typeof(int) });

        [TestMethod("Transform replaces method-call expression with field expression")]
        public void Transform_ReplacesMethodCallExpressionWithAddAssignExpression_ForFieldEvents()
        {
            // Creating the expression "instance.SetProperty(value)"
            ParameterExpression instance = Expression.Parameter(typeof(Fixture), "instance");
            ParameterExpression value = Expression.Parameter(typeof(int), "value");
            Expression input = Expression.Call(instance, FixtureSetProperty, Expression.Constant(5));

            // Validating that PropertySetAttribute creates expression assigning Property
            Expression output = new PropertySetAttribute("Property").Transform(input);
            Func<Fixture, int, int> setProperty = Expression.Lambda<Func<Fixture, int, int>>(output, new[] { instance, value }).Compile();

            Fixture fixture = new Fixture();
            setProperty(fixture, 5);
            Assert.AreEqual(5, fixture.Property);
        }

        [TestMethod("Transform throws ArgumentException if there is no property with name")]
        public void Transform_ThrowsArgumentException_IfThereIsNoFieldOrPropertyWithName()
        {
            // Creating the expression "instance.SetNonExistentProperty(5)"
            Expression instance = Expression.Parameter(typeof(Fixture), "instance");
            Expression input = Expression.Call(instance, FixtureSetNonExistentProperty, Expression.Constant(5));

            // Validating that PropertySetAttribute fails on non-existent property
            PropertySetAttribute attribute = new PropertySetAttribute("NonExistentProperty");
            Assert.ThrowsException<ArgumentException>(() => attribute.Transform(input));
        }

        [TestMethod("Transform throws ArgumentException on readonly property")]
        public void Transform_ThrowsArgumentException_OnReadonlyProperty()
        {
            // Creating the expression "instance.SetReadonlyProperty(5)"
            Expression instance = Expression.Parameter(typeof(Fixture), "instance");
            Expression input = Expression.Call(instance, FixtureReadonlyProperty, Expression.Constant(5));

            // Validating that PropertySetAttribute fails on non-writable property
            PropertySetAttribute attribute = new PropertySetAttribute("ReadonlyProperty");
            Assert.ThrowsException<ArgumentException>(() => attribute.Transform(input));
        }
    }
}
