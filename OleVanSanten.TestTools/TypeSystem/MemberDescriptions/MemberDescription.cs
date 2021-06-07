using System;
using System.Collections.Generic;
using System.Text;

namespace OleVanSanten.TestTools.TypeSystem
{
    public abstract class MemberDescription
    {
        public abstract TypeDescription DeclaringType { get; }

        public abstract Attribute[] GetCustomAttributes();

        public abstract TypeDescription[] GetCustomAttributeTypes();

        public abstract MemberTypes MemberType { get; }

        public abstract string Name { get; }
    }
}
