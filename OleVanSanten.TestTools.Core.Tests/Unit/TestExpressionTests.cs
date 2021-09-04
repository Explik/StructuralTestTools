using Microsoft.VisualStudio.TestTools.UnitTesting;
using OleVanSanten.TestTools.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using static TestTools_Tests.TestHelper;

namespace TestTools_Tests.Unit
{
    [TestClass]
    public class TestExpressionTests
    {
        [TestMethod("Expr with equal arguments returns equal TestExpressions"), TestCategory("Constructor Methods")]
        public void Expr_WithSameParameter_AreEqual()
        {
            var expr1 = TestExpression.Expr(() => 5);
            var expr2 = TestExpression.Expr(() => 5);

            AssertAreEqualExpressions(expr1, expr2);
        }

        [TestMethod("Expr inserts 1 variable into TestExpression"), TestCategory("Constructor Methods")]
        public void Expr_Inserts1VariableIntoTestExpression()
        {
            var expression = Expression.Lambda<Action>(Expression.Parameter(typeof(int), "a"));
            var expected = TestExpression.Expr(expression);

            var variable = TestExpression.Var<int>("a");
            var actual = TestExpression.Expr(variable, a => a);

            AssertAreEqualExpressions(expected, actual);
        }

        [TestMethod("Expr inserts 2 variable into TestExpression"), TestCategory("Constructor Methods")]
        public void Expr_Inserts2VariablesIntoTestExpression()
        {
            var addition = Expression.Add(
                Expression.Parameter(typeof(int), "a"),
                Expression.Parameter(typeof(int), "b"));
            var expression = Expression.Lambda<Action>(addition);
            var expected = TestExpression.Expr(expression);

            var variable1 = TestExpression.Var<int>("a");
            var variable2 = TestExpression.Var<int>("b");
            var actual = TestExpression.Expr(variable1, variable2, (a, b) => a + b);

            AssertAreEqualExpressions(expected, actual);
        }

        [TestMethod("Expr inserts 3 variable into TestExpression"), TestCategory("Constructor Methods")]
        public void Expr_Inserts3VariablesIntoTestExpression()
        {
            var addition1 = Expression.Add(
                Expression.Parameter(typeof(int), "a"),
                Expression.Parameter(typeof(int), "b"));
            var addition2 = Expression.Add(
                addition1,
                Expression.Parameter(typeof(int), "c"));
            var expression = Expression.Lambda<Action>(addition2);
            var expected = TestExpression.Expr(expression);

            var variable1 = TestExpression.Var<int>("a");
            var variable2 = TestExpression.Var<int>("b");
            var variable3 = TestExpression.Var<int>("c");
            var actual = TestExpression.Expr(variable1, variable2, variable3, (a, b, c) => a + b + c);

            AssertAreEqualExpressions(expected, actual);
        }

        [TestMethod("Const with equal arguments returns equal TestExpressions"), TestCategory("Constructor Methods")]
        public void Const_WithEqualArguments_ReturnsEqualTestExpressions()
        {
            var expr1 = TestExpression.Const(5);
            var expr2 = TestExpression.Const(5);

            AssertAreEqualExpressions(expr1, expr2);
        }

        [TestMethod("Const(v) returns equal to Expr(() => v)"), TestCategory("Constructor Methods")]
        public void Const_MayReturnEqualToExpr()
        {
            var expected = TestExpression.Expr(() => 5);
            var actual = TestExpression.Const(5);

            AssertAreEqualExpressions(expected, actual);
        }

        [TestMethod("Lambda with equal arguments returns equal TestExpressions"), TestCategory("Constructor Methods")]
        public void Var_WithEqualArguments_ReturnsEqualTestExpressions()
        {
            var expr1 = TestExpression.Var<int>("n");
            var expr2 = TestExpression.Var<int>("n");

            AssertAreEqualExpressions(expr1, expr2);
        }

        [TestMethod("Lambda(TestExpression) correctly creates lambda expression"), TestCategory("Constructor Methods")]
        public void Lambda1_CorrectlyCreatesLambdaExpression()
        {
            var constant = Expression.Constant(5);
            var body = Expression.Lambda<Action>(constant);
            var expected = new TestExpression<Action>(body);

            var actual = TestExpression.Lambda((TestExpression)TestExpression.Const(5));

            AssertAreEqualExpressions(expected, actual);
        }

        [TestMethod("Lambda(TestExpression<T>) correctly creates lambda expression"), TestCategory("Constructor Methods")]
        public void Lambda2_CorrectlyCreatesLambdaExpression()
        {
            var constant = Expression.Constant(5);
            var body = Expression.Lambda<Func<int>>(constant);
            var expected = new TestExpression<Func<int>>(body);

            var actual = TestExpression.Lambda(TestExpression.Const(5));

            AssertAreEqualExpressions(expected, actual);
        }

        [TestMethod("Lambda(Expression<Func<T1, TestExpression<T>>>) correctly creates lambda expression"), TestCategory("Constructor Methods")]
        public void Lambda3_CorrectlyCreatesLambdaExpression()
        {
            var constant = Expression.Parameter(typeof(int), "variable");
            var parameter = Expression.Parameter(typeof(int), "parameter");
            var addition = Expression.Add(parameter, constant);
            var body = Expression.Lambda<Action<int>>(addition, parameter);
            var expected = new TestExpression<Action<int>>(body);

            var variable = TestExpression.Var<int>("variable");
            var actual = TestExpression.Lambda<int>(parameter => (TestExpression)TestExpression.Expr(variable, b => parameter + b));

            AssertAreEqualExpressions(expected, actual);
        }

        [TestMethod("Lambda(Expression<Func<T1, TestExpression>>) correctly creates lambda expression"), TestCategory("Constructor Methods")]
        public void Lambda4_CorrectlyCreatesLambdaExpression()
        {
            var constant = Expression.Parameter(typeof(int), "variable");
            var parameter = Expression.Parameter(typeof(int), "parameter");
            var addition = Expression.Add(parameter, constant);
            var body = Expression.Lambda<Func<int, int>>(addition, parameter);
            var expected = new TestExpression<Func<int, int>>(body);

            var variable = TestExpression.Var<int>("variable");
            var actual = TestExpression.Lambda<int, int>(parameter => TestExpression.Expr(variable, b => parameter + b));

            AssertAreEqualExpressions(expected, actual);
        }

        [TestMethod("Lambda(Expression<Func<T1, T2, TestExpression>>) correctly creates lambda expression"), TestCategory("Constructor Methods")]
        public void Lambda5_CorrectlyCreatesLambdaExpression()
        {
            var constant = Expression.Parameter(typeof(int), "variable");
            var parameter1 = Expression.Parameter(typeof(int), "parameter1");
            var parameter2 = Expression.Parameter(typeof(int), "parameter2");
            var addition1 = Expression.Add(parameter1, parameter2);
            var addition2 = Expression.Add(addition1, constant);
            var body = Expression.Lambda<Action<int, int>>(addition2, parameter1, parameter2);
            var expected = new TestExpression<Action<int, int>>(body);

            var variable = TestExpression.Var<int>("variable");
            var actual = TestExpression.Lambda<int, int>((parameter1, parameter2) => (TestExpression)TestExpression.Expr(variable, b => parameter1 + parameter2 + b));

            AssertAreEqualExpressions(expected, actual);
        }

        [TestMethod("Lambda(Expression<Func<T1, T2, TestExpression<T>>>) correctly creates lambda expression"), TestCategory("Constructor Methods")]
        public void Lambda6_CorrectlyCreatesLambdaExpression()
        {
            var constant = Expression.Parameter(typeof(int), "variable");
            var parameter1 = Expression.Parameter(typeof(int), "parameter1");
            var parameter2 = Expression.Parameter(typeof(int), "parameter2");
            var addition1 = Expression.Add(parameter1, parameter2);
            var addition2 = Expression.Add(addition1, constant);
            var body = Expression.Lambda<Func<int, int, int>>(addition2, parameter1, parameter2);
            var expected = new TestExpression<Func<int, int, int>>(body);

            var variable = TestExpression.Var<int>("variable");
            var actual = TestExpression.Lambda<int, int, int>((parameter1, parameter2) => TestExpression.Expr(variable, b => parameter1 + parameter2 + b));

            AssertAreEqualExpressions(expected, actual);
        }

        [TestMethod("Lambda(Expression<Func<T1, T2, T3, TestExpression>>) correctly creates lambda expression"), TestCategory("Constructor Methods")]
        public void Lambda7_CorrectlyCreatesLambdaExpression()
        {
            var constant = Expression.Parameter(typeof(int), "variable");
            var parameter1 = Expression.Parameter(typeof(int), "parameter1");
            var parameter2 = Expression.Parameter(typeof(int), "parameter2");
            var parameter3 = Expression.Parameter(typeof(int), "parameter3");
            var addition1 = Expression.Add(parameter1, parameter2);
            var addition2 = Expression.Add(addition1, parameter3);
            var addition3 = Expression.Add(addition2, constant);
            var body = Expression.Lambda<Action<int, int, int>>(addition3, parameter1, parameter2, parameter3);
            var expected = new TestExpression<Action<int, int, int>>(body);

            var variable = TestExpression.Var<int>("variable");
            var actual = TestExpression.Lambda<int, int, int>((parameter1, parameter2, parameter3) => (TestExpression)TestExpression.Expr(variable, b => parameter1 + parameter2 + parameter3 + b));

            AssertAreEqualExpressions(expected, actual);
        }

        [TestMethod("Lambda(Expression<Func<T1, T2, T3, TestExpression<T>>>) correctly creates lambda expression"), TestCategory("Constructor Methods")]
        public void Lambda8_CorrectlyCreatesLambdaExpression()
        {
            var constant = Expression.Parameter(typeof(int), "variable");
            var parameter1 = Expression.Parameter(typeof(int), "parameter1");
            var parameter2 = Expression.Parameter(typeof(int), "parameter2");
            var parameter3 = Expression.Parameter(typeof(int), "parameter3");
            var addition1 = Expression.Add(parameter1, parameter2);
            var addition2 = Expression.Add(addition1, parameter3);
            var addition3 = Expression.Add(addition2, constant);
            var body = Expression.Lambda<Func<int, int, int, int>>(addition3, parameter1, parameter2, parameter3);
            var expected = new TestExpression<Func<int, int, int, int>>(body);

            var variable = TestExpression.Var<int>("variable");
            var actual = TestExpression.Lambda<int, int, int, int>((parameter1, parameter2, parameter3) => TestExpression.Expr(variable, b => parameter1 + parameter2 + parameter3 + b));

            AssertAreEqualExpressions(expected, actual);
        }
    }
}
