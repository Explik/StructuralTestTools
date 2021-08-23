using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using OleVanSanten.TestTools.Helpers;
using OleVanSanten.TestTools.Structure;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class StructureTest
    {
        Action _executeAction;
        IStructureService _structureService;
        VerifierServiceBase _verifierService;

        public StructureTest(IStructureService structureService, VerifierServiceBase verifierService) 
        {
            _structureService = structureService;
            _verifierService = verifierService;
        }

        public void AssertType(Type type, params ITypeVerifier[] verifiers)
        {
            _executeAction += () =>
            {
                RuntimeTypeDescription typeDescription = new RuntimeTypeDescription(type);
                TypeVerifierArgs args = new TypeVerifierArgs()
                {
                    Verifier = _verifierService,
                    TypeTranslatorService = _structureService,
                    OriginalType = typeDescription,
                    TranslatedType = _structureService.TranslateType(typeDescription)
                };

                foreach (ITypeVerifier implicitVerifier in typeDescription.GetCustomVerifiers())
                    implicitVerifier.Verify(args);

                foreach (ITypeVerifier explicitVerifier in verifiers)
                    explicitVerifier.Verify(args);
            };
        }

        public void AssertMember(MemberInfo memberInfo, params IMemberVerifier[] verifiers)
        {
            
            _executeAction += () =>
            {
                RuntimeDescriptionFactory factory = new RuntimeDescriptionFactory();
                MemberDescription memberDescription = factory.Create(memberInfo);
                TypeVerifierArgs typeVerifierArgs = new TypeVerifierArgs()
                {
                    Verifier = _verifierService,
                    TypeTranslatorService = _structureService,
                    OriginalType = memberDescription.DeclaringType,
                    TranslatedType = _structureService.TranslateType(memberDescription.DeclaringType)
                };
                MemberVerifierArgs memberVerifierArgs = new MemberVerifierArgs()
                {
                    Verifier = _verifierService,
                    TypeVerifierService = _structureService,
                    TypeTranslatorService = _structureService,
                    MemberTranslatorService = _structureService,
                    OriginalMember = memberDescription,
                    TranslatedMember = _structureService.TranslateMember(memberDescription)
                };

                foreach (ITypeVerifier implicitTypeVerifier in memberDescription.DeclaringType.GetCustomVerifiers())
                    implicitTypeVerifier.Verify(typeVerifierArgs);

                foreach (IMemberVerifier implicitMemberVerifier in memberDescription.GetCustomVerifiers())
                    implicitMemberVerifier.Verify(memberVerifierArgs);

                foreach (IMemberVerifier explicitMemberVerifier in verifiers)
                    explicitMemberVerifier.Verify(memberVerifierArgs);
            };
        }

        public void Execute()
        {
            _executeAction?.Invoke();
        }
    }
}
