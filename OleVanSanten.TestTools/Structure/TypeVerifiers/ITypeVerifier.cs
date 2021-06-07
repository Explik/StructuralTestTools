using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public interface ITypeVerifier
    {
        VerifierServiceBase Verifier { get; set; }
        IStructureService Service { get; set; }
        TypeVerificationAspect[] Aspects { get; }
        void Verify(TypeDescription originalType, TypeDescription translatedType);
    }
}
