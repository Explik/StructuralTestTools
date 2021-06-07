using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Syntax;

namespace Lecture_9_Solutions
{
    public class Product : ICloneable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            Product other = obj as Product;
            return other?.ID == ID;
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }
}
