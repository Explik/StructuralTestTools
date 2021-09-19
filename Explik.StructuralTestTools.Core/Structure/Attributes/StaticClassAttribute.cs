  using System;
using System.Collections.Generic;
using System.Text;

namespace Explik.StructuralTestTools
{
    public class StaticClassAttribute : Attribute, ITypeVerifier
    {
        public TypeVerificationAspect[] Aspects => new[]
        {
            TypeVerificationAspect.IsStatic
        };

        public void Verify(TypeVerifierArgs args)
        {
            args.Verifier.VerifyIsStatic(args.TranslatedType, isStatic: true);
        }
    }
}
