﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using TestTools.Syntax;
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

        readonly MethodInfo DelegateRemove = typeof(Delegate).GetMethod("Remove");

        readonly FieldInfo FixtureFieldDelegate = typeof(Fixture).GetField("FieldDelegate");
        readonly PropertyInfo FixturePropertyDelegate = typeof(Fixture).GetProperty("PropertyDelegate");
        readonly MethodInfo FixtureRemoveField = typeof(Fixture).GetMethod("RemoveFieldDelegate", new Type[] { typeof(Action) });
        readonly MethodInfo FixtureRemovePropertyDelegate = typeof(Fixture).GetMethod("RemovePropertyDelegate", new Type[] { typeof(Action) });
        readonly MethodInfo FixtureRemoveNonExistentDelegate = typeof(Fixture).GetMethod("RemoveNonExistentDelegate", new Type[] { typeof(Action) });

        [TestMethod("Transform replaces method-call expression with add-assign expression for field events")]
        public void Transform_ReplacesMethodCallExpressionWithAddAssignExpression_ForFieldEvents()
        {
            Expression instance = Expression.Parameter(typeof(Fixture), "instance");
            Expression handler = Expression.Parameter(typeof(Action), "handler");

            // instance.RemoveFieldDelegate(handler);
            Expression input = Expression.Call(instance, FixtureRemoveField, handler);

            // instance.FieldDelegate = (Action)Deletate.Combine(instance.FieldDelegate, handler)
            Expression field = Expression.Field(instance, FixtureFieldDelegate);
            Expression call = Expression.Call(DelegateRemove, field, handler);
            Expression castedCall = Expression.Convert(call, typeof(Action));
            Expression expected = Expression.Assign(field, castedCall);

            Expression actual = new DelegateRemoveAttribute("FieldDelegate").Transform(input);

            AssertAreEqualExpressions(expected, actual);
        }

        [TestMethod("Transform replaces method-call expression with add-assign expression for property events")]
        public void Transform_ReplacesMethodCallExpressionWithAddAssignExpression_ForPropertyEvents()
        {
            Expression instance = Expression.Parameter(typeof(Fixture), "instance");
            Expression handler = Expression.Parameter(typeof(Action), "handler");

            // instance.RemovePropertyDelegate(handler);
            Expression input = Expression.Call(instance, FixtureRemovePropertyDelegate, handler);

            // instance.PropertyDelegate = (Action)Deletate.Remove(instance.PropertyDelegate, handler)
            Expression property = Expression.Property(instance, FixturePropertyDelegate);
            Expression call = Expression.Call(DelegateRemove, property, handler);
            Expression castedCall = Expression.Convert(call, typeof(Action));
            Expression expected = Expression.Assign(property, castedCall);

            Expression actual = new DelegateRemoveAttribute("PropertyDelegate").Transform(input);

            AssertAreEqualExpressions(expected, actual);
        }

        [TestMethod("Transform throws ArgumentException if there is no field or property with name")]
        public void Transform_ThrowsArgumentException_IfThereIsNoFieldOrPropertyWithName()
        {
            Expression instance = Expression.Parameter(typeof(Fixture), "instance");
            Expression handler = Expression.Parameter(typeof(Action), "handler");

            // instance.RemoveNonExistentDelegate(handler);
            Expression input = Expression.Call(instance, FixtureRemoveNonExistentDelegate, handler);

            DelegateRemoveAttribute attribute = new DelegateRemoveAttribute("NonExistentDelegate");
            Assert.ThrowsException<ArgumentException>(() => attribute.Transform(input));
        }
    }
}
