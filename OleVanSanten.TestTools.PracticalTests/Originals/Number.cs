using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Structure.Attributes;

namespace Lecture_2_Solutions
{
    public class Number
    {
        public Number(int value)
        {
            Value = value;
        }

        [ReadonlyProperty]
        public int Value { get; private set; }

        public void Add(Number operand)
        {
            Value += operand.Value;
        }

        public void Subtract(Number operand)
        {
            Value -= operand.Value;
        }

        public void Multiply(Number operand)
        {
            Value *= operand.Value;
        }

        public override bool Equals(object obj)
        {
            Number other = (Number)obj;

            return other.Value == Value;
        }

        public override int GetHashCode()
        {
            return Value;
        }
    }
}
