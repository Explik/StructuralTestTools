using Explik.StructuralTestTools;
using Explik.StructuralTestTools.TypeSystem;
using System.IO;

namespace Explik.StructuralTestTools
{
    public interface IConfiguration
    {
        NamespaceDescription GlobalNamespace { get; set; }
        NamespaceDescription FromNamespace { get; }
        NamespaceDescription ToNamespace { get; }

        FileInfo ProjectFile { get; }
        FileInfo[] TemplateFiles { get; }

        ITypeTranslator TypeTranslator { get; }
        IMemberTranslator MemberTranslator { get; }

        ITypeVerifier[] TypeVerifiers { get; }
        TypeVerificationAspect[] TypeVerificationOrder { get; }
        IMemberVerifier[] MemberVerifiers { get; }
        MemberVerificationAspect[] MemberVerificationOrder { get; }
    }
}