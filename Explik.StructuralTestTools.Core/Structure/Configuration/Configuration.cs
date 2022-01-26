using Explik.StructuralTestTools.TypeSystem;
using System;
using System.Collections.Generic;
using System.IO;
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
                TemplateFiles = new FileInfo[0],
                TypeTranslator = null,
                MemberTranslator = null,
                TypeVerifiers = new ITypeVerifier[0],
                TypeVerificationOrder = new TypeVerificationAspect[0],
                MemberVerifiers = new IMemberVerifier[0],
                MemberVerificationOrder = new MemberVerificationAspect[0]
            };
        }

        public static IConfiguration CreateDefault(NamespaceDescription globalNamespace, FileInfo projectFile)
        {
            return new DefaultConfiguration() { ProjectFile = projectFile, GlobalNamespace = globalNamespace };
        }

        public static IConfiguration CreateFromXML(NamespaceDescription globalNamespace, FileInfo projectFile, string xml)
        {
            return new XMLConfiguration(xml) { ProjectFile = projectFile, GlobalNamespace = globalNamespace };
        }

        public static IConfiguration CreateFromXMLWithDefaults(NamespaceDescription globalNamespace, FileInfo projectFile, string xml)
        {
            var defaultConfig = CreateDefault(globalNamespace, projectFile);
            var xmlConfig = CreateFromXML(globalNamespace, projectFile, xml);

            return new MemoryConfiguration()
            {
                ProjectFile = projectFile,
                GlobalNamespace = globalNamespace,
                FromNamespace = xmlConfig.FromNamespace ?? defaultConfig.FromNamespace,
                ToNamespace = xmlConfig.ToNamespace ?? defaultConfig.ToNamespace,
                TemplateFiles = xmlConfig.TemplateFiles ?? defaultConfig.TemplateFiles,
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
