using System;
using System.Collections.Generic;
using System.Text;

namespace OleVanSanten.TestTools.Structure
{
    public class TypeVerificationAspect
    {
        string _name;

        public TypeVerificationAspect(string name)
        {
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }

        public static TypeVerificationAspect AccessLevel { get; } = new TypeVerificationAspect(nameof(AccessLevel));
        public static TypeVerificationAspect IsAbstract { get; } = new TypeVerificationAspect(nameof(IsAbstract));
        public static TypeVerificationAspect IsStatic { get; } = new TypeVerificationAspect(nameof(IsStatic));
        public static TypeVerificationAspect IsSubclassOf { get; } = new TypeVerificationAspect(nameof(IsSubclassOf)); 
        public static TypeVerificationAspect IsInterface { get; } = new TypeVerificationAspect(nameof(IsInterface));
        public static TypeVerificationAspect IsDelegate { get; } = new TypeVerificationAspect(nameof(IsDelegate));
        public static TypeVerificationAspect DelegateSignature { get; } = new TypeVerificationAspect(nameof(IsDelegate));
    }
}
