using OleVanSanten.TestTools;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools
{
    public interface IConfiguration
    {
        NamespaceDescription GlobalNamespace { get; set; }
        NamespaceDescription FromNamespace { get; }
        NamespaceDescription ToNamespace { get; }

        ITypeTranslator TypeTranslator { get; }
        IMemberTranslator MemberTranslator { get; }

        ITypeVerifier[] TypeVerifiers { get; }
        TypeVerificationAspect[] TypeVerificationOrder { get; }
        IMemberVerifier[] MemberVerifiers { get; }
        MemberVerificationAspect[] MemberVerificationOrder { get; }
    }
}