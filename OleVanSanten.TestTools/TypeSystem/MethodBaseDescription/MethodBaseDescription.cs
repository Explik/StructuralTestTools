using System;
using System.Collections.Generic;
using System.Text;

namespace OleVanSanten.TestTools.TypeSystem
{
    public abstract class MethodBaseDescription : MemberDescription
    {
        // TODO add Attributes

        public abstract bool IsAbstract { get; }

        public abstract bool IsAssembly { get; }

        public abstract bool IsGenericMethod { get; }

        public abstract bool IsFamily { get; }

        public abstract bool IsPrivate { get; }

        public abstract bool IsPublic { get; }

        public abstract bool IsStatic { get; }

        public abstract bool IsVirtual { get; }

        public abstract TypeDescription[] GetGenericArguments();

        public abstract ParameterDescription[] GetParameters();
    }
}
