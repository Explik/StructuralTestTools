using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools
{
    public class FieldOrPropertyAttribute : Attribute, IMemberVerifier
    {
        public MemberVerificationAspect[] Aspects => new[] { MemberVerificationAspect.MemberType };

        public void Verify(MemberVerifierArgs args)
        {
            args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Field, MemberTypes.Property });
        }
    }
}
