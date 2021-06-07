using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class TypeIsAbstractVerifier : TypeVerifier
    {
        bool _isAbstract;

        public TypeIsAbstractVerifier(bool isAbstract = true)
        {
            _isAbstract = isAbstract;
        }

        public override TypeVerificationAspect[] Aspects => new[] {
            TypeVerificationAspect.IsAbstract
        };

        public override void Verify(TypeDescription originalType, TypeDescription translatedType)
        {
            Verifier.VerifyIsAbstract(translatedType, _isAbstract);
        }
    }
}
