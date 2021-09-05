using System;
using System.Collections.Generic;
using System.Text;

namespace OleVanSanten.TestTools
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
