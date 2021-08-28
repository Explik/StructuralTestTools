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
    public class MethodCallAttributeTests
    {
        class Fixture
        {
            public int IntMethod()
            {
                return 1;
            }

            public int IntMethod(int value)
            {
                return value;
            }

            public int CallIntMethod() => throw new NotImplementedException();

            public int CallIntMethod(int value) => throw new NotImplementedException();

            public void CallIntMethodAsVoid() => throw new NotImplementedException();

            public int CallIntMethod(int value1, int value2) => throw new NotImplementedException();
        }

        MethodInfo FixtureCallIntMethod1 = typeof(Fixture).GetMethod("CallIntMethod", new Type[0]);
        MethodInfo FixtureCallIntMethod2 = typeof(Fixture).GetMethod("CallIntMethod", new Type[] { typeof(int) });
        MethodInfo FixtureCallIntMethod3 = typeof(Fixture).GetMethod("CallIntMethod", new Type[] { typeof(int), typeof(int) });
        MethodInfo FixtureCallIntMethodAsVoid = typeof(Fixture).GetMethod("CallIntMethodAsVoid", new Type[0]);

        [TestMethod("Transform replaces method-call expression without arguments with method-call expression")]
        public void Transform_ReplacesMethodCallWithoutArgumentsExpressionWithMethodExpression()
        {
            // Creating the expression "instance.CallVoidMethod()"
            ParameterExpression instance = Expression.Parameter(typeof(Fixture), "instance");
            Expression input = Expression.Call(instance, FixtureCallIntMethod1);

            // Validating that MethodCallAttribute creates expression calling IntMethod()
            Expression output = new MethodCallAttribute("IntMethod").Transform(input);
            Func<Fixture, int> callIntMethod = Expression.Lambda<Func<Fixture, int>>(output, new[] { instance }).Compile();

            Fixture fixture = new Fixture();
            Assert.AreEqual(1, callIntMethod(fixture));
        }

        [TestMethod("Transform replaces method-call expression with arguments with method-call expression")]
        public void Transform_ReplacesMethodCallExpressionWithArgumentsWithMethodCallExpression()
        {
            // Creating the expression "instance.CallIntMethod(5)"
            ParameterExpression instance = Expression.Parameter(typeof(Fixture), "instance");
            ParameterExpression value = Expression.Parameter(typeof(int), "value");
            Expression input = Expression.Call(instance, FixtureCallIntMethod2, value);

            // Validating that MethodCallAttribute creates expression calling IntMethod(int)
            Expression output = new MethodCallAttribute("IntMethod").Transform(input);
            Func<Fixture, int, int> callIntMethod = Expression.Lambda<Func<Fixture, int, int>>(output, new[] { instance, value }).Compile();

            Fixture fixture = new Fixture();
            Assert.AreEqual(5, callIntMethod(fixture, 5));
        }

        [TestMethod("Transform throws ArgumentException if no method with equavilent parameter list is found")]
        public void Transform_ThrowsArgumentException_IfNoConstructorWithEquavilentParameterListIsFound()
        {
            // Creating the expression "Fixture.CallIntMethod(5, 5)"
            Expression instance = Expression.Parameter(typeof(Fixture), "instance");
            Expression input = Expression.Call(instance, FixtureCallIntMethod3, Expression.Constant(5), Expression.Constant(5));

            // Validating that MethodCallAttribute fails on non-matching method
            MethodCallAttribute attribute = new MethodCallAttribute("IntMethod");
            Assert.ThrowsException<ArgumentException>(() => attribute.Transform(input));
        }

        [TestMethod("Transform throws ArgumentException if method type is not matched")]
        public void Transform_ThrowsArgumetnException_IfMethodReturnTypeIsNotMatched()
        {
            // Creating the expression "Fixture.CallIntMethodAsVoid()"
            Expression instance = Expression.Parameter(typeof(Fixture), "instance");
            Expression input = Expression.Call(instance, FixtureCallIntMethodAsVoid);

            // Validating that MethodCallAttribute fails if return types do not match
            MethodCallAttribute attribute = new MethodCallAttribute("IntMethod");
            Assert.ThrowsException<ArgumentException>(() => attribute.Transform(input));
        }
    }
}
