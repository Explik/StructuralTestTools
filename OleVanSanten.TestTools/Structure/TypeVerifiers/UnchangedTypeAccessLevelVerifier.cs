using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Helpers;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class UnchangedTypeAccessLevelVerifier : TypeVerifier
    {
        public override TypeVerificationAspect[] Aspects => new[] { 
            TypeVerificationAspect.AccessLevel 
        };

        public override void Verify(TypeDescription originalType, TypeDescription translatedType)
        {
            AccessLevels accessLevel = DescriptionHelper.GetAccessLevel(originalType);
            Verifier.VerifyAccessLevel(translatedType, new[] { accessLevel });
        }
    }
}
