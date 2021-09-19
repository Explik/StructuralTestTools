using Explik.StructuralTestTools.TypeSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Explik.StructuralTestTools
{
    public class PropertyTypeVerifier : IMemberVerifier
    {
        private readonly Type _propertyType;

        public PropertyTypeVerifier(Type propertyType)
        {
            _propertyType = propertyType;
        }

        public MemberVerificationAspect[] Aspects => new[]
        {
            MemberVerificationAspect.MemberType,
            MemberVerificationAspect.PropertyType
        };

        public void Verify(MemberVerifierArgs args)
        {
            args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Property });

            if (args.TranslatedMember is PropertyDescription propertyDescription)
            {
                args.Verifier.VerifyPropertyType(propertyDescription, new RuntimeTypeDescription(_propertyType));
            }
            else throw new NotImplementedException();
        }
    }
}
