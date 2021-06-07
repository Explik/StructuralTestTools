using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Structure.Attributes;
using OleVanSanten.TestTools.Syntax;

namespace Lecture_4_Solutions
{
    public class Person
    {
        private string _name;
        private double _height;
        private double _weight;
        private int _age;

        public Person(string name)
        {
            Name = name;
        }

        [ReadonlyProperty]
        public string Name
        {
            get { return _name; }
            private set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _name = value;
            }
        }

        public double Height
        {
            get { return _height; }
            set
            {
                if (value < 0)
                    throw new ArgumentException($"{value} is negative", "value");
                _height = value;
            }
        }

        public double Weight
        {
            get { return _weight; }
            set
            {
                if (value < 0)
                    throw new ArgumentException($"{value} is negative", "value");
                _weight = value;
            }
        }

        public int Age
        {
            get { return _age; }
            set
            {
                if (value < 0)
                    throw new ArgumentException($"{value} is negative", "value");
                _age = value;
            }
        }

        public double CalculateBMI()
        {
            if (Age < 16)
                throw new NotOldEnoughException("calculate BMI");

            return Weight / Math.Pow(Height, 2);
        }

        public string GetClassification()
        {
            double BMI = CalculateBMI();

            if (BMI <= 18.4)
                return "under-weight";
            if (BMI <= 24.9)
                return "normal weight";
            if (BMI <= 29.9)
                return "over-weight";
            
            return "obese";
        }
    }
}
