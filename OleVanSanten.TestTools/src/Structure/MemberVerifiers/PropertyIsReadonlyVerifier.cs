using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Helpers;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class PropertyIsReadonlyVerifier : IMemberVerifier
    {
        public MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.MemberType,
            MemberVerificationAspect.PropertyGetAccessLevel,
            MemberVerificationAspect.PropertySetAccessLevel
        };

        public void Verify(MemberVerifierArgs args)
        {
            args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Property });

            if (args.TranslatedMember is PropertyDescription propertyInfo)
            {
                args.Verifier.VerifyIsReadonly(propertyInfo, DescriptionHelper.GetAccessLevel(propertyInfo.GetMethod));
            }
            else throw new NotImplementedException();
        }
    }
}
