using Lecture_9_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_9_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_9_Tests
{
    [TemplatedTestClass]
    public class Exercise_2_Tests_Template
    {
        #region Exercise 2A
        [TestMethod("a. Student.ID is a public property"), TestCategory("Exercise 2A")]
        public void StudentIDIsAPublicProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Student, int>(s => s.ID);
            test.Execute();
        }

        [TestMethod("b. Student.FirstName is a public property"), TestCategory("Exercise 2A")]
        public void StudentFirstNameIsAPublicProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Student, string>(s => s.FirstName);
            test.Execute();
        }

        [TestMethod("c. Student.LastName is a public property"), TestCategory("Exercise 2A")]
        public void StudentLastNameIsAPublicProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Student, string>(s => s.LastName);
            test.Execute();
        }

        [TestMethod("d. Student.LastName is a public property"), TestCategory("Exercise 2A")]
        public void StudentAgeIsAPublicProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Student, int>(s => s.Age);
            test.Execute();
        }
        #endregion

        #region Exercise 2B
        [TestMethod("a. Course.Student is a readonly property"), TestCategory("Exercise 2B")]
        public void CourseStudentIsAReadonlyProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicReadonlyProperty<Course, IEnumerable<Student>>(c => c.Students);
            test.Execute();
        }

        [TestMethod("b. Course.Enroll(Student s) is a public method"), TestCategory("Exercise 2B")]
        public void CourseEnrollIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Course, Student>((c, s) => c.Enroll(s));
            test.Execute();
        }

        [TestMethod("c. Course.Disenroll(Student s) is a public method"), TestCategory("Exercise 2B")]
        public void CourseDisenrollIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Course, Student>((c, s) => c.Disenroll(s));
            test.Execute();
        }

        [TemplatedTestMethod("d. Course.Enroll(Student s) adds student"), TestCategory("Exercise 2B")]
        public void CourseEnrollAddsStudent()
        {
            Course course = new Course();
            Student student = new Student();

            course.Enroll(student);

            Assert.IsTrue(course.Students.SequenceEqual(new Student[] { student }));
        }

        [TemplatedTestMethod("e. Course.Disenroll(Student s) removes student again"), TestCategory("Exercise 2B")]
        public void CourseDisenrollRemovesStudentAgain()
        {
            Course course = new Course();
            Student student = new Student();

            course.Enroll(student);
            course.Disenroll(student);

            Assert.IsFalse(course.Students.Any());
        }

        #endregion

        #region Exercise 2C
        [TestMethod("a. Course.GetStudentByID(int id) is a public method"), TestCategory("Exercise 2C")]
        public void CourseGetStudentByIDIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Course, int, Student>((c, i) => c.GetStudentByID(i));
            test.Execute();
        }

        [TestMethod("b. Course.GetYoungestStudent() is a public method"), TestCategory("Exercise 2C")]
        public void CourseGetYoungestStudentIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Course, Student>(c => c.GetYoungestStudent());
            test.Execute();
        }

        [TestMethod("c. Course.GetOldestStudent() is a public method"), TestCategory("Exercise 2C")]
        public void CourseGetOldestStudentIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Course, Student>(c => c.GetOldestStudent());
            test.Execute();
        }

        [TestMethod("d. Course.GetAverageStudentAge() is a public method"), TestCategory("Exercise 2C")]
        public void CourseGetAverageStudentAgeIsAPublicMethod()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<Course, double>(c => c.GetAverageStudentAge());
            test.Execute();
        }

        [TemplatedTestMethod("e. Course.GetStudentByID(int id) returns correctly"), TestCategory("Exercise 2C")]
        public void CourseGetStudentByIDReturnsCorrectly()
        {
            Course course = new Course();
            Student student = new Student() { ID = 5 };

            course.Enroll(student);

            Assert.AreEqual(student, course.GetStudentByID(5));
        }

        [TemplatedTestMethod("f. Course.GetYoungestStudent() returns correctly"), TestCategory("Exercise 2C")]
        public void CourseGetYoungestStudentReturnsCorrectly()
        {
            Course course = new Course();
            Student youngestStudent = new Student() { Age = 19 };
            Student oldestStudent = new Student() { Age = 23 };

            course.Enroll(youngestStudent);
            course.Disenroll(oldestStudent);

            Assert.AreEqual(course.GetYoungestStudent(), youngestStudent);
        }

        [TemplatedTestMethod("g. Course.GetOldestStudent() returns correctly"), TestCategory("Exercise 2C")]
        public void CourseGetOldestStudentReturnsCorrectly()
        {
            Course course = new Course();
            Student youngestStudent = new Student() { Age = 19 };
            Student oldestStudent = new Student() { Age = 23 };

            course.Enroll(youngestStudent);
            course.Enroll(oldestStudent);

            Assert.AreEqual(oldestStudent, course.GetOldestStudent());
        }

        [TemplatedTestMethod("h. Course.GetAverageStudentAge() returns correctly"), TestCategory("Exercise 2C")]
        public void CourseGetAverageStudentAgeReturnsCorrectly()
        {
            Course course = new Course();
            Student youngestStudent = new Student() { Age = 19 };
            Student oldestStudent = new Student() { Age = 23 };

            course.Enroll(youngestStudent);
            course.Enroll(oldestStudent);

            Assert.AreEqual(21.0, course.GetAverageStudentAge());
        }
        #endregion
    }
}
