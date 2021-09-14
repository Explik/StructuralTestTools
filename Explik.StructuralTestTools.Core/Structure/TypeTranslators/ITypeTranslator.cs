using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public interface ITypeTranslator
    {
        TypeDescription Translate(TypeTranslateArgs args);
    }

    public struct TypeTranslateArgs
    {
        public VerifierService Verifier { get; set; }
        public NamespaceDescription TargetNamespace { get; set; }
        public TypeDescription OriginalType { get; set; }
    }
}
