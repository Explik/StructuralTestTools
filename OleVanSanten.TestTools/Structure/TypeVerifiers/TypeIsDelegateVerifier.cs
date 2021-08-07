using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
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
