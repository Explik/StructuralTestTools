using Lecture_6_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_6_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_6_Tests
{
    [TemplatedTestClass]
    public class Exercise_4_Tests_Template
    {
        #region Exercise 4A
        [TestMethod("a. Die constructor takes IRandom"), TestCategory("Exercise 4A")]
        public void DieConstructorTakesIRandom()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicConstructor<IRandom, Die>(r => new Die(r));
            test.Execute();
        }
        #endregion

        #region Exercise 4B
        [TestMethod("a. Die constructor takes IRandom and int"), TestCategory("Exercise 4B")]
        public void DieConstructorTakesIRandomAndInt()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicConstructor<IRandom, int, Die>((r, s) => new Die(r, s));
            test.Execute();
        }
        #endregion

        #region Exercise 4C
        [TemplatedTestMethod("a. Die.Roll returns 5 if constructed PredictablyRandom(5)"), TestCategory("Exercise 4C")]
        public void DieRollReturns5()
        {
            PredictableRandom random = new PredictableRandom(5);
            Die die = new Die(random, 6);

            Assert.AreEqual(5, die.Roll());
        }

        [TemplatedTestMethod("b. Die.Roll returns a number between 1 and 6 if constructed with 6 sides and MyRandom"), TestCategory("Exercise 4C")]
        public void DieRollReturnsANumberBetween1And6()
        {
            MyRandom random = new MyRandom();
            Die die = new Die(random, 6);

            int value = die.Roll();

            Assert.IsTrue(1 <= value && value <= 6);
        }
        #endregion
    }
}
