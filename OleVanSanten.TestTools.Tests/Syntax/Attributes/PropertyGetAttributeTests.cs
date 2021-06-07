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
    public class PropertyGetAttributeTests
    {
        class Fixture
        {
            public int Property { get; set; }

            public int WriteonlyProperty { set { } }

            public int GetProperty() => throw new NotImplementedException();

            public int GetNonExistentProperty() => throw new NotImplementedException();

            public int GetWriteonlyProperty() => throw new NotImplementedException();
        }

        readonly MethodInfo FixtureGetProperty = typeof(Fixture).GetMethod("GetProperty", new Type[0]);
        readonly MethodInfo FixtureGetNonExistentProperty = typeof(Fixture).GetMethod("GetNonExistentProperty", new Type[0]);
        readonly MethodInfo FixtureGetWriteonlyProperty = typeof(Fixture).GetMethod("GetWriteonlyProperty", new Type[0]);

        [TestMethod("Transform replaces method-call expression with property expression")]
        public void Transform_ReplacesMethodCallExpressionWithAddAssignExpression_ForFieldEvents()
        {
            // Creating the expression "instance.GetProperty()"
            ParameterExpression instance = Expression.Parameter(typeof(Fixture), "instance");
            Expression input = Expression.Call(instance, FixtureGetProperty);

            // Validating that PropertyGetAttribute creates expression reading Property
            Expression output = new PropertyGetAttribute("Property").Transform(input);
            Func<Fixture, int> getProperty = Expression.Lambda<Func<Fixture, int>>(output, new[] { instance }).Compile();

            Fixture fixture = new Fixture() { Property = 5 };
            Assert.AreEqual(5, getProperty(fixture));
        }

        [TestMethod("Transform throws ArgumentException if there is no property with name")]
        public void Transform_ThrowsArgumentException_IfThereIsNoFieldOrPropertyWithName()
        {
            // Creating the expression "instance.GetNonExistentProperty()"
            Expression instance = Expression.Parameter(typeof(Fixture), "instance");
            Expression input = Expression.Call(instance, FixtureGetNonExistentProperty);

            // Validating that PropertyGetAttribute fails on non-existent property
            PropertyGetAttribute attribute = new PropertyGetAttribute("NonExistentProperty");
            Assert.ThrowsException<ArgumentException>(() => attribute.Transform(input));
        }

        [TestMethod("Transform throws ArgumentException on writeonly property")]
        public void Transform_ThrowsArgumentExceptionOnReadonlyProperty()
        {
            // Creating the expression "instance.GetWriteonlyProperty()"
            Expression instance = Expression.Parameter(typeof(Fixture), "instance");
            Expression input = Expression.Call(instance, FixtureGetWriteonlyProperty);

            // Validating that PropertyGetAttribute fails on non-readable property
            PropertyGetAttribute attribute = new PropertyGetAttribute("WriteonlyProperty");
            Assert.ThrowsException<ArgumentException>(() => attribute.Transform(input));
        }
    }
}
