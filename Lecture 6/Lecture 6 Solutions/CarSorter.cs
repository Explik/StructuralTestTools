using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Syntax;

namespace Lecture_6_Solutions
{
    public class CarSorter
    {
        public IComparer<Car> Comparer { get; set; }

        public void Sort(Car[] cars)
        {
            if (Comparer == null)
                return;

            Array.Sort(cars, Comparer);
        }
    }
}
