using Lecture_2_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Explik.StructuralTestTools;
using Explik.StructuralTestTools.MSTest;
using static Lecture_2_Tests.TestHelper;

namespace Lecture_2_Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class Exercise_1_Tests
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
        public void FirstNameIsPublicStringProperty()
        {
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

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("d. Person.FirstName is initialized as \"Unknown\""), TestCategory("Exercise 1A")]
        public void FirstNameIsInitializedAsUnnamed()
        {
            Lecture_2.Person person = new Lecture_2.Person();
            Assert.AreEqual("Unknown", person.FirstName);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("e. Person.FirstName is initialized as \"Unknown\""), TestCategory("Exercise 1A")]
        public void LastNameIsInitializedAsUnnamed()
        {
            Lecture_2.Person person = new Lecture_2.Person();
            Assert.AreEqual("Unknown", person.LastName);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("f. Person.FirstName ignores assigment of null"), TestCategory("Exercise 1A")]
        public void FirstNameIgnoresAssignmentOfNull()
        {
            Lecture_2.Person person = new Lecture_2.Person();
            person.FirstName = null;
            Assert.AreEqual("Unknown", person.FirstName);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("g. Person.LastName ignores assigment of null"), TestCategory("Exercise 1A")]
        public void LastNameIgnoresAssignmentOfNull()
        {
            Lecture_2.Person person = new Lecture_2.Person();
            person.LastName = null;
            Assert.AreEqual("Unknown", person.LastName);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("h. Person.FirstName ignores assigment of \"123456789\""), TestCategory("Exercise 1A")]
        public void FirstNameIgnoresAssignmentOf012345689()
        {
            Lecture_2.Person person = new Lecture_2.Person();
            person.FirstName = "123456789";
            Assert.AreEqual("Unknown", person.FirstName);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("i. Person.LastName ignores assigment of \"123456789\""), TestCategory("Exercise 1A")]
        public void LastNameIgnoresAssignmentOf012345689()
        {
            Lecture_2.Person person = new Lecture_2.Person();
            person.LastName = "123456789";
            Assert.AreEqual("Unknown", person.LastName);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("j. Person.FirstName ignores assigment of string with length 101"), TestCategory("Exercise 1A")]
        public void FirstNameIgnoresAssignmentOfStringWithLength101()
        {
            Lecture_2.Person person = new Lecture_2.Person();
            person.FirstName = CreateName(101);
            Assert.AreEqual("Unknown", person.FirstName);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("k. Person.LastName ignores assignment of string with length 101"), TestCategory("Exercise 1A")]
        public void LastNameIgnoresAssignmentOfStringWithLength101()
        {
            Lecture_2.Person person = new Lecture_2.Person();
            person.LastName = CreateName(101);
            Assert.AreEqual("Unknown", person.LastName);
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("l. Person.Age is initialized as 0"), TestCategory("Exercise 1A")]
        public void AgeIsInitilizedAs0()
        {
            // == Failed To Compile ==
            // Person person = new Person();
            // Assert.AreEqual(0, person.Age);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Person.Age is not of type Int32");
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("m. Person.Age ignores assigment of -1"), TestCategory("Exercise 1A")]
        public void AgeIgnoresAssignmentOfMinusOne()
        {
            // == Failed To Compile ==
            // Person person = new Person();
            // person.Age = -1;
            // Assert.AreEqual(0, person.Age);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Person.Age is not of type Int32");
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
        public void FatherIsPublicPersonProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Person, Person>(p => p.Father);
            test.Execute();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("c. Person.Mother is initialized as null"), TestCategory("Exercise 1B")]
        public void MotherIsInitilizedAsnull()
        {
            // == Failed To Compile ==
            // Person person = new Person();
            // Assert.IsNull(person.Mother);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Person does not contain member Mother");
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("d. Person.Father is initialized as null"), TestCategory("Exercise 1B")]
        public void FatherIsInitilizedAsnull()
        {
            // == Failed To Compile ==
            // Person person = new Person();
            // Assert.IsNull(person.Father);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Person does not contain member Father");
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("c. Person.Mother ignores assigment if mother is younger than child"), TestCategory("Exercise 1B")]
        public void MotherIgnoresAssigmentIfMotherIsYoungerThanChild()
        {
            // == Failed To Compile ==
            // Person mother = new Person()
            // {Age = 0};
            // Person child = new Person()
            // {Age = 1};
            // child.Mother = mother;
            // Assert.IsNull(child.Mother);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Person does not contain member Mother");
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("d. Person.Father ignores assigment if mother is younger than child"), TestCategory("Exercise 1B")]
        public void FatherIgnoresAssigmentIfMotherIsYoungerThanChild()
        {
            // == Failed To Compile ==
            // Person father = new Person()
            // {Age = 0};
            // Person child = new Person()
            // {Age = 1};
            // child.Father = father;
            // Assert.IsNull(child.Father);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Person does not contain member Father");
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

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("b. PersonGenerator.GeneratePerson generates Adam Smith (36)"), TestCategory("Exercise 1C")]
        public void GeneratePersonCreatesAdamSmith()
        {
            // == Failed To Compile ==
            // PersonGenerator generator = new PersonGenerator();
            // Person person = generator.GeneratePerson();
            // Assert.AreEqual("Adam", person.FirstName);
            // Assert.AreEqual("Smith", person.LastName);
            // Assert.AreEqual(36, person.Age);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Namespace Lecture_2 does not contain the type PersonGenerator");
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

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("b. PersonGenerator.GenerateFamily generates Robin Rich (10) as child"), TestCategory("Exercise 1D")]
        public void GenerateFamilyCreatesRobinRichAsChild()
        {
            // == Failed To Compile ==
            // PersonGenerator generator = new PersonGenerator();
            // Person person = generator.GenerateFamily();
            // Assert.AreEqual("Robin", person.FirstName);
            // Assert.AreEqual("Rich", person.LastName);
            // Assert.AreEqual(10, person.Age);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Namespace Lecture_2 does not contain the type PersonGenerator");
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("c. PersonGenerator.GenerateFamily generates Warren Rich (36) as father"), TestCategory("Exercise 1D")]
        public void GenerateFamilyCreatesRobinRichAsFather()
        {
            // == Failed To Compile ==
            // PersonGenerator generator = new PersonGenerator();
            // Person father = generator.GenerateFamily().Father;
            // Assert.AreEqual("Warren", father.FirstName);
            // Assert.AreEqual("Rich", father.LastName);
            // Assert.AreEqual(36, father.Age);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Namespace Lecture_2 does not contain the type PersonGenerator");
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("d. PersonGenerator.GenerateFamily generates Anna Smith (38) as mother"), TestCategory("Exercise 1D")]
        public void GenerateFamilyCreatesAnnaRichAsMother()
        {
            // == Failed To Compile ==
            // PersonGenerator generator = new PersonGenerator();
            // Person mother = generator.GenerateFamily().Mother;
            // Assert.AreEqual("Anna", mother.FirstName);
            // Assert.AreEqual("Smith", mother.LastName);
            // Assert.AreEqual(38, mother.Age);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Namespace Lecture_2 does not contain the type PersonGenerator");
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("e. PersonGenerator.GenerateFamily generates Gustav Rich (66) as grandfather"), TestCategory("Exercise 1D")]
        public void GenerateFamilyCreatesGustavRichAsGrandfather()
        {
            // == Failed To Compile ==
            // PersonGenerator generator = new PersonGenerator();
            // Person grandFather = generator.GenerateFamily().Father.Father;
            // Assert.AreEqual("Gustav", grandFather.FirstName);
            // Assert.AreEqual("Rich", grandFather.LastName);
            // Assert.AreEqual(66, grandFather.Age);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Namespace Lecture_2 does not contain the type PersonGenerator");
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("f. PersonGenerator.GenerateFamily generates Elsa Johnson (65) as grandmother"), TestCategory("Exercise 1D")]
        public void GenerateFamilyCreatesElsaJohnsonAsGrandMother()
        {
            // == Failed To Compile ==
            // PersonGenerator generator = new PersonGenerator();
            // Person grandMother = generator.GenerateFamily().Father.Mother;
            // Assert.AreEqual("Elsa", grandMother.FirstName);
            // Assert.AreEqual("Johnson", grandMother.LastName);
            // Assert.AreEqual(65, grandMother.Age);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Namespace Lecture_2 does not contain the type PersonGenerator");
        }

#endregion
#region Exercise 1E
        [TestMethod("a. PersonPrinter.PrintPerson takes person as argument and returns nothing"), TestCategory("Exercise 1E")]
        public void PrintPersonTakesPersonAsArgumentAndReturnsNothing()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<PersonPrinter, Person>((p1, p2) => p1.PrintPerson(p2));
            test.Execute();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("b. PersonPrinter.PrintPrints prints correctly"), TestCategory("Exercise 1E")]
        public void PrintPersonPrintsCorrectly()
        {
            // == Failed To Compile ==
            // // Extended MSTest 
            // Person person = new Person()
            // {FirstName = "Adam", LastName = "Smith", Age = 36};
            // PersonPrinter printer = new PersonPrinter();
            // ConsoleAssert.WritesOut(() => printer.PrintPerson(person), "Adam Smith (36)");
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Namespace Lecture_2 does not contain the type PersonPrinter");
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

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("b. PersonPrinter.PrintFamily prints correctly"), TestCategory("Exercise 1F")]
        public void PrintFamilyPrintsCorrectly()
        {
            // == Failed To Compile ==
            // // Extended MSTest 
            // Person person = new Person()
            // {FirstName = "Robin", LastName = "Rich", Age = 10, Mother = new Person()
            // {FirstName = "Anna", LastName = "Smith", Age = 38}, Father = new Person()
            // {FirstName = "Warren", LastName = "Rich", Age = 36, Mother = new Person()
            // {FirstName = "Elsa", LastName = "Johnson", Age = 65}, Father = new Person()
            // {FirstName = "Gustav", LastName = "Rich", Age = 66}}};
            // PersonPrinter printer = new PersonPrinter();
            // string expectedOutput = string.Join(Environment.NewLine, "Robin Rich (10)", "Warren Rich (36)", "Gustav Rich (66)", "Elsa Johnson (65)", "Anna Smith (38)");
            // ConsoleAssert.WritesOut(() => printer.PrintFamily(person), expectedOutput);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Namespace Lecture_2 does not contain the type PersonPrinter");
        }

#endregion
#region Exercise 1G
        [TestMethod("a. Person has constructor which takes no arguments"), TestCategory("Exercise 1G")]
        public void PersonHasConstructorWhichTakesNoArguments()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertConstructor<Person>(() => new Person(), new MemberAccessLevelVerifier(AccessLevels.Public));
            test.Execute();
        }

        [TestMethod("b. Person has constructor which two persons as arguments"), TestCategory("Exercise 1G")]
        public void PersonHasconstructorWhichTakesTwoPersonsAsArguments()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertConstructor<Person, Person, Person>((p1, p2) => new Person(p1, p2), new MemberAccessLevelVerifier(AccessLevels.Public));
            test.Execute();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("c. Person constructor with 2 persons as arguments sets mother and father property"), TestCategory("Exercise 1G")]
        public void PersonConstructorWithTwoPersonArgumentsSetsMotherAndFatherProperty()
        {
            // == Failed To Compile ==
            // Person mother = new Person()
            // {Age = 37};
            // Person father = new Person()
            // {Age = 37};
            // Person child = new Person(mother, father);
            // Assert.AreSame(mother, child.Mother);
            // Assert.AreSame(father, child.Father);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Person does not contain member Person(Person mother, Person father)");
        }

#endregion
#region Exercise 1H
        [TestMethod("a. Person.ID is public read-only int property"), TestCategory("Exercise 1H")]
        public void IDIsPublicReadonlyIntProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicReadonlyProperty<Person, int>(p => p.ID);
            test.Execute();
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod("b. Person.ID increases by 1 for each new person"), TestCategory("Exercise 1H")]
        public void IDIncreasesByOneForEachNewPerson()
        {
            // == Failed To Compile ==
            // Person person1 = new Person();
            // Person person2 = new Person();
            // Assert.IsTrue(person1.ID + 1 == person2.ID);
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Person does not contain member ID");
        }
#endregion
    }
}