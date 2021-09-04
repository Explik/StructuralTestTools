using Lecture_2_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_2_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_2_Tests
{
    [TestClass]
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

        [TestMethod("b. ImmutableNumber constructor with int as argument sets value property"), TestCategory("Exercise 3B")]
        public void ImmutableNumberConstructorWithIntAsArgumentSetsValueProperty()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("ImmutableNumber is not public (AUTO)");
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

        [TestMethod("b. ImmutableNumber.Add performs 1 + 2 = 3"), TestCategory("Exercise 3C")]
        public void AddProducesExpectedResult()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("ImmutableNumber is not public (AUTO)");
        }

        [TestMethod("c. ImmutableNumber.Subtract takes ImmutableNumber as argument and returns ImmutableNumber"), TestCategory("Exercise 3C")]
        public void SubtractTakesImmutableNumberAsArgumentAndReturnsImmutableNumber()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<ImmutableNumber, ImmutableNumber, ImmutableNumber>((n1, n2) => n1.Subtract(n2));
            test.Execute();
        }

        [TestMethod("d. ImmutableNumber.Subtract performs 8 - 3 = 5"), TestCategory("Exercise 3C")]
        public void SubstractProducesExpectedResult()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("ImmutableNumber is not public (AUTO)");
        }

        [TestMethod("e. ImmutableNumber.Multiply takes ImmutableNumber as argument and returns ImmutableNumber"), TestCategory("Exercise 3C")]
        public void MultiplyTakesImmutableAsArgumentAndReturnsNothing()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<ImmutableNumber, ImmutableNumber, ImmutableNumber>((n1, n2) => n1.Multiply(n2));
            test.Execute();
        }

        [TestMethod("f. ImmutableNumber.Multiply performs 2 * 3 = 6"), TestCategory("Exercise 3C")]
        public void MultiplyProducesExpectedResult()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("ImmutableNumber is not public (AUTO)");
        }
#endregion
    }
}