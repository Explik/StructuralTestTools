using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools
{
    public interface ITypeVerifier
    {
        TypeVerificationAspect[] Aspects { get; }
        void Verify(TypeVerifierArgs args);
    }

    public struct TypeVerifierArgs 
    {
        public VerifierService Verifier { get; set; }
        public ITypeTranslatorService TypeTranslatorService { get; set; }
        public TypeDescription OriginalType { get; set; }
        public TypeDescription TranslatedType { get; set; }
    }
}
