using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class TypeIsDelegateVerifier : ITypeVerifier
    {
        public TypeVerificationAspect[] Aspects => new[]
        {
            TypeVerificationAspect.IsDelegate
        };

        public void Verify(TypeVerifierArgs args)
        {
            args.Verifier.VerifyIsDelegate(args.TranslatedType);
        }
    }
}
