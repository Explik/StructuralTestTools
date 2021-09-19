using Explik.StructuralTestTools.TypeSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Explik.StructuralTestTools
{
    public class FieldTypeVerifier : IMemberVerifier
    {
        private readonly Type _fieldType;

        public FieldTypeVerifier(Type fieldType)
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
            args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Field });

            if (args.TranslatedMember is FieldDescription fieldDescription)
            {
                args.Verifier.VerifyFieldType(fieldDescription, new RuntimeTypeDescription(_fieldType));
            }
            else throw new NotImplementedException();
        } 
    }
}
