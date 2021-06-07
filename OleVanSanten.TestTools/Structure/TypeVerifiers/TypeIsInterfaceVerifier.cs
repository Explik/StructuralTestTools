using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class TypeIsInterfaceVerifier : TypeVerifier
    {
        public override TypeVerificationAspect[] Aspects => new[] 
        {
            TypeVerificationAspect.IsInterface 
        };

        public override void Verify(TypeDescription originalType, TypeDescription translatedType)
        {
            Verifier.VerifyIsInterface(translatedType);
        }
    }
}
