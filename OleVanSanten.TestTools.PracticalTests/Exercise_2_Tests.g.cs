using Lecture_2_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OleVanSanten.TestTools;
using OleVanSanten.TestTools.MSTest;
using static Lecture_2_Tests.TestHelper;

namespace Lecture_2_Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class Exercise_2_Tests
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
        public void NumberConstructorTakesIntAsArgument()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicConstructor<int, Number>(i => new Number(i));
            test.Execute();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("b. Number constructor with int as argument sets value property"), TestCategory("Exercise 2B")]
        public void NumberConstructorWithIntAsArgumentSetsValueProperty()
        {
            // == Failed To Compile ==
            // Number number = new Number(2);
            // Assert.AreEqual(number.Value, 2);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Number is not public");
        }

#endregion
#region Exercise 2C
        [TestMethod("a. Number.Add takes Number as argument and returns nothing"), TestCategory("Exercise 2C")]
        public void AddTakesNumberAsArgumentsAndReturnsNothing()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Number, Number>((n1, n2) => n1.Add(n2));
            test.Execute();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("b. Number.Add performs 1 + 2 = 3"), TestCategory("Exercise 2C")]
        public void AddProducesExpectedResult()
        {
            // == Failed To Compile ==
            // Number number1 = new Number(1);
            // Number number2 = new Number(2);
            // number1.Add(number2);
            // Assert.AreEqual(3, number1.Value);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Number is not public");
        }

        [TestMethod("c. Number.Subtract takes Number as argument and returns nothing"), TestCategory("Exercise 2C")]
        public void SubtractTakesNumberAsArgumentAndReturnsNothing()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Number, Number>((n1, n2) => n1.Subtract(n2));
            test.Execute();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("d. Number.Subtract performs 8 - 3 = 5"), TestCategory("Exercise 2C")]
        public void SubtractProducesExpectedResult()
        {
            // == Failed To Compile ==
            // Number number1 = new Number(8);
            // Number number2 = new Number(3);
            // number1.Subtract(number2);
            // Assert.AreEqual(5, number1.Value);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Number is not public");
        }

        [TestMethod("e. Number.Multiply takes Number as argument and returns nothing"), TestCategory("Exercise 2C")]
        public void MultiplyTakesNumberAsArgumentAndReturnsNothing()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Number, Number>((n1, n2) => n1.Multiply(n2));
            test.Execute();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("f. Number.Multiply performs 2 * 3 = 6"), TestCategory("Exercise 2C")]
        public void MultiplyProducesExpectedResult()
        {
            // == Failed To Compile ==
            // Number number1 = new Number(2);
            // Number number2 = new Number(3);
            // number1.Multiply(number2);
            // Assert.AreEqual(6, number1.Value);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Number is not public");
        }
#endregion
    }
}