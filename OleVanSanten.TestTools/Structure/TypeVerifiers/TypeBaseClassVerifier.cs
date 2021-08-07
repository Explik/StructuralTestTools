using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class TypeBaseClassVerifier : ITypeVerifier
    {
        Type _type;

        public TypeBaseClassVerifier(Type type)
        {
            _type = type;
        }

        public TypeVerificationAspect[] Aspects => new[]
        {
            TypeVerificationAspect.IsSubclassOf
        };

        public void Verify(TypeVerifierArgs args)
        {
            args.Verifier.VerifyBaseType(args.TranslatedType, new RuntimeTypeDescription(_type));
        }
    }
}
