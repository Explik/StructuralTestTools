using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class MemberTypeVerifier : IMemberVerifier
    {
        MemberTypes[] _memberTypes;

        public MemberTypeVerifier(MemberTypes memberType) : this(new[] { memberType })
        {
        }

        public MemberTypeVerifier(MemberTypes[] memberTypes)
        {
            _memberTypes = memberTypes;
        }

        public MemberVerificationAspect[] Aspects => new[] { MemberVerificationAspect.MemberType };

        public void Verify(MemberVerifierArgs args)
        {
            args.Verifier.VerifyMemberType(args.TranslatedMember, _memberTypes);
        }
    }
}
