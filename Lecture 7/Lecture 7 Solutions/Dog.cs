using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Syntax;

namespace Lecture_7_Solutions
{
    public class Dog : ICloneable
    {
        int _id = 0;
        string _name = "Unknown";
        string _breed = "Unknown";
        int _age = 0;

        public int ID
        {
            get { return _id; }
            set
            {
                if (value < 0)
                    throw new ArgumentException();
                _id = value;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _name = value;
            }
        }

        public string Breed
        {
            get { return _breed; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _breed = value;
            }
        }

        public int Age
        {
            get { return _age; }
            set
            {
                if (value < 0)
                    throw new ArgumentException();
                _age = value;
            }
        }

        public object Clone()
        {
            /** 
             * this.MemberwiseClone() is equivalent to the following.
             * Dog copy = new Dog();
             * copy._id = _id;
             * copy._name = _name;
             * copy._breed = _breed;
             * copy._age = _age;
             */
            return this.MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            Dog other = obj as Dog;

            return other?.ID == this.ID;
        }

        public override int GetHashCode()
        {
            return _id;
        }

        public override string ToString()
        {
            return $"Dog {Name} ({ID})";
        }
    }
}
