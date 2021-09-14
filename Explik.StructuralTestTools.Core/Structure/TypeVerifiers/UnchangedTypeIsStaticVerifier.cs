using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools.Helpers;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class UnchangedTypeIsStaticVerifier : ITypeVerifier
    {
        public TypeVerificationAspect[] Aspects => new[] { 
            TypeVerificationAspect.IsStatic
        };

        public void Verify(TypeVerifierArgs args)
        {
            bool isStatic = args.OriginalType.IsAbstract && args.OriginalType.IsSealed;
            args.Verifier.VerifyIsStatic(args.TranslatedType, isStatic);
        }
    }
}
