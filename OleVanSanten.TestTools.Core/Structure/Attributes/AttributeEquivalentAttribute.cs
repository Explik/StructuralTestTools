using System;
using System.Collections.Generic;
using System.Text;

namespace OleVanSanten.TestTools
{
    public class AttributeEquivalentAttribute : Attribute
    {
        public AttributeEquivalentAttribute(string equvilentAttribute)
        {
            EquavilentAttribute = equvilentAttribute;
        }

        public string EquavilentAttribute { get; }
    }
}
