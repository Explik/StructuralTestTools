using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class UnchangedPropertyTypeVerifier : IMemberVerifier
    {
        public MemberVerificationAspect[] Aspects => new[] { MemberVerificationAspect.PropertyType };

        public void Verify(MemberVerifierArgs args)
        {
            PropertyDescription originalProperty = args.OriginalMember as PropertyDescription;
            TypeDescription originalPropertyType = args.TypeTranslatorService.TranslateType(originalProperty.PropertyType);

            args.Verifier.VerifyMemberType(args.TranslatedMember, new MemberTypes[] { MemberTypes.Field, MemberTypes.Property });

            if (args.TranslatedMember is FieldDescription translatedField)
                args.Verifier.VerifyFieldType(translatedField, originalPropertyType);
            else if (args.TranslatedMember is PropertyDescription translatedProperty)
                args.Verifier.VerifyPropertyType(translatedProperty, originalPropertyType);
            else throw new NotImplementedException();
        }
    }
}
