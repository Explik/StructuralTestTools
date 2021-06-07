using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

namespace OleVanSanten.TestTools.Syntax
{
    public class EventRemoveAttribute
    {
        public string EventName { get; }

        public EventRemoveAttribute(string eventName)
        {
            EventName = eventName;
        }

        public Expression Transform(Expression expression)
        {
            // Ensuring that the method with the FieldSetAttribute is called correctly
            MethodCallExpression methodCall = (MethodCallExpression)expression;
            Type type = methodCall.Method.DeclaringType;
            if (methodCall.Arguments.Count != 1)
                throw new ArgumentException($"Method {type.Name}.{methodCall.Method.Name} must be called with 1 argument");

            // Ensuring that event exists on type
            EventInfo eventInfo = type.GetEvent(EventName);
            if (eventInfo == null)
                throw new ArgumentException($"Class {type.Name} does not contain event {EventName}");

            // Transforming the method call to an event unsubscription expression
            // "obj.GetType().GetEvent(EventName).RemoveMethod.Invoke(obj, new object[] { handler })"
            MethodInfo getType = typeof(object).GetMethod("GetType");
            MethodInfo getEvent = typeof(Type).GetMethod("GetEvent", new[] { typeof(string) });
            PropertyInfo removeMethod = typeof(EventInfo).GetProperty("RemoveMethod");
            MethodInfo invoke = typeof(MethodInfo).GetMethod("Invoke", new[] { typeof(object), typeof(object[]) });

            Expression getTypeExpression = Expression.Call(methodCall.Object, getType);
            Expression getEventExpression = Expression.Call(getTypeExpression, getEvent, Expression.Constant(EventName));
            Expression removeMethodExpression = Expression.Property(getEventExpression, removeMethod);
            Expression arrayExpression = Expression.NewArrayInit(typeof(object), methodCall.Arguments[0]);
            Expression invokeExpression = Expression.Call(removeMethodExpression, invoke, methodCall.Object, arrayExpression);

            return invokeExpression;
        }
    }
}
