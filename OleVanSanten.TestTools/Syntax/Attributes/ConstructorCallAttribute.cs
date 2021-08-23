using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using System.Linq;

namespace OleVanSanten.TestTools.Syntax
{
    public class ConstructorCallAttribute : Attribute, ISyntaxTransformer
    {
        public ConstructorCallAttribute()
        {
        }

        public virtual Expression Transform(Expression expression)
        {
            // Ensuring that the method with the MethodCallAttribute is called correctly
            MethodCallExpression methodCall = (MethodCallExpression)expression;
            Type type = methodCall.Method.DeclaringType;

            // Ensuring that field exists on type
            Type[] parameterTypes = methodCall.Method.GetParameters().Select(p => p.ParameterType).ToArray();
            ConstructorInfo constructorInfo = type.GetConstructor(parameterTypes);
            if (constructorInfo == null)
                throw new ArgumentException($"Class {type.Name} does not contain a matching constructor");

            // Transforming the method call to another method call
            return Expression.New(constructorInfo, methodCall.Arguments);
        }
    }
}
