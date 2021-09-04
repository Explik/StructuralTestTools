using OleVanSanten.TestTools.TypeSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace OleVanSanten.TestTools
{
    public class MemoryConfiguration : IConfiguration
    {
        public NamespaceDescription GlobalNamespace { get; set; }

        public NamespaceDescription FromNamespace { get; set; }

        public NamespaceDescription ToNamespace { get; set; }


        public ITypeTranslator TypeTranslator { get; set; }

        public IMemberTranslator MemberTranslator { get; set; }


        public ITypeVerifier[] TypeVerifiers { get; set; } = new ITypeVerifier[0];

        public TypeVerificationAspect[] TypeVerificationOrder { get; set; } = new TypeVerificationAspect[0];

        public IMemberVerifier[] MemberVerifiers { get; set; } = new IMemberVerifier[0];

        public MemberVerificationAspect[] MemberVerificationOrder { get; set; } = new MemberVerificationAspect[0];
    }
}
