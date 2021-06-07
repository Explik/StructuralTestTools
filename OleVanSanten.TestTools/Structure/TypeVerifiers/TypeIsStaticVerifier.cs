using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class TypeIsStaticVerifier : TypeVerifier
    {
        bool _isStatic;

        public TypeIsStaticVerifier(bool isStatic = true)
        {
            _isStatic = isStatic;
        }

        public override TypeVerificationAspect[] Aspects => new[] {
            TypeVerificationAspect.IsStatic 
        };

        public override void Verify(TypeDescription originalType, TypeDescription translatedType)
        {
            Verifier.VerifyIsStatic(translatedType, _isStatic);
        }
    }
}
