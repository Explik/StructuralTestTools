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
    public class DelegateAddAttributeTests
    {
        class Fixture
        {
            public Action FieldDelegate;

            public Action PropertyDelegate { get; set; }

            public void AddFieldDelegate(Action handler) => throw new NotImplementedException();

            public void AddPropertyDelegate(Action handler) => throw new NotImplementedException();

            public void AddNonExistentDelegate(Action handler) => throw new NotImplementedException();
        }

        readonly MethodInfo FixtureAddDelegate = typeof(Fixture).GetMethod("AddFieldDelegate", new Type[] { typeof(Action) });
        readonly MethodInfo FixtureAddPropertyDelegate = typeof(Fixture).GetMethod("AddPropertyDelegate", new Type[] { typeof(Action) });
        readonly MethodInfo FixtureAddNonExistentDelegate = typeof(Fixture).GetMethod("AddNonExistentDelegate", new Type[] { typeof(Action) });

        [TestMethod("Transform replaces method-call expression with add-assign expression for field events")]
        public void Transform_ReplacesMethodCallExpressionWithAddAssignExpression_ForFieldEvents()
        {
            // Creating the expression "instance.AddFieldDelegate(handler)"
            ParameterExpression instance = Expression.Parameter(typeof(Fixture), "instance");
            ParameterExpression handler = Expression.Parameter(typeof(Action), "handler");
            Expression input = Expression.Call(instance, FixtureAddDelegate, handler);

            // Validating that DelegateAddAttribute creates expression subscribing to FieldDelegate
            Expression output = new DelegateAddAttribute("FieldDelegate").Transform(input);
            Action<Fixture, Action> addFieldDelegate = Expression.Lambda<Action<Fixture, Action>>(output, new[] { instance, handler }).Compile();

            Fixture fixture = new Fixture();
            Action action = () => { };
            addFieldDelegate(fixture, action);
            CollectionAssert.Contains(fixture.FieldDelegate.GetInvocationList(), action);
        }

        [TestMethod("Transform replaces method-call expression with add-assign expression for property events")]
        public void Transform_ReplacesMethodCallExpressionWithAddAssignExpression_ForPropertyEvents()
        {
            // Creating the expression "instance.AddPropertyDelegate(handler)"
            ParameterExpression instance = Expression.Parameter(typeof(Fixture), "instance");
            ParameterExpression handler = Expression.Parameter(typeof(Action), "handler");
            Expression input = Expression.Call(instance, FixtureAddPropertyDelegate, handler);

            // Validating that DelegateAddAttribute creates expression subscribing to PropertyDelegate
            Expression output = new DelegateAddAttribute("PropertyDelegate").Transform(input);
            Action<Fixture, Action> addPropertyDelegate = Expression.Lambda<Action<Fixture, Action>>(output, new[] { instance, handler }).Compile();

            Fixture fixture = new Fixture();
            Action action = () => { };
            addPropertyDelegate(fixture, action);
            CollectionAssert.Contains(fixture.PropertyDelegate.GetInvocationList(), action);
        }

        [TestMethod("Transform throws ArgumentException if there is no field or property with name")]
        public void Transform_ThrowsArgumentException_IfThereIsNoFieldOrPropertyWithName()
        {
            // Creating the expression "instance.AddNonExistentDelegate(handler)"
            Expression instance = Expression.Parameter(typeof(Fixture), "instance");
            Expression handler = Expression.Parameter(typeof(Action), "handler");
            Expression input = Expression.Call(instance, FixtureAddNonExistentDelegate, handler);
            
            // Validating that DelegateAddAttribute fails on non-existent field or property
            DelegateAddAttribute attribute = new DelegateAddAttribute("NonExistentMember");
            Assert.ThrowsException<ArgumentException>(() => attribute.Transform(input));
        }
    }
}
