using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools;

namespace Lecture_2_Solutions
{
    public class ImmutableNumber
    {
        public ImmutableNumber(int value)
        {
            Value = value;
        }

        [ReadonlyProperty]
        public int Value { get; }

        public ImmutableNumber Add(ImmutableNumber operand)
        {
            return new ImmutableNumber(Value + operand.Value);
        }

        public ImmutableNumber Subtract(ImmutableNumber operand)
        {
            return new ImmutableNumber(Value - operand.Value);
        }

        public ImmutableNumber Multiply(ImmutableNumber operand)
        {
            return new ImmutableNumber(Value * operand.Value);
        }
    }
}
