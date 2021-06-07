using Lecture_8_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_8_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_8_Tests
{
    [TemplatedTestClass]
    public class Exercise_5_Tests_Template
    {
        #region Exercise 5A
        [TestMethod("a. Customer.ID is a public property"), TestCategory("Exercise 5A")]
        public void CustomerIDIsAPublicProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Customer, int>(c => c.ID);
            test.Execute();
        }

        [TestMethod("b. Customer.FirstName is a public property"), TestCategory("Exercise 5A")]
        public void CustomerFirstNameIsAPublicProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Customer, string>(c => c.FirstName);
            test.Execute();
        }

        [TestMethod("c. Customer.LastName is a public property"), TestCategory("Exercise 5A")]
        public void CustomerLastNameIsAPublicProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Customer, string>(c => c.LastName);
            test.Execute();
        }
        #endregion

        #region exercise 5B
        [TemplatedTestMethod("a. Customer.ID emits PropertyChanged event on new value"), TestCategory("Exercise 5B")]
        public void CustomerIDEmitsPropertyChangedEvent()
        {
            bool isCalled = false;
            Customer customer = new Customer()
            {
                ID = 0
            };
            customer.PropertyChanged += (sender, e) => isCalled = true;

            customer.ID = 1;

            Assert.IsTrue(isCalled, "The Customer.ID event is never emitted");
        }

        [TemplatedTestMethod("b. Customer.ID does not emit PropertyChanged event on same value"), TestCategory("Exercise 5B")]
        public void CustomerIDDoesNotEmitPropertyChangedEvent()
        {
            bool isCalled = false;
            Customer customer = new Customer()
            {
                ID = 0
            };
            customer.PropertyChanged += (sender, e) => isCalled = true;

            customer.ID = 0;

            Assert.IsFalse(isCalled, "The Customer.ID event is emitted");
        }

        [TemplatedTestMethod("c. Customer.FirstName emits PropertyChanged event on new value"), TestCategory("Exercise 5B")]
        public void CustomerFirstNameEmitsPropertyChangedEvent()
        {
            bool isCalled = false;
            Customer customer = new Customer()
            {
                FirstName = "abc"
            };
            customer.PropertyChanged += (sender, e) => isCalled = true;

            customer.FirstName = "bcd";

            Assert.IsTrue(isCalled, "The Customer.ID event is never emitted");
        }

        [TemplatedTestMethod("d. Customer.FirstName does not emit PropertyChanged event on same value"), TestCategory("Exercise 5B")]
        public void CustomerFirstNameDoesNotEmitPropertyChangedEvent()
        {
            bool isCalled = false;
            Customer customer = new Customer()
            {
                FirstName = "abc"
            };
            customer.PropertyChanged += (sender, e) => isCalled = true;

            customer.FirstName = "abc";

            Assert.IsFalse(isCalled, "The Customer.ID event is emitted");
        }

        [TemplatedTestMethod("e. Customer.LastName emits PropertyChanged event on new value"), TestCategory("Exercise 5B")]
        public void CustomerLastNameEmitsPropertyChangedEvent()
        {
            bool isCalled = false;
            Customer customer = new Customer()
            {
                LastName = "abc"
            };
            customer.PropertyChanged += (sender, e) => isCalled = true;

            customer.LastName = "bcd";

            Assert.IsTrue(isCalled, "The Customer.ID event is never emitted");
        }

        [TemplatedTestMethod("f. Customer.LastName does not emit PropertyChanged event on same value"), TestCategory("Exercise 5B")]
        public void CustomerLastNameDoesNotEmitPropertyChangedEvent()
        {
            bool isCalled = false;
            Customer customer = new Customer()
            {
                LastName = "abc"
            };
            customer.PropertyChanged += (sender, e) => isCalled = true;

            customer.LastName = "abc";

            Assert.IsFalse(isCalled, "The Customer.ID event is emitted");
        } 
        #endregion
    }
}
