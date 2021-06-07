using Lecture_9_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_9_Tests.TestHelper;
using static OleVanSanten.TestTools.Helpers.StructureHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_9_Tests
{
    [TemplatedTestClass]
    public class Exercise_1_Tests_Template
    {
        #region Exercise 1A
        [TestMethod("SortedCollection<T> implements ICollection<T>"), TestCategory("Exercise 1A")]
        public void SortedCollectionImplementsICollections()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertClass<SortedCollection<int>>(new TypeIsSubclassOfVerifier(typeof(ICollection<int>)));
            test.AssertClass<SortedCollection<double>>(new TypeIsSubclassOfVerifier(typeof(ICollection<double>)));
            test.Execute();
        }

        [TemplatedTestMethod("SortedCollection<T>.Add(T elem) adds element in correct order"), TestCategory("Exercise 1A")]
        public void SortedCollectionAddAddsElementInOrder()
        {
            SortedCollection<int> collection = new SortedCollection<int>();

            collection.Add(3);
            collection.Add(5);
            collection.Add(2);

            Assert.IsTrue(collection.SequenceEqual(new int[] { 2, 3, 5 }));
        }

        [TemplatedTestMethod("SortedCollection<T>.Clear() removes all elements"), TestCategory("Exercise 1A")]
        public void SortedCollectionClearRemovesAllElements()
        {
            SortedCollection<int> collection = new SortedCollection<int>() { 1, 2, 3 };

            collection.Clear();

            Assert.IsFalse(collection.Any());
        }

        [TemplatedTestMethod("SortedCollection<T>.Contains(T elem) returns true if element in collection"), TestCategory("Exercise 1A")]
        public void SortedCollectionContainsReturnsTrue()
        {
            SortedCollection<int> collection = new SortedCollection<int>() { 1, 2, 3 };
            Assert.IsTrue(collection.Contains(2));
        }

        [TemplatedTestMethod("SortedCollection<T>.Contains(T elem) returns false if element is not in collection"), TestCategory("Exercise 1A")]
        public void SortedCollectionContainsReturnsFalse()
        {
            SortedCollection<int> collection = new SortedCollection<int>() { 1, 2, 3 };
            Assert.IsFalse(collection.Contains(4));
        }

        [TemplatedTestMethod("SortedCollection<T>.CopyTo(T[] arr, int i) copies elements to array"), TestCategory("Exercise 1A")]
        public void SortedCollectionCopyToCopiesElementsToArray()
        {
            SortedCollection<int> collection = new SortedCollection<int>() { 2, 3, 5 };
            int[] array = new int[3];

            collection.CopyTo(array, 0);

            Assert.IsTrue(array.SequenceEqual(new int[] { 2, 3, 5 }));
        }

        [TemplatedTestMethod("SortedCollection<T>.Remove(T elem) removes element"), TestCategory("Exercise 1A")]
        public void SortedCollectionRemoveRemovesElement()
        {
            SortedCollection<int> collection = new SortedCollection<int>() { 1, 2, 3 };

            collection.Remove(1);

            Assert.IsTrue(collection.SequenceEqual(new int[] { 2, 3 }));
        }
        #endregion

        #region Exercise 1B
        [TestMethod("SortedCollection<T>[int index] is a readonly property"), TestCategory("Exercise 1B")]
        public void SortedCollectionIsAReadonlyProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicReadonlyProperty(GetIndexProperty<SortedCollection<int>>());
            test.Execute();
        }

        [TemplatedTestMethod("SortedCollection<T>[int index] returns correct element"), TestCategory("Exercise 1B")]
        public void SortedCollectionReturnsCorrectElement()
        {
            SortedCollection<int> collection = new SortedCollection<int>() { 1, 2, 3 };
            Assert.AreEqual(collection[1], 2);
        }
        #endregion

        #region Exercise 1C
        [TemplatedTestMethod("SortedCollection<T>.GetAll() returns enumeration of collection"), TestCategory("Exercise 1C")]
        public void SortCollectionGetAllReturnsEnumerationOfCollection()
        {
            SortedCollection<int> collection = new SortedCollection<int>() { 1, 2, 3 };
            Assert.IsTrue(collection.GetAll().SequenceEqual(new int[] { 1, 2, 3 }));
        }

        [TestMethod("SortedCollection<T>.GetAllReversed() returns reversed enumeration of collection"), TestCategory("Exercise 1C")]
        public void SortedCollectionGetAllReversedReturnsReversedEnumerationOfCollection()
        {
            SortedCollection<int> collection = new SortedCollection<int>() { 1, 2, 3 };
            Assert.IsTrue(collection.GetAllReversed().SequenceEqual(new int[] { 3, 2, 1 }));
        }
        #endregion

        #region Exercise 1D
        [TemplatedTestMethod("SortedCollection<T>.GetAll(Predicate<T> p) returns enumeration of collection"), TestCategory("Exercise 1D")]
        public void SortCollectionGetAllReturnsEnumerationOfCollection2()
        {
            SortedCollection<int> collection = new SortedCollection<int>() { 1, 2, 3, 4 };
            Assert.IsTrue(collection.GetAll(x => x % 2 == 0).SequenceEqual(new int[] { 2, 4 }));
        }

        [TemplatedTestMethod("SortedCollection<T>.GetAllReversed(Predicate<T> p) returns reversed enumeration of collection"), TestCategory("Exercise 1D")]
        public void SortedCollectionGetAllReversedReturnsReversedEnumerationOfCollection2()
        {
            SortedCollection<int> collection = new SortedCollection<int>() { 1, 2, 3, 4 };
            Assert.IsTrue(collection.GetAllReversed(x => x % 2 == 0).SequenceEqual(new int[] { 4, 2 }));
        }
        #endregion
    }
}
