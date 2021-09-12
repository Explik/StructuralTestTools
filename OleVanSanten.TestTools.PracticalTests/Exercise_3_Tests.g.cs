using Lecture_2_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OleVanSanten.TestTools;
using OleVanSanten.TestTools.MSTest;
using static Lecture_2_Tests.TestHelper;

namespace Lecture_2_Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class Exercise_3_Tests
    {
#region Exercise 3A
        [TestMethod("a. ImmutableNumber.Value is public readonly int property"), TestCategory("Exercise 3A")]
        public void ValueIsPublicReadonlyProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicReadonlyProperty<ImmutableNumber, int>(n => n.Value);
            test.Execute();
        }

#endregion
#region Exercise 3B
        [TestMethod("a. ImmutableNumber.Number constructor takes int as argument"), TestCategory("Exercise 3B")]
        public void ImmutableNumberConstructorTakesIntAsArgument()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicConstructor<int, ImmutableNumber>(i => new ImmutableNumber(i));
            test.Execute();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("b. ImmutableNumber constructor with int as argument sets value property"), TestCategory("Exercise 3B")]
        public void ImmutableNumberConstructorWithIntAsArgumentSetsValueProperty()
        {
            // == Failed To Compile ==
            // ImmutableNumber number = new ImmutableNumber(2);
            // Assert.AreEqual(2, number.Value);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("ImmutableNumber is not public");
        }

#endregion
#region Exercise 2C
        [TestMethod("a. ImmutableNumber.Add takes ImmutableNumber as argument and returns ImmutableNumber"), TestCategory("Exercise 3C")]
        public void AddTakesImmutableNumberAsArgumentAndReturnsImmutableNumber()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<ImmutableNumber, ImmutableNumber, ImmutableNumber>((n1, n2) => n1.Add(n2));
            test.Execute();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("b. ImmutableNumber.Add performs 1 + 2 = 3"), TestCategory("Exercise 3C")]
        public void AddProducesExpectedResult()
        {
            // == Failed To Compile ==
            // ImmutableNumber number1 = new ImmutableNumber(1);
            // ImmutableNumber number2 = new ImmutableNumber(2);
            // ImmutableNumber number3 = number1.Add(number2);
            // Assert.AreEqual(3, number3.Value);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("ImmutableNumber is not public");
        }

        [TestMethod("c. ImmutableNumber.Subtract takes ImmutableNumber as argument and returns ImmutableNumber"), TestCategory("Exercise 3C")]
        public void SubtractTakesImmutableNumberAsArgumentAndReturnsImmutableNumber()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<ImmutableNumber, ImmutableNumber, ImmutableNumber>((n1, n2) => n1.Subtract(n2));
            test.Execute();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("d. ImmutableNumber.Subtract performs 8 - 3 = 5"), TestCategory("Exercise 3C")]
        public void SubstractProducesExpectedResult()
        {
            // == Failed To Compile ==
            // ImmutableNumber number1 = new ImmutableNumber(8);
            // ImmutableNumber number2 = new ImmutableNumber(3);
            // ImmutableNumber number3 = number1.Subtract(number2);
            // Assert.AreEqual(5, number3.Value);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("ImmutableNumber is not public");
        }

        [TestMethod("e. ImmutableNumber.Multiply takes ImmutableNumber as argument and returns ImmutableNumber"), TestCategory("Exercise 3C")]
        public void MultiplyTakesImmutableAsArgumentAndReturnsNothing()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<ImmutableNumber, ImmutableNumber, ImmutableNumber>((n1, n2) => n1.Multiply(n2));
            test.Execute();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("f. ImmutableNumber.Multiply performs 2 * 3 = 6"), TestCategory("Exercise 3C")]
        public void MultiplyProducesExpectedResult()
        {
            // == Failed To Compile ==
            // ImmutableNumber number1 = new ImmutableNumber(2);
            // ImmutableNumber number2 = new ImmutableNumber(3);
            // ImmutableNumber number3 = number1.Multiply(number2);
            // Assert.AreEqual(6, number3.Value);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("ImmutableNumber is not public");
        }
#endregion
    }
}