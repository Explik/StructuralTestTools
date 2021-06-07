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
    public class DelegateRemoveTests
    {
        class Fixture
        {
            public Action FieldDelegate;

            public Action PropertyDelegate { get; set; }

            public void RemoveFieldDelegate(Action handler) => FieldDelegate -= handler;

            public void RemovePropertyDelegate(Action handler) => PropertyDelegate -= handler;

            public void RemoveNonExistentDelegate(Action handler) => throw new NotImplementedException();
        }

        readonly MethodInfo FixtureRemoveField = typeof(Fixture).GetMethod("RemoveFieldDelegate", new Type[] { typeof(Action) });
        readonly MethodInfo FixtureRemovePropertyDelegate = typeof(Fixture).GetMethod("RemovePropertyDelegate", new Type[] { typeof(Action) });
        readonly MethodInfo FixtureRemoveNonExistentDelegate = typeof(Fixture).GetMethod("RemoveNonExistentDelegate", new Type[] { typeof(Action) });

        [TestMethod("Transform replaces method-call expression with add-assign expression for field events")]
        public void Transform_ReplacesMethodCallExpressionWithAddAssignExpression_ForFieldEvents()
        {
            // Creating the expression "instance.RemoveFieldDelegate(handler)"
            ParameterExpression instance = Expression.Parameter(typeof(Fixture), "instance");
            ParameterExpression handler = Expression.Parameter(typeof(Action), "handler");
            Expression input = Expression.Call(instance, FixtureRemoveField, handler);

            // Validating that DelegateAddAttribute creates expression unsubscribing to FieldDelegate
            Expression output = new DelegateRemoveAttribute("FieldDelegate").Transform(input);
            Action<Fixture, Action> removeFieldDelegate = Expression.Lambda<Action<Fixture, Action>>(output, new[] { instance, handler }).Compile();

            Fixture fixture = new Fixture();
            Action action = () => { };
            fixture.FieldDelegate += action;
            removeFieldDelegate(fixture, action);
            Assert.IsNull(fixture.FieldDelegate);
        }

        [TestMethod("Transform replaces method-call expression with add-assign expression for property events")]
        public void Transform_ReplacesMethodCallExpressionWithAddAssignExpression_ForPropertyEvents()
        {
            // Creating the expression "instance.RemovePropertyDelegate(handler)"
            ParameterExpression instance = Expression.Parameter(typeof(Fixture), "instance");
            ParameterExpression handler = Expression.Parameter(typeof(Action), "handler");
            Expression input = Expression.Call(instance, FixtureRemovePropertyDelegate, handler);

            // Validating that DelegateAddAttribute creates expression unsubscribing to PropertyDelegate
            Expression output = new DelegateRemoveAttribute("PropertyDelegate").Transform(input);
            Action<Fixture, Action> removePropertyDelegate = Expression.Lambda<Action<Fixture, Action>>(output, new[] { instance, handler }).Compile();

            Fixture fixture = new Fixture();
            Action action = () => { };
            fixture.PropertyDelegate += action;
            removePropertyDelegate(fixture, action);
            Assert.IsNull(fixture.PropertyDelegate);
        }

        [TestMethod("Transform throws ArgumentException if there is no field or property with name")]
        public void Transform_ThrowsArgumentException_IfThereIsNoFieldOrPropertyWithName()
        {
            // Creating the expression "instance.RemoveNonExistentDelegate(handler)"
            Expression instance = Expression.Parameter(typeof(Fixture), "instance");
            Expression handler = Expression.Parameter(typeof(Action), "handler");
            Expression input = Expression.Call(instance, FixtureRemoveNonExistentDelegate, handler);

            // Validating that DelegateRemoveAttribute fails on non-existent field or property
            DelegateRemoveAttribute attribute = new DelegateRemoveAttribute("NonExistentDelegate");
            Assert.ThrowsException<ArgumentException>(() => attribute.Transform(input));
        }
    }
}
