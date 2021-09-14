using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class TypeIsStaticVerifier : ITypeVerifier
    {
        bool _isStatic;

        public TypeIsStaticVerifier(bool isStatic = true)
        {
            _isStatic = isStatic;
        }

        public TypeVerificationAspect[] Aspects => new[] {
            TypeVerificationAspect.IsStatic 
        };

        public void Verify(TypeVerifierArgs args)
        {
            args.Verifier.VerifyIsStatic(args.TranslatedType, _isStatic);
        }
    }
}
