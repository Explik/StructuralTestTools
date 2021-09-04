using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using OleVanSanten.TestTools.Expressions;

namespace OleVanSanten.TestTools.Expressions
{
    // The TestExpression classes encapsulate System.Linq.Expression, and provide
    // type-safe manipulation. This type-safety is needed in order to avoid
    // flawed test code.

    // TestExpression represents a void type expression
    // TestExpression<T> represents a T type expression'

    // TODO create a more generic solution for Lambda functions to remove code dublication
    public class TestExpression
    {
        public Expression Expression { get; }

        public TestExpression(Expression expression)
        {
            Expression = expression;
        }

        public static TestExpression<T> Const<T>(T value)
        {
            return new TestExpression<T>(Expression.Constant(value));
        }

        public static TestExpression Expr(Expression<Action> expression)
        {
            return new TestExpression(expression.Body);
        }

        public static TestExpression Expr<T1>(TestExpression<T1> var, Expression<Action<T1>> expression)
        {
            ParameterExpression parameter = expression.Parameters[0];
            ReplacerVisitor replacer = new ReplacerVisitor(parameter, var.Expression);

            Expression rewrittenExpression = replacer.Visit(expression.Body);

            return new TestExpression(rewrittenExpression);
        }

        public static TestExpression Expr<T1, T2>(TestExpression<T1> var1, TestExpression<T2> var2, Expression<Action<T1, T2>> expression)
        {
            ParameterExpression parameter1 = expression.Parameters[0];
            ParameterExpression parameter2 = expression.Parameters[1];

            ReplacerVisitor replacer1 = new ReplacerVisitor(parameter1, var1.Expression);
            ReplacerVisitor replacer2 = new ReplacerVisitor(parameter2, var2.Expression);

            Expression rewrittenExpression = replacer2.Visit(replacer1.Visit(expression.Body));

            return new TestExpression(rewrittenExpression);
        }

        public static TestExpression Expr<T1, T2, T3>(TestExpression<T1> var1, TestExpression<T2> var2, TestExpression<T3> var3, Expression<Action<T1, T2, T3>> expression)
        {
            ParameterExpression parameter1 = expression.Parameters[0];
            ParameterExpression parameter2 = expression.Parameters[1];
            ParameterExpression parameter3 = expression.Parameters[2];

            ReplacerVisitor replacer1 = new ReplacerVisitor(parameter1, var1.Expression);
            ReplacerVisitor replacer2 = new ReplacerVisitor(parameter2, var2.Expression);
            ReplacerVisitor replacer3 = new ReplacerVisitor(parameter3, var3.Expression);

            Expression rewrittenExpression = replacer3.Visit(replacer2.Visit(replacer1.Visit(expression.Body)));

            return new TestExpression(rewrittenExpression);
        }

        public static TestExpression Expr<T1, T2, T3, T4>(TestExpression<T1> var1, TestExpression<T2> var2, TestExpression<T3> var3, TestExpression<T4> var4, Expression<Action<T1, T2, T3, T4>> expression)
        {
            ParameterExpression parameter1 = expression.Parameters[0];
            ParameterExpression parameter2 = expression.Parameters[1];
            ParameterExpression parameter3 = expression.Parameters[2];
            ParameterExpression parameter4 = expression.Parameters[3];

            ReplacerVisitor replacer1 = new ReplacerVisitor(parameter1, var1.Expression);
            ReplacerVisitor replacer2 = new ReplacerVisitor(parameter2, var2.Expression);
            ReplacerVisitor replacer3 = new ReplacerVisitor(parameter3, var3.Expression);
            ReplacerVisitor replacer4 = new ReplacerVisitor(parameter4, var4.Expression);

            Expression rewrittenExpression = replacer4.Visit(replacer3.Visit(replacer2.Visit(replacer1.Visit(expression.Body))));

            return new TestExpression(rewrittenExpression);
        }

        public static TestExpression Expr<T1, T2, T3, T4, T5>(TestExpression<T1> var1, TestExpression<T2> var2, TestExpression<T3> var3, TestExpression<T4> var4, TestExpression<T5> var5, Expression<Action<T1, T2, T3, T4, T5>> expression)
        {
            ParameterExpression parameter1 = expression.Parameters[0];
            ParameterExpression parameter2 = expression.Parameters[1];
            ParameterExpression parameter3 = expression.Parameters[2];
            ParameterExpression parameter4 = expression.Parameters[3];
            ParameterExpression parameter5 = expression.Parameters[4];

            ReplacerVisitor replacer1 = new ReplacerVisitor(parameter1, var1.Expression);
            ReplacerVisitor replacer2 = new ReplacerVisitor(parameter2, var2.Expression);
            ReplacerVisitor replacer3 = new ReplacerVisitor(parameter3, var3.Expression);
            ReplacerVisitor replacer4 = new ReplacerVisitor(parameter4, var4.Expression);
            ReplacerVisitor replacer5 = new ReplacerVisitor(parameter5, var5.Expression);

            Expression rewrittenExpression = replacer5.Visit(replacer4.Visit(replacer4.Visit(replacer3.Visit(replacer2.Visit(replacer1.Visit(expression.Body))))));

            return new TestExpression(rewrittenExpression);
        }

        public static TestExpression Expr<T1, T2, T3, T4, T5, T6>(TestExpression<T1> var1, TestExpression<T2> var2, TestExpression<T3> var3, TestExpression<T4> var4, TestExpression<T5> var5, TestExpression<T6> var6, Expression<Action<T1, T2, T3, T4, T5, T6>> expression)
        {
            ParameterExpression parameter1 = expression.Parameters[0];
            ParameterExpression parameter2 = expression.Parameters[1];
            ParameterExpression parameter3 = expression.Parameters[2];
            ParameterExpression parameter4 = expression.Parameters[3];
            ParameterExpression parameter5 = expression.Parameters[4];
            ParameterExpression parameter6 = expression.Parameters[5];

            ReplacerVisitor replacer1 = new ReplacerVisitor(parameter1, var1.Expression);
            ReplacerVisitor replacer2 = new ReplacerVisitor(parameter2, var2.Expression);
            ReplacerVisitor replacer3 = new ReplacerVisitor(parameter3, var3.Expression);
            ReplacerVisitor replacer4 = new ReplacerVisitor(parameter4, var4.Expression);
            ReplacerVisitor replacer5 = new ReplacerVisitor(parameter5, var5.Expression);
            ReplacerVisitor replacer6 = new ReplacerVisitor(parameter6, var6.Expression);

            Expression rewrittenExpression = replacer6.Visit(replacer5.Visit(replacer4.Visit(replacer4.Visit(replacer3.Visit(replacer2.Visit(replacer1.Visit(expression.Body)))))));

            return new TestExpression(rewrittenExpression);
        }

        public static TestExpression<T> Expr<T>(Expression<Func<T>> expression)
        {
            return new TestExpression<T>(expression.Body);
        }

        public static TestExpression<T> Expr<T1, T>(TestExpression<T1> var, Expression<Func<T1, T>> expression)
        {
            ParameterExpression parameter = expression.Parameters[0];
            ReplacerVisitor replacer = new ReplacerVisitor(parameter, var.Expression);

            Expression rewrittenExpression = replacer.Visit(expression.Body);

            return new TestExpression<T>(rewrittenExpression);
        }

        public static TestExpression<T> Expr<T1, T2, T>(TestExpression<T1> var1, TestExpression<T2> var2, Expression<Func<T1, T2, T>> expression)
        {
            ParameterExpression parameter1 = expression.Parameters[0];
            ParameterExpression parameter2 = expression.Parameters[1];

            ReplacerVisitor replacer1 = new ReplacerVisitor(parameter1, var1.Expression);
            ReplacerVisitor replacer2 = new ReplacerVisitor(parameter2, var2.Expression);

            Expression rewrittenExpression = replacer2.Visit(replacer1.Visit(expression.Body));

            return new TestExpression<T>(rewrittenExpression);
        }

        public static TestExpression<T> Expr<T1, T2, T3, T>(TestExpression<T1> var1, TestExpression<T2> var2, TestExpression<T3> var3, Expression<Func<T1, T2, T3, T>> expression)
        {
            ParameterExpression parameter1 = expression.Parameters[0];
            ParameterExpression parameter2 = expression.Parameters[1];
            ParameterExpression parameter3 = expression.Parameters[2];

            ReplacerVisitor replacer1 = new ReplacerVisitor(parameter1, var1.Expression);
            ReplacerVisitor replacer2 = new ReplacerVisitor(parameter2, var2.Expression);
            ReplacerVisitor replacer3 = new ReplacerVisitor(parameter3, var3.Expression);

            Expression rewrittenExpression = replacer3.Visit(replacer2.Visit(replacer1.Visit(expression.Body)));

            return new TestExpression<T>(rewrittenExpression);
        }

        public static TestExpression<Action> Lambda(TestExpression expression)
        {
            Expression<Action> rewrittenExpression = Expression.Lambda<Action>(expression.Expression);

            return new TestExpression<Action>(rewrittenExpression);
        }

        public static TestExpression<Action<T1>> Lambda<T1>(Expression<Func<T1, TestExpression>> expression)
        {
            /* In order to access the TestExpression value, the expression needs
             * to be evaluated and this requires that no unused parameters are 
             * present at the evaluation time as this results in an error. 
             * So to avoid this, the unused parameters are replaced with an default
             * value, then everything is evaluated, and the default value is 
             * replaced back to a parameter
             */
            UnaryExpression constant1 = Expression.Convert(Expression.Constant(default(T1)), typeof(T1));

            ReplacerVisitor toReplacer = new ReplacerVisitor(expression.Parameters[0], constant1);
            ReplacerVisitor backReplacer = new ReplacerVisitor(constant1, expression.Parameters[0]);

            Expression body1 = toReplacer.Visit(expression.Body);
            Expression<Func<TestExpression>> lambda1 = Expression.Lambda<Func<TestExpression>>(body1);
            TestExpression testExpression = lambda1.Compile().Invoke();

            Expression body2 = backReplacer.Visit(testExpression.Expression);
            Expression<Action<T1>> lambda2 = Expression.Lambda<Action<T1>>(body2, expression.Parameters[0]);

            return new TestExpression<Action<T1>>(lambda2);
        }

        public static TestExpression<Action<T1, T2>> Lambda<T1, T2>(Expression<Func<T1, T2, TestExpression>> expression)
        {
            /* In order to access the TestExpression value, the expression needs
             * to be evaluated and this requires that no unused parameters are 
             * present at the evaluation time as this results in an error. 
             * So to avoid this, the unused parameters are replaced with an default
             * value, then everything is evaluated, and the default value is 
             * replaced back to a parameter
             */
            UnaryExpression constant1 = Expression.Convert(Expression.Constant(default(T1)), typeof(T1));
            UnaryExpression constant2 = Expression.Convert(Expression.Constant(default(T2)), typeof(T2));

            ReplacerVisitor toReplacer1 = new ReplacerVisitor(expression.Parameters[0], constant1);
            ReplacerVisitor toReplacer2 = new ReplacerVisitor(expression.Parameters[1], constant2);
            ReplacerVisitor backReplacer1 = new ReplacerVisitor(constant1, expression.Parameters[0]);
            ReplacerVisitor backReplacer2 = new ReplacerVisitor(constant2, expression.Parameters[1]);

            Expression body1 = toReplacer2.Visit(toReplacer1.Visit(expression.Body));
            Expression<Func<TestExpression>> lambda1 = Expression.Lambda<Func<TestExpression>>(body1);
            TestExpression testExpression = lambda1.Compile().Invoke();

            Expression body2 = backReplacer2.Visit(backReplacer1.Visit(testExpression.Expression));
            Expression<Action<T1, T2>> lambda2 = Expression.Lambda<Action<T1, T2>>(body2, expression.Parameters[0], expression.Parameters[1]);

            return new TestExpression<Action<T1, T2>>(lambda2);
        }

        public static TestExpression<Action<T1, T2, T3>> Lambda<T1, T2, T3>(Expression<Func<T1, T2, T3, TestExpression>> expression)
        {
            /* In order to access the TestExpression value, the expression needs
             * to be evaluated and this requires that no unused parameters are 
             * present at the evaluation time as this results in an error. 
             * So to avoid this, the unused parameters are replaced with an default
             * value, then everything is evaluated, and the default value is 
             * replaced back to a parameter
             */
            UnaryExpression constant1 = Expression.Convert(Expression.Constant(default(T1)), typeof(T1));
            UnaryExpression constant2 = Expression.Convert(Expression.Constant(default(T2)), typeof(T2));
            UnaryExpression constant3 = Expression.Convert(Expression.Constant(default(T3)), typeof(T3));

            ReplacerVisitor toReplacer1 = new ReplacerVisitor(expression.Parameters[0], constant1);
            ReplacerVisitor toReplacer2 = new ReplacerVisitor(expression.Parameters[1], constant2);
            ReplacerVisitor toReplacer3 = new ReplacerVisitor(expression.Parameters[2], constant3);
            ReplacerVisitor backReplacer1 = new ReplacerVisitor(constant1, expression.Parameters[0]);
            ReplacerVisitor backReplacer2 = new ReplacerVisitor(constant2, expression.Parameters[1]);
            ReplacerVisitor backReplacer3 = new ReplacerVisitor(constant3, expression.Parameters[2]);

            Expression body1 = toReplacer3.Visit(toReplacer2.Visit(toReplacer1.Visit(expression.Body)));
            Expression<Func<TestExpression>> lambda1 = Expression.Lambda<Func<TestExpression>>(body1);
            TestExpression testExpression = lambda1.Compile().Invoke();

            Expression body2 = backReplacer3.Visit(backReplacer2.Visit(backReplacer1.Visit(testExpression.Expression)));
            Expression<Action<T1, T2, T3>> lambda2 = Expression.Lambda<Action<T1, T2, T3>>(body2, expression.Parameters[0], expression.Parameters[1], expression.Parameters[2]);

            return new TestExpression<Action<T1, T2, T3>>(lambda2);
        }

        public static TestExpression<Func<T>> Lambda<T>(TestExpression<T> expression)
        {
            Expression<Func<T>> rewrittenExpression = Expression.Lambda<Func<T>>(expression.Expression);

            return new TestExpression<Func<T>>(rewrittenExpression);
        }

        public static TestExpression<Func<T1, T>> Lambda<T1, T>(Expression<Func<T1, TestExpression<T>>> expression)
        {
            /* In order to access the TestExpression value, the expression needs
             * to be evaluated and this requires that no unused parameters are 
             * present at the evaluation time as this results in an error. 
             * So to avoid this, the unused parameters are replaced with an default
             * value, then everything is evaluated, and the default value is 
             * replaced back to a parameter
             */
            UnaryExpression constant1 = Expression.Convert(Expression.Constant(default(T1)), typeof(T1));

            ReplacerVisitor toReplacer = new ReplacerVisitor(expression.Parameters[0], constant1);
            ReplacerVisitor backReplacer = new ReplacerVisitor(constant1, expression.Parameters[0]);

            Expression body1 = toReplacer.Visit(expression.Body);
            Expression<Func<TestExpression<T>>> lambda1 = Expression.Lambda<Func<TestExpression<T>>>(body1);
            TestExpression<T> testExpression = lambda1.Compile().Invoke();

            Expression body2 = backReplacer.Visit(testExpression.Expression);
            Expression<Func<T1, T>> lambda2 = Expression.Lambda<Func<T1, T>>(body2, expression.Parameters[0]);

            return new TestExpression<Func<T1, T>>(lambda2);
        }

        public static TestExpression<Func<T1, T2, T>> Lambda<T1, T2, T>(Expression<Func<T1, T2, TestExpression<T>>> expression)
        {
            /* In order to access the TestExpression value, the expression needs
             * to be evaluated and this requires that no unused parameters are 
             * present at the evaluation time as this results in an error. 
             * So to avoid this, the unused parameters are replaced with an default
             * value, then everything is evaluated, and the default value is 
             * replaced back to a parameter
             */
            UnaryExpression constant1 = Expression.Convert(Expression.Constant(default(T1)), typeof(T1));
            UnaryExpression constant2 = Expression.Convert(Expression.Constant(default(T2)), typeof(T2));

            ReplacerVisitor toReplacer1 = new ReplacerVisitor(expression.Parameters[0], constant1);
            ReplacerVisitor toReplacer2 = new ReplacerVisitor(expression.Parameters[1], constant2);
            ReplacerVisitor backReplacer1 = new ReplacerVisitor(constant1, expression.Parameters[0]);
            ReplacerVisitor backReplacer2 = new ReplacerVisitor(constant2, expression.Parameters[1]);

            Expression body1 = toReplacer2.Visit(toReplacer1.Visit(expression.Body));
            Expression<Func<TestExpression<T>>> lambda1 = Expression.Lambda<Func<TestExpression<T>>>(body1);
            TestExpression<T> testExpression = lambda1.Compile().Invoke();

            Expression body2 = backReplacer2.Visit(backReplacer1.Visit(testExpression.Expression));
            Expression<Func<T1, T2, T>> lambda2 = Expression.Lambda<Func<T1, T2, T>>(body2, expression.Parameters[0], expression.Parameters[1]);

            return new TestExpression<Func<T1, T2, T>>(lambda2);
        }

        public static TestExpression<Func<T1, T2, T3, T>> Lambda<T1, T2, T3, T>(Expression<Func<T1, T2, T3, TestExpression<T>>> expression)
        {
            /* In order to access the TestExpression value, the expression needs
             * to be evaluated and this requires that no unused parameters are 
             * present at the evaluation time as this results in an error. 
             * So to avoid this, the unused parameters are replaced with an default
             * value, then everything is evaluated, and the default value is 
             * replaced back to a parameter
             */
            UnaryExpression constant1 = Expression.Convert(Expression.Constant(default(T1)), typeof(T1));
            UnaryExpression constant2 = Expression.Convert(Expression.Constant(default(T2)), typeof(T2));
            UnaryExpression constant3 = Expression.Convert(Expression.Constant(default(T3)), typeof(T3));

            ReplacerVisitor toReplacer1 = new ReplacerVisitor(expression.Parameters[0], constant1);
            ReplacerVisitor toReplacer2 = new ReplacerVisitor(expression.Parameters[1], constant2);
            ReplacerVisitor toReplacer3 = new ReplacerVisitor(expression.Parameters[2], constant3);
            ReplacerVisitor backReplacer1 = new ReplacerVisitor(constant1, expression.Parameters[0]);
            ReplacerVisitor backReplacer2 = new ReplacerVisitor(constant2, expression.Parameters[1]);
            ReplacerVisitor backReplacer3 = new ReplacerVisitor(constant3, expression.Parameters[2]);

            Expression body1 = toReplacer3.Visit(toReplacer2.Visit(toReplacer1.Visit(expression.Body)));
            Expression<Func<TestExpression<T>>> lambda1 = Expression.Lambda<Func<TestExpression<T>>>(body1);
            TestExpression<T> testExpression = lambda1.Compile().Invoke();

            Expression body2 = backReplacer3.Visit(backReplacer2.Visit(backReplacer1.Visit(testExpression.Expression)));
            Expression<Func<T1, T2, T3, T>> lambda2 = Expression.Lambda<Func<T1, T2, T3, T>>(body2, expression.Parameters[0], expression.Parameters[1], expression.Parameters[2]);

            return new TestExpression<Func<T1, T2, T3, T>>(lambda2);
        }

        public static TestVariable<T> Var<T>()
        {
            ParameterExpression expression = Expression.Parameter(typeof(T));
            return new TestVariable<T>(expression);
        }

        public static TestVariable<T> Var<T>(string name)
        {
            ParameterExpression expression = Expression.Parameter(typeof(T), name);
            return new TestVariable<T>(expression);
        }

        public static TestExpression<T> Assign<T>(TestExpression<T> left, TestExpression<T> right)
        {
            Expression assignment = Expression.Assign(left.Expression, right.Expression);
            return new TestExpression<T>(assignment);
        }

        private class ReplacerVisitor : ExpressionVisitor
        {
            Expression _oldValue;
            Expression _newValue;

            public ReplacerVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                {
                    return _newValue;
                }
                return base.Visit(node);
            }
        }
    }

    public class TestExpression<T> : TestExpression
    {
        public TestExpression(Expression expression) : base(expression)
        {
            if (expression.Type != typeof(T))
                throw new ArgumentException($"expression.Type is not {typeof(T).Name}");
        }
    }
}
