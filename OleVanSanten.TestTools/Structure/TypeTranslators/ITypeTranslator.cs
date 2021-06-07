using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public interface ITypeTranslator
    {
        NamespaceDescription TargetNamespace { get; set; }
        VerifierServiceBase Verifier { get; set; }
        TypeDescription Translate(TypeDescription type);
    }
}
