using Lecture_3_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_3_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_3_Tests
{
    [TemplatedTestClass]
    public class Exercise_2_Tests_Template 
    {   
        #region Exercise 2A
        [TestMethod("a. Employee.Name is public read-only string property"), TestCategory("Exercise 2A")]
        public void NameIsPublicStringProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicReadonlyProperty<Employee, string>(e => e.Name);
            test.Execute();
        }

        [TestMethod("b. Employee.Title is public string property"), TestCategory("Exercise 2A")]
        public void TitleIsPublicStringProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Employee, string>(e => e.Title);
            test.Execute();
        }

        [TestMethod("c. Employee.MonthlySalary is public decimal property"), TestCategory("Exercise 2A")]
        public void MonthlySalaryIsPublicDecimalProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Employee, decimal>(e => e.MonthlySalary);
            test.Execute();
        }

        [TestMethod("d. Employee.Seniority is public int property"), TestCategory("Exercise 2A")]
        public void SeniorityIsPublicIntProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Employee, int>(e => e.Seniority);
            test.Execute();
        }

        [TestMethod("e. Employee constructor takes string as argument"), TestCategory("Exercise 2A")]
        public void EmployeeConstructorTakesStringAsArgument()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicConstructor<string, Employee>(name => new Employee(name));
            test.Execute();
        }

        [TemplatedTestMethod("f. Employee constructor(string name) sets name property"), TestCategory("Exercise 2A")]
        public void EmployeeConstructorNameSetsNameProperty()
        {
            Employee employee = new Employee("abc");
            Assert.AreEqual("abc", employee.Name);
        }

        [TemplatedTestMethod("g. Employee.MonthlySalary is initialized as 0"), TestCategory("Exercise 2A")]
        public void MonthlySalaryIsInitializedAs0()
        {
            Employee employee = new Employee("abc");
            Assert.AreEqual(0M, employee.MonthlySalary);
        }

        [TemplatedTestMethod("h. Employee.Seniority is initialized as 1"), TestCategory("Exercise 2A")]
        public void SenorityIsInitializedAs1()
        {
            Employee employee = new Employee("abc");
            Assert.AreEqual(1, employee.Seniority);
        }

        [TemplatedTestMethod("i. Employee.Title ignores assignment of null"), TestCategory("Exercise 2A")]
        public void TitleIgnoresAssignmentOfNull()
        {
            Employee employee = new Employee("abc");
            Assert.AreEqual("Unknown", employee.Title);
        }
       
        [TemplatedTestMethod("j. Employee.Title ignores assignment of empty string"), TestCategory("Exercise 2A")]
        public void TitleIgnoresAssignmentOfEmptyString()
        {
            Employee employee = new Employee("abc");
            
            employee.Title = "";
            
            Assert.AreEqual("Unknown", employee.Title);
        }
        
        [TemplatedTestMethod("k. Employee.MonthlySalary ignores assignment of -1M"), TestCategory("Exercise 2A")]
        public void MonthlySalaryIgnoresAssignmentOfMinusOne()
        {
            Employee employee = new Employee("abc");

            employee.MonthlySalary = -1;

            Assert.AreEqual(0M, employee.MonthlySalary);
        }

        [TemplatedTestMethod("l. Employee.Seniority ignores assignment of 0"), TestCategory("Exercise 2A")]
        public void SeniorityIgnoresAssignmentOfZero()
        {
            Employee employee = new Employee("abc");

            employee.Seniority = 0;

            Assert.AreEqual(1, employee.Seniority);
        }

        [TemplatedTestMethod("m. Employee.Seniority ignores assigment of 11"), TestCategory("Exercise 2A")]
        public void SeniorityIgnoresAssignmentOfEleven()
        {
            Employee employee = new Employee("abc");

            employee.Seniority = 11;

            Assert.AreEqual(1, employee.Seniority);
        }
        #endregion

        #region Exercise 2B
        [TemplatedTestMethod("a. Employee.CalculateYearlySalary() returns expected output for seniority 1"), TestCategory("Exercise 2B")]
        public void EmployeeCalculateYearlySalaryAddsTenProcentForSeniorityLevelOne()
        {
            Employee employee = new Employee("abc") 
            { 
                MonthlySalary = 34000M, 
                Seniority = 1 
            };
            Assert.AreEqual(40800M, employee.CalculateYearlySalary());
        }

        [TemplatedTestMethod("b. Employee.CalculateYearlySalary() returns expected output for seniority 2"), TestCategory("Exercise 2B")]
        public void EmployeeCalculateYearlySalaryAddsTenProcentForSeniorityLevelTwo()
        {
            Employee employee = new Employee("abc")
            {
                MonthlySalary = 15340,
                Seniority = 2
            };
            Assert.AreEqual(18408M, employee.CalculateYearlySalary());
        }

        [TemplatedTestMethod("c. Employee.CalculateYearlySalary() returns expected output for seniority 3"), TestCategory("Exercise 2B")]
        public void EmployeeCalculateYearlySalaryAddsTenProcentForSeniorityLevelThree()
        {
            Employee employee = new Employee("abc")
            {
                MonthlySalary = 20000,
                Seniority = 3
            };
            Assert.AreEqual(24000M, employee.CalculateYearlySalary());
        }

        [TemplatedTestMethod("d. Employee.CalculateYearlySalary() returns expected output for seniority 6"), TestCategory("Exercise 2B")]
        public void EmployeeCalculateYearlySalaryAddsTenProcentForSeniorityLevelSix()
        {
            Employee employee = new Employee("abc")
            {
                MonthlySalary = 20000,
                Seniority = 6
            };
            Assert.AreEqual(96000M, employee.CalculateYearlySalary());
        }

        [TemplatedTestMethod("e. Employee.CalculateYearlySalary() returns expected output for seniority 7"), TestCategory("Exercise 2B")]
        public void EmployeeCalculateYearlySalaryAddsTenProcentForSeniorityLevelSeven()
        {
            Employee employee = new Employee("abc")
            {
                MonthlySalary = 12300,
                Seniority = 7
            };
            Assert.AreEqual(103320M, employee.CalculateYearlySalary());
        }

        [TemplatedTestMethod("f. Employee.CalculateYearlySalary() returns expected output for seniority 10"), TestCategory("Exercise 2B")]
        public void EmployeeCalculateYearlySalaryAddsTenProcentForSeniorityLevelTen()
        {
            Employee employee = new Employee("abc")
            {
                MonthlySalary = 35250,
                Seniority = 10
            };
            Assert.AreEqual(296100M, employee.CalculateYearlySalary());
        }
        #endregion

        #region Exercise 2C
        [TestMethod("a. Manager is subclass of Employee"), TestCategory("Exercise 2C")]
        public void ManagerIsSubclassOfEmployee()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertClass<Manager>(new TypeBaseClassVerifier(typeof(Employee)));
            test.Execute();
        }

        [TestMethod("b. Bonus is public decimal property"), TestCategory("Exercise 2C")]
        public void BonusIsPublicDecimalProperty() 
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Manager, decimal>(e => e.Bonus);
            test.Execute();
        }

        [TemplatedTestMethod("c. Manager.Bonus is initialized as 0"), TestCategory("Exercise 2A")]
        public void BonusIsInitializedAs0()
        {
            Manager manager = new Manager("abc");
            Assert.AreEqual(0M, manager.Bonus);
        }

        [TemplatedTestMethod("d. Bonus ignores assignment of -1M"), TestCategory("Exercise 2C")]
        public void BonusIgnoresAssignmentOfMinusOne()
        {
            Manager manager = new Manager("abc");

            manager.Bonus = -1;

            Assert.AreEqual(0M, manager.Bonus);
        }
        #endregion

        #region Exercise 2D
        [TemplatedTestMethod("a. Manager.CalculateYearlySalary() returns expected output for seniority 1"), TestCategory("Exercise 2D")]
        public void ManagerCalculateYearlySalaryAddsTenProcentForSeniorityLevelOne()
        {
            Manager manager = new Manager("abc")
            {
                MonthlySalary = 35250,
                Bonus = 0,
                Seniority = 1
            };
            Assert.AreEqual(42300M, manager.CalculateYearlySalary());
        }

        [TemplatedTestMethod("b. Manager.CalculateYearlySalary() returns expected output for seniority 2"), TestCategory("Exercise 2D")]
        public void ManagerCalculateYearlySalaryAddsTenProcentForSeniorityLevelTwo()
        {
            Manager manager = new Manager("abc")
            {
                MonthlySalary = 15340,
                Bonus = 500,
                Seniority = 2
            };
            Assert.AreEqual(18908M, manager.CalculateYearlySalary());
        }

        [TemplatedTestMethod("c. Manager.CalculateYearlySalary() returns expected output for seniority 3"), TestCategory("Exercise 2D")]
        public void ManagerCalculateYearlySalaryAddsTenProcentForSeniorityLevelThree()
        {
            Manager manager = new Manager("abc")
            {
                MonthlySalary = 26500,
                Bonus = 1000,
                Seniority = 3
            };
            Assert.AreEqual(32800M, manager.CalculateYearlySalary());
        }

        [TemplatedTestMethod("d. Manager.CalculateYearlySalary() returns expected output for seniority 6"), TestCategory("Exercise 2D")]
        public void ManagerCalculateYearlySalaryAddsTenProcentForSeniorityLevelSix()
        {
            Manager manager = new Manager("abc")
            {
                MonthlySalary = 20000,
                Bonus = 300,
                Seniority = 6
            };
            Assert.AreEqual(96300M, manager.CalculateYearlySalary());
        }

        [TemplatedTestMethod("e. Manager.CalculateYearlySalary() returns expected output for seniority 7"), TestCategory("Exercise 2D")]
        public void ManagerCalculateYearlySalaryAddsTenProcentForSeniorityLevelSeven()
        {
            Manager manager = new Manager("abc")
            {
                MonthlySalary = 12300,
                Bonus = 3000,
                Seniority = 7
            };
            Assert.AreEqual(106320M, manager.CalculateYearlySalary());
        }

        [TemplatedTestMethod("f. Employee.CalculateYearlySalary() returns expected output for seniority 10"), TestCategory("Exercise 2D")]
        public void ManagerCalculateYearlySalaryAddsTenProcentForSeniorityLevelTen()
        {
            Manager manager = new Manager("abc")
            {
                MonthlySalary = 35250,
                Bonus = 34000,
                Seniority = 10
            };
            Assert.AreEqual(330100M, manager.CalculateYearlySalary());
        }
        #endregion

        #region Exercise 2E
        [TestMethod("a. Company.Employees is a public read-only List<Employee> property"), TestCategory("Exercise 2E")]
        public void EmployeesIsPublicListEmployeeProperty() {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicReadonlyProperty<Company, List<Employee>>(c => c.Employees);
            test.Execute();
        }

        [TemplatedTestMethod("b. Company.Hire(Employee e) adds employee to Employees"), TestCategory("Exercise 2E")]
        public void CompanyHireAddsEmployeeToEmployees()
        {
            Company company = new Company();
            Employee employee = new Employee("Ellen Stevens");

            company.Hire(employee);

            Assert.AreEqual(1, company.Employees.Count);
        }

        [TemplatedTestMethod("c. Company.Fire(Employee e) removes employee from Employees"), TestCategory("Exercise 2E")]
        public void CompanyFireAddsEmployeeToEmployees()
        {
            Company company = new Company();
            Employee employee = new Employee("Ellen Stevens");

            company.Hire(employee);
            company.Fire(employee);

            Assert.AreEqual(0, company.Employees.Count);
        }

        [TemplatedTestMethod("d. Company.Hire(Employee e) adds manager to Employees"), TestCategory("Exercise 2E")]
        public void CompanyHireAddsManagerToEmployees()
        {
            Company company = new Company();
            Employee employee = new Employee("Katja Holmes");

            company.Hire(employee);

            Assert.AreEqual(1, company.Employees.Count);
        }

        [TemplatedTestMethod("e. Company.Fire(Employee e) removes manager to Employees"), TestCategory("Exercise 2E")]
        public void CompanyFireAddsManagerToEmployees()
        {
            Company company = new Company();
            Manager manager = new Manager("Katja Holmes");

            company.Hire(manager);
            company.Fire(manager);

            Assert.AreEqual(0, company.Employees.Count);
        }
        #endregion

        #region Exercise 2F
        [TemplatedTestMethod("a. Company.CalculateYearlySalaryCosts() returns 0 for company without employees"), TestCategory("Exercise 2F")]
        public void CompanyCalculateYearlySalaryCostsReturnsZeroForCompanyWithoutEmployees()
        {
            Company company = new Company();
            Assert.AreEqual(0M, company.CalculateYearlySalaryCosts());
        }
        
        [TemplatedTestMethod("b. Company.CalculateYearlySalaryCosts() returns expected output for company with 1 Employee"), TestCategory("Exercise 2F")]
        public void CompanyCalculateYearlySalaryCostsReturnsExpectedOutputForCompanyWithOneEmployee()
        {
            Company company = new Company();
            Employee employee = new Employee("Allan Walker") 
            { 
                MonthlySalary = 30000M, 
                Seniority = 4 
            };

            company.Hire(employee);

            Assert.AreEqual(144000M, company.CalculateYearlySalaryCosts());
        }

        [TemplatedTestMethod("c. Company.CalculateYearlySalaryCosts() returns expected output for company with 2 Employees"), TestCategory("Exercise 2F")]
        public void CompanyCalculateYearlySalaryCostsReturnsExpectedOutputForCompanyWithTwoEmployee() 
        {
            Company company = new Company();
            Employee employee1 = new Employee("Allan Walker")
            {
                MonthlySalary = 30000M,
                Seniority = 4
            };
            Employee employee2 = new Employee("Amy Walker")
            {
                MonthlySalary = 30000M,
                Seniority = 7
            };

            company.Hire(employee1);
            company.Hire(employee2);

            Assert.AreEqual(396000M, company.CalculateYearlySalaryCosts());
        }
        #endregion
    }
}
