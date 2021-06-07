using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Helpers;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class MemberIsReadonlyVerifier : MemberVerifier
    {
        public override MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.MemberType,
            MemberVerificationAspect.FieldAccessLevel
        };

        public override void Verify(MemberDescription originalMember, MemberDescription translatedMember)
        {
            Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Field, MemberTypes.Property });
            
            if (translatedMember is FieldDescription fieldInfo)
            {
                Verifier.VerifyIsInitOnly(fieldInfo, true);
            }
            else if(translatedMember is PropertyDescription propertyInfo)
            {
                Verifier.VerifyIsReadonly(propertyInfo, DescriptionHelper.GetAccessLevel(propertyInfo));
            } 
        }
    }
}
