using Explik.StructuralTestTools.TypeSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Explik.StructuralTestTools
{
    public class MemoryConfiguration : IConfiguration
    {
        public NamespaceDescription GlobalNamespace { get; set; }

        public NamespaceDescription FromNamespace { get; set; }

        public NamespaceDescription ToNamespace { get; set; }


        public FileInfo ProjectFile { get; set; }

        public FileInfo[] TemplateFiles { get; set; }


        public ITypeTranslator TypeTranslator { get; set; }

        public IMemberTranslator MemberTranslator { get; set; }


        public ITypeVerifier[] TypeVerifiers { get; set; }

        public TypeVerificationAspect[] TypeVerificationOrder { get; set; }

        public IMemberVerifier[] MemberVerifiers { get; set; }

        public MemberVerificationAspect[] MemberVerificationOrder { get; set; }
    }
}
