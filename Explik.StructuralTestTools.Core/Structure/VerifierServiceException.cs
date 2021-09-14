using System;
using System.Collections.Generic;
using System.Text;

namespace Explik.StructuralTestTools
{
    public class VerifierServiceException : Exception
    {
        public VerifierServiceException(string message) : base(message)
        {
        }
    }
}
