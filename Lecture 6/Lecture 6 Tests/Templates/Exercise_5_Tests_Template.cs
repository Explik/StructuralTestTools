using Lecture_6_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using OleVanSanten.TestTools.Structure;
using static Lecture_6_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_6_Tests
{
    [TemplatedTestClass]
    public class Exercise_5_Tests_Template
    {
        #region Exercise 5A
        [TestMethod("a. CarListSorter.Comparer is a publuc IComparer<Car> property"), TestCategory("Exercise 5A")]
        public void CarListSorterComparerIsPublicProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<CarSorter, IComparer<Car>>(c => c.Comparer);
            test.Execute();
        }

        [TemplatedTestMethod("b. CarSorter.Comparer initializes to null"), TestCategory("Exercise 5A")]
        public void CarSorterComparerInitializesToNull()
        {
            CarSorter sorter = new CarSorter();
            Assert.IsNull(sorter.Comparer);
        }
        #endregion

        #region Exercise 5B
        [TestMethod("a. CarListSorter.Sort takes an array of cars"), TestCategory("Exercise 5B")]
        public void DieConstructorTakesIRandomAndInt()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicMethod<CarSorter, Car[]>((c1, c2) => c1.Sort(c2));
            test.Execute();
        }

        [TemplatedTestMethod("b. CarListSorter.Sort does not sort array if Comparer = null"), TestCategory("Exercise 5B")]
        public void CarListSorterSortDoesNotSort()
        {
            CarSorter sorter = new CarSorter();
            Car[] cars = new Car[]
            {
                new Car() { ID = 0, Make = "Audi", Model = "S3", Price = 20M},
                new Car() { ID = 1, Make = "Audi", Model = "S4", Price = 30M },
                new Car() { ID = 2, Make = "Suzuki", Model = "Splash", Price = 10M }
            };

            sorter.Sort(cars);

            Assert.IsTrue(cars.Select(c => c.ID).SequenceEqual(new[] { 0, 1, 2 }));
        }

        [TemplatedTestMethod("c. CarListSorter.Sort sorts according to price if Comparer = new CarPriceComparer()"), TestCategory("Exercise 5B")]
        public void CarListSorterSorts()
        {
            CarSorter sorter = new CarSorter() 
            {
                Comparer = new CarPriceComparer()
            };
            Car[] cars = new Car[]
            {
                new Car() { ID = 0, Make = "Audi", Model = "S3", Price = 20M },
                new Car() { ID = 1, Make = "Audi", Model = "S4", Price = 30M },
                new Car() { ID = 2, Make = "Suzuki", Model = "Splash", Price = 10M }
            };

            sorter.Sort(cars);

            Assert.IsTrue(cars.Select(c => c.ID).SequenceEqual(new[] { 2, 0, 1 }));
        }
        #endregion
    }
}
