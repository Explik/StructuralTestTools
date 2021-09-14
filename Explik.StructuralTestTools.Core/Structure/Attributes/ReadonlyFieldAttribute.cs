using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class ReadonlyFieldAttribute : Attribute, IMemberVerifier
    {
        public MemberVerificationAspect[] Aspects => new[] {  
            MemberVerificationAspect.MemberType,
            MemberVerificationAspect.FieldAccessLevel 
        };

        public void Verify(MemberVerifierArgs args)
        {
            args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Field });
            args.Verifier.VerifyIsInitOnly((FieldDescription)args.TranslatedMember, true);
        }
    }
}
