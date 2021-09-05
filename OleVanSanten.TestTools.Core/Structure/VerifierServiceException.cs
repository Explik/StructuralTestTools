using System;
using System.Collections.Generic;
using System.Text;

namespace OleVanSanten.TestTools
{
    public class VerifierServiceException : Exception
    {
        public VerifierServiceException(string message) : base(message)
        {
        }
    }
}
