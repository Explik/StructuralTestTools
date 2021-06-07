using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public abstract class MemberVerifier : IMemberVerifier
    {
        public VerifierServiceBase Verifier { get; set; }

        public IStructureService Service { get; set; }

        public abstract MemberVerificationAspect[] Aspects { get; }

        public abstract void Verify(MemberDescription originalMember, MemberDescription translatedMember);
    }
}
