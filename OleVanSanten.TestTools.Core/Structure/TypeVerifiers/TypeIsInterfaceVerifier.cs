using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools
{
    public class TypeIsInterfaceVerifier : ITypeVerifier
    {
        public TypeVerificationAspect[] Aspects => new[] 
        {
            TypeVerificationAspect.IsInterface 
        };

        public void Verify(TypeVerifierArgs args)
        {
            args.Verifier.VerifyIsInterface(args.TranslatedType);
        }
    }
}
