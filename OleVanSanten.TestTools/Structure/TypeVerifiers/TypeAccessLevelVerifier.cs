using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class TypeAccessLevelVerifier : TypeVerifier
    {
        AccessLevels[] _accessLevels;

        public TypeAccessLevelVerifier(AccessLevels accessLevel) : this(new[] { accessLevel })
        {
        }

        public TypeAccessLevelVerifier(AccessLevels[] accessLevels)
        {
            _accessLevels = accessLevels;
        }

        public override TypeVerificationAspect[] Aspects => new[]
        {
            TypeVerificationAspect.AccessLevel
        };

        public override void Verify(TypeDescription originalType, TypeDescription translatedType)
        {
            Verifier.VerifyAccessLevel(translatedType, _accessLevels);
        }
    }
}
