using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using Explik.StructuralTestTools;
using Explik.StructuralTestTools.TypeSystem;

namespace TestTools_Tests.Structure
{
    public class TestTypeWithoutCustomTranslator
    {
        public int FieldWithoutCustomTranslator;

        [AlternateNames("AlternateField")]
        public int FieldWithCustomTranslator;
    }

    [AlternateNames("AlternateTestType")]
    public class TestTypeWithCustomTranslator
    {
    }

    [TestClass]
    public class StructureServiceTests
    {
        [TestMethod("TranslateType returns input if type is not in FromNamespace")]
        public void TranslateTypeDoesNotUseTypeTranslatorIfTypeIsNotInFromNamespace()
        {
            var @namespace = new RuntimeNamespaceDescription("TestTools_Tests.Structure");
            var originalType = new RuntimeTypeDescription(typeof(object));

            IConfiguration configuration = new MemoryConfiguration()
            {
                FromNamespace = @namespace,
                ToNamespace = @namespace,
                TypeTranslator = Substitute.For<ITypeTranslator>(),
                MemberTranslator = Substitute.For<IMemberTranslator>(),
                TypeVerifiers = new ITypeVerifier[0],
                TypeVerificationOrder = new TypeVerificationAspect[0],
                MemberVerifiers = new IMemberVerifier[0],
                MemberVerificationOrder = new MemberVerificationAspect[0]
            };
            StructureService service = new StructureService(configuration);

            var translatedType = service.TranslateType(originalType);

            Assert.AreSame(originalType, translatedType);
        }

        [TestMethod("TranslateType uses TypeTranslator if no custom translator is defined on type")]
        public void TranslateTypeUsesTypeTranslatorIfNoCustomTranslatorIsDefinedOnType()
        {
            var @namespace = new RuntimeNamespaceDescription("TestTools_Tests.Structure");
            var originalType = new RuntimeTypeDescription(typeof(TestTypeWithoutCustomTranslator));

            ITypeTranslator translator = Substitute.For<ITypeTranslator>();

            IConfiguration configuration = new MemoryConfiguration()
            {
                FromNamespace = @namespace,
                ToNamespace = @namespace,
                TypeTranslator = translator,
                MemberTranslator = Substitute.For<IMemberTranslator>(),
                TypeVerifiers = new ITypeVerifier[0],
                TypeVerificationOrder = new TypeVerificationAspect[0],
                MemberVerifiers = new IMemberVerifier[0],
                MemberVerificationOrder = new MemberVerificationAspect[0]
            };
            StructureService service = new StructureService(configuration);

            service.TranslateType(originalType);

            translator.ReceivedWithAnyArgs().Translate(new TypeTranslateArgs());
        }

        [TestMethod("TranslateType does not use TypeTranslator if custom translator is defined on type")]
        public void TranslateTypeDoesNotUseTypeTranslatorIfCustomTranslatorIsDefinedOnType()
        {
            var @namespace = new RuntimeNamespaceDescription("TestTools_Tests.Structure");
            var originalType = new RuntimeTypeDescription(typeof(TestTypeWithCustomTranslator));

            ITypeTranslator translator = Substitute.For<ITypeTranslator>();

            IConfiguration configuration = new MemoryConfiguration()
            {
                FromNamespace = @namespace,
                ToNamespace = @namespace,
                TypeTranslator = translator,
                MemberTranslator = Substitute.For<IMemberTranslator>(),
                TypeVerifiers = new ITypeVerifier[0],
                TypeVerificationOrder = new TypeVerificationAspect[0],
                MemberVerifiers = new IMemberVerifier[0],
                MemberVerificationOrder = new MemberVerificationAspect[0]
            };
            StructureService service = new StructureService(configuration);

            service.TranslateType(originalType);

            translator.DidNotReceiveWithAnyArgs().Translate(new TypeTranslateArgs());
        }

        [TestMethod("TranslateType caches result if no custom translator is defined on type")]
        public void TranslateType_CachesTypeTranslatorResult()
        {
            var @namespace = new RuntimeNamespaceDescription("TestTools_Tests.Structure");
            var originalType = new RuntimeTypeDescription(typeof(TestTypeWithoutCustomTranslator));

            ITypeTranslator translator = Substitute.For<ITypeTranslator>();

            IConfiguration configuration = new MemoryConfiguration()
            {
                FromNamespace = @namespace,
                ToNamespace = @namespace,
                TypeTranslator = translator,
                MemberTranslator = Substitute.For<IMemberTranslator>(),
                TypeVerifiers = new ITypeVerifier[0],
                TypeVerificationOrder = new TypeVerificationAspect[0],
                MemberVerifiers = new IMemberVerifier[0],
                MemberVerificationOrder = new MemberVerificationAspect[0]
            };
            StructureService service = new StructureService(configuration);

            var translatedType1 = service.TranslateType(originalType);
            var translatedType2 = service.TranslateType(originalType);

            Assert.AreSame(translatedType1, translatedType2);
        }

        [TestMethod("TranslateType caches result if custom translator is defined on type")]
        public void TranslateType_CachesResult_IfCustomTranslatorIsDefinedOnType()
        {
            var @namespace = new RuntimeNamespaceDescription("TestTools_Tests.Structure");
            var originalType = new RuntimeTypeDescription(typeof(TestTypeWithCustomTranslator));

            IConfiguration configuration = new MemoryConfiguration()
            {
                FromNamespace = @namespace,
                ToNamespace = @namespace,
                TypeTranslator = Substitute.For<ITypeTranslator>(),
                MemberTranslator = Substitute.For<IMemberTranslator>(),
                TypeVerifiers = new ITypeVerifier[0],
                TypeVerificationOrder = new TypeVerificationAspect[0],
                MemberVerifiers = new IMemberVerifier[0],
                MemberVerificationOrder = new MemberVerificationAspect[0]
            };
            StructureService service = new StructureService(configuration);

            service.TranslateType(originalType);

            var translatedType1 = service.TranslateType(originalType);
            var translatedType2 = service.TranslateType(originalType);

            Assert.AreSame(translatedType1, translatedType2);
        }

        [TestMethod("TranslateMember returns input if the member's declaring type is not in FromNamespace")]
        public void TranslateMember_ReturnsInput_IfMembersDeclaringTypeIsNotInFromNamespace()
        {
            var @namespace = new RuntimeNamespaceDescription("TestTools_Tests.Structure");
            var originalMember = new RuntimeMethodDescription(typeof(object).GetMethod("GetHashCode"));

            var configuration = new MemoryConfiguration()
            {
                FromNamespace = @namespace,
                ToNamespace = @namespace,
                TypeTranslator = Substitute.For<ITypeTranslator>(),
                MemberTranslator = Substitute.For<IMemberTranslator>(),
                TypeVerifiers = new ITypeVerifier[0],
                TypeVerificationOrder = new TypeVerificationAspect[0],
                MemberVerifiers = new IMemberVerifier[0],
                MemberVerificationOrder = new MemberVerificationAspect[0]
            };
            var service = new StructureService(configuration);

            var translatedMember = service.TranslateMember(originalMember);

            Assert.AreSame(originalMember, translatedMember);
        }

        [TestMethod("TranslateMember uses MemberTranslator if no custom translator is defined on member")]
        public void TranslateMemberUsesMemberTranslatorIfNoCustomTranslatorIsDefinedOnMember()
        {
            var @namespace = new RuntimeNamespaceDescription("TestTools_Tests.Structure");
            var typeToTranslate = new RuntimeTypeDescription(typeof(TestTypeWithoutCustomTranslator));
            var fieldToTranslate = new RuntimeFieldDescription(typeof(TestTypeWithoutCustomTranslator).GetField("FieldWithoutCustomTranslator"));

            ITypeTranslator typeTranslator = Substitute.For<ITypeTranslator>();
            typeTranslator.Translate(new TypeTranslateArgs()).ReturnsForAnyArgs(typeToTranslate);
            IMemberTranslator memberTranslator = Substitute.For<IMemberTranslator>();

            IConfiguration configuration = new MemoryConfiguration()
            {
                FromNamespace = @namespace,
                ToNamespace = @namespace,
                TypeTranslator = typeTranslator,
                MemberTranslator = memberTranslator,
                TypeVerifiers = new ITypeVerifier[0],
                TypeVerificationOrder = new TypeVerificationAspect[0],
                MemberVerifiers = new IMemberVerifier[0],
                MemberVerificationOrder = new MemberVerificationAspect[0]
            };
            StructureService service = new StructureService(configuration);

            service.TranslateMember(fieldToTranslate);

            memberTranslator.ReceivedWithAnyArgs().Translate(new MemberTranslatorArgs());
        }

        [TestMethod("TranslateMember does not use MemberTranslator if member's DeclaringType is not within FromNamespace")]
        public void TranslateMemberDoesNotUseMemberTranslatorIfMemberDeclaringTypeIsNotWithinFromNamespace()
        {
            var @namespace = new RuntimeNamespaceDescription("TestTools_Tests.Structure");
            var propertyToVerify = new RuntimePropertyDescription(typeof(string).GetProperty("Length"));

            IMemberTranslator memberTranslator = Substitute.For<IMemberTranslator>();
            IConfiguration configuration = new MemoryConfiguration()
            {
                FromNamespace = @namespace,
                ToNamespace = @namespace,
                TypeTranslator = Substitute.For<ITypeTranslator>(),
                MemberTranslator = memberTranslator,
                TypeVerifiers = new ITypeVerifier[0],
                TypeVerificationOrder = new TypeVerificationAspect[0],
                MemberVerifiers = new IMemberVerifier[0],
                MemberVerificationOrder = new MemberVerificationAspect[0]
            };
            StructureService service = new StructureService(configuration);

            service.TranslateMember(propertyToVerify);

            memberTranslator.DidNotReceiveWithAnyArgs().Translate(new MemberTranslatorArgs());
        }

        [TestMethod("TranslateMember does not use MemberTranslator if custom translator is defined on member")]
        public void TranslateMemberDoesNotUseMemberTranslatorIfCustomerTranslatorIsDefinedOnMember()
        {
            var @namespace = new RuntimeNamespaceDescription("TestTools_Tests.Structure");
            var typeToTranslate = new RuntimeTypeDescription(typeof(TestTypeWithoutCustomTranslator));
            var fieldToTranslate = new RuntimeFieldDescription(typeof(TestTypeWithoutCustomTranslator).GetField("FieldWithCustomTranslator"));

            ITypeTranslator typeTranslator = Substitute.For<ITypeTranslator>();
            typeTranslator.Translate(new TypeTranslateArgs()).ReturnsForAnyArgs(typeToTranslate);
            IMemberTranslator memberTranslator = Substitute.For<IMemberTranslator>();

            IConfiguration configuration = new MemoryConfiguration()
            {
                FromNamespace = @namespace,
                ToNamespace = @namespace,
                TypeTranslator = typeTranslator,
                MemberTranslator = memberTranslator,
                TypeVerifiers = new ITypeVerifier[0],
                TypeVerificationOrder = new TypeVerificationAspect[0],
                MemberVerifiers = new IMemberVerifier[0],
                MemberVerificationOrder = new MemberVerificationAspect[0]
            };
            StructureService service = new StructureService(configuration);

            service.TranslateMember(fieldToTranslate);

            memberTranslator.DidNotReceiveWithAnyArgs().Translate(new MemberTranslatorArgs());
        }
    }
}
