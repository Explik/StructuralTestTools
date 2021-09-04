using Lecture_2_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using static Lecture_2_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_2_Tests
{
    [TestClass]
    public class Exercise_4_Tests
    {
#region Exercise 4B
        [TestMethod("a. Number.Equals does not equate 4 and 5"), TestCategory("Exercise 4B")]
        public void EqualsDoesNotEquateFourAndFive()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Number is not public (AUTO)");
        }

        [TestMethod("b. Number.Equals equates 5 and 5"), TestCategory("Exercise 4B")]
        public void EqualsEquatesFiveAndFive()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Number is not public (AUTO)");
        }

#endregion
#region Exercise 4C
        [TestMethod("a. Number.GetHashCode does not equate 4 and 5"), TestCategory("Exercise 4C")]
        public void GetHashCodeDoesNotEquateFourAndFive()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Number is not public (AUTO)");
        }

        [TestMethod("b. Number.GetHashCode equates 5 and 5"), TestCategory("Exercise 4C")]
        public void GetHashCodeEquatesFiveAndFice()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Number is not public (AUTO)");
        }
#endregion
    }
}