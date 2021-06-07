using Lecture_8_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_8_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_8_Tests
{
    [TemplatedTestClass]
    public class Exercise_1_Tests_Template
    {
        #region Exercise 1B
        [TestMethod("a. ArrayHelper is an static class"), TestCategory("Exercise 1B")]
        public void ArrayHelperIsAnStaticClass()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertType(typeof(ArrayHelper), new TypeIsStaticVerifier());
            test.Execute();
        }

        [TestMethod("b. ArrayHelper.Filter<T>(T[] array, Predicate<T> p) is a public method"), TestCategory("Exercise 1B")]
        public void ArrayHelperFilterIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertStaticMethod<int[], Predicate<int>, int[]>(
                (array, p) => ArrayHelper.Filter(array, p), 
                new MemberAccessLevelVerifier(AccessLevels.Public));
            test.AssertStaticMethod<double[], Predicate<double>, double[]>((array, p) => ArrayHelper.Filter(array, p));
            test.Execute();
        }

        [TestMethod("c. ArrayHelper.Map<T1, T2>(T1[] array, Func<T1, T2> f) is a public method"), TestCategory("Exercise 1B")]
        public void ArrayHelperMapIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertStaticMethod<int[], Func<int, double>, double[]>(
                (array, f) => ArrayHelper.Map(array, f), 
                new MemberAccessLevelVerifier(AccessLevels.Public));
            test.AssertStaticMethod<double[], Func<double, int>, int[]>((array, f) => ArrayHelper.Map(array, f));
            test.Execute();
        }

        [TestMethod("d. ArrayHelper.Sort<T>(T[] array, Func<T, T, int> f) is a public method"), TestCategory("Exercise 1B")]
        public void ArrayHelperSortIsPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertStaticMethod<string[], Func<string, string, int>>(
                (array, c) => ArrayHelper.Sort(array, c), 
                new MemberAccessLevelVerifier(AccessLevels.Public));
            test.AssertStaticMethod<double[], Func<double, double, int>>((array, c) => ArrayHelper.Sort(array, c));
            test.Execute();
        }

        [TestMethod("e. ArrayHelper.Find<T>(T[] array, Predicate<T> p) is a public method"), TestCategory("Exercise 1B")]
        public void ArrayHelperFindIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertStaticMethod<int[], Predicate<int>, int>(
                (array, p) => ArrayHelper.Find(array, p), 
                new MemberAccessLevelVerifier(AccessLevels.Public));
            test.AssertStaticMethod<double[], Predicate<double>, double>((array, p) => ArrayHelper.Find(array, p));
            test.Execute();
        }

        [TestMethod("f. ArrayHelper.Contains<T>(T[] array, Predicate<T> p) is a public method"), TestCategory("Exercise 1B")]
        public void ArrayContainsIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertStaticMethod<int[], Predicate<int>, bool>(
                (array, c) => ArrayHelper.Contains(array, c),
                new MemberAccessLevelVerifier(AccessLevels.Public));
            test.AssertStaticMethod<double[], Predicate<double>, bool>((array, c) => ArrayHelper.Contains(array, c));
            test.Execute();
        }

        [TemplatedTestMethod("g. ArrayHelper.Filter<T>(T[] array, Predicate<T> p) can filter out negative numbers"), TestCategory("Exercise 1B")]
        public void ArrayFilterReturnsCorrectly()
        {
            int[] input = new int[] { -2, -1, 0, 1, 2 };

            int[] expectedOutput = new int[] { 0, 1, 2 };
            int[] actualOutput = ArrayHelper.Filter(input, x => x >= 0);

            Assert.IsTrue(actualOutput.SequenceEqual(expectedOutput));
        }

        [TemplatedTestMethod("h. ArrayHelper.Map<T1, T2>(T1[] array, Func<T1, T2> f) can convert multiply number by 2"), TestCategory("Exercise 1B")]
        public void ArrayMapReturnsCorrectly()
        {
            int[] input = new int[] { 0, 1, 2 };

            int[] expectedOutput = new int[] { 0, 2, 4 };
            int[] actualOutput = ArrayHelper.Map(input, x => 2*x);

            Assert.IsTrue(actualOutput.SequenceEqual(expectedOutput));
        }

        [TemplatedTestMethod("i. ArrayHelper.Sort<T>(T[] array, Func<T, T, int> f) can sort array of numbers"), TestCategory("Exercise 1B")]
        public void ArraySortReturnsCorrectly()
        {
            int[] input = new int[] { 0, 5, 4, 1, 3, 2 };

            int[] expectedOutput = new int[] { 0, 1, 2, 3, 4, 5 };
            ArrayHelper.Sort(input, (x, y) => y - x);

            Assert.IsTrue(input.SequenceEqual(expectedOutput));
        }

        [TemplatedTestMethod("j. ArrayHelper.Find<T>(T[] array, Predicate<T> p) can a number"), TestCategory("Exercise 1B")]
        public void ArrayFindReturnsCorrectly()
        {
            int[] array = new int[] { 0, 1, 2 };
            Assert.AreEqual(1, ArrayHelper.Find(array, x => x == 1));
        }

        [TemplatedTestMethod("j. ArrayHelper.Contains<T>(T[] array, Predicate<T> p) returns true if predicate equals true"), TestCategory("Exercise 1B")]
        public void ArrayContainsReturnsTrue()
        {
            int[] array = new int[] { 0, 1, 2 };
            Assert.IsTrue(ArrayHelper.Contains(array, x => x == 1));
        }

        [TemplatedTestMethod("j. ArrayHelper.Contains<T>(T[] array, Predicate<T> p) returns false if predicate equals false"), TestCategory("Exercise 1B")]
        public void ArrayContainsReturnsFalse()
        {
            int[] array = new int[] { 0, 1, 2 };
            Assert.IsFalse(ArrayHelper.Contains(array, x => x == 3));
        }
        #endregion
    }
}
