using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Helpers;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools
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
