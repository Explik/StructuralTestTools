using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class UnchangedTypeIsAbstractVerifier : ITypeVerifier
    {
        public TypeVerificationAspect[] Aspects => new[] { 
            TypeVerificationAspect.IsAbstract
        };

        public void Verify(TypeVerifierArgs args)
        {
            args.Verifier.VerifyIsAbstract(args.TranslatedType, args.OriginalType.IsAbstract);
        }
    }
}
