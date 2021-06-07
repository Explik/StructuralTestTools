using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Syntax;

namespace Lecture_6_Solutions
{
    public class Temperature : IComparable
    {
        double _value;

        public double Celcius
        {
            get { return _value + 273.15; }
            set {
                double valueInKelvin = value + 273.15;

                if (valueInKelvin < 0)
                    throw new ArgumentException($"Value {value}C is below absolute zero");
                _value = valueInKelvin; 
            }
        }

        public double Fahrenheit
        {
            get { return (_value - 273.15) * 9 / 5 + 32; }
            set
            {
                double valueInKelvin = (value - 32) * 5 / 9 + 273.15;

                if (valueInKelvin < 0)
                    throw new ArgumentException($"Value {value}C is below absolute zero");
                _value = valueInKelvin;
            }
        }

        public double Kelvin
        {
            get { return _value; }
            set { 
                if (value < 0)
                    throw new ArgumentException($"Value {value}K is below absolute zero");
                _value = value; 
            }
        }

        public int CompareTo(object obj)
        {
            Temperature other = obj as Temperature;

            if (obj == null)
                return -1;
            else return (int)(other._value - this._value);
        }
    }
}
