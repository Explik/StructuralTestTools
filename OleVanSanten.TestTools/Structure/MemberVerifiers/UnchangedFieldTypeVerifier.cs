using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class UnchangedFieldTypeVerifier : MemberVerifier
    {
        public override MemberVerificationAspect[] Aspects => new[] { MemberVerificationAspect.FieldType };

        public override void Verify(MemberDescription originalMember, MemberDescription translatedMember)
        {
            FieldDescription translatedField = translatedMember as FieldDescription;

            Verifier.VerifyMemberType(translatedMember, new MemberTypes[] { MemberTypes.Field, MemberTypes.Property });

            if (originalMember is FieldDescription originalField)
                Verifier.VerifyFieldType(translatedField, originalField.FieldType);
            else if (originalMember is PropertyDescription originalProperty)
                Verifier.VerifyFieldType(translatedField, originalProperty.PropertyType);
            else throw new NotImplementedException();
        }
    }
}
