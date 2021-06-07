using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using OleVanSanten.TestTools.Syntax;
using static TestTools_Tests.TestHelper;

namespace TestTools_Tests.Syntax
{
    [TestClass]
    public class EventRemoveAttributeTests
    {
        class Fixture
        {
            public event EventHandler Event;

            public void RemoveEvent(EventHandler handler) => throw new NotImplementedException();

            public void RemoveNonExistentEvent(EventHandler handler) => throw new NotImplementedException();

            public void TriggerEvent() => Event?.Invoke(this, new EventArgs());
        }

        readonly MethodInfo FixtureRemoveEvent = typeof(Fixture).GetMethod("RemoveEvent", new Type[] { typeof(EventHandler) });
        readonly MethodInfo FixtureRemoveNonExistentEvent = typeof(Fixture).GetMethod("RemoveNonExistentEvent", new Type[] { typeof(EventHandler) });

        [TestMethod("Transform replaces method-call expression with unsubscribe expression")]
        public void Transform_ReplacesMethodCallExpressionWithAddAssignExpression_ForFieldEvents()
        {
            // Creating the expression "instance.RemoveEvent(handler)"
            ParameterExpression instance = Expression.Parameter(typeof(Fixture), "instance");
            ParameterExpression handler = Expression.Parameter(typeof(EventHandler), "handler");
            Expression input = Expression.Call(instance, FixtureRemoveEvent, handler);

            // Validating that EventRemoveAttribute creates expression unsubscribing to Event
            Expression output = new EventRemoveAttribute("Event").Transform(input);
            Action<Fixture, EventHandler> removeEvent = Expression.Lambda<Action<Fixture, EventHandler>>(output, new[] { instance, handler }).Compile();

            bool eventEmitted = false;
            Fixture fixture = new Fixture();
            EventHandler eventHandler = (sender, e) => eventEmitted = true;
            fixture.Event += eventHandler;
            removeEvent(fixture, eventHandler);
            fixture.TriggerEvent();
            Assert.IsFalse(eventEmitted);
        }

        [TestMethod("Transform throws ArgumentException if there is no event with name")]
        public void Transform_ThrowsArgumentException_IfThereIsNoFieldOrPropertyWithName()
        {
            // instance.RemoveNonExistentEvent(handler);
            Expression instance = Expression.Parameter(typeof(Fixture), "instance");
            Expression handler = Expression.Parameter(typeof(EventHandler), "handler");
            Expression input = Expression.Call(instance, FixtureRemoveNonExistentEvent, handler);

            // Validating that EventRemoveAttribute fails on non-existent event
            EventRemoveAttribute attribute = new EventRemoveAttribute("NonExistentEvent");
            Assert.ThrowsException<ArgumentException>(() => attribute.Transform(input));
        }
    }
}
