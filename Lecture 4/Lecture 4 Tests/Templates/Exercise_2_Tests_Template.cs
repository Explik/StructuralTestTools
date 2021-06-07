using Lecture_4_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static OleVanSanten.TestTools.Expressions.TestExpression;
using static Lecture_4_Tests.TestHelper;
using static OleVanSanten.TestTools.Helpers.StructureHelper;

namespace Lecture_4_Tests
{
    [TemplatedTestClass]
    public class Exercise_2_Tests_Template
    {
        #region Exercise 2A
        [TestMethod("a. Person.Name is public readonly string property"), TestCategory("Exercise 2A")]
        public void PersonNameIsReadonlyStringProperty() 
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicReadonlyProperty<Person, string>(p => p.Name);
            test.Execute();
        }

        [TestMethod("b. Person.Height is public double property"), TestCategory("Exercise 2A")]
        public void PersonHeightIsPublicDoubleProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Person, double>(p => p.Height);
            test.Execute();
        }

        [TestMethod("c. Person.Weight is public double property"), TestCategory("Exercise 2A")]
        public void PersonWeightIsPublicDoubleProperty() 
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Person, double>(p => p.Weight);
            test.Execute();
        }

        [TemplatedTestMethod("d. Person(string name) assigns Name property"), TestCategory("Exercise 2A")]
        public void PersonConstructorAssignsNameProperty()
        {
            Person person = new Person("abc");
            Assert.AreEqual("abc", person.Name);
        }

        [TemplatedTestMethod("e. Person constructor throws ArgumentNullException if called with null"), TestCategory("Exercise 2A")]
        public void PersonConstructorThrowsArgumentNullExceptionIfCalledWithNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Person(null));
        }

        [TemplatedTestMethod("f. Person.Height throws ArgumentException on assignment of -1.0"), TestCategory("Exercise 2A")]
        public void PersonHeightIgnoresAssignmentOfMinusOne()
        {
            Person person = new Person("abc");
            Assert.ThrowsException<ArgumentException>(() => person.Height = -0.1);
        }
            

        [TemplatedTestMethod("g. Person.Weight throws ArgumentException on assignment of -1.0"), TestCategory("Exercise 2A")]
        public void PersonWeightIgnoresAssignmentOfMinusOne()
        {
            Person person = new Person("abc");
            Assert.ThrowsException<ArgumentException>(() => person.Weight = -0.1);
        }
        #endregion

        #region Exercise 2B
        [TemplatedTestMethod("a. Person.CalculateBMI() returns expected output"), TestCategory("Exercise 2B")]
        public void PersonCalculateBMIReturnsExpectedOutput()
        {
            double expectedBMI = 80 / (1.80 * 1.80);

            Person person = new Person("abc")
            {
                Height = 1.80,
                Weight = 80,
                Age = 18
            };
            Assert.AreEqual(expectedBMI, person.CalculateBMI(), 0.001);
        }
        #endregion

        #region Exercise 2C
        [TemplatedTestMethod("a. Person.GetClassification() returns \"under-weight\" for Height = 1.64 & Weight = 47.0"), TestCategory("Exercise 2C")]
        public void PersonGetClassificationReturnsUnderWeight()
        {
            Person person = new Person("abc")
            {
                Height = 1.64,
                Weight = 47.0,
                Age = 18
            };
            Assert.AreEqual("under-weight", person.GetClassification());
        }

        [TemplatedTestMethod("b. Person.GetClassification() returns \"normal weight\" for Height = 1.73 & Weight = 58.0"), TestCategory("Exercise 2C")]
        public void PersonGetClassificationReturnsNormalWeight()
        {
            Person person = new Person("abc")
            {
                Height = 1.73,
                Weight = 58.0,
                Age = 18
            };
            Assert.AreEqual("normal weight", person.GetClassification());
        }

        [TemplatedTestMethod("c. Person.GetClassification() returns \"over-weight\" for Height = 1.70 & Weight = 74.0"), TestCategory("Exercise 2C")]
        public void PersonGetClassificationReturnsOverWeight()
        {
            Person person = new Person("abc")
            {
                Height = 1.70,
                Weight = 74,
                Age = 18
            };
            Assert.AreEqual("over-weight", person.GetClassification());
        }

        [TemplatedTestMethod("d. Person.GetClassification() returns \"obese\" for Height = 1.85 & Weight = 120.0"), TestCategory("Exercise 2C")]
        public void PersonGetClassificationReturnsObese()
        {
            Person person = new Person("abc")
            {
                Height = 1.85,
                Weight = 120.0,
                Age = 18
            };
            Assert.AreEqual("obese", person.GetClassification());
        }
        #endregion
    }
}
