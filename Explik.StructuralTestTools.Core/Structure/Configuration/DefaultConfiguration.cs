using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class DefaultConfiguration : IConfiguration
    {
        public NamespaceDescription GlobalNamespace { get; set; }

        public NamespaceDescription FromNamespace => GlobalNamespace.GetNamespace("FromNamespace");

        public NamespaceDescription ToNamespace => GlobalNamespace.GetNamespace("ToNamespace");

        public ITypeTranslator TypeTranslator => new SameNameTypeTranslator();

        public IMemberTranslator MemberTranslator => new SameNameMemberTranslator();

        public ITypeVerifier[] TypeVerifiers => new ITypeVerifier[]
        {
            new UnchangedTypeAccessLevelVerifier()
        };

        public TypeVerificationAspect[] TypeVerificationOrder => new[]
        {
            TypeVerificationAspect.IsInterface,
            TypeVerificationAspect.IsDelegate,
            TypeVerificationAspect.IsSubclassOf,
            TypeVerificationAspect.DelegateSignature,
            TypeVerificationAspect.IsStatic,
            TypeVerificationAspect.IsAbstract,
            TypeVerificationAspect.AccessLevel
        };

        public IMemberVerifier[] MemberVerifiers => new IMemberVerifier[]
        {
            new UnchangedFieldTypeVerifier(),
            new UnchangedMemberAccessLevelVerifier(),
            new UnchangedMemberIsStaticVerifier(),
            new UnchangedMemberTypeVerifier(),
            new UnchangedPropertyTypeVerifier()
        };

        public MemberVerificationAspect[] MemberVerificationOrder => new[]
        {
            MemberVerificationAspect.MemberType,
            MemberVerificationAspect.FieldType,
            MemberVerificationAspect.FieldAccessLevel,
            MemberVerificationAspect.FieldWriteability,
            MemberVerificationAspect.PropertyType,
            MemberVerificationAspect.PropertyIsStatic,
            MemberVerificationAspect.PropertyGetIsAbstract,
            MemberVerificationAspect.PropertyGetIsVirtual,
            MemberVerificationAspect.PropertyGetDeclaringType,
            MemberVerificationAspect.PropertyGetAccessLevel,
            MemberVerificationAspect.PropertySetIsAbstract,
            MemberVerificationAspect.PropertySetIsVirtual,
            MemberVerificationAspect.PropertySetDeclaringType,
            MemberVerificationAspect.PropertySetAccessLevel,
            MemberVerificationAspect.MethodReturnType,
            MemberVerificationAspect.MethodIsAbstract,
            MemberVerificationAspect.MethodIsVirtual,
            MemberVerificationAspect.MethodDeclaringType,
            MemberVerificationAspect.MethodAccessLevel
        };
    }
}