using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools.Helpers;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class PropertyIsReadonlyVerifier : IMemberVerifier
    {
        AccessLevels _accessLevels;

        public PropertyIsReadonlyVerifier(AccessLevels accessLevels)
        {
            _accessLevels = accessLevels;
        }

        public MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.MemberType,
            MemberVerificationAspect.PropertyGetAccessLevel,
            MemberVerificationAspect.PropertySetAccessLevel
        };

        public void Verify(MemberVerifierArgs args)
        {
            args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Property });

            if (args.TranslatedMember is PropertyDescription propertyInfo)
            {
                args.Verifier.VerifyIsReadonly(propertyInfo, _accessLevels);
            }
            else throw new NotImplementedException();
        }
    }
}
