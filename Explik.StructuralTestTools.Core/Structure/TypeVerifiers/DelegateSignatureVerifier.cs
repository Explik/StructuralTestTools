using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class DelegateSignatureVerifier : ITypeVerifier
    {
        Type _delegateType; 

        public DelegateSignatureVerifier(Type delegateType) 
        {
            _delegateType = delegateType;
        }

        public TypeVerificationAspect[] Aspects => new[]
        {
            TypeVerificationAspect.DelegateSignature
        };

        public void Verify(TypeVerifierArgs args)
        {
            args.Verifier.VerifyIsDelegate(args.TranslatedType);
            args.Verifier.VerifyDelegateSignature(args.TranslatedType, new RuntimeMethodDescription(_delegateType.GetMethod("Invoke")));
        }
    }
}
