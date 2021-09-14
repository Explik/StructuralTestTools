using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public interface IMemberVerifier
    {
        MemberVerificationAspect[] Aspects { get; }
        
        void Verify(MemberVerifierArgs args);
    }

    public struct MemberVerifierArgs
    {
        public VerifierService Verifier { get; set; }
        public ITypeTranslatorService TypeTranslatorService { get; set; }
        public ITypeVerifierService TypeVerifierService { get; set; }
        public IMemberTranslatorService MemberTranslatorService { get; set; }
        public MemberDescription OriginalMember { get; set; }
        public MemberDescription TranslatedMember { get; set; }
    }
}
