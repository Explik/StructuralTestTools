using OleVanSanten.TestTools.Structure;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public interface IConfiguration
    {
        NamespaceDescription GetFromNamespace(NamespaceDescription globalNamespace);
        string GetFromNamespaceName();
        IMemberTranslator GetMemberTranslator();
        IMemberVerifier[] GetMemberVerifiers();
        NamespaceDescription GetToNamespace(NamespaceDescription globalNamespace);
        string GetToNamespaceName();
        ITypeTranslator GetTypeTranslator();
        ITypeVerifier[] GetTypeVerifiers();
    }
}