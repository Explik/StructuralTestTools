using Lecture_7_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_7_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_7_Tests
{
    [TemplatedTestClass]
    public class Exercise_2_Tests_Template
    {
        #region Exercise 2A
        [TestMethod("a. MyQueue<T> is a public class"), TestCategory("Exercise 2A")]
        public void MyQueueIsAPublicClass()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicClass<MyQueue<int>>();
            test.AssertPublicClass<MyQueue<string>>();
            test.Execute();
        }
        #endregion

        #region Exercise 2B
        [TestMethod("a. MyQueue<T> has constructor which takes int"), TestCategory("Exercise 2B")]
        public void MyQueueHasConstructorWhichTakesInt()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicConstructor<int, MyQueue<string>>(i => new MyQueue<string>(i));
            test.Execute();
        }

        [TestMethod("b. MyQueue<T>.MaxCount is public read-only property"), TestCategory("Exercise 2B")]
        public void MyQueueMaxCountIsPublicReadonlyProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicReadonlyProperty<MyQueue<int>, int>(q => q.MaxCount);
            test.Execute();
        }

        [TemplatedTestMethod("c. MyQueue(int value) sets MaxCount equal to value"), TestCategory("Exercise 2B")]
        public void MyQueueConstructorSetsMaxCount()
        {
            MyQueue<int> queue = new MyQueue<int>(5);
            Assert.AreEqual(5, queue.MaxCount);
        }
        #endregion

        #region Exercise 2C
        [TestMethod("a. MyQueue<T>.Count is read-only int property"), TestCategory("Exercise 2C")]
        public void MyQueueCountIsReadOnlyIntProoerty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicReadonlyProperty<MyQueue<int>, int>(q => q.Count);
            test.Execute();
        }

        [TemplatedTestMethod("b. MyQueue<T>.Count is initialized as 0"), TestCategory("Exercise 2C")]
        public void MyQueueCountIsInitializedAs0()
        {
            MyQueue<int> queue = new MyQueue<int>(5);
            Assert.AreEqual(0, queue.Count);
        }
        #endregion

        #region Exercise 2D
        [TestMethod("a. MyQueue<T>.Enqueue() takes T and returns nothing"), TestCategory("Exercise 2D")]
        public void MyQueueEnqueueTakesTAndReturnsNothing()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<MyQueue<int>, int>(q => q.Peek());
            test.AssertPublicMethod<MyQueue<double>, double>(q => q.Peek());
            test.Execute();
        }

        [TestMethod("b. MyQueue<T>.Dequeue() takes T and returns nothing"), TestCategory("Exercise 2D")]
        public void MyQueueDequeueTakesNothingAndReturnsNothing()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<MyQueue<int>, int>(q => q.Peek());
            test.AssertPublicMethod<MyQueue<double>, double>(q => q.Peek());
            test.Execute();
        }

        [TemplatedTestMethod("c. MyQueue<T>.Enqueue(T value) increases Count"), TestCategory("Exercise 2D")]
        public void MyQueueEnqueueIncreasesCount()
        {
            MyQueue<int> queue = new MyQueue<int>(5);
            
            queue.Enqueue(1);

            Assert.AreEqual(1, queue.Count);
        }

        [TemplatedTestMethod("d. MyQueue<T>.Dequeue(T value) decreases Count"), TestCategory("Exercise 2D")]
        public void MyQueueDequeueDecreaseCount()
        {
            MyQueue<int> queue = new MyQueue<int>(5);
            
            queue.Enqueue(1);
            queue.Dequeue();
            
            Assert.AreEqual(0, queue.Count);
        }

        [TemplatedTestMethod("e. MyQueue<T>.Enqueue(T value) throws InvalidOperationException if queue is already full"), TestCategory("Exercise 2D")]
        public void MyQueueEnqueueThrowsInvalidOperationException()
        {
            MyQueue<int> queue = new MyQueue<int>(1);
            queue.Enqueue(1);
            Assert.ThrowsException<InvalidOperationException>(() => queue.Enqueue(2));
        }

        [TemplatedTestMethod("f. MyQueue<T>.Enqueue(T value) throws ArgumentException if queue is already empty"), TestCategory("Exercise 2D")]
        public void MyQueueDequeueThrowsInvalidOperationException()
        {
            MyQueue<int> queue = new MyQueue<int>(5);
            Assert.ThrowsException<InvalidOperationException>(() => queue.Dequeue());
        }
        #endregion

        #region Exercise 2E
        [TestMethod("a. MyQueue<T>.Peek() takes nothing and returns T"), TestCategory("Exercise 2E")]
        public void MyQueuePeekIsPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<MyQueue<int>, int>(q => q.Peek());
            test.AssertPublicMethod<MyQueue<double>, double>(q => q.Peek());
            test.Execute();
        }

        [TemplatedTestMethod("b. MyQueue<T>.Peek() returns first element in queue"), TestCategory("Exercise 2E")]
        public void MyQueuePeekReturnsFirstElementInQueue()
        {
            MyQueue<int> queue = new MyQueue<int>(5);

            queue.Enqueue(1);
            queue.Enqueue(2);

            Assert.AreEqual(1, queue.Peek());
        }

        [TemplatedTestMethod("c. MyQueue<T>.Peek() does not modify queue"), TestCategory("Exercise 2E")]
        public void MyQueuePeekDoesNotModifyQueue()
        {
            MyQueue<int> queue = new MyQueue<int>(5);
            
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Peek();

            Assert.AreEqual(1, queue.Peek());
        }
        #endregion
    }
}
