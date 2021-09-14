using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
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
