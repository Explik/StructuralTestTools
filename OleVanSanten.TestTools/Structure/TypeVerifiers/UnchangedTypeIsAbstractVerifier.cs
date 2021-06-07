using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class UnchangedTypeIsAbstractVerifier : TypeVerifier
    {
        public override TypeVerificationAspect[] Aspects => new[] { 
            TypeVerificationAspect.IsAbstract
        };

        public override void Verify(TypeDescription originalType, TypeDescription translatedType)
        {
            Verifier.VerifyIsAbstract(translatedType, originalType.IsAbstract);
        }
    }
}
