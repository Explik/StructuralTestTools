using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class DelegateSignatureVerifier : TypeVerifier
    {
        Type _delegateType; 

        public DelegateSignatureVerifier(Type delegateType) 
        {
            _delegateType = delegateType;
        }

        public override TypeVerificationAspect[] Aspects => new[]
        {
            TypeVerificationAspect.DelegateSignature
        };

        public override void Verify(TypeDescription originalType, TypeDescription translatedType)
        {
            Verifier.VerifyIsDelegate(translatedType);
            Verifier.VerifyDelegateSignature(translatedType, new RuntimeMethodDescription(_delegateType.GetMethod("Invoke")));
        }
    }
}
