using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using OleVanSanten.TestTools.Helpers;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class StructureService : IStructureService
    {
        NamespaceDescription FromNamespace { get; set; }

        NamespaceDescription ToNamespace { get; set; }

        public VerifierServiceBase StructureVerifier { get; set; }

        public ITypeTranslator TypeTranslator { get; set; } = new SameNameTypeTranslator();

        public IMemberTranslator MemberTranslator { get; set; } = new SameNameMemberTranslator();

        public ICollection<TypeVerificationAspect> TypeVerificationOrder { get; set; } = new[]
        {
            TypeVerificationAspect.IsInterface,
            TypeVerificationAspect.IsDelegate,
            TypeVerificationAspect.IsSubclassOf,
            TypeVerificationAspect.DelegateSignature,
            TypeVerificationAspect.IsStatic,
            TypeVerificationAspect.IsAbstract,
            TypeVerificationAspect.AccessLevel
        };

        public ICollection<MemberVerificationAspect> MemberVerificationOrder { get; set; } = new[]
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

        public StructureService(NamespaceDescription fromNamespace, NamespaceDescription toNamespace)
        {
            if (fromNamespace == null)
                throw new ArgumentNullException("fromNamespace cannot be null");
            if (toNamespace == null)
                throw new ArgumentNullException("toNamespace cannot be null");

            FromNamespace = fromNamespace;
            ToNamespace = toNamespace;
        }

        public bool IsTranslatableType(TypeDescription type)
        {
            return type.Namespace == FromNamespace.Name;
        }

        public TypeDescription TranslateType(TypeDescription type)
        {
            TypeDescription translatedType;

            if (type.IsArray)
            {
                var elementType = TranslateType(type.GetElementType());
                return elementType.MakeArrayType();
            }

            if (type.Namespace == FromNamespace.Name)
            {
                TypeTranslateArgs translateArgs = new TypeTranslateArgs()
                {
                    Verifier = StructureVerifier,
                    TargetNamespace = ToNamespace,
                    OriginalType = type
                };
                ITypeTranslator translator = type.GetCustomTranslator() ?? TypeTranslator;
                translatedType = translator.Translate(translateArgs);
            }
            else translatedType = type;

            if (type.IsGenericType)
            {
                // TODO add validation so that TypeArguments must match
                var typeArguments = type.GetGenericArguments().Select(TranslateType).ToArray();
                return translatedType.GetGenericTypeDefinition().MakeGenericType(typeArguments);
            }
            return translatedType;
        }

        public MemberDescription TranslateMember(MemberDescription memberInfo)
        {
            if (memberInfo.DeclaringType.Namespace != FromNamespace.Name)
                return memberInfo;

            MemberTranslatorArgs translatorArgs = new MemberTranslatorArgs()
            {
                Verifier = StructureVerifier,
                TypeTranslatorService = this,
                TypeVerifierService = this,
                TargetType = TranslateType(memberInfo.DeclaringType),
                OriginalMember = memberInfo,
            };
            IMemberTranslator translator = memberInfo.GetCustomTranslator() ?? MemberTranslator;

            return translator.Translate(translatorArgs);
        }

        public MemberDescription TranslateMember(TypeDescription targetType, MemberDescription memberInfo)
        {
            if (memberInfo.DeclaringType.Namespace != FromNamespace.Name)
                return memberInfo;

            MemberTranslatorArgs translatorArgs = new MemberTranslatorArgs()
            {
                Verifier = StructureVerifier,
                TypeTranslatorService = this,
                TypeVerifierService = this,
                TargetType = targetType,
                OriginalMember = memberInfo,
            };
            IMemberTranslator translator = memberInfo.GetCustomTranslator() ?? MemberTranslator;

            return translator.Translate(translatorArgs);
        }

        public void VerifyType(TypeDescription original, ITypeVerifier[] verifiers)
        {
            TypeVerifierArgs verifierArgs = new TypeVerifierArgs()
            {
                Verifier = StructureVerifier,
                TypeTranslatorService = this,
                OriginalType = original,
                TranslatedType = TranslateType(original)
            };

            foreach (TypeVerificationAspect aspect in TypeVerificationOrder)
            {
                ITypeVerifier defaultVerifier = verifiers.FirstOrDefault(ver => ver.Aspects.Contains(aspect));
                ITypeVerifier verifier = original.GetCustomVerifier(aspect) ?? defaultVerifier;
                verifier?.Verify(verifierArgs);
            }
        }

        public void VerifyMember(MemberDescription original, IMemberVerifier[] verifiers)
        {
            TypeDescription translatedType = TranslateType(original.DeclaringType);

            MemberVerifierArgs verifierArgs = new MemberVerifierArgs()
            {
                Verifier = StructureVerifier,
                TypeTranslatorService = this,
                TypeVerifierService = this,
                MemberTranslatorService = this,
                OriginalMember = original,
                TranslatedMember = TranslateMember(translatedType, original)
            };

            foreach (MemberVerificationAspect aspect in MemberVerificationOrder)
            {
                IMemberVerifier defaultVerifier = verifiers.FirstOrDefault(ver => ver.Aspects.Contains(aspect));
                IMemberVerifier verifier = original.GetCustomVerifier(aspect) ?? defaultVerifier;

                verifier?.Verify(verifierArgs);
            }
        }
    }
}
