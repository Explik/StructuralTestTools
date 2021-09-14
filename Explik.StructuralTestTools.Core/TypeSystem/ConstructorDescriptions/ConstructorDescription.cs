using System;
using System.Collections.Generic;
using System.Text;

namespace Explik.StructuralTestTools.TypeSystem
{
    public abstract class ConstructorDescription : MethodBaseDescription
    {
        // TODO add Attributes

        public override bool IsGenericMethod => false;

        public override MemberTypes MemberType => MemberTypes.Constructor;

        public override TypeDescription[] GetGenericArguments()
        {
            return new TypeDescription[0];
        }
    }
}
