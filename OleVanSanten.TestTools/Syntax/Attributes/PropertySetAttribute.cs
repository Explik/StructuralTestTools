using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

namespace OleVanSanten.TestTools.Syntax
{
    public class PropertySetAttribute : Attribute, ISyntaxTransformer
    {
        public string PropertyName { get; }

        public PropertySetAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        public Expression Transform(Expression expression)
        {
            // Ensuring that the method with the PropertySetAttribute is called correctly
            MethodCallExpression methodCall = (MethodCallExpression)expression;
            Type type = methodCall.Method.DeclaringType;
            if (methodCall.Arguments.Count != 1)
                throw new ArgumentException($"Method {type.Name}.{methodCall.Method.Name} must be called with 1 argument");

            // Ensuring that property exists on type and is writable
            PropertyInfo propertyInfo = type.GetProperty(PropertyName);
            if (propertyInfo == null)
                throw new ArgumentException($"Class {type.Name} does not contain property {PropertyName}");
            if (!propertyInfo.CanWrite)
                throw new ArgumentException($"Property {type.Name}.{PropertyName} must be writable");

            // Transforming the method call to a property assignment
            MemberExpression property = Expression.Property(methodCall.Object, propertyInfo);
            return Expression.Assign(property, methodCall.Arguments[0]);
        }
    }
}
