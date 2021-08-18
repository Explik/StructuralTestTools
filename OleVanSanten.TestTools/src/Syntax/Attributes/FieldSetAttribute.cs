using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

namespace OleVanSanten.TestTools.Syntax
{
    public class FieldSetAttribute : Attribute, ISyntaxTransformer
    {
        public string FieldName { get; }

        public FieldSetAttribute(string fieldName)
        {
            FieldName = fieldName;
        }

        public Expression Transform(Expression expression)
        {
            // Ensuring that the method with the FieldSetAttribute is called correctly
            MethodCallExpression methodCall = (MethodCallExpression)expression;
            Type type = methodCall.Method.DeclaringType;
            if (methodCall.Arguments.Count != 1)
                throw new ArgumentException($"Method {type.Name}.{methodCall.Method.Name} must be called with 1 argument");

            // Ensuring that field exists on type
            FieldInfo fieldInfo = type.GetField(FieldName);
            if (fieldInfo == null)
                throw new ArgumentException($"Class {type.Name} does not contain field {FieldName}");

            // Transforming the method call to a field assignment
            MemberExpression field = Expression.Field(methodCall.Object, fieldInfo);
            return Expression.Assign(field, methodCall.Arguments[0]);
        }
    }
}
