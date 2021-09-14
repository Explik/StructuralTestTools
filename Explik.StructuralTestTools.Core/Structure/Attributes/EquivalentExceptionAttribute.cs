using System;
using System.Collections.Generic;
using System.Text;

namespace Explik.StructuralTestTools
{
    public class EquivalentExceptionAttribute : Attribute
    {
        public EquivalentExceptionAttribute(string equvilentException)
        {
            EquavilentException = equvilentException;
        }

        public string EquavilentException { get; }
    }
}
