using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public interface IMemberTranslator
    {
        TypeDescription TargetType { get; set; }
        VerifierServiceBase Verifier { get; set; }
        MemberDescription Translate(MemberDescription member);
    }
}
