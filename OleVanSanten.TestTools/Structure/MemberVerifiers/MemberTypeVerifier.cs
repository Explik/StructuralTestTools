using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class MemberTypeVerifier : MemberVerifier
    {
        MemberTypes[] _memberTypes;

        public MemberTypeVerifier(MemberTypes memberType) : this(new[] { memberType })
        {
        }

        public MemberTypeVerifier(MemberTypes[] memberTypes)
        {
            _memberTypes = memberTypes;
        }

        public override MemberVerificationAspect[] Aspects => new[] { MemberVerificationAspect.MemberType };

        public override void Verify(MemberDescription originalMember, MemberDescription translatedMember)
        {
            Verifier.VerifyMemberType(translatedMember, _memberTypes);
        }
    }
}
