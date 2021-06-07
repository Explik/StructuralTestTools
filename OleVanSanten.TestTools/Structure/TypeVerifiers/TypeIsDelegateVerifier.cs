using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class TypeIsDelegateVerifier : TypeVerifier
    {
        public override TypeVerificationAspect[] Aspects => new[]
        {
            TypeVerificationAspect.IsDelegate
        };

        public override void Verify(TypeDescription originalType, TypeDescription translatedType)
        {
            Verifier.VerifyIsDelegate(translatedType);
        }
    }
}
