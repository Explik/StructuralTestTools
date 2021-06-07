using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using System.Linq;

namespace OleVanSanten.TestTools.Syntax
{
    public class DelegateAddAttribute : Attribute, ISyntaxTransformer
    {
        public string MemberName { get; }

        public DelegateAddAttribute(string memberName)
        {
            MemberName = memberName;
        }

        public virtual Expression Transform(Expression expression)
        {
            // Ensuring that the method with DelegateAddAttribute is called correctly
            MethodCallExpression methodCall = (MethodCallExpression)expression;
            Type type = methodCall.Method.DeclaringType;
            if (methodCall.Arguments.Count != 1)
                throw new ArgumentException($"Method {type.Name}.{methodCall.Method.Name} must be called with 1 argument");

            // Ensuring that field exists on type
            MemberExpression member;
            MemberInfo memberInfo = type.GetMember(MemberName).FirstOrDefault();
            if (memberInfo is FieldInfo fieldInfo) 
            {
                member = Expression.Field(methodCall.Object, fieldInfo); 
            }
            else if (memberInfo is PropertyInfo propertyInfo)
            {
                member = Expression.Property(methodCall.Object, propertyInfo);
            }
            else throw new ArgumentException($"Class {type.Name} does not contain field or property {MemberName}");

            // Transforming the method call to delegate subscription
            MethodInfo delegateCombine = typeof(Delegate).GetMethod("Combine", new[] { typeof(Delegate), typeof(Delegate) });
            Expression call = Expression.Call(delegateCombine, member, methodCall.Arguments[0]);
            Expression castedCall = Expression.Convert(call, methodCall.Arguments[0].Type);
            return Expression.Assign(member, castedCall);
        }
    }
}
