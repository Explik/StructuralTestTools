using Lecture_6_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.Structure;
using OleVanSanten.TestTools.MSTest;
using static OleVanSanten.TestTools.Expressions.TestExpression;
using static Lecture_6_Tests.TestHelper;
using static OleVanSanten.TestTools.Helpers.StructureHelper;

namespace Lecture_6_Tests
{
    [TemplatedTestClass]
    public class Exercise_1_Tests_Template
    {
        #region Exercise 1A
        [TestMethod("a. Temperature.Celcius is public double property"), TestCategory("Exercise 1A")]
        public void TemperatureCelciusIsPublicDoubleProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Temperature, double>(t => t.Celcius);
            test.Execute();
        }

        [TestMethod("b. Temperature.Fahrenheit is public double property"), TestCategory("Exercise 1A")]
        public void TemperatureFahrenheitIsPublicDoubleProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Temperature, double>(t => t.Fahrenheit);
            test.Execute();
        }

        [TestMethod("c. Temperature.Kelvin is public double property"), TestCategory("Exercise 1A")]
        public void TemperatureKelvinIsPublicDoubleProperty()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertPublicProperty<Temperature, double>(t => t.Kelvin);
            test.Execute();
        }

        [TemplatedTestMethod("d. Temperature.Celcius = -276.0 throws ArgumentException"), TestCategory("Exercise 1A")]
        public void TemperatureCelciusAssignmentOfMinus276ThrowsArgumentException()
        {
            Temperature temperature = new Temperature();
            Assert.ThrowsException<ArgumentException>(() => temperature.Celcius = -276.0);
        }

        [TemplatedTestMethod("e. Temperature.Fahrenheit = -460.0 throws ArgumentException"), TestCategory("Exercise 1A")]
        public void TemperatureFahrenheitAssignmentOfMinus460ThrowsArgumentException()
        {
            Temperature temperature = new Temperature();
            Assert.ThrowsException<ArgumentException>(() => temperature.Fahrenheit = -476.0);
        }

        [TemplatedTestMethod("f. Temperature.Kelvin = -1 throws ArgumentException"), TestCategory("Exercise 1A")]
        public void TemperatureKelvinAssignmentOfMinus1ThrowsArgumentException()
        {
            Temperature temperature = new Temperature();
            Assert.ThrowsException<ArgumentException>(() => temperature.Kelvin = -1.0);
        }

        [TemplatedTestMethod("g. Temperature.Kelvin equals 0 after Temperature.Celcius = -273.15"), TestCategory("Exercise 1A")]
        public void TemperatureKelvinEquals0AfterTemperatureCelciusAssignmentOfMinus275()
        {
            Temperature temperature = new Temperature();

            temperature.Celcius = -273.15;

            Assert.AreEqual(0.0, temperature.Kelvin, 0.001);
        }

        [TemplatedTestMethod("h. Temperature.Kelvin equals 0 after Temperature.Fahrenheit = -459.67"), TestCategory("Exercise 1A")]
        public void TemperatureKelvinEquals0AfterTemperatureFahrenheitAssignmentOfMinus459()
        {
            Temperature temperature = new Temperature();

            temperature.Fahrenheit = -459.67;

            Assert.AreEqual(0.0, temperature.Kelvin, 0.001);
        }
        #endregion

        #region Exercise 1B
        [TestMethod("a. Temperature implements IComparable"), TestCategory("Exercise 1B")]
        public void TemperatureImplementsIComparable()
        {
            // TestTools Code
            StructureTest test = Factory.CreateStructureTest();
            test.AssertClass<Temperature>(new TypeIsSubclassOfVerifier(typeof(IComparable)));
            test.Execute();
        }

        [TemplatedTestMethod("b. Temperature.CompareTo sorts null first"), TestCategory("Exercise 1B")]
        public void TemperatureCompareToSortsNullFirst()
        {
            Temperature temperature = new Temperature();
            Assert.IsTrue(temperature.CompareTo(null) < 0);
        }

        [TemplatedTestMethod("c. Temperature.CompareTo sorts higher temperature first"), TestCategory("Exercise 1B")]
        public void TemperatureCompareToSortsHigherTemperatureFirst()
        {
            Temperature temperature1 = new Temperature() { Kelvin = 0 };
            Temperature temperature2 = new Temperature() { Kelvin = 1 };

            Assert.IsTrue(temperature1.CompareTo(temperature2) > 0);
        }

        [TemplatedTestMethod("d. Temperature.CompareTo does not sort equal temperatures"), TestCategory("Exercise 1B")]
        public void Test1B4()
        {
            Temperature temperature1 = new Temperature() { Kelvin = 0 };
            Temperature temperature2 = new Temperature() { Kelvin = 0 };

            Assert.IsTrue(temperature1.CompareTo(temperature2) == 0);
        }
        #endregion
    }
}
