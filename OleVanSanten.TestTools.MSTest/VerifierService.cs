using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OleVanSanten.TestTools.Helpers;
using OleVanSanten.TestTools.Structure;

namespace OleVanSanten.TestTools.MSTest
{
    public class VerifierService : VerifierServiceBase
    {
        public override void Fail(string message)
        {
            throw new AssertFailedException(message);
        }
    }
}
