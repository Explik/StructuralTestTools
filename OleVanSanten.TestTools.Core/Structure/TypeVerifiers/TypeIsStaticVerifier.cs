using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools
{
    public class TypeIsStaticVerifier : ITypeVerifier
    {
        bool _isStatic;

        public TypeIsStaticVerifier(bool isStatic = true)
        {
            _isStatic = isStatic;
        }

        public TypeVerificationAspect[] Aspects => new[] {
            TypeVerificationAspect.IsStatic 
        };

        public void Verify(TypeVerifierArgs args)
        {
            args.Verifier.VerifyIsStatic(args.TranslatedType, _isStatic);
        }
    }
}
