using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure.Attributes
{
    public class ReadonlyFieldAttribute : Attribute, IMemberVerifier
    {
        public VerifierServiceBase Verifier { get; set; }

        public IStructureService Service { get; set; }

        public MemberVerificationAspect[] Aspects => new[] {  
            MemberVerificationAspect.MemberType,
            MemberVerificationAspect.FieldAccessLevel 
        };

        public void Verify(MemberDescription originalMember, MemberDescription translatedMember)
        {
            Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Field });
            Verifier.VerifyIsInitOnly((FieldDescription)translatedMember, true);
        }
    }
}
