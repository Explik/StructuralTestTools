using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;

namespace OleVanSanten.TestTools.MSTest
{
    public static class DelegateAssert
    {
        static List<Action> _invocationStatusAssertions = new List<Action>();

        public static void IsInvoked<TDelegate>(Action<TDelegate> subscribe) where TDelegate : Delegate
        {
            bool isCalled = false;
            Expression<Func<bool>> isCalledExpression = () => isCalled;
            Expression isCalledAssignmentExpression = Expression.Assign(isCalledExpression.Body, Expression.Constant(true));

            TDelegate updateInvocationStatus = CreateDelegate<TDelegate>(isCalledAssignmentExpression);
            Action verifyInvocationStatus = () => Assert.IsTrue(isCalled);

            subscribe(updateInvocationStatus);
            _invocationStatusAssertions.Add(verifyInvocationStatus);
        }

        public static void IsInvoked<TDelegate>(Action<TDelegate> subscribe, TDelegate assertionCallback) where TDelegate : Delegate
        {
            bool isCalled = false;
            Expression<Func<bool>> isCalledExpression = () => isCalled;
            Expression isCalledAssignmentExpression = Expression.Assign(isCalledExpression.Body, Expression.Constant(true));

            TDelegate updateInvocationStatus = CreateDelegate<TDelegate>(isCalledAssignmentExpression);
            Action verifyInvocationStatus = () => Assert.IsTrue(isCalled);

            subscribe(updateInvocationStatus);
            subscribe(assertionCallback);
            _invocationStatusAssertions.Add(verifyInvocationStatus);
        }

        public static void IsNotInvoked<TDelegate>(Action<TDelegate> subscribe) where TDelegate : Delegate
        {
            bool isCalled = false;
            Expression<Func<bool>> isCalledExpression = () => isCalled;
            Expression isCalledAssignmentExpression = Expression.Assign(isCalledExpression.Body, Expression.Constant(true));

            TDelegate updateInvocationStatus = CreateDelegate<TDelegate>(isCalledAssignmentExpression);
            Action verifyInvocationStatus = () => Assert.IsFalse(isCalled);

            subscribe(updateInvocationStatus);
            _invocationStatusAssertions.Add(verifyInvocationStatus);
        }

        public static void Verify()
        {
            Action[] assertions = _invocationStatusAssertions.ToArray();

            // As an assertion may fail, the assertion list must be cleared beforehand
            _invocationStatusAssertions.Clear();

            foreach (var assertion in assertions)
                assertion();
        }

        // For void-returning delegates, creates a method (par1, par2, ...) => [body]
        // For non-void-returning delegates, creates a method (par1, par2, ....) => { [body]; return default(TReturn) }
        private static TDelegate CreateDelegate<TDelegate>(Expression body) where TDelegate : Delegate
        {
            MethodInfo invoke = typeof(TDelegate).GetMethod("Invoke");
            
            ParameterExpression[] parameters = invoke.GetParameters().Select(p => Expression.Parameter(p.ParameterType)).ToArray();

            Expression extendedBody;
            if (invoke.ReturnType != typeof(void))
            {
                extendedBody = Expression.Block(body, Expression.Default(invoke.ReturnType));
            }
            else extendedBody = body;

            // This method will only be used once, and therefore intepretation is the best option
            return Expression.Lambda<TDelegate>(extendedBody, parameters).Compile(preferInterpretation: true);
        }
    }
}
