using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public abstract class TypeVerifier : ITypeVerifier
    {
        public VerifierServiceBase Verifier { get; set; }

        public IStructureService Service { get; set; }

        public abstract TypeVerificationAspect[] Aspects { get; }

        public abstract void Verify(TypeDescription originalType, TypeDescription translatedType);
    }
}
