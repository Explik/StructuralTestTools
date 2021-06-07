using Lecture_7_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_7_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_7_Tests
{
    [TemplatedTestClass]
    public class Exercise_3_Tests_Template 
    {
        #region Exercise 3A
        [TestMethod("a. Dog has a public default constructor"), TestCategory("Exercise 3A")]
        public void DogHasPublicDefaultConstructor()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicConstructor<Dog>(() => new Dog());
            test.Execute();
        }

        [TestMethod("b. Dog.ID is public property"), TestCategory("Exercise 3A")]
        public void DogIDIsPublicProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Dog, int>(d => d.ID);
            test.Execute();
        }

        [TestMethod("c. Dog.Name has is public property"), TestCategory("Exercise 3A")]
        public void DogNameIsPublicProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Dog, string>(d => d.Name);
            test.Execute();
        }

        [TestMethod("d. Dog.Breed has is public property"), TestCategory("Exercise 3A")]
        public void DogBreedIsPublicProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Dog, string>(d => d.Breed);
            test.Execute();
        }

        [TestMethod("e. Dog.Age has is public property"), TestCategory("Exercise 3A")]
        public void DogAgeIsPublicProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Dog, int>(d => d.Age);
            test.Execute();
        }

        [TemplatedTestMethod("f. Dog.ID = -1 throws ArgumentException"), TestCategory("Exercise 3A")]
        public void DogIDAssignmentOfMinus1ThrowsArgumentException()
        {
            Dog dog = new Dog();
            Assert.ThrowsException<ArgumentException>(() => dog.ID = -1);
        }

        [TemplatedTestMethod("g. Dog.Age = -1 throws ArgumentException"), TestCategory("Exercise 3A")]
        public void DogAgeAssignmentOfMinus1ThrowsArgumentException()
        {
            Dog dog = new Dog();
            Assert.ThrowsException<ArgumentException>(() => dog.Age = -1);
        }
        #endregion

        #region Exercise 3B
        [TestMethod("a. Dog implements ICloneable"), TestCategory("Exercise 3B")]
        public void MyQueueCountIsReadOnlyIntProoerty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertClass<Dog>(new TypeIsSubclassOfVerifier(typeof(ICloneable)));
            test.Execute();
        }

        [TemplatedTestMethod("b. Dog.Clone() clones fields"), TestCategory("Exercise 3B")]
        public void MyQueueCountIsInitializedAs0()
        {
            Dog dog = new Dog()
            {
                ID = 5,
                Name = "Buddy",
                Breed = "Labrador",
                Age = 4
            };

            Dog clonedDog = (Dog)dog.Clone();

            Assert.AreEqual(5, clonedDog.ID);
            Assert.AreEqual("Buddy", clonedDog.Name);
            Assert.AreEqual("Labrador", clonedDog.Breed);
            Assert.AreEqual(4, clonedDog.Age);
        }
        #endregion

        #region Exercise 3C
        [TemplatedTestMethod("a. Dog.Equals(Dog dog) does not equate dogs with different IDs"), TestCategory("Exercise 3C")]
        public void DogEqualsDoesNotEquateDogsWithDifferentIDs()
        {
            Dog dog1 = new Dog() { ID = 4 };
            Dog dog2 = new Dog() { ID = 5 };

            Assert.IsFalse(dog1.Equals(dog2));
        }

        [TemplatedTestMethod("b. Dog.Equals(Dog dog) equates dogs with same IDs"), TestCategory("Exercise 3C")]
        public void DogEqualsEquatesDogsWithSameIDs()
        {
            Dog dog1 = new Dog() { ID = 5 };
            Dog dog2 = new Dog() { ID = 5 };

            Assert.IsTrue(dog1.Equals(dog2));
        }

        [TemplatedTestMethod("c. Dog.GetHashCode() does not equate dogs with different IDs"), TestCategory("Exercise 3C")]
        public void DogGetHashCodeDoesNotEquateDogsWithDifferentIDs()
        {
            Dog dog1 = new Dog() { ID = 4 };
            Dog dog2 = new Dog() { ID = 5 };

            Assert.IsTrue(dog1.GetHashCode() != dog2.GetHashCode());
        }

        [TemplatedTestMethod("d. Dog.GetHashCode() equates dogs with same IDs"), TestCategory("Exercise 3C")]
        public void DogGetHashCodeEquatesDogsWithSameIDs()
        {
            Dog dog1 = new Dog() { ID = 5 };
            Dog dog2 = new Dog() { ID = 5 };

            Assert.IsTrue(dog1.GetHashCode() == dog2.GetHashCode());
        }
        #endregion

        #region Exercise 3D
        [TemplatedTestMethod("a. Dog.ToString() returns expected result"), TestCategory("Exercise 3D")]
        public void DogToStringReturnsExpectedResult()
        {
            Dog dog = new Dog()
            {
                ID = 3,
                Name = "Bella"
            };
            Assert.AreEqual("Dog Bella (3)", dog.ToString());
        }
        #endregion
    }
}
