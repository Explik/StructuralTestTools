using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class UnchangedMemberIsVirtualVerifier : MemberVerifier
    {
        public override MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.PropertyGetIsVirtual,
            MemberVerificationAspect.PropertySetIsVirtual,
            MemberVerificationAspect.MethodIsVirtual
        };

        public override void Verify(MemberDescription originalMember, MemberDescription translatedMember)
        {
            if (originalMember is PropertyDescription originalProperty)
            {
                Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Property });

                if (originalProperty.CanRead)
                    Verifier.VerifyIsVirtual((PropertyDescription)translatedMember, originalProperty.GetMethod.IsVirtual, GetMethod: true);

                if (originalProperty.CanWrite)
                    Verifier.VerifyIsVirtual((PropertyDescription)translatedMember, originalProperty.SetMethod.IsVirtual, SetMethod: true);
            }
            else if (translatedMember is MethodDescription originalMethod)
            {
                Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Method });
                Verifier.VerifyIsStatic((MethodDescription)translatedMember, originalMethod.IsStatic);
            }
            else throw new NotImplementedException();
        }
    }
}
