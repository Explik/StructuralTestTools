using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public interface IMemberTranslator
    {
        MemberDescription Translate(MemberTranslatorArgs args);
    }

    public struct MemberTranslatorArgs
    {
        public VerifierService Verifier { get; set; }
        public ITypeTranslatorService TypeTranslatorService { get; set; }
        public ITypeVerifierService TypeVerifierService { get; set; }
        public TypeDescription TargetType { get; set; }
        public MemberDescription OriginalMember { get; set; }
    }
}
