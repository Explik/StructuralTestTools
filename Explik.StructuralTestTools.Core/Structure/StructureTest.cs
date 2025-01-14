﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Explik.StructuralTestTools.Helpers;
using Explik.StructuralTestTools;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class StructureTest
    {
        Action _executeAction;
        IStructureService _structureService;
        VerifierService _verifierService;

        public StructureTest(IStructureService structureService, VerifierService verifierService) 
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
            // Rethrowing the exception to erase the stack trace as it probably confuses more than it helps.
            try
            {
                _executeAction?.Invoke();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
