using System;
using System.Collections.Generic;
using System.Text;

namespace OleVanSanten.TestTools.TypeSystem
{
    // Unifying class for wrappers of System.Reflection.PropertyInfo and Microsoft.CodeAnalysis.IPropertySymbol 
    public abstract class PropertyDescription : MemberDescription
    {
        // TODO add attributes

        public abstract bool CanRead { get; }

        public abstract bool CanWrite { get; }
        
        public abstract MethodDescription GetMethod { get; }

        public override MemberTypes MemberType => MemberTypes.Property;

        public abstract TypeDescription PropertyType { get; }

        public abstract MethodDescription SetMethod { get; }
    }
}
