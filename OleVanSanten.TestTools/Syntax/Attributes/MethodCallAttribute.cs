using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using System.Linq;

namespace OleVanSanten.TestTools.Syntax
{
    public class MethodCallAttribute : Attribute, ISyntaxTransformer
    {
        public string MethodName { get; }

        public MethodCallAttribute(string methodName)
        {
            MethodName = methodName;
        }

        public Expression Transform(Expression expression)
        {
            // Ensuring that the method with the MethodCallAttribute is called correctly
            MethodCallExpression methodCall = (MethodCallExpression)expression;
            Type type = methodCall.Method.DeclaringType;

            // Ensuring that method exists on type and has the correct return type
            Type[] parameterTypes = methodCall.Method.GetParameters().Select(p => p.ParameterType).ToArray();
            MethodInfo methodInfo = type.GetMethod(MethodName, parameterTypes);
            if (methodInfo == null)
                throw new ArgumentException($"Class {type.Name} does not contain a matching method");
            if (methodInfo.ReturnType != methodCall.Method.ReturnType)
                throw new ArgumentException($"{type.Name}.{MethodName} must have return type {methodCall.Method.ReturnType}");

            // Transforming the method call to another method call
            return Expression.Call(methodCall.Object, methodInfo, methodCall.Arguments);
        }
    }
}
