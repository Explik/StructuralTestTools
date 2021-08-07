using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public interface IMemberVerifier
    {
        MemberVerificationAspect[] Aspects { get; }
        
        void Verify(MemberVerifierArgs args);
    }

    public struct MemberVerifierArgs
    {
        public VerifierServiceBase Verifier { get; set; }
        public ITypeTranslatorService TypeTranslatorService { get; set; }
        public ITypeVerifierService TypeVerifierService { get; set; }
        public IMemberTranslatorService MemberTranslatorService { get; set; }
        public MemberDescription OriginalMember { get; set; }
        public MemberDescription TranslatedMember { get; set; }
    }
}
