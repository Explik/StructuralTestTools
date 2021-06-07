using Lecture_7_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_7_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_7_Tests
{
    [TemplatedTestClass]
    public class Exercise_5_Tests_Template
    {
        #region Exercise 5A
        [TestMethod("a. ArrayHelper is a public static class"), TestCategory("Exercise 5A")]
        public void ArrayHelperIsAStaticClass()
        {
            StructureTest test = Factory.CreateStructureTest();
            test.AssertClass(
                typeof(ArrayHelper),
                new TypeIsStaticVerifier(),
                new TypeAccessLevelVerifier(AccessLevels.Public));
            test.Execute();
        }
        #endregion

        #region Exercise 5B
        [TestMethod("a. ArrayHelper.Min<T>(T[] array) is a public static method"), TestCategory("Exercise 5B")]
        public void ArrayHelperMinIsAPublicStaticMethod()
        {
            StructureTest test = Factory.CreateStructureTest();
            test.AssertStaticMethod<int[], int>(
                array => ArrayHelper.Min(array),
                new MemberAccessLevelVerifier(AccessLevels.Public));
            test.AssertStaticMethod<DateTime[], DateTime>(array => ArrayHelper.Min(array));
            test.Execute();
        }

        [TemplatedTestMethod("b. ArrayHelper.Min<T>(T[] array) returns the smallest object"), TestCategory("Exercise 5B")]
        public void ArrayHelperMinReturnsTheSmallestObject()
        {
            int[] array = new int[] { 2, 5, 3 };
            Assert.AreEqual(2, ArrayHelper.Min(array));
        }
        #endregion

        #region Exercise 5C
        [TestMethod("a. ArrayHelper.Max<T>(T[] array) is a public static method"), TestCategory("Exercise 5C")]
        public void ArrayHelperMaxIsAPublicStaticMethod()
        {
            StructureTest test = Factory.CreateStructureTest();
            test.AssertStaticMethod<int[], int>(
                array => ArrayHelper.Max(array),
                new MemberAccessLevelVerifier(AccessLevels.Public));
            test.AssertStaticMethod<DateTime[], DateTime>(array => ArrayHelper.Max(array));
            test.Execute();
        }

        [TemplatedTestMethod("b. ArrayHelper.Max<T>(T[] array) returns the largest object"), TestCategory("Exercise 5C")]
        public void ArrayHelperMinReturnsTheLargestObject()
        {
            int[] array = new int[] { 2, 5, 3 };
            Assert.AreEqual(5, ArrayHelper.Max(array));
        }
        #endregion

        #region Exercise 5D
        [TestMethod("a. ArrayHelper.Copy<T>(T[] array) is a public static method"), TestCategory("Exercise 5D")]
        public void ArrayHelperCopyIsAPublicStaticMethod()
        {
            StructureTest test = Factory.CreateStructureTest();
            test.AssertStaticMethod<int[], int[]>(
                array => ArrayHelper.Copy(array),
                new MemberAccessLevelVerifier(AccessLevels.Public));
            test.AssertStaticMethod<DateTime[], DateTime[]>(array => ArrayHelper.Copy(array));
            test.Execute();
        }

        [TemplatedTestMethod("b. ArrayHelper.Copy<T>(T[] array) returns a copy of the array"), TestCategory("Exercise 5D")]
        public void ArrayHelperCopyReturnsACopyOfTheArray()
        {
            int[] array = new int[] { 2, 5, 3, 8, 9 };

            CollectionAssert.AreEqual(array, ArrayHelper.Copy(array));
        }
        #endregion

        #region Exercise 5E
        [TestMethod("a. ArrayHelper.Shuffle<T>(T[] array) is a public static method"), TestCategory("Exercise 5E")]
        public void ArrayHelperShuffleIsAPublicStaticMethod()
        {
            StructureTest test = Factory.CreateStructureTest();
            test.AssertStaticMethod<int[]>(
                array => ArrayHelper.Shuffle(array),
                new MemberAccessLevelVerifier(AccessLevels.Public));
            test.AssertStaticMethod<DateTime[]>(array => ArrayHelper.Shuffle(array));
            test.Execute();
        }

        [TemplatedTestMethod("b. ArrayHelper.Shuffle<T>(T[] array) swaps elements in original array (may fail)"), TestCategory("Exercise 5E")]
        public void ArrayHelperShuffleSwapsElementsInOriginalArray()
        {
            int[] expected = new int[] { 1, 2, 3, 4, 5 };
            int[] actual = new int[] { 1, 2, 3, 4, 5 };

            ArrayHelper.Shuffle(actual);

            CollectionAssert.AreNotEqual(expected, actual);
        }
        #endregion
    }
}
