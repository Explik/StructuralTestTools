using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class TypeIsAbstractVerifier : ITypeVerifier
    {
        bool _isAbstract;

        public TypeIsAbstractVerifier(bool isAbstract = true)
        {
            _isAbstract = isAbstract;
        }

        public TypeVerificationAspect[] Aspects => new[] {
            TypeVerificationAspect.IsAbstract
        };

        public void Verify(TypeVerifierArgs args)
        {
            args.Verifier.VerifyIsAbstract(args.TranslatedType, _isAbstract);
        }
    }
}
