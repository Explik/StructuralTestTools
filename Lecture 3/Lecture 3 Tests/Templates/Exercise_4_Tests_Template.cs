using Lecture_3_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using static Lecture_3_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_3_Tests
{
    [TemplatedTestClass]
    public class Exercise_4_Tests_Template
    {
        #region Exercise 4B
        [TemplatedTestMethod("a. Employee.ToString() returns expected output"), TestCategory("Exercise 4B")]
        public void EmployeeToStringReturnsExpectedOutput()
        {
            Employee employee = new Employee("Joe Stevens")
            {
                Title = "Programmer"
            };
            Assert.AreEqual("Employee Joe Stevens (Programmer)", employee.ToString());
        }
        #endregion

        #region Exercise 4C
        [TemplatedTestMethod("a. Manager.ToString() returns expected output"), TestCategory("Exercise 4C")]
        public void ManagerToStringReturnsExpectedOutput()
        {
            Manager manager = new Manager("Mary Stevens")
            {
                Title = "Software Engineer"
            };
            Assert.AreEqual("Manager Mary Stevens (Software Engineer)", manager.ToString());
        }
        #endregion
    }
}
