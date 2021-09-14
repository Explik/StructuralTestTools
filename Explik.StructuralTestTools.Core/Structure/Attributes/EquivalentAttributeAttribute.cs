using System;
using System.Collections.Generic;
using System.Text;

namespace Explik.StructuralTestTools
{
    public class EquivalentAttributeAttribute : Attribute
    {
        public EquivalentAttributeAttribute(string equvilentAttribute)
        {
            EquavilentAttribute = equvilentAttribute;
        }

        public string EquavilentAttribute { get; }
    }
}
