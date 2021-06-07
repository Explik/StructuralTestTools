using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

namespace OleVanSanten.TestTools.Syntax
{
    public class FieldGetAttribute : Attribute, ISyntaxTransformer
    {
        public string FieldName { get; }

        public FieldGetAttribute(string fieldName)
        {
            FieldName = fieldName;
        }

        public Expression Transform(Expression expression)
        {
            // Ensuring that the method with FieldGetAttribute is called correctly
            MethodCallExpression methodCall = (MethodCallExpression)expression;
            Type type = methodCall.Method.DeclaringType;
            if (methodCall.Arguments.Count != 0)
                throw new ArgumentException($"Method {type.Name}.{methodCall.Method.Name} cannot be called with arguments");

            // Ensuring that field exists on type
            FieldInfo fieldInfo = type.GetField(FieldName);
            if (fieldInfo == null)
                throw new ArgumentException($"Class {type.Name} does not contain field {FieldName}");

            // Transforming the method call to a field
            return Expression.Field(methodCall.Object, FieldName);
        }
    }
}
