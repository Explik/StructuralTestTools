using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace OleVanSanten.TestTools.Syntax
{
    public class PropertyGetAttribute : Attribute, ISyntaxTransformer
    {
        public string PropertyName { get; }

        public PropertyGetAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        public Expression Transform(Expression expression)
        {
            // Ensuring that the method with the PropertyGetAttribute is called correctly
            MethodCallExpression methodCall = (MethodCallExpression)expression;
            Type type = methodCall.Method.DeclaringType;
            if (methodCall.Arguments.Count != 0)
                throw new ArgumentException($"Method {type.Name}.{methodCall.Method.Name} cannot be called with any arguments");

            // Ensuring that property exists on type and is readable
            PropertyInfo propertyInfo = type.GetProperty(PropertyName);
            if (propertyInfo == null)
                throw new ArgumentException($"Class {type.Name} does not contain property {PropertyName}");
            if (!propertyInfo.CanRead)
                throw new ArgumentException($"Property {type.Name}.{PropertyName} must be readable");

            // Transforming the method call to a property
            return Expression.Property(methodCall.Object, propertyInfo);
        }
    }
}
