using Lecture_2_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_2_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_2_Tests
{
    [TemplatedTestClass]
    public class Exercise_1_Tests_Template
    {
        private string CreateName(int length)
        {
            string buffer = "";

            for (int i = 0; i < length; i++)
                buffer += "a";

            return buffer;
        }

        #region Exercise 1A
        [TestMethod("a. Person.FirstName is public string property"), TestCategory("Exercise 1A")]
        public void FirstNameIsPublicStringProperty() {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Person, string>(p => p.FirstName);
            test.Execute();
        }

        [TestMethod("b. Person.LastName is public string property"), TestCategory("Exercise 1A")]
        public void LastNameIsPublicStringProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Person, string>(p => p.LastName);
            test.Execute();
        }

        [TestMethod("c. Person.Age is public int property"), TestCategory("Exercise 1A")]
        public void AgeIsPublicIntProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Person, int>(p => p.Age);
            test.Execute();
        }

        [TemplatedTestMethod("d. Person.FirstName is initialized as \"Unknown\""), TestCategory("Exercise 1A")]
        public void FirstNameIsInitializedAsUnnamed()
        {
            Person person = new Person();
            Assert.AreEqual("Unknown", person.FirstName);
        }

        [TemplatedTestMethod("e. Person.FirstName is initialized as \"Unknown\""), TestCategory("Exercise 1A")]
        public void LastNameIsInitializedAsUnnamed()
        {
            Person person = new Person();
            Assert.AreEqual("Unknown", person.LastName);
        }

        [TemplatedTestMethod("f. Person.FirstName ignores assigment of null"), TestCategory("Exercise 1A")]
        public void FirstNameIgnoresAssignmentOfNull() 
        {
            Person person = new Person();
            person.FirstName = null;
            Assert.AreEqual("Unknown", person.FirstName);
        }

        [TemplatedTestMethod("g. Person.LastName ignores assigment of null"), TestCategory("Exercise 1A")]
        public void LastNameIgnoresAssignmentOfNull()
        {
            Person person = new Person();
            person.LastName = null;
            Assert.AreEqual("Unknown", person.LastName);
        }

        [TemplatedTestMethod("h. Person.FirstName ignores assigment of \"123456789\""), TestCategory("Exercise 1A")]
        public void FirstNameIgnoresAssignmentOf012345689()
        {
            Person person = new Person();
            person.FirstName = "123456789";
            Assert.AreEqual("Unknown", person.FirstName);
        }

        [TemplatedTestMethod("i. Person.LastName ignores assigment of \"123456789\""), TestCategory("Exercise 1A")]
        public void LastNameIgnoresAssignmentOf012345689()
        {
            Person person = new Person();
            person.LastName = "123456789";
            Assert.AreEqual("Unknown", person.LastName);
        }

        [TemplatedTestMethod("j. Person.FirstName ignores assigment of string with length 101"), TestCategory("Exercise 1A")]
        public void FirstNameIgnoresAssignmentOfStringWithLength101()
        {
            Person person = new Person();
            person.FirstName = CreateName(101);
            Assert.AreEqual("Unknown", person.FirstName);
        }

        [TemplatedTestMethod("k. Person.LastName ignores assignment of string with length 101"), TestCategory("Exercise 1A")]
        public void LastNameIgnoresAssignmentOfStringWithLength101()
        {
            Person person = new Person();
            person.LastName = CreateName(101);
            Assert.AreEqual("Unknown", person.LastName);
        }

        [TemplatedTestMethod("l. Person.Age is initialized as 0"), TestCategory("Exercise 1A")]
        public void AgeIsInitilizedAs0()
        {
            Person person = new Person();
            Assert.AreEqual(0, person.Age);
        }

        [TemplatedTestMethod("m. Person.Age ignores assigment of -1"), TestCategory("Exercise 1A")]
        public void AgeIgnoresAssignmentOfMinusOne()
        {
            Person person = new Person();
            person.Age = -1;
            Assert.AreEqual(0, person.Age);
        }
        #endregion

        #region Exercise 1B
        [TestMethod("a. Person.Mother is public Person property"), TestCategory("Exercise 1B")]
        public void MotherIsPublicPersonProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Person, Person>(p => p.Mother);
            test.Execute();
        }

        [TestMethod("b. Person.Father is public Person property"), TestCategory("Exercise 1B")]
        public void FatherIsPublicPersonProperty() {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Person, Person>(p => p.Father);
            test.Execute();
        }

        [TemplatedTestMethod("c. Person.Mother is initialized as null"), TestCategory("Exercise 1B")]
        public void MotherIsInitilizedAsnull()
        {
            Person person = new Person();
            Assert.IsNull(person.Mother);
        }

        [TemplatedTestMethod("d. Person.Father is initialized as null"), TestCategory("Exercise 1B")]
        public void FatherIsInitilizedAsnull()
        {
            Person person = new Person();
            Assert.IsNull(person.Father);
        }

        [TemplatedTestMethod("c. Person.Mother ignores assigment if mother is younger than child"), TestCategory("Exercise 1B")]
        public void MotherIgnoresAssigmentIfMotherIsYoungerThanChild()
        {
            Person mother = new Person() { Age = 0 };
            Person child = new Person() { Age = 1 };
            
            child.Mother = mother;
            
            Assert.IsNull(child.Mother);
        }
        
        [TemplatedTestMethod("d. Person.Father ignores assigment if mother is younger than child"), TestCategory("Exercise 1B")]
        public void FatherIgnoresAssigmentIfMotherIsYoungerThanChild()
        {
            Person father = new Person() { Age = 0 };
            Person child = new Person() { Age = 1 };
            
            child.Father = father;
            
            Assert.IsNull(child.Father);
        }
        #endregion

        #region Exercise 1C
        [TestMethod("a. PersonGenerator.GeneratePerson takes no arguments and returns Person"), TestCategory("Exercise 1C")]
        public void GeneratePersonReturnsPerson()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<PersonGenerator, Person>(g => g.GeneratePerson());
            test.Execute();
        }

        [TemplatedTestMethod("b. PersonGenerator.GeneratePerson generates Adam Smith (36)"), TestCategory("Exercise 1C")]
        public void GeneratePersonCreatesAdamSmith()
        {
            PersonGenerator generator = new PersonGenerator();
            Person person = generator.GeneratePerson();

            Assert.AreEqual("Adam", person.FirstName);
            Assert.AreEqual("Smith", person.LastName);
            Assert.AreEqual(36, person.Age);
        }
        #endregion

        #region Exercise 1D
        [TestMethod("a. PersonGenerator.GenerateFamily takes no arguments and returns Person "), TestCategory("Exercise 1D")]
        public void GenerateFamilyReturnsPerson()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<PersonGenerator, Person>(g => g.GenerateFamily());
            test.Execute();
        }

        [TemplatedTestMethod("b. PersonGenerator.GenerateFamily generates Robin Rich (10) as child"), TestCategory("Exercise 1D")]
        public void GenerateFamilyCreatesRobinRichAsChild()
        {
            PersonGenerator generator = new PersonGenerator();
            Person person = generator.GenerateFamily();

            Assert.AreEqual("Robin", person.FirstName);
            Assert.AreEqual("Rich", person.LastName);
            Assert.AreEqual(10, person.Age);
        }

        [TemplatedTestMethod("c. PersonGenerator.GenerateFamily generates Warren Rich (36) as father"), TestCategory("Exercise 1D")]
        public void GenerateFamilyCreatesRobinRichAsFather()
        {
            PersonGenerator generator = new PersonGenerator();
            Person father = generator.GenerateFamily().Father;

            Assert.AreEqual("Warren", father.FirstName);
            Assert.AreEqual("Rich", father.LastName);
            Assert.AreEqual(36, father.Age);
        }

        [TemplatedTestMethod("d. PersonGenerator.GenerateFamily generates Anna Smith (38) as mother"), TestCategory("Exercise 1D")]
        public void GenerateFamilyCreatesAnnaRichAsMother()
        {
            PersonGenerator generator = new PersonGenerator();
            Person mother = generator.GenerateFamily().Mother;

            Assert.AreEqual("Anna", mother.FirstName);
            Assert.AreEqual("Smith", mother.LastName);
            Assert.AreEqual(38, mother.Age);
        }

        [TemplatedTestMethod("e. PersonGenerator.GenerateFamily generates Gustav Rich (66) as grandfather"), TestCategory("Exercise 1D")]
        public void GenerateFamilyCreatesGustavRichAsGrandfather()
        {
            PersonGenerator generator = new PersonGenerator();
            Person grandFather = generator.GenerateFamily().Father.Father;

            Assert.AreEqual("Gustav", grandFather.FirstName);
            Assert.AreEqual("Rich", grandFather.LastName);
            Assert.AreEqual(66, grandFather.Age);
        }

        [TemplatedTestMethod("f. PersonGenerator.GenerateFamily generates Elsa Johnson (65) as grandmother"), TestCategory("Exercise 1D")]
        public void GenerateFamilyCreatesElsaJohnsonAsGrandMother()
        {
            PersonGenerator generator = new PersonGenerator();
            Person grandMother = generator.GenerateFamily().Father.Mother;

            Assert.AreEqual("Elsa", grandMother.FirstName);
            Assert.AreEqual("Johnson", grandMother.LastName);
            Assert.AreEqual(65, grandMother.Age);
        }
        #endregion

        #region Exercise 1E
        [TestMethod("a. PersonPrinter.PrintPerson takes person as argument and returns nothing"), TestCategory("Exercise 1E")]
        public void PrintPersonTakesPersonAsArgumentAndReturnsNothing() {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<PersonPrinter, Person>((p1, p2) => p1.PrintPerson(p2));
            test.Execute();
        }

        [TemplatedTestMethod("b. PersonPrinter.PrintPrints prints correctly"), TestCategory("Exercise 1E")]
        public void PrintPersonPrintsCorrectly()
        {
            // Extended MSTest 
            Person person = new Person()
            {
                FirstName = "Adam",
                LastName = "Smith",
                Age = 36
            };
            PersonPrinter printer = new PersonPrinter();

            ConsoleAssert.WritesOut(
                () => printer.PrintPerson(person), 
                "Adam Smith (36)");
        }
        #endregion

        #region Exercise 1F
        [TestMethod("a. PersonPrinter.PrintFamily takes person as argument and returns nothing"), TestCategory("Exercise 1F")]
        public void PrintFamilyTakesPersonAsArgumentAndReturnsNothing()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<PersonPrinter, Person>((p1, p2) => p1.PrintFamily(p2));
            test.Execute();
        }

        [TemplatedTestMethod("b. PersonPrinter.PrintFamily prints correctly"), TestCategory("Exercise 1F")]
        public void PrintFamilyPrintsCorrectly()
        {
            // Extended MSTest 
            Person person = new Person()
            {
                FirstName = "Robin",
                LastName = "Rich",
                Age = 10,
                Mother = new Person()
                {
                    FirstName = "Anna",
                    LastName = "Smith",
                    Age = 38
                },
                Father = new Person()
                {
                    FirstName = "Warren",
                    LastName = "Rich",
                    Age = 36,
                    Mother = new Person()
                    {
                        FirstName = "Elsa",
                        LastName = "Johnson",
                        Age = 65
                    },
                    Father = new Person()
                    {
                        FirstName = "Gustav",
                        LastName = "Rich",
                        Age = 66
                    }
                }
            };
            PersonPrinter printer = new PersonPrinter();

            string expectedOutput = string.Join(
                Environment.NewLine,
                "Robin Rich (10)",
                "Warren Rich (36)",
                "Gustav Rich (66)",
                "Elsa Johnson (65)",
                "Anna Smith (38)");
            ConsoleAssert.WritesOut(() => printer.PrintFamily(person), expectedOutput);
        }
        #endregion

        #region Exercise 1G
        [TestMethod("a. Person has constructor which takes no arguments"), TestCategory("Exercise 1G")]
        public void PersonHasConstructorWhichTakesNoArguments()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertConstructor<Person>(
                () => new Person(),
                new MemberAccessLevelVerifier(AccessLevels.Public));
            test.Execute();
        }

        [TestMethod("b. Person has constructor which two persons as arguments"), TestCategory("Exercise 1G")]
        public void PersonHasconstructorWhichTakesTwoPersonsAsArguments()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertConstructor<Person, Person, Person>(
                (p1, p2) => new Person(p1, p2), 
                new MemberAccessLevelVerifier(AccessLevels.Public));
            test.Execute();
        }

        [TemplatedTestMethod("c. Person constructor with 2 persons as arguments sets mother and father property"), TestCategory("Exercise 1G")]
        public void PersonConstructorWithTwoPersonArgumentsSetsMotherAndFatherProperty()
        {
            Person mother = new Person() { Age = 37 };
            Person father = new Person() { Age = 37 };
            Person child = new Person(mother, father);

            Assert.AreSame(mother, child.Mother);
            Assert.AreSame(father, child.Father);
        }
        #endregion

        #region Exercise 1H
        [TestMethod("a. Person.ID is public read-only int property"), TestCategory("Exercise 1H")]
        public void IDIsPublicReadonlyIntProperty() {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicReadonlyProperty<Person, int>(p => p.ID);
            test.Execute();
        }

        [TemplatedTestMethod("b. Person.ID increases by 1 for each new person"), TestCategory("Exercise 1H")]
        public void IDIncreasesByOneForEachNewPerson()
        {
            Person person1 = new Person();
            Person person2 = new Person();

            Assert.IsTrue(person1.ID + 1 == person2.ID);
        }
        #endregion
    }
}
