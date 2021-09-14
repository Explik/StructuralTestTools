using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools.Helpers;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class UnchangedTypeAccessLevelVerifier : ITypeVerifier
    {
        public TypeVerificationAspect[] Aspects => new[] { 
            TypeVerificationAspect.AccessLevel 
        };

        public void Verify(TypeVerifierArgs args)
        {
            AccessLevels accessLevel = DescriptionHelper.GetAccessLevel(args.OriginalType);
            args.Verifier.VerifyAccessLevel(args.TranslatedType, new[] { accessLevel });
        }
    }
}
