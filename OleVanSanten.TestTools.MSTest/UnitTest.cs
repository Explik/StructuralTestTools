using OleVanSanten.TestTools.Expressions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using OleVanSanten.TestTools.Structure;
using OleVanSanten.TestTools.Syntax;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace OleVanSanten.TestTools.MSTest
{
    public class UnitTest
    {
        List<ParameterExpression> _variables = new List<ParameterExpression>();

        List<Expression> _expressions = new List<Expression>();

        public AssertObject Assert { get; } 

        public CollectionAssertObject CollectionAssert { get; }

        public DelegateAssertObject DelegateAssert { get; } 

        public ConsoleAssertObject ConsoleAssert { get; }

        public TypeVisitor TypeVisitor { get; set; }

        public SyntaxVisitor SyntaxVisitor { get; set; }

        internal UnitTest(IStructureService structureService)
        {
            TypeVisitor = new TypeVisitor(structureService);
            SyntaxVisitor = new SyntaxVisitor();

            Assert = new AssertObject(this);
            CollectionAssert = new CollectionAssertObject(this);
            DelegateAssert = new DelegateAssertObject(this);
            ConsoleAssert = new ConsoleAssertObject(this);
        }

        public TestVariable<T> CreateVariable<T>()
        {
            return CreateVariable<T>(typeof(T).Name.ToLower());
        }

        public TestVariable<T> CreateVariable<T>(string nickname)
        {
            Random random = new Random();

            ParameterExpression variable = Expression.Variable(typeof(T), random.Next().ToString());

            _variables.Add(variable);

            return new TestVariable<T>(variable);
        }

        public void Arrange<T>(TestVariable<T> variable)
        {
            Arrange(variable, TestExpression.Expr(() => default(T)));
        }

        public void Arrange<T>(TestVariable<T> variable, TestExpression<T> initialization)
        {
            Expression assignment = Expression.Assign(variable.Expression, initialization.Expression);
            
            _expressions.Add(assignment);
        }

        public void Act(TestExpression action)
        {
            _expressions.Add(action.Expression);
        }

        public void Execute()
        {
            BlockExpression block1 = Expression.Block(_variables, _expressions);

            BlockExpression block2 = (BlockExpression)SyntaxVisitor.Visit(block1);

            BlockExpression block3 = (BlockExpression)TypeVisitor.Visit(block2);

            Expression.Lambda<Action>(block3).Compile().Invoke();

            // Delegate.Verify is called everytime to avoid having to call Verify explicitly everytime
            OleVanSanten.TestTools.MSTest.DelegateAssert.Verify();
        }

        // Based on Microsoft.VisualStudio.TestTools.UnitTesting.Assert
        public class AssertObject
        {
            UnitTest _test;

            public AssertObject(UnitTest test)
            {
                _test = test;
            }

            #region AreEqual Overloads
            public void AreEqual<T>(TestExpression<T> expected, TestExpression<T> actual)
            {
                TestExpression action = Expr(
                    expected, 
                    actual, 
                    (e, a) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(e, a));
                
                _test.Act(action);
            }

            public void AreEqual<T>(TestExpression<T> expected, TestExpression<T> actual, string message)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(message),
                    (e, a, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(e, a, m));

                _test.Act(action);
            }

            public void AreEqual<T>(TestExpression<T> expected, TestExpression<T> actual, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(message),
                    Const(parameters),
                    (e, a, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(e, a, m, p));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<object> expected, TestExpression<object> actual)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    (e, a) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(e, a));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<object> expected, TestExpression<object> actual, string message)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(message),
                    (e, a, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(e, a, m));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<object> expected, TestExpression<object> actual, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(message),
                    Const(parameters),
                    (e, a, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(e, a, m, p));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<float> expected, TestExpression<float> actual, float delta)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(delta),
                    (e, a, d) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(e, a, d));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<float> expected, TestExpression<float> actual, float delta, string message)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(delta),
                    Const(message),
                    (e, a, d, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(e, a, d, m));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<float> expected, TestExpression<float> actual, float delta, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(delta),
                    Const(message),
                    Const(parameters),
                    (e, a, d, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(e, a, d, m, p));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<double> expected, TestExpression<double> actual, double delta)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(delta),
                    (e, a, d) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(e, a, d));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<double> expected, TestExpression<double> actual, double delta, string message)
            {
                TestExpression action = Expr(
                    expected, 
                    actual,
                    Const(delta), 
                    Const(message),
                    (e, a, d, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(e, a, d, m));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<double> expected, TestExpression<double> actual, double delta, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(delta),
                    Const(message),
                    Const(parameters),
                    (e, a, d, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(e, a, d, m, p));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<string> expected, TestExpression<string> actual, bool ignoreCase)
            {
                TestExpression action = Expr(
                    expected, 
                    actual, 
                    Const(ignoreCase), 
                    (e, a, i) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(e, a, i));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<string> expected, TestExpression<string> actual, bool ignoreCase, string message)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(ignoreCase),
                    Const(message),
                    (e, a, i, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(e, a, i, m));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<string> expected, TestExpression<string> actual, bool ignoreCase, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    expected, 
                    actual, 
                    Const(ignoreCase), 
                    Const(message), 
                    Const(parameters), 
                    (e, a, i, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(e, a, i, m, p));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<string> expected, TestExpression<string> actual, bool ignoreCase, CultureInfo culture)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(ignoreCase),
                    Const(culture),
                    (e, a, i, c) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(e, a, i, c));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<string> expected, TestExpression<string> actual, bool ignoreCase, CultureInfo culture, string message)
            {
                TestExpression action = Expr(
                    expected, 
                    actual, 
                    Const(ignoreCase), 
                    Const(culture), 
                    Const(message), 
                    (e, a, i, c, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(e, a, i, c, m));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<string> expected, TestExpression<string> actual, bool ignoreCase, CultureInfo culture, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(ignoreCase),
                    Const(culture),
                    Const(message),
                    Const(parameters),
                    (e, a, i, c, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(e, a, i, c, m, p));

                _test.Act(action);
            }
            #endregion

            #region AreNotEqual Overloads
            public void AreNotEqual<T>(TestExpression<T> notExpected, TestExpression<T> actual)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    (e, a) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(e, a));

                _test.Act(action);
            }

            public void AreNotEqual<T>(TestExpression<T> notExpected, TestExpression<T> actual, string message)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(message),
                    (e, a, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(e, a, m));

                _test.Act(action);
            }

            public void AreNotEqual<T>(TestExpression<T> notExpected, TestExpression<T> actual, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(message),
                    Const(parameters),
                    (e, a, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(e, a, m, p));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<object> notExpected, TestExpression<object> actual)
            {
                TestExpression action = Expr(
                    notExpected, 
                    actual, 
                    (e, a) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(e, a));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<object> notExpected, TestExpression<object> actual, string message)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(message),
                    (e, a, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(e, a, m));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<object> notExpected, TestExpression<object> actual, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                   notExpected,
                   actual,
                   Const(message),
                   Const(parameters),
                   (e, a, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(e, a, m, p));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<float> notExpected, TestExpression<float> actual, float delta)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(delta),
                    (e, a, d) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(e, a, d));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<float> notExpected, TestExpression<float> actual, float delta, string message)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(delta),
                    Const(message),
                    (e, a, d, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(e, a, d, m));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<float> notExpected, TestExpression<float> actual, float delta, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(delta),
                    Const(message),
                    Const(parameters),
                    (e, a, d, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(e, a, d, m, p));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<double> notExpected, TestExpression<double> actual, double delta)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(delta),
                    (e, a, d) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(e, a, d));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<double> notExpected, TestExpression<double> actual, double delta, string message)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(delta),
                    Const(message),
                    (e, a, d, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(e, a, d, m));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<double> notExpected, TestExpression<double> actual, double delta, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(delta),
                    Const(message),
                    Const(parameters),
                    (e, a, d, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(e, a, d, m, p));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<string> notExpected, TestExpression<string> actual, bool ignoreCase)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(ignoreCase),
                    (e, a, i) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(e, a, i));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<string> notExpected, TestExpression<string> actual, bool ignoreCase, string message)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(ignoreCase),
                    Const(message),
                    (e, a, i, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(e, a, i, m));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<string> notExpected, TestExpression<string> actual, bool ignoreCase, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(ignoreCase),
                    Const(message),
                    Const(parameters),
                    (e, a, i, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(e, a, i, m, p));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<string> notExpected, TestExpression<string> actual, bool ignoreCase, CultureInfo culture)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(ignoreCase),
                    Const(culture),
                    (e, a, i, c) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(e, a, i, c));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<string> notExpected, TestExpression<string> actual, bool ignoreCase, CultureInfo culture, string message)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(ignoreCase),
                    Const(culture),
                    Const(message),
                    (e, a, i, c, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(e, a, i, c, m));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<string> notExpected, TestExpression<string> actual, bool ignoreCase, CultureInfo culture, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(ignoreCase),
                    Const(culture),
                    Const(message),
                    Const(parameters),
                    (e, a, i, c, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(e, a, i, c, m, p));

                _test.Act(action);
            }
            #endregion

            #region AreSame Overloads
            public void AreSame<T>(TestExpression<T> expected, TestExpression<T> actual)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    (e, a) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreSame(e, a));

                _test.Act(action);
            }

            public void AreSame<T>(TestExpression<T> expected, TestExpression<T> actual, string message)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(message),
                    (e, a, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreSame(e, a, m));

                _test.Act(action);
            }

            public void AreSame<T>(TestExpression<T> expected, TestExpression<T> actual, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(message),
                    Const(parameters),
                    (e, a, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreSame(e, a, m, p));

                _test.Act(action);
            }

            public void AreSame(TestExpression<object> expected, TestExpression<object> actual)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    (e, a) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreSame(e, a));

                _test.Act(action);
            }

            public void AreSame(TestExpression<object> expected, TestExpression<object> actual, string message)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(message),
                    (e, a, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreSame(e, a, m));

                _test.Act(action);
            }

            public void AreSame(TestExpression<object> expected, TestExpression<object> actual, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(message),
                    Const(parameters),
                    (e, a, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreSame(e, a, m, p));

                _test.Act(action);
            }
            #endregion

            #region AreNotSame Overloads
            public void AreNotSame<T>(TestExpression<T> notExpected, TestExpression<T> actual)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    (e, a) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotSame(e, a));

                _test.Act(action);
            }

            public void AreNotSame<T>(TestExpression<T> notExpected, TestExpression<T> actual, string message)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(message),
                    (e, a, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotSame(e, a, m));

                _test.Act(action);
            }

            public void AreNotSame<T>(TestExpression<T> notExpected, TestExpression<T> actual, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(message),
                    Const(parameters),
                    (e, a, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotSame(e, a, m, p));

                _test.Act(action);
            }

            public void AreNotSame(TestExpression<object> notExpected, TestExpression<object> actual)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    (e, a) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotSame(e, a));

                _test.Act(action);
            }

            public void AreNotSame<T>(TestExpression<object> notExpected, TestExpression<object> actual, string message)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(message),
                    (e, a, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotSame(e, a, m));

                _test.Act(action);
            }

            public void AreNotSame<T>(TestExpression<object> notExpected, TestExpression<object> actual, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(message),
                    Const(parameters),
                    (e, a, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotSame(e, a, m, p));

                _test.Act(action);
            }
            #endregion

            #region Fail Overloads
            public void Fail()
            {
                TestExpression action = Expr(() => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail());
                _test.Act(action);
            }

            public void Fail(string message)
            {
                TestExpression action = Expr(
                    Const(message),
                    m => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail(m));

                _test.Act(action);
            }

            public void Fail(string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    Const(message),
                    Const(parameters),
                    (m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail(m, p));

                _test.Act(action);
            }
            #endregion

            #region Inconclusive Overloads
            public void Inconclusive()
            {
                TestExpression action = Expr(() => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Inconclusive());
                _test.Act(action);
            }

            public void Inconclusive(string message)
            {
                TestExpression action = Expr(
                    Const(message),
                    m => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail(m));

                _test.Act(action);
            }

            public void Inconclusive(string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    Const(message),
                    Const(parameters),
                    (m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail(m, p));

                _test.Act(action);
            }
            #endregion

            #region IsTrue Overloads
            public void IsTrue(TestExpression<bool> assertion)
            {
                TestExpression action = Expr(
                    assertion,
                    a => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(a));

                _test.Act(action);
            }

            public void IsTrue(TestExpression<bool> assertion, string message)
            {
                TestExpression action = Expr(
                    assertion,
                    Const(message),
                    (a, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(a, m));

                _test.Act(action);
            }

            public void IsTrue(TestExpression<bool> assertion, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    assertion,
                    Const(message),
                    Const(parameters),
                    (a, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(a, m, p));

                _test.Act(action);
            }
            #endregion

            #region IsFalse Overloads
            public void IsFalse(TestExpression<bool> assertion)
            {
                TestExpression action = Expr(
                    assertion,
                    a => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(a));

                _test.Act(action);
            }

            public void IsFalse(TestExpression<bool> assertion, string message)
            {
                TestExpression action = Expr(
                    assertion,
                    Const(message),
                    (a, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(a, m));

                _test.Act(action);
            }

            public void IsFalse(TestExpression<bool> assertion, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    assertion,
                    Const(message),
                    Const(parameters),
                    (a, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(a, m, p));

                _test.Act(action);
            }
            #endregion

            #region IsInstanceOfType Overloads
            public void IsInstanceOfType(TestExpression<object> value, TestExpression<Type> expectedType)
            {
                TestExpression action = Expr(
                    value,
                    expectedType,
                    (v, t) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(v, t));

                _test.Act(action);
            }

            public void IsInstanceOfType(TestExpression<object> value, TestExpression<Type> expectedType, string message)
            {
                TestExpression action = Expr(
                    value,
                    expectedType,
                    Const(message),
                    (v, t, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(v, t, m));

                _test.Act(action);
            }

            public void IsInstanceOfType(TestExpression<object> value, TestExpression<Type> expectedType, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    value,
                    expectedType,
                    Const(message),
                    Const(parameters),
                    (v, t, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(v, t, m, p));

                _test.Act(action);
            }
            #endregion

            #region IsNotInstanceOfType Overloads
            public void IsNotInstanceOfType(TestExpression<object> value, TestExpression<Type> notExpectedType)
            {
                TestExpression action = Expr(
                    value,
                    notExpectedType,
                    (v, t) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotInstanceOfType(v, t));

                _test.Act(action);
            }

            public void IsNotInstanceOfType(TestExpression<object> value, TestExpression<Type> notExpectedType, string message)
            {
                TestExpression action = Expr(
                     value,
                     notExpectedType,
                     Const(message),
                     (v, t, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotInstanceOfType(v, t, m));

                _test.Act(action);
            }

            public void IsNotInstanceOfType(TestExpression<object> value, TestExpression<Type> notExpectedType, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    value,
                    notExpectedType,
                    Const(message),
                    Const(parameters),
                    (v, t, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(v, t, m, p));

                _test.Act(action);
            }
            #endregion

            #region IsNull Overloads
            public void IsNull<T>(TestExpression<T> value)
            {
                TestExpression action = Expr(
                    value,
                    v => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNull(v));

                _test.Act(action);
            }

            public void IsNull<T>(TestExpression<T> value, string message)
            {
                TestExpression action = Expr(
                     value,
                     Const(message),
                     (v, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNull(v, m));

                _test.Act(action);
            }

            public void IsNull<T>(TestExpression<T> value, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                     value,
                     Const(message),
                     Const(parameters),
                     (v, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNull(v, m, p));

                _test.Act(action);
            }
            #endregion

            #region IsNotNull Overloads
            public void IsNotNull<T>(TestExpression<T> value)
            {
                TestExpression action = Expr(
                    value,
                    v => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(v));

                _test.Act(action);
            }

            public void IsNotNull<T>(TestExpression<T> value, string message)
            {
                TestExpression action = Expr(
                     value,
                     Const(message),
                     (v, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(v, m));

                _test.Act(action);
            }

            public void IsNotNull<T>(TestExpression<T> value, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                     value,
                     Const(message),
                     Const(parameters),
                     (v, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(v, m, p));

                _test.Act(action);
            }
            #endregion

            #region ThrowsException Overloads
            public void ThrowsException<TException>(TestExpression<Action> action) where TException : Exception
            {
                TestExpression assertAction = Expr(
                    action,
                    a => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.ThrowsException<TException>(a));

                _test.Act(assertAction);
            }

            public void ThrowsException<TException>(TestExpression<Action> action, string message) where TException : Exception
            {
                TestExpression assertAction = Expr(
                    action,
                    Const(message),
                    (a, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.ThrowsException<TException>(a, m));

                _test.Act(assertAction);
            }

            public void ThrowsException<TException>(TestExpression<Action> action, string message, params object[] parameters) where TException : Exception
            {
                TestExpression assertAction = Expr(
                    action,
                    Const(message),
                    Const(parameters),
                    (a, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.ThrowsException<TException>(a, m, p));

                _test.Act(assertAction);
            }

            public void ThrowsException<TException>(TestExpression<Func<object>> action) where TException : Exception
            {
                TestExpression assertAction = Expr(
                    action,
                    a => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.ThrowsException<TException>(a));

                _test.Act(assertAction);
            }

            public void ThrowsException<TException>(TestExpression<Func<object>> action, string message) where TException : Exception
            {
                TestExpression assertAction = Expr(
                    action,
                    Const(message),
                    (a, m) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.ThrowsException<TException>(a, m));

                _test.Act(assertAction);
            }

            public void ThrowsException<TException>(TestExpression<Func<object>> action, string message, params object[] parameters) where TException : Exception
            {
                TestExpression assertAction = Expr(
                    action,
                    Const(message),
                    Const(parameters),
                    (a, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.ThrowsException<TException>(a, m, p));

                _test.Act(assertAction);
            }
            #endregion
        }

        // Based on Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert
        public class CollectionAssertObject
        {
            UnitTest _test;

            public CollectionAssertObject(UnitTest test)
            {
                _test = test;
            }

            #region AllItemsAreInstancesOfType Overloads
            public void AllItemsAreInstancesOfType(TestExpression<ICollection> collection, TestExpression<Type> expectedType)
            {
                TestExpression action = Expr(
                    collection,
                    expectedType,
                    (c, t) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AllItemsAreInstancesOfType(c, t));

                _test.Act(action);
            }

            public void AllItemsAreInstancesOfType(TestExpression<ICollection> collection, TestExpression<Type> expectedType, string message)
            {
                TestExpression action = Expr(
                   collection,
                   expectedType,
                   Const(message),
                   (c, t, m) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AllItemsAreInstancesOfType(c, t, m));

                _test.Act(action);
            }

            public void AllItemsAreInstancesOfType(TestExpression<ICollection> collection, TestExpression<Type> expectedType, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                   collection,
                   expectedType,
                   Const(message),
                   Const(parameters),
                   (c, t, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AllItemsAreInstancesOfType(c, t, m, p));

                _test.Act(action);
            }
            #endregion

            #region AllItemsAreNotNull Overloads
            public void AllItemsAreNotNull(TestExpression<ICollection> collection)
            {
                TestExpression action = Expr(
                    collection,
                    c => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AllItemsAreNotNull(c));

                _test.Act(action);
            }

            public void AllItemsAreNotNull(TestExpression<ICollection> collection, string message)
            {
                TestExpression action = Expr(
                   collection,
                   Const(message),
                   (c, m) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AllItemsAreNotNull(c, m));

                _test.Act(action);
            }

            public void AllItemsAreNotNull(TestExpression<ICollection> collection, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                   collection,
                   Const(message),
                   Const(parameters),
                   (c, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AllItemsAreNotNull(c, m, p));

                _test.Act(action);
            }
            #endregion

            #region AllItemsAreUnique Overloads
            public void AllItemsAreUnique(TestExpression<ICollection> collection)
            {
                TestExpression action = Expr(
                   collection,
                   c => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AllItemsAreUnique(c));

                _test.Act(action);
            }

            public void AllItemsAreUnique(TestExpression<ICollection> collection, string message)
            {
                TestExpression action = Expr(
                  collection,
                  Const(message),
                  (c, m) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AllItemsAreUnique(c, m));

                _test.Act(action);
            }

            public void AllItemsAreUnique(TestExpression<ICollection> collection, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                   collection,
                   Const(message),
                   Const(parameters),
                   (c, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AllItemsAreUnique(c, m, p));

                _test.Act(action);
            }
            #endregion

            #region AreEqual Overloads
            public void AreEqual(TestExpression<ICollection> expected, TestExpression<ICollection> actual)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    (e, a) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AreEqual(e, a));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<ICollection> expected, TestExpression<ICollection> actual, string message)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(message),
                    (e, a, m) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AreEqual(e, a, m));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<ICollection> expected, TestExpression<ICollection> actual, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(message),
                    Const(parameters),
                    (e, a, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AreEqual(e, a, m, p));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<ICollection> expected, TestExpression<ICollection> actual, TestExpression<IComparer> comparer)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    comparer,
                    (e, a, c) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AreEqual(e, a, c));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<ICollection> expected, TestExpression<ICollection> actual, TestExpression<IComparer> comparer, string message)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    comparer,
                    Const(message),
                    (e, a, c, m) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AreEqual(e, a, c, m));

                _test.Act(action);
            }

            public void AreEqual(TestExpression<ICollection> expected, TestExpression<ICollection> actual, TestExpression<IComparer> comparer, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    comparer,
                    Const(message),
                    Const(parameters),
                    (e, a, c, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AreEqual(e, a, c, m, p));

                _test.Act(action);
            }
            #endregion

            #region AreNotEqual Overloads
            public void AreNotEqual(TestExpression<ICollection> notExpected, TestExpression<ICollection> actual)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    (e, a) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AreNotEqual(e, a));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<ICollection> notExpected, TestExpression<ICollection> actual, string message)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(message),
                    (e, a, m) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AreNotEqual(e, a, m));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<ICollection> notExpected, TestExpression<ICollection> actual, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(message),
                    Const(parameters),
                    (e, a, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AreNotEqual(e, a, m, p));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<ICollection> notExpected, TestExpression<ICollection> actual, TestExpression<IComparer> comparer)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    comparer,
                    (e, a, c) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AreNotEqual(e, a, c));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<ICollection> notExpected, TestExpression<ICollection> actual, TestExpression<IComparer> comparer, string message)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    comparer,
                    Const(message),
                    (e, a, c, m) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AreNotEqual(e, a, c, m));

                _test.Act(action);
            }

            public void AreNotEqual(TestExpression<ICollection> notExpected, TestExpression<ICollection> actual, TestExpression<IComparer> comparer, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    comparer,
                    Const(message),
                    Const(parameters),
                    (e, a, c, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AreNotEqual(e, a, c, m, p));

                _test.Act(action);
            }
            #endregion

            #region AreEquivalent Overloads
            public void AreEquivalent(TestExpression<ICollection> expected, TestExpression<ICollection> actual)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    (e, a) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AreEquivalent(e, a));

                _test.Act(action);
            }

            public void AreEquivalent(TestExpression<ICollection> expected, TestExpression<ICollection> actual, string message)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(message),
                    (e, a, m) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AreEquivalent(e, a, m));

                _test.Act(action);
            }

            public void AreEquivalent(TestExpression<ICollection> expected, TestExpression<ICollection> actual, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    expected,
                    actual,
                    Const(message),
                    Const(parameters),
                    (e, a, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AreEquivalent(e, a, m, p));

                _test.Act(action);
            }
            #endregion

            #region AreNotEquivalent Overloads
            public void AreNotEquivalent(TestExpression<ICollection> notExpected, TestExpression<ICollection> actual)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    (e, a) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AreNotEquivalent(e, a));

                _test.Act(action);
            }

            public void AreNotEquivalent(TestExpression<ICollection> notExpected, TestExpression<ICollection> actual, string message)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(message),
                    (e, a, m) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AreNotEquivalent(e, a, m));

                _test.Act(action);
            }

            public void AreNotEquivalent(TestExpression<ICollection> notExpected, TestExpression<ICollection> actual, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    notExpected,
                    actual,
                    Const(message),
                    Const(parameters),
                    (e, a, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.AreNotEquivalent(e, a, m, p));

                _test.Act(action);
            }
            #endregion

            #region Contains Overloads
            public void Contains(TestExpression<ICollection> collection, TestExpression<object> element)
            {
                TestExpression action = Expr(
                    collection,
                    element,
                    (c, e) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.Contains(c, e));

                _test.Act(action);
            }

            public void Contains(TestExpression<ICollection> collection, TestExpression<object> element, string message)
            {
                TestExpression action = Expr(
                    collection,
                    element,
                    Const(message),
                    (c, e, m) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.Contains(c, e, m));

                _test.Act(action);
            }

            public void Contains(TestExpression<ICollection> collection, TestExpression<object> element, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    collection,
                    element,
                    Const(message),
                    Const(parameters),
                    (c, e, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.Contains(c, e, m, p));

                _test.Act(action);
            }
            #endregion

            #region DoesNotContain Overloads
            public void DoesNotContain(TestExpression<ICollection> collection, TestExpression<object> element)
            {
                TestExpression action = Expr(
                    collection,
                    element,
                    (c, e) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.DoesNotContain(c, e));

                _test.Act(action);
            }

            public void DoesNotContain(TestExpression<ICollection> collection, TestExpression<object> element, string message)
            {
                TestExpression action = Expr(
                    collection,
                    element,
                    Const(message),
                    (c, e, m) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.DoesNotContain(c, e, m));

                _test.Act(action);
            }

            public void DoesNotContain(TestExpression<ICollection> collection, TestExpression<object> element, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    collection,
                    element,
                    Const(message),
                    Const(parameters),
                    (c, e, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.Contains(c, e, m, p));

                _test.Act(action);
            }
            #endregion

            #region IsSubsetOf Overloads
            public void IsSubsetOf(TestExpression<ICollection> subset, TestExpression<ICollection> superset) 
            {
                TestExpression action = Expr(
                    subset,
                    superset,
                    (s1, s2) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.IsSubsetOf(s1, s2));

                _test.Act(action);
            }
            public void IsSubsetOf(TestExpression<ICollection> subset, TestExpression<ICollection> superset, string message)
            {
                TestExpression action = Expr(
                    subset,
                    superset,
                    Const(message),
                    (s1, s2, m) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.IsSubsetOf(s1, s2, m));

                _test.Act(action);
            }

            public void IsSubsetOf(TestExpression<ICollection> subset, TestExpression<ICollection> superset, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                    subset,
                    superset,
                    Const(message),
                    Const(parameters),
                    (s1, s2, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.IsSubsetOf(s1, s2, m, p));

                _test.Act(action);
            }
            #endregion

            #region IsNotSubsetOf Overloads
            public void IsNotSubsetOf(TestExpression<ICollection> subset, TestExpression<ICollection> superset)
            {
                TestExpression action = Expr(
                    subset,
                    superset,
                    (s1, s2) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.IsNotSubsetOf(s1, s2));

                _test.Act(action);
            }

            public void IsNotSubsetOf(TestExpression<ICollection> subset, TestExpression<ICollection> superset, string message)
            {
                TestExpression action = Expr(
                    subset,
                    superset,
                    Const(message),
                    (s1, s2, m) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.IsNotSubsetOf(s1, s2, m));

                _test.Act(action);
            }

            public void IsNotSubsetOf(TestExpression<ICollection> subset, TestExpression<ICollection> superset, string message, params object[] parameters)
            {
                TestExpression action = Expr(
                   subset,
                   superset,
                   Const(message),
                   Const(parameters),
                   (s1, s2, m, p) => Microsoft.VisualStudio.TestTools.UnitTesting.CollectionAssert.IsSubsetOf(s1, s2, m, p));

                _test.Act(action);
            }
            #endregion
        }

        public class DelegateAssertObject
        {
            UnitTest _test;

            public DelegateAssertObject(UnitTest test)
            {
                _test = test;
            }

            public void IsInvoked<TDelegate>(TestExpression<Action<TDelegate>> subscribe) where TDelegate : Delegate
            {
                TestExpression action = Expr(
                    subscribe,
                    s => OleVanSanten.TestTools.MSTest.DelegateAssert.IsInvoked(s));

                _test.Act(action);
            }

            public void IsInvoked<TDelegate>(TestExpression<Action<TDelegate>> subscribe, TDelegate assertionCallback) where TDelegate : Delegate
            {
                TestExpression action = Expr(
                    subscribe,
                    Const(assertionCallback),
                    (s, c) => OleVanSanten.TestTools.MSTest.DelegateAssert.IsInvoked(s, c));

                _test.Act(action);
            }

            public void IsNotInvoked<TDelegate>(TestExpression<Action<TDelegate>> subscribe) where TDelegate : Delegate
            {
                TestExpression action = Expr(
                    subscribe,
                    s => OleVanSanten.TestTools.MSTest.DelegateAssert.IsNotInvoked(s));

                _test.Act(action);
            }
        }

        public class ConsoleAssertObject
        {
            UnitTest _test;

            public ConsoleAssertObject(UnitTest test)
            {
                _test = test;
            }

            public void WritesOut(TestExpression<Action> action, TestExpression<string> writeout)
            {
                TestExpression assertionAction = Expr(
                    action,
                    writeout,
                    (a, w) => OleVanSanten.TestTools.MSTest.ConsoleAssert.WritesOut(a, w));

                _test.Act(action);
            }

            public void WritesErr(TestExpression<Action> action, TestExpression<string> writeout)
            {
                TestExpression assertionAction = Expr(
                    action,
                    writeout,
                    (a, w) => OleVanSanten.TestTools.MSTest.ConsoleAssert.WritesErr(a, w));

                _test.Act(action);
            }
        }
    }
}
