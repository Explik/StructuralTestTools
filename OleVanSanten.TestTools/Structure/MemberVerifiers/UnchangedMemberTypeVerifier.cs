using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class UnchangedMemberTypeVerifier : IMemberVerifier
    {
        public MemberVerificationAspect[] Aspects => new[] { MemberVerificationAspect.MemberType };

        public void Verify(MemberVerifierArgs args)
        {
            switch (args.OriginalMember.MemberType)
            {
                case MemberTypes.Constructor:
                    args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Constructor });
                    break;
                case MemberTypes.Event:
                    args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Event });
                    break;
                case MemberTypes.Field:
                    args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Field });
                    break;
                case MemberTypes.Method:
                    args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Method });
                    break;
                case MemberTypes.Property:
                    args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Property });
                    break;
                default: throw new NotImplementedException();
            }
        }
    }
}
