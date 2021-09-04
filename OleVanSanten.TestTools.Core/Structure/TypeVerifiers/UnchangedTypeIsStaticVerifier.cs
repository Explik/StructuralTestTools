using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Helpers;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools
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
