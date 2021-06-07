using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public abstract class TypeTranslator : ITypeTranslator
    {
        public NamespaceDescription TargetNamespace { get; set; }

        public VerifierServiceBase Verifier { get; set; }

        public abstract TypeDescription Translate(TypeDescription type);
    }
}
