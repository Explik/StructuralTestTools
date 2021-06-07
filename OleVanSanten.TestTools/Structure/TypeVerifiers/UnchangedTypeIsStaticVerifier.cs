using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Helpers;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class UnchangedTypeIsStaticVerifier : TypeVerifier
    {
        public override TypeVerificationAspect[] Aspects => new[] { 
            TypeVerificationAspect.IsStatic
        };

        public override void Verify(TypeDescription originalType, TypeDescription translatedType)
        {
            bool isStatic = originalType.IsAbstract && originalType.IsSealed;
            Verifier.VerifyIsStatic(translatedType, isStatic);
        }
    }
}
