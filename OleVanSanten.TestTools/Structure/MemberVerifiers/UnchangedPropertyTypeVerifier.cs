using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class UnchangedPropertyTypeVerifier : MemberVerifier
    {
        public override MemberVerificationAspect[] Aspects => new[] { MemberVerificationAspect.PropertyType };

        public override void Verify(MemberDescription originalMember, MemberDescription translatedMember)
        {
            PropertyInfo originalProperty = originalMember as PropertyInfo;
            Type originalPropertyType = Service.TranslateType(originalProperty.PropertyType);

            Verifier.VerifyMemberType(translatedMember, new MemberTypes[] { MemberTypes.Field, MemberTypes.Property });

            if (translatedMember is FieldInfo translatedField)
                Verifier.VerifyFieldType(translatedField, originalPropertyType);
            else if (translatedMember is PropertyInfo translatedProperty)
                Verifier.VerifyPropertyType(translatedProperty, originalPropertyType);
            else throw new NotImplementedException();
        }
    }
}
