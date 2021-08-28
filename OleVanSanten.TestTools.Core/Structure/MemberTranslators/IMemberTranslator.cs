using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public interface IMemberTranslator
    {
        MemberDescription Translate(MemberTranslatorArgs args);
    }

    public struct MemberTranslatorArgs
    {
        public VerifierServiceBase Verifier { get; set; }
        public ITypeTranslatorService TypeTranslatorService { get; set; }
        public ITypeVerifierService TypeVerifierService { get; set; }
        public TypeDescription TargetType { get; set; }
        public MemberDescription OriginalMember { get; set; }
    }
}
