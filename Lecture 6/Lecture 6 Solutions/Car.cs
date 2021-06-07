using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Syntax;

namespace Lecture_6_Solutions
{
    public class Car : IComparable
    {
        int _id;
        decimal _price;

        public int ID { 
            get { return _id; }
            set
            {
                if (value < 0)
                    throw new ArgumentException();
                _id = value;
            }
        }

        public string Make { get; set; }

        public string Model { get; set; }

        public decimal Price {
            get { return _price; }
            set {
                if (value < 0)
                    throw new ArgumentException();
                _price = value;
            }
        }

        public Car()
        {
        }

        public int CompareTo(object obj)
        {
            Car other = obj as Car;
            return other == null ? 1 : this.ID.CompareTo(other.ID);
        }
    }
}
