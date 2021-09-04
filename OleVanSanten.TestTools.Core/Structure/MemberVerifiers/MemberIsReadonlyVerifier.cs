using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Helpers;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools
{
    public class MemberIsReadonlyVerifier : IMemberVerifier
    {
        public MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.MemberType,
            MemberVerificationAspect.FieldAccessLevel
        };

        public void Verify(MemberVerifierArgs args)
        {
            args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Field, MemberTypes.Property });
            
            if (args.TranslatedMember is FieldDescription fieldInfo)
            {
                args.Verifier.VerifyIsInitOnly(fieldInfo, true);
            }
            else if(args.TranslatedMember is PropertyDescription propertyInfo)
            {
                args.Verifier.VerifyIsReadonly(propertyInfo, DescriptionHelper.GetAccessLevel(propertyInfo));
            } 
        }
    }
}
