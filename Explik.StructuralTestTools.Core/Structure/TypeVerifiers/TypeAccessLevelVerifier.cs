﻿using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class TypeAccessLevelVerifier : ITypeVerifier
    {
        AccessLevels[] _accessLevels;

        public TypeAccessLevelVerifier(AccessLevels accessLevel) : this(new[] { accessLevel })
        {
        }

        public TypeAccessLevelVerifier(AccessLevels[] accessLevels)
        {
            _accessLevels = accessLevels;
        }

        public TypeVerificationAspect[] Aspects => new[]
        {
            TypeVerificationAspect.AccessLevel
        };

        public void Verify(TypeVerifierArgs args)
        {
            args.Verifier.VerifyAccessLevel(args.TranslatedType, _accessLevels);
        }
    }
}
