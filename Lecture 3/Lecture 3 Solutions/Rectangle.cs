using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Structure.Attributes;

namespace Lecture_3_Solutions
{
    public class Rectangle : Figure
    {
        Point _bottomRight;
        Point _topLeft;

        public Rectangle(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;

            if (p1 == null)
                P1 = new Point(0, 0);
            if (p2 == null)
                P2 = new Point(0, 0);
        }

        [ReadonlyProperty]
        public Point P1 { get; }

        [ReadonlyProperty]
        public Point P2 { get; }

        public override double CalculateArea()
        {
            double width = Math.Abs(P2.X - P1.X);
            double height = Math.Abs(P2.Y - P1.Y);
            return width * height;
        }

        public override bool Contains(Point p)
        {
            double right = Math.Max(P1.X, P2.X);
            double left = Math.Min(P1.X, P2.X);
            double top = Math.Max(P1.Y, P2.Y);
            double bottom = Math.Min(P1.Y, P2.Y);

            if (p.X > right)
                return false;
            if (p.X < left)
                return false;
            if (p.Y > top)
                return false;
            if (p.Y < bottom)
                return false;

            return true;
        }
    }
}
