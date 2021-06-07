using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using OleVanSanten.TestTools.Structure.Attributes;
using OleVanSanten.TestTools.Syntax;

namespace Lecture_3_Solutions
{
    public class Employee
    {
        private string _name = "Unknown";
        private string _jobTitle = "Unknown";
        private decimal _monthlySalary;
        private int _seniority = 1;

        public Employee(string name) {
            Name = name;
        }

        public int ID { get; }

        [ReadonlyProperty]
        public string Name {
            get { return _name; }
            private set 
            {
                if (!string.IsNullOrEmpty(value))
                    _name = value;
            }
        }

        public string Title {
            get { return _jobTitle; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _jobTitle = value;
            }
        }

        public decimal MonthlySalary {
            get { return _monthlySalary; } 
            set
            {
                if (value >= 0)
                    _monthlySalary = value;
            }
        }

        public int Seniority {
            get { return _seniority; }
            set {
                if (1 <= value && value <= 10)
                    _seniority = value;
            }
        }

        public virtual decimal CalculateYearlySalary()
        {
            decimal seniorityBonus;

            if (Seniority >= 7)
                seniorityBonus = 0.7M;
            else if (Seniority >= 4)
                seniorityBonus = 0.4M;
            else seniorityBonus = 0.1M;

            return 12 * MonthlySalary * seniorityBonus;
        }

        public override string ToString()
        {
            return $"Employee {Name} ({Title})";
        }
    }
}
