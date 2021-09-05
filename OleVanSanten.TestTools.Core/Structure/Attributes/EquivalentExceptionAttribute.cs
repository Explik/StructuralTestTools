using System;
using System.Collections.Generic;
using System.Text;

namespace OleVanSanten.TestTools
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
