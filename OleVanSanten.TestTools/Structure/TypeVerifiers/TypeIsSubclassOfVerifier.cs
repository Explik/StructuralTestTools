using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class TypeIsSubclassOfVerifier : TypeVerifier
    {
        Type _type;

        public TypeIsSubclassOfVerifier(Type type)
        {
            _type = type;
        }

        public override TypeVerificationAspect[] Aspects => new[]
        {
            TypeVerificationAspect.IsSubclassOf
        };

        public override void Verify(TypeDescription originalType, TypeDescription translatedType)
        {
            var baseType = Service.TranslateType(new RuntimeTypeDescription(_type));
            Verifier.VerifyIsSubclassOf(translatedType, baseType);
        }
    }
}
