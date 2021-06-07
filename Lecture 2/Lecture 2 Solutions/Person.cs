using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Syntax;
using OleVanSanten.TestTools.Structure.Attributes;

namespace Lecture_2_Solutions
{
    public class Person
    {
        private string _firstName = "Unknown";
        private string _lastName = "Unknown";
        private int _age;
        private Person _mother;
        private Person _father;
        private static int _nextID;

        public Person()
        {
            ID = _nextID;
            _nextID++;
        }

        public Person(Person mother, Person father) : this()
        {
            Mother = mother;
            Father = father;
        }

        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                if (IsValidName(value))
                    _firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                if (IsValidName(value))
                    _lastName = value;
            }
        }

        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                if (value >= 0)
                    _age = value;
            }
        }

        [ReadonlyProperty]
        public int ID { get; }

        public Person Mother
        {
            get
            {
                return _mother;
            }
            set
            {
                if (IsValidParent(value))
                    _mother = value;
            }
        }

        public Person Father
        {
            get
            {
                return _father;
            }
            set
            {
                if (IsValidParent(value))
                    _father = value;
            }
        }

        private bool IsValidName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            if (name.Length > 100)
                return false;

            foreach (char c in name.ToLower())
            {
                if (c < 'a' || c > 'z')
                    return false;
            }
            return true;
        }

        private bool IsValidParent(Person parent)
        {
            if (parent == null)
                return true;

            return parent.Age > Age;
        }
    }
}
