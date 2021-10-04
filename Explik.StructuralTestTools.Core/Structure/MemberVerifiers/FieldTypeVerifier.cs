using Explik.StructuralTestTools.TypeSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Explik.StructuralTestTools
{
    public class FieldTypeVerifier : IMemberVerifier
    {
        private readonly TypeDescription _fieldType;

        public FieldTypeVerifier(Type fieldType) : this(new RuntimeTypeDescription(fieldType))
        {
        }

        public FieldTypeVerifier(TypeDescription fieldType)
        {
            _fieldType = fieldType;
        }

        public MemberVerificationAspect[] Aspects => new[]
        {
            MemberVerificationAspect.MemberType,
            MemberVerificationAspect.FieldType
        };

        public void Verify(MemberVerifierArgs args)
        {
            TypeDescription translatedFieldType = args.TypeTranslatorService.TranslateType(_fieldType);

            args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Field });

            if (args.TranslatedMember is FieldDescription fieldDescription)
            {
                args.Verifier.VerifyFieldType(fieldDescription, translatedFieldType);
            }
            else throw new NotImplementedException();
        } 
    }
}
