using System;
using System.Collections.Generic;
using System.Text;

namespace OleVanSanten.TestTools.TypeSystem
{
    public abstract class FieldDescription : MemberDescription
    {
        // Todo add Attributes

        public abstract TypeDescription FieldType { get; }

        public abstract bool IsAssembly { get; }

        public abstract bool IsFamily { get; }

        public abstract bool IsInitOnly { get; }

        public abstract bool IsPrivate { get; }

        public abstract bool IsPublic { get; }

        public abstract bool IsStatic { get; }

        public override MemberTypes MemberType => MemberTypes.Field;
    }
}