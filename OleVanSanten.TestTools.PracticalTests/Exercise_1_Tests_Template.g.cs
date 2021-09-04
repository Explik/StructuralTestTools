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
    [TestClass]
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

        [TestMethod("d. Person.FirstName is initialized as \"Unknown\""), TestCategory("Exercise 1A")]
        public void FirstNameIsInitializedAsUnnamed()
        {
            Lecture_2.Person person = new Lecture_2.Person();
            Assert.AreEqual("Unknown", person.FirstName);
        }

        [TestMethod("e. Person.FirstName is initialized as \"Unknown\""), TestCategory("Exercise 1A")]
        public void LastNameIsInitializedAsUnnamed()
        {
            Lecture_2.Person person = new Lecture_2.Person();
            Assert.AreEqual("Unknown", person.LastName);
        }

        [TestMethod("f. Person.FirstName ignores assigment of null"), TestCategory("Exercise 1A")]
        public void FirstNameIgnoresAssignmentOfNull()
        {
            Lecture_2.Person person = new Lecture_2.Person();
            person.FirstName = null;
            Assert.AreEqual("Unknown", person.FirstName);
        }

        [TestMethod("g. Person.LastName ignores assigment of null"), TestCategory("Exercise 1A")]
        public void LastNameIgnoresAssignmentOfNull()
        {
            Lecture_2.Person person = new Lecture_2.Person();
            person.LastName = null;
            Assert.AreEqual("Unknown", person.LastName);
        }

        [TestMethod("h. Person.FirstName ignores assigment of \"123456789\""), TestCategory("Exercise 1A")]
        public void FirstNameIgnoresAssignmentOf012345689()
        {
            Lecture_2.Person person = new Lecture_2.Person();
            person.FirstName = "123456789";
            Assert.AreEqual("Unknown", person.FirstName);
        }

        [TestMethod("i. Person.LastName ignores assigment of \"123456789\""), TestCategory("Exercise 1A")]
        public void LastNameIgnoresAssignmentOf012345689()
        {
            Lecture_2.Person person = new Lecture_2.Person();
            person.LastName = "123456789";
            Assert.AreEqual("Unknown", person.LastName);
        }

        [TestMethod("j. Person.FirstName ignores assigment of string with length 101"), TestCategory("Exercise 1A")]
        public void FirstNameIgnoresAssignmentOfStringWithLength101()
        {
            Lecture_2.Person person = new Lecture_2.Person();
            person.FirstName = CreateName(101);
            Assert.AreEqual("Unknown", person.FirstName);
        }

        [TestMethod("k. Person.LastName ignores assignment of string with length 101"), TestCategory("Exercise 1A")]
        public void LastNameIgnoresAssignmentOfStringWithLength101()
        {
            Lecture_2.Person person = new Lecture_2.Person();
            person.LastName = CreateName(101);
            Assert.AreEqual("Unknown", person.LastName);
        }

        [TestMethod("l. Person.Age is initialized as 0"), TestCategory("Exercise 1A")]
        public void AgeIsInitilizedAs0()
        {
            Lecture_2.Person person = new Lecture_2.Person();
            Assert.AreEqual(0, person.Age);
        }

        [TestMethod("m. Person.Age ignores assigment of -1"), TestCategory("Exercise 1A")]
        public void AgeIgnoresAssignmentOfMinusOne()
        {
            Lecture_2.Person person = new Lecture_2.Person();
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
        public void FatherIsPublicPersonProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Person, Person>(p => p.Father);
            test.Execute();
        }

        [TestMethod("c. Person.Mother is initialized as null"), TestCategory("Exercise 1B")]
        public void MotherIsInitilizedAsnull()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Person does not contain member Mother (AUTO)");
        }

        [TestMethod("d. Person.Father is initialized as null"), TestCategory("Exercise 1B")]
        public void FatherIsInitilizedAsnull()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Person does not contain member Father (AUTO)");
        }

        [TestMethod("c. Person.Mother ignores assigment if mother is younger than child"), TestCategory("Exercise 1B")]
        public void MotherIgnoresAssigmentIfMotherIsYoungerThanChild()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Person does not contain member Mother (AUTO)");
        }

        [TestMethod("d. Person.Father ignores assigment if mother is younger than child"), TestCategory("Exercise 1B")]
        public void FatherIgnoresAssigmentIfMotherIsYoungerThanChild()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Person does not contain member Father (AUTO)");
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

        [TestMethod("b. PersonGenerator.GeneratePerson generates Adam Smith (36)"), TestCategory("Exercise 1C")]
        public void GeneratePersonCreatesAdamSmith()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Namespace Lecture_2 does not contain the type PersonGenerator (AUTO)");
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

        [TestMethod("b. PersonGenerator.GenerateFamily generates Robin Rich (10) as child"), TestCategory("Exercise 1D")]
        public void GenerateFamilyCreatesRobinRichAsChild()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Namespace Lecture_2 does not contain the type PersonGenerator (AUTO)");
        }

        [TestMethod("c. PersonGenerator.GenerateFamily generates Warren Rich (36) as father"), TestCategory("Exercise 1D")]
        public void GenerateFamilyCreatesRobinRichAsFather()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Namespace Lecture_2 does not contain the type PersonGenerator (AUTO)");
        }

        [TestMethod("d. PersonGenerator.GenerateFamily generates Anna Smith (38) as mother"), TestCategory("Exercise 1D")]
        public void GenerateFamilyCreatesAnnaRichAsMother()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Namespace Lecture_2 does not contain the type PersonGenerator (AUTO)");
        }

        [TestMethod("e. PersonGenerator.GenerateFamily generates Gustav Rich (66) as grandfather"), TestCategory("Exercise 1D")]
        public void GenerateFamilyCreatesGustavRichAsGrandfather()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Namespace Lecture_2 does not contain the type PersonGenerator (AUTO)");
        }

        [TestMethod("f. PersonGenerator.GenerateFamily generates Elsa Johnson (65) as grandmother"), TestCategory("Exercise 1D")]
        public void GenerateFamilyCreatesElsaJohnsonAsGrandMother()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Namespace Lecture_2 does not contain the type PersonGenerator (AUTO)");
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

        [TestMethod("b. PersonPrinter.PrintPrints prints correctly"), TestCategory("Exercise 1E")]
        public void PrintPersonPrintsCorrectly()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Namespace Lecture_2 does not contain the type PersonPrinter (AUTO)");
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

        [TestMethod("b. PersonPrinter.PrintFamily prints correctly"), TestCategory("Exercise 1F")]
        public void PrintFamilyPrintsCorrectly()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Namespace Lecture_2 does not contain the type PersonPrinter (AUTO)");
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

        [TestMethod("c. Person constructor with 2 persons as arguments sets mother and father property"), TestCategory("Exercise 1G")]
        public void PersonConstructorWithTwoPersonArgumentsSetsMotherAndFatherProperty()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Person does not contain member Person(Person mother, Person father) (AUTO)");
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

        [TestMethod("b. Person.ID increases by 1 for each new person"), TestCategory("Exercise 1H")]
        public void IDIncreasesByOneForEachNewPerson()
        {
            throw new Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException("Person does not contain member ID (AUTO)");
        }
#endregion
    }
}