using Lecture_2_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Explik.StructuralTestTools;
using Explik.StructuralTestTools.MSTest;
using static Lecture_2_Tests.TestHelper;

namespace Lecture_2_Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class Exercise_4_Tests
    {
#region Exercise 4B
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("a. Number.Equals does not equate 4 and 5"), TestCategory("Exercise 4B")]
        public void EqualsDoesNotEquateFourAndFive()
        {
            // == Failed To Compile ==
            // Number number1 = new Number(4);
            // Number number2 = new Number(5);
            // Assert.IsFalse(number1.Equals(number2));
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Number is not public");
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("b. Number.Equals equates 5 and 5"), TestCategory("Exercise 4B")]
        public void EqualsEquatesFiveAndFive()
        {
            // == Failed To Compile ==
            // Number number1 = new Number(5);
            // Number number2 = new Number(5);
            // Assert.IsTrue(number1.Equals(number2));
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Number is not public");
        }

#endregion
#region Exercise 4C
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("a. Number.GetHashCode does not equate 4 and 5"), TestCategory("Exercise 4C")]
        public void GetHashCodeDoesNotEquateFourAndFive()
        {
            // == Failed To Compile ==
            // Number number1 = new Number(4);
            // Number number2 = new Number(5);
            // Assert.IsFalse(number1.GetHashCode() == number2.GetHashCode());
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Number is not public");
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("b. Number.GetHashCode equates 5 and 5"), TestCategory("Exercise 4C")]
        public void GetHashCodeEquatesFiveAndFice()
        {
            // == Failed To Compile ==
            // Number number1 = new Number(5);
            // Number number2 = new Number(5);
            // Assert.IsTrue(number1.GetHashCode() == number2.GetHashCode());
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Number is not public");
        }
#endregion
    }
}