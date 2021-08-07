using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class FieldIsInitOnlyVerifier : IMemberVerifier
    {
        bool _isInitOnly;

        public FieldIsInitOnlyVerifier(bool isInitOnly = true)
        {
            _isInitOnly = isInitOnly;
        }

        public MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.MemberType,
            MemberVerificationAspect.FieldAccessLevel
        };

        public void Verify(MemberVerifierArgs args)
        {
            args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Field });
            args.Verifier.VerifyIsInitOnly((FieldDescription)args.TranslatedMember, _isInitOnly);
        }
    }
}
