using System;
using System.Collections.Generic;
using System.Text;

namespace OleVanSanten.TestTools.Structure
{
    // Platform independent implementation of VerifierServiceBase used by Source Generators
    public class VerifierService : VerifierServiceBase
    {
        public override void Fail(string message)
        {
            throw new VerifierServiceException(message);
        }
    }

    public class VerifierServiceException : Exception
    {
        public VerifierServiceException(string message) : base(message)
        {
        }
    }
}
