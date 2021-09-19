using Explik.StructuralTestTools.TypeSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Explik.StructuralTestTools
{
    public class Configuration
    {
        public static IConfiguration CreateEmpty()
        {
            return new MemoryConfiguration()
            {
                GlobalNamespace = null,
                FromNamespace = null,
                ToNamespace = null,
                TypeTranslator = null,
                MemberTranslator = null,
                TypeVerifiers = new ITypeVerifier[0],
                TypeVerificationOrder = new TypeVerificationAspect[0],
                MemberVerifiers = new IMemberVerifier[0],
                MemberVerificationOrder = new MemberVerificationAspect[0]
            };
        }

        public static IConfiguration CreateDefault(NamespaceDescription globalNamespace)
        {
            return new DefaultConfiguration() { GlobalNamespace = globalNamespace };
        }

        public static IConfiguration CreateFromXML(NamespaceDescription globalNamespace, string xml)
        {
            return new XMLConfiguration(xml) { GlobalNamespace = globalNamespace };
        }

        public static IConfiguration CreateFromXMLWithDefaults(NamespaceDescription globalNamespace, string xml)
        {
            var defaultConfig = CreateDefault(globalNamespace);
            var xmlConfig = CreateFromXML(globalNamespace, xml);

            return new MemoryConfiguration()
            {
                GlobalNamespace = globalNamespace,
                FromNamespace = xmlConfig.FromNamespace ?? defaultConfig.FromNamespace,
                ToNamespace = xmlConfig.ToNamespace ?? defaultConfig.ToNamespace,
                TypeTranslator = xmlConfig.TypeTranslator ?? defaultConfig.TypeTranslator,
                MemberTranslator = xmlConfig.MemberTranslator ?? defaultConfig.MemberTranslator,
                TypeVerifiers = xmlConfig.TypeVerifiers ?? defaultConfig.TypeVerifiers,
                TypeVerificationOrder = xmlConfig.TypeVerificationOrder ?? defaultConfig.TypeVerificationOrder,
                MemberVerifiers = xmlConfig.MemberVerifiers ?? defaultConfig.MemberVerifiers,
                MemberVerificationOrder = xmlConfig.MemberVerificationOrder ?? defaultConfig.MemberVerificationOrder
            };
        }
    }
}
