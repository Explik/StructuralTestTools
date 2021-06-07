using Lecture_4_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.Structure;
using OleVanSanten.TestTools.MSTest;
using static Lecture_4_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;
using static OleVanSanten.TestTools.Helpers.StructureHelper;

namespace Lecture_4_Tests
{
    [TemplatedTestClass]
    public class Exercise_7_Tests_Template
    {
        #region Exercise 7A
        [TestMethod("a. NotOldEnoughException's base class is Exception"), TestCategory("Exercise 7A")]
        public void NotOldEnoughExceptionIsSubclassOfException()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertClass<NotOldEnoughException>(new TypeBaseClassVerifier(typeof(Exception)));
            test.Execute();
        }
        #endregion

        #region Exercise 7B
        [TemplatedTestMethod("a. NotOldEnoughException() results in Message = \"Person is too young\""), TestCategory("Exercise 7B")]
        public void ParameterlessPersonConstructorAssignsMessageProperty()
        {
            NotOldEnoughException exception = new NotOldEnoughException();
            Assert.AreEqual("Person is too young", exception.Message);
        }
        #endregion

        #region Exercise 7C
        [TemplatedTestMethod("d. NotOldEnoughException(string activity) results in Message = \"Person is too young to [activity]\""), TestCategory("Exercise 7C")]
        public void PersonConstructorAssignsMessageProperty()
        {
            NotOldEnoughException exception = new NotOldEnoughException("do something");
            Assert.AreEqual("Person is too young to do something", exception.Message);
        }
        #endregion

        #region Exercise 7D
        [TestMethod("a. Person.Age is public int property"), TestCategory("Exercise 7D")]
        public void PersonAgeIsPublicIntProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Person, int>(p => p.Age);
            test.Execute();
        }

        [TemplatedTestMethod("b. Person.CalculateBMI() throws NotOldEnoughException for Age = 15"), TestCategory("Exercise 7D")]
        public void PersonCalculateBMIThrowsNotOldEnoughException()
        {
            Person person = new Person("abc")
            {
                Age = 15
            };
            Assert.ThrowsException<NotOldEnoughException>(() => person.CalculateBMI());
        }
        #endregion
    }
}
