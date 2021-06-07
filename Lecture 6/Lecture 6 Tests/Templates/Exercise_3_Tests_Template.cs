using Lecture_6_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_6_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_6_Tests
{
    [TemplatedTestClass]
    public class Exercise_3_Tests_Template
    {
        #region Exercise 3A
        [TestMethod("a. IRandom is an interface"), TestCategory("Exercise 3A")]
        public void IRandomIsAnInterface()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertInterface<IRandom>();
            test.Execute();
        }

        [TestMethod("b. IRandom.Next() is a method"), TestCategory("Exercise 3A")]
        public void IRandomNextOverloadTakesNothing()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertMethod<IRandom, int>(r => r.Next());
            test.Execute();
        }

        [TestMethod("c. IRandom.Next(max) is a method"), TestCategory("Exercise 3A")]
        public void IRandomNextOverloadTakesInt()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertMethod<IRandom, int, int>((r, max) => r.Next(max));
            test.Execute();
        }

        [TestMethod("d. IRandom.Next(min, max) is a method"), TestCategory("Exercise 3A")]
        public void IRandomNextOverlaodTakes2Ints()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertMethod<IRandom, int, int, int>((r, min, max) => r.Next(min, max));
            test.Execute();
        }
        #endregion

        #region Exercise 3B
        [TestMethod("a. MyRandom implements IRandom"), TestCategory("Exercise 3B")]
        public void MyRandomImplementsIRandom()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertClass<MyRandom>(new TypeIsSubclassOfVerifier(typeof(IRandom)));
            test.Execute();
        }

        [TemplatedTestMethod("b. MyRandom.Next() does not return the same value twice (may randomly fail)"), TestCategory("Exercise 3B")]
        public void MyRandomNextReturnsRandomNumber()
        {
            MyRandom random = new MyRandom();
            Assert.AreNotEqual(random.Next(), random.Next());
        }

        [TemplatedTestMethod("c. MyRandom.Next(6) returns a number lower or equal to 6"), TestCategory("Exercise 3B")]
        public void MyRandomNextReturnsExpectedResult()
        {
            MyRandom random = new MyRandom();
            Assert.IsTrue(random.Next(6) <= 6);
        }

        [TemplatedTestMethod("d. MyRandom.Next(1, 6) returns a number between 1 and 6"), TestCategory("Exercise 3B")]
        public void MyRandomNextReturnsExpectedResult2()
        {
            MyRandom random = new MyRandom();

            int value = random.Next(1, 6);

            Assert.IsTrue(1 <= value && value <= 6);
        }
        #endregion

        #region Exercise 3C
        [TestMethod("a. PredictableRandom implements IRandom"), TestCategory("Exercise 3C")]
        public void PredictableRandomImplementsIRandom()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertClass<PredictableRandom>(new TypeIsSubclassOfVerifier(typeof(IRandom)));
            test.Execute();
        }

        [TemplatedTestMethod("b. PredictableRandom.Next() returns 4 if constructed with 4"), TestCategory("Exercise 3C")]
        public void PredictableRandomNextReturns4A()
        {
            PredictableRandom random = new PredictableRandom(4);
            Assert.AreEqual(4, random.Next());
        }

        [TemplatedTestMethod("c. PredictableRandom.Next(6) returns 4 if constructed with 4"), TestCategory("Exercise 3C")]
        public void PredictableRandomNextReturns4B()
        {
            PredictableRandom random = new PredictableRandom(4);
            Assert.AreEqual(4, random.Next(6));
        }

        [TemplatedTestMethod("d. PredictableRandom.Next(6) throws ArgumentException if constructed with 7"), TestCategory("Exercise 3C")]
        public void PredictableRandomNextThrowsOn6()
        {
            PredictableRandom random = new PredictableRandom(7);
            Assert.ThrowsException<ArgumentException>(() => random.Next(6));
        }

        [TemplatedTestMethod("e. PredictableRandom.Next(1, 6) returns 4 if constructed with 4"), TestCategory("Exercise 3C")]
        public void PredictableRandomNextReturns4C()
        {
            PredictableRandom random = new PredictableRandom(4);
            Assert.AreEqual(4, random.Next(1, 6));
        }

        [TemplatedTestMethod("f. PredictableRandom.Next(1, 6) throws ArgumentException if constructed with 0"), TestCategory("Exercise 3C")]
        public void PredictableRandomNextThrowsOn1And6()
        {
            PredictableRandom random = new PredictableRandom(0);
            Assert.ThrowsException<ArgumentException>(() => random.Next(1, 6));
        }
        #endregion
    }
}
