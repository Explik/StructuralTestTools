using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Structure.Attributes;

namespace Lecture_3_Solutions
{
    public class Circle : Figure
    {
        public Circle(Point center, double radius)
        {
            Center = center;
            Radius = radius;

            // A good default for when center is null
            if (center == null)
                Center = new Point(0, 0);
            if (radius < 0)
                Radius = 0;
        }

        [ReadonlyProperty]
        public Point Center { get; }

        [ReadonlyProperty]
        public double Radius { get; }

        public override double CalculateArea()
        {
            return Math.PI * Math.Pow(Radius, 2);
        }

        public override bool Contains(Point p)
        {
            if (p == null)
                return false;

            return Radius >= Math.Sqrt(Math.Pow(p.X - Center.X, 2) + Math.Pow(p.Y - Center.Y, 2));
        }
    }
}
