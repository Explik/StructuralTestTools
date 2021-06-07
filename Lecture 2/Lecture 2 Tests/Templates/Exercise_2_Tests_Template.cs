using Lecture_2_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_2_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_2_Tests
{
    [TemplatedTestClass]
    public class Exercise_2_Tests_Template
    {
        #region Exercise 2A
        [TestMethod("Number.Value is public readonly int property"), TestCategory("Exercise 2A")]
        public void ValueIsPublicReadonlyIntProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicReadonlyProperty<Number, int>(n => n.Value);
            test.Execute();
        }
        #endregion

        #region Exercise 2B
        [TestMethod("a. Number constructor takes int as argument"), TestCategory("Exercise 2B")]
        public void NumberConstructorTakesIntAsArgument() {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicConstructor<int, Number>(i => new Number(i));
            test.Execute();
        }

        [TemplatedTestMethod("b. Number constructor with int as argument sets value property"), TestCategory("Exercise 2B")]
        public void NumberConstructorWithIntAsArgumentSetsValueProperty()
        {
            Number number = new Number(2);
            Assert.AreEqual(number.Value, 2);
        }
        #endregion

        #region Exercise 2C
        [TestMethod("a. Number.Add takes Number as argument and returns nothing"), TestCategory("Exercise 2C")]
        public void AddTakesNumberAsArgumentsAndReturnsNothing() {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Number, Number>((n1, n2) => n1.Add(n2));
            test.Execute();
        }

        [TemplatedTestMethod("b. Number.Add performs 1 + 2 = 3"), TestCategory("Exercise 2C")]
        public void AddProducesExpectedResult() 
        {
            Number number1 = new Number(1);
            Number number2 = new Number(2);

            number1.Add(number2);

            Assert.AreEqual(3, number1.Value);
        }

        [TestMethod("c. Number.Subtract takes Number as argument and returns nothing"), TestCategory("Exercise 2C")]
        public void SubtractTakesNumberAsArgumentAndReturnsNothing()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Number, Number>((n1, n2) => n1.Subtract(n2));
            test.Execute();
        }

        [TemplatedTestMethod("d. Number.Subtract performs 8 - 3 = 5"), TestCategory("Exercise 2C")]
        public void SubtractProducesExpectedResult() 
        {
            Number number1 = new Number(8);
            Number number2 = new Number(3);

            number1.Subtract(number2);

            Assert.AreEqual(5, number1.Value);
        }

        [TestMethod("e. Number.Multiply takes Number as argument and returns nothing"), TestCategory("Exercise 2C")]
        public void MultiplyTakesNumberAsArgumentAndReturnsNothing()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Number, Number>((n1, n2) => n1.Multiply(n2));
            test.Execute();
        }

        [TemplatedTestMethod("f. Number.Multiply performs 2 * 3 = 6"), TestCategory("Exercise 2C")]
        public void MultiplyProducesExpectedResult()
        {
            Number number1 = new Number(2);
            Number number2 = new Number(3);

            number1.Multiply(number2);

            Assert.AreEqual(6, number1.Value);
        }
        #endregion
    }
}
