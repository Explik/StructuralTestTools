using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class UnchangedFieldTypeVerifier : IMemberVerifier
    {
        public MemberVerificationAspect[] Aspects => new[] { MemberVerificationAspect.FieldType };

        public void Verify(MemberVerifierArgs args)
        {
            FieldDescription translatedField = args.TranslatedMember as FieldDescription;

            args.Verifier.VerifyMemberType(args.TranslatedMember, new MemberTypes[] { MemberTypes.Field, MemberTypes.Property });

            if (args.OriginalMember is FieldDescription originalField)
                args.Verifier.VerifyFieldType(translatedField, originalField.FieldType);
            else if (args.OriginalMember is PropertyDescription originalProperty)
                args.Verifier.VerifyFieldType(translatedField, originalProperty.PropertyType);
            else throw new NotImplementedException();
        }
    }
}
