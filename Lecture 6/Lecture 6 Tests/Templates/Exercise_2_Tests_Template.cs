using Lecture_6_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_6_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_6_Tests
{
    [TemplatedTestClass]
    public class Exercise_2_Tests_Template
    {
        #region Exercise 2A
        [TestMethod("a. Car.ID is public int property"), TestCategory("Exercise 2A")]
        public void CarIDIsPublicReadOnlyIntProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Car, int>(c => c.ID);
            test.Execute();
        }

        [TestMethod("b. Car.Make is public string property"), TestCategory("Exercise 2A")]
        public void CarMakeIsPublicReadOnlyStringProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Car, string>(c => c.Make);
            test.Execute();
        }

        [TestMethod("c. Car.Model is public string property"), TestCategory("Exercise 2A")]
        public void CarModelIsPublicReadOnlyStringProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Car, string>(c => c.Model);
            test.Execute();
        }

        [TestMethod("d. Car.Price is public decimal property"), TestCategory("Exercise 2A")]
        public void CarPriceIsPublicDecimalReadOnlyProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Car, decimal>(c => c.Price);
            test.Execute();
        }

        [TemplatedTestMethod("e. Car.ID throws ArgumentException on assignment of -1"), TestCategory("Exercise 2A")]
        public void CarIDThrowsArgumentExceptionOnAssignmentOfMinusOne()
        {
            Car car = new Car();
            Assert.ThrowsException<ArgumentException>(() => car.ID = -1);
        }

        [TemplatedTestMethod("f. Car.Price throws ArgumentException on assignment of -1M"), TestCategory("Exercise 2A")]
        public void CarPriceThrowsArgumentExceptionOnAssignmentOfMinusOne()
        {
            Car car = new Car();
            Assert.ThrowsException<ArgumentException>(() => car.Price = -1M);
        }
        #endregion

        #region Exercise 2B
        [TestMethod("a. Car implements IComparable"), TestCategory("Exercise 2B")]
        public void CarImplementsIcomparable()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertClass<Car>(
                new TypeAccessLevelVerifier(AccessLevels.Public),
                new TypeIsSubclassOfVerifier(typeof(IComparable)));
            test.Execute();
        }

        [TemplatedTestMethod("b. Car.CompareTo sorts null first"), TestCategory("Exercise 2B")]
        public void CarCompareToSortsNullFirst()
        {
            Car car = new Car();
            Assert.IsTrue(car.CompareTo(null) > 0);
        }

        [TemplatedTestMethod("c. Car.CompareTo sorts higher ID first"), TestCategory("Exercise 2B")]
        public void CarCompareToSortsHigherIDFirst()
        {
            Car car1 = new Car() { ID = 0 };
            Car car2 = new Car() { ID = 1 };

            Assert.IsTrue(car2.CompareTo(car1) > 0);
        }
        #endregion

        #region Exercise 2C
        [TestMethod("a. CarPriceComparer implements IComparer<Car>"), TestCategory("Exercise 2C")]
        public void CarPriceComparerImplementsIComparerCar()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertClass<CarPriceComparer>(
                new TypeAccessLevelVerifier(AccessLevels.Public),
                new TypeIsSubclassOfVerifier(typeof(IComparer<Car>)));
            test.Execute();
        }

        [TemplatedTestMethod("b. CarPriceComparer.Compare sorts null first"), TestCategory("Exercise 2C")]
        public void CarPriceComparerCompareSortsNullFirst()
        {
            Car car = new Car();
            CarPriceComparer comparer = new CarPriceComparer();

            Assert.IsTrue(comparer.Compare(car, null) > 0);
        }

        [TemplatedTestMethod("c. CarPriceComparer.Compare sorts cars with higher Price first"), TestCategory("Exercise 2C")]
        public void CarPriceComparerCompareSortsCarsWithHigherPriceFirst()
        {
            Car car1 = new Car() { Price = 0M };
            Car car2 = new Car() { Price = 1M };
            CarPriceComparer comparer = new CarPriceComparer();

            Assert.IsTrue(comparer.Compare(car1, car2) < 0);
        }

        [TemplatedTestMethod("d. CarPriceComparer.Compare does not sort cars if equal Price"), TestCategory("Exercise 2C")]
        public void CarPriceComparerCompareDoesNotSortCarsIfEqualPrice()
        {
            Car car1 = new Car() { Price = 0M };
            Car car2 = new Car() { Price = 0M };
            CarPriceComparer comparer = new CarPriceComparer();

            Assert.IsTrue(comparer.Compare(car1, car2) == 0);
        }
        #endregion

        #region Exercise 2D
        [TestMethod("a. CarMakeModelPriceComparer implements IComparer<Car>"), TestCategory("Exercise 2D")]
        public void Test2E1()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertClass<CarMakeModelPriceComparer>(
                new TypeAccessLevelVerifier(AccessLevels.Public),
                new TypeIsSubclassOfVerifier(typeof(IComparer<Car>)));
            test.Execute();
        }

        [TemplatedTestMethod("b. CarMakeModelPriceComparer.Compare sorts null first"), TestCategory("Exercise 2D")]
        public void CarMakeModelPriceComparerCompareSortsNullFirst()
        {
            Car car = new Car();
            CarMakeModelPriceComparer comparer = new CarMakeModelPriceComparer();

            Assert.IsTrue(comparer.Compare(car, null) > 0);
        }

        [TemplatedTestMethod("c. CarMakeModelPriceComparer.Compare sorts according to Car.Make"), TestCategory("Exercise 2D")]
        public void CarMakeModelPriceComparerCompareSortsAccordingToCarMake()
        {
            Car car1 = new Car() { Make = "Audi" };
            Car car2 = new Car() { Make = "BMW" };
            CarMakeModelPriceComparer comparer = new CarMakeModelPriceComparer();

            Assert.IsTrue(comparer.Compare(car1, car2) < 0);
        }

        [TemplatedTestMethod("d. CarMakeModelPriceComparer.Compare sorts according to Car.Model if Car.Make are equal"), TestCategory("Exercise 2D")]
        public void CarMakeModelPriceComparerCompareSortsAccordingToCarModel()
        {
            Car car1 = new Car() { Model = "A3" };
            Car car2 = new Car() { Model = "S3" };
            CarMakeModelPriceComparer comparer = new CarMakeModelPriceComparer();

            Assert.IsTrue(comparer.Compare(car1, car2) < 0);
        }

        [TemplatedTestMethod("e. CarMakeModelPriceComparer.Compare sorts according to Car.Price if Car.Make and Car.Model are equal"), TestCategory("Exercise 2D")]
        public void CarMakeModelPriceComparerCompareSortsAccordingToCarPrice()
        {
            Car car1 = new Car() { Price = 0M };
            Car car2 = new Car() { Price = 1M };
            CarMakeModelPriceComparer comparer = new CarMakeModelPriceComparer();

            Assert.IsTrue(comparer.Compare(car1, car2) < 0);
        }

        [TemplatedTestMethod("f. CarMakeModelPriceComparer.Compare does not sort if Car.Make, Car.Model and Car.Price are equal"), TestCategory("Exercise 2D")]
        public void CarMakeModelPriceComparerCompareDoesNotSortIfEverythingIsEqual()
        {
            Car car1 = new Car();
            Car car2 = new Car();
            CarMakeModelPriceComparer comparer = new CarMakeModelPriceComparer();

            Assert.IsTrue(comparer.Compare(car1, car2) == 0);
        }
        #endregion
    }
}
