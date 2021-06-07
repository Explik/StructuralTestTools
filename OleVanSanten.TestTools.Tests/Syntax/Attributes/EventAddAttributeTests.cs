using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using OleVanSanten.TestTools.Syntax;

namespace TestTools_Tests.Syntax
{
    [TestClass]
    public class EventAddAttributeTests
    {
        class Fixture
        {
            public event EventHandler Event;

            public void AddEvent(EventHandler handler) => throw new NotImplementedException();

            public void AddNonExistentEvent(EventHandler handler) => throw new NotImplementedException();

            public void TriggerEvent() => Event?.Invoke(this, new EventArgs());
        }

        readonly MethodInfo FixtureAddEvent = typeof(Fixture).GetMethod("AddEvent", new Type[] { typeof(EventHandler) });
        readonly MethodInfo FixtureAddNonExistentEvent = typeof(Fixture).GetMethod("AddNonExistentEvent", new Type[] { typeof(EventHandler) });

        [TestMethod("Transform replaces method-call expression with subscribe expression")]
        public void Transform_ReplacesMethodCallExpressionWithAddAssignExpression_ForFieldEvents()
        {
            // Creating the expression "instance.AddEvent(handler)"
            ParameterExpression instance = Expression.Parameter(typeof(Fixture), "instance");
            ParameterExpression handler = Expression.Parameter(typeof(EventHandler), "handler");
            Expression input = Expression.Call(instance, FixtureAddEvent, handler);

            // Validating that EventAddAttribute creates expression subscribing to Event
            Expression output = new EventAddAttribute("Event").Transform(input);
            Action<Fixture, EventHandler> addEvent = Expression.Lambda<Action<Fixture, EventHandler>>(output, new[] { instance, handler }).Compile();

            bool eventEmitted = false;
            Fixture fixture = new Fixture();
            EventHandler eventHandler = (sender, e) => eventEmitted = true;
            addEvent(fixture, eventHandler);
            fixture.TriggerEvent();
            Assert.IsTrue(eventEmitted);
        }

        [TestMethod("Transform throws ArgumentException if there is no event with name")]
        public void Transform_ThrowsArgumentException_IfThereIsNoFieldOrPropertyWithName()
        {
            // Creating the expression "instance.AddNonExistentEvent(handler)"
            Expression instance = Expression.Parameter(typeof(Fixture), "instance");
            Expression handler = Expression.Parameter(typeof(EventHandler), "handler");
            Expression input = Expression.Call(instance, FixtureAddNonExistentEvent, handler);

            // Validating that EventAddAttribute fails on non-existent event
            EventAddAttribute attribute = new EventAddAttribute("NonExistentEvent");
            Assert.ThrowsException<ArgumentException>(() => attribute.Transform(input));
        }
    }
}
