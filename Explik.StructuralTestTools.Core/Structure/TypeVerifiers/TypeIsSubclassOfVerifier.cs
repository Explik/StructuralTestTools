﻿using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class TypeIsSubclassOfVerifier : ITypeVerifier
    {
        Type _type;

        public TypeIsSubclassOfVerifier(Type type)
        {
            _type = type;
        }

        public TypeVerificationAspect[] Aspects => new[]
        {
            TypeVerificationAspect.IsSubclassOf
        };

        public void Verify(TypeVerifierArgs args)
        {
            var baseType = args.TypeTranslatorService.TranslateType(new RuntimeTypeDescription(_type));
            args.Verifier.VerifyIsSubclassOf(args.TranslatedType, baseType);
        }
    }
}
