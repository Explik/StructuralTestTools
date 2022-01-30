using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Explik.StructuralTestTools.Helpers;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class StructureService : IStructureService
    {
        private readonly NamespaceDescription _fromNamespace;
        private readonly NamespaceDescription _toNamespace;
        private readonly ITypeTranslator _typeTranslator;
        private readonly IMemberTranslator _memberTranslator;
        private readonly ITypeVerifier[] _typeVerifiers; 
        private readonly TypeVerificationAspect[] _typeVerificationOrder;
        private readonly IMemberVerifier[] _memberVerifiers;
        private readonly MemberVerificationAspect[] _memberVerificationOrder;

        private readonly Dictionary<TypeDescription, TypeDescription> _translateTypeCache = new Dictionary<TypeDescription, TypeDescription>();
        private readonly Dictionary<MemberDescription, MemberDescription> _translateMemberCache = new Dictionary<MemberDescription, MemberDescription>();
        private readonly Dictionary<Tuple<TypeDescription, ITypeVerifier>, Exception> _verifyTypeCache = new Dictionary<Tuple<TypeDescription, ITypeVerifier>, Exception>();
        private readonly Dictionary<Tuple<MemberDescription, IMemberVerifier>, Exception> _verifyMemberCache = new Dictionary<Tuple<MemberDescription, IMemberVerifier>, Exception>();

        public StructureService(IConfiguration configuration)
        {
            // Unpacking configuration to avoid configuration changes affecting instance and invalidating the caches
            _fromNamespace = configuration.FromNamespace;
            _toNamespace = configuration.ToNamespace;
            _typeTranslator = configuration.TypeTranslator;
            _memberTranslator = configuration.MemberTranslator;
            _typeVerifiers = configuration.TypeVerifiers.Select(i => i).ToArray();
            _typeVerificationOrder = configuration.TypeVerificationOrder.Select(i => i).ToArray();
            _memberVerifiers = configuration.MemberVerifiers.Select(i => i).ToArray();
            _memberVerificationOrder = configuration.MemberVerificationOrder.Select(i => i).ToArray();
        }

        public VerifierService StructureVerifier { get; set; }

        public NamespaceDescription TranslateNamespace(NamespaceDescription @namespace)
        {
            return (@namespace == _fromNamespace) ? _toNamespace : @namespace;
        }

        public TypeDescription TranslateType(TypeDescription type)
        {
            if (type.Namespace != _fromNamespace.Name)
                return type;

            if (_translateTypeCache.ContainsKey(type))
                return _translateTypeCache[type];

            // Translating type
            TypeDescription translatedType;

            if (type.IsArray)
            {
                var elementType = TranslateType(type.GetElementType());
                return elementType.MakeArrayType();
            }

            if (type.Namespace == _fromNamespace.Name)
            {
                TypeTranslateArgs translateArgs = new TypeTranslateArgs()
                {
                    Verifier = StructureVerifier,
                    TargetNamespace = _toNamespace,
                    OriginalType = type
                };
                ITypeTranslator translator = type.GetCustomTranslator() ?? _typeTranslator;
                translatedType = translator.Translate(translateArgs);
            }
            else translatedType = type;

            if (type.IsGenericType)
            {
                // TODO add validation so that TypeArguments must match
                var typeArguments = type.GetGenericArguments().Select(TranslateType).ToArray();
                return translatedType.GetGenericTypeDefinition().MakeGenericType(typeArguments);
            }

            _translateTypeCache.Add(type, translatedType);
            return translatedType;
        }

        public MemberDescription TranslateMember(MemberDescription member)
        {
            if (member.DeclaringType.Namespace != _fromNamespace.Name)
                return member;

            if (_translateMemberCache.ContainsKey(member))
                return _translateMemberCache[member];
            
            // Translating member
            MemberTranslatorArgs translatorArgs = new MemberTranslatorArgs()
            {
                Verifier = StructureVerifier,
                TypeTranslatorService = this,
                TypeVerifierService = this,
                TargetType = TranslateType(member.DeclaringType),
                OriginalMember = member,
            };
            IMemberTranslator translator = member.GetCustomTranslator() ?? _memberTranslator;
            MemberDescription translatedMember = translator.Translate(translatorArgs);
            
            _translateMemberCache.Add(member, translatedMember);
            return translatedMember;
        }

        public void VerifyType(TypeDescription type)
        {
            VerifyType(type, _typeVerificationOrder);
        }

        public void VerifyType(TypeDescription type, TypeVerificationAspect[] verificationAspects)
        {
            foreach (TypeVerificationAspect aspect in _typeVerificationOrder.Where(verificationAspects.Contains))
            {
                ITypeVerifier defaultVerifier = _typeVerifiers.FirstOrDefault(ver => ver.Aspects.Contains(aspect));
                ITypeVerifier verifier = type.GetCustomVerifier(aspect) ?? defaultVerifier;

                if (verifier != null)
                    VerifyType(type, verifier);
            }
        }

        public void VerifyMember(MemberDescription member, MemberVerificationAspect[] verificationAspects)
        {
            foreach (MemberVerificationAspect aspect in _memberVerificationOrder.Where(verificationAspects.Contains))
            {
                IMemberVerifier defaultVerifier = _memberVerifiers.FirstOrDefault(ver => ver.Aspects.Contains(aspect));
                IMemberVerifier verifier = member.GetCustomVerifier(aspect) ?? defaultVerifier;

                if (verifier != null)
                    VerifyMember(member, verifier);
            }
        }

        private void VerifyType(TypeDescription type, ITypeVerifier verifier)
        {
            Tuple<TypeDescription, ITypeVerifier> cacheKey = Tuple.Create(type, verifier);

            if (_verifyTypeCache.ContainsKey(cacheKey))
            {
                Exception ex = _verifyTypeCache[cacheKey];

                if (ex != null)
                    throw ex;
            }
            else
            {
                TypeVerifierArgs verifierArgs = new TypeVerifierArgs()
                {
                    Verifier = StructureVerifier,
                    TypeTranslatorService = this,
                    OriginalType = type,
                    TranslatedType = TranslateType(type)
                };

                try
                {
                    verifier.Verify(verifierArgs);
                    _verifyTypeCache.Add(cacheKey, null);
                }
                catch (Exception ex)
                {
                    _verifyTypeCache.Add(cacheKey, ex);
                    throw ex;
                }
            }
        }

        private void VerifyMember(MemberDescription member, IMemberVerifier verifier)
        {
            Tuple<MemberDescription, IMemberVerifier> cacheKey = Tuple.Create(member, verifier);

            if (_verifyMemberCache.ContainsKey(cacheKey))
            {
                Exception ex = _verifyMemberCache[cacheKey];

                if (ex != null)
                    throw ex;
            }
            else
            {
                MemberVerifierArgs verifierArgs = new MemberVerifierArgs()
                {
                    Verifier = StructureVerifier,
                    TypeTranslatorService = this,
                    TypeVerifierService = this,
                    MemberTranslatorService = this,
                    OriginalMember = member,
                    TranslatedMember = TranslateMember(member)
                };

                try
                {
                    verifier.Verify(verifierArgs);
                    _verifyMemberCache.Add(cacheKey, null);
                }
                catch (Exception ex)
                {
                    _verifyMemberCache.Add(cacheKey, ex);
                    throw ex;
                }
            }
        }
    }
}
