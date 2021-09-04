using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools
{
    public interface ITypeTranslator
    {
        TypeDescription Translate(TypeTranslateArgs args);
    }

    public struct TypeTranslateArgs
    {
        public VerifierServiceBase Verifier { get; set; }
        public NamespaceDescription TargetNamespace { get; set; }
        public TypeDescription OriginalType { get; set; }
    }
}
