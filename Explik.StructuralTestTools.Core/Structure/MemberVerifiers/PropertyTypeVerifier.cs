using Explik.StructuralTestTools.TypeSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Explik.StructuralTestTools
{
    public class PropertyTypeVerifier : IMemberVerifier
    {
        private readonly TypeDescription _propertyType;

        public PropertyTypeVerifier(Type propertyType) : this(new RuntimeTypeDescription(propertyType))
        {
        }

        public PropertyTypeVerifier(TypeDescription propertyType)
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
            TypeDescription translatedPropertyType = args.TypeTranslatorService.TranslateType(_propertyType);

            args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Property });

            if (args.TranslatedMember is PropertyDescription propertyDescription)
            {
                args.Verifier.VerifyPropertyType(propertyDescription, translatedPropertyType);
            }
            else throw new NotImplementedException();
        }
    }
}
