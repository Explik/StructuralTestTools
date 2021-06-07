using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class UnchangedMemberTypeVerifier : MemberVerifier
    {
        public override MemberVerificationAspect[] Aspects => new[] { MemberVerificationAspect.MemberType };

        public override void Verify(MemberDescription originalMember, MemberDescription translatedMember)
        {
            switch (originalMember.MemberType)
            {
                case MemberTypes.Constructor:
                    Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Constructor });
                    break;
                case MemberTypes.Event:
                    Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Event });
                    break;
                case MemberTypes.Field:
                    Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Field });
                    break;
                case MemberTypes.Method:
                    Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Method });
                    break;
                case MemberTypes.Property:
                    Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Property });
                    break;
                default: throw new NotImplementedException();
            }
        }
    }
}
