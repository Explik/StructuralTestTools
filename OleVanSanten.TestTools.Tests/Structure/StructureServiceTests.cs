using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OleVanSanten.TestTools.Structure;
using OleVanSanten.TestTools.TypeSystem;

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
        [TestMethod("IsTranslatableType returns false if type is not from FromNamespace")]
        public void IsTranslatableType_ReturnsFalseIfTypeIsFromFromNamespace()
        {
            var @namespace = new RuntimeNamespaceDescription("Empty.Namespace");
            var type = new RuntimeTypeDescription(typeof(TestTypeWithoutCustomTranslator));

            StructureService service = new StructureService(@namespace, @namespace);

            Assert.IsFalse(service.IsTranslatableType(type));
        }

        [TestMethod("IsTranslatableType returns true if type is from FromNamespace")]
        public void IsTranslatableType_ReturnsTrueIfTypeIsFromFromNamespace()
        {
            var @namespace = new RuntimeNamespaceDescription("TestTools_Tests.Structure");
            var type = new RuntimeTypeDescription(typeof(TestTypeWithoutCustomTranslator));

            StructureService service = new StructureService(@namespace, @namespace);

            Assert.IsTrue(service.IsTranslatableType(type));
        }

        [TestMethod("TranslateType uses TypeTranslator if no custom translator is defined on type")]
        public void TranslateTypeUsesTypeTranslatorIfNoCustomTranslatorIsDefinedOnType()
        {
            var @namespace = new RuntimeNamespaceDescription("TestTools_Tests.Structure");
            var originalType = new RuntimeTypeDescription(typeof(TestTypeWithoutCustomTranslator));

            ITypeTranslator translator = Substitute.For<ITypeTranslator>();
            StructureService service = new StructureService(@namespace, @namespace)
            {
                TypeTranslator = translator
            };

            service.TranslateType(originalType);

            translator.Received().Translate(originalType);
        }

        [TestMethod("TranslateType does not use TypeTranslator if type is not in FromNamespace")]
        public void TranslateTypeDoesNotUseTypeTranslatorIfTypeIsNotInFromNamespace()
        {
            var @namespace = new RuntimeNamespaceDescription("TestTools_Tests.Structure");
            var originalType = new RuntimeTypeDescription(typeof(object));

            ITypeTranslator translator = Substitute.For<ITypeTranslator>();
            StructureService service = new StructureService(@namespace, @namespace)
            {
                TypeTranslator = translator
            };

            service.TranslateType(originalType);

            translator.DidNotReceive().Translate(originalType);
        }

        [TestMethod("TranslateType does not use TypeTranslator if custom translator is defined on type")]
        public void TranslateTypeDoesNotUseTypeTranslatorIfCustomTranslatorIsDefinedOnType()
        {
            var @namespace = new RuntimeNamespaceDescription("TestTools_Tests.Structure");
            var originalType = new RuntimeTypeDescription(typeof(TestTypeWithCustomTranslator));

            ITypeTranslator translator = Substitute.For<ITypeTranslator>();
            StructureService service = new StructureService(@namespace, @namespace)
            {
                TypeTranslator = translator
            };

            service.TranslateType(originalType);

            translator.DidNotReceive().Translate(originalType);
        }

        [TestMethod("VerifyType(Type, ITypeVerifier[]) uses all type verifiers")]
        public void VerifyTypeOverloadUsesAllTypeVerifiers()
        {
            var @namespace = new RuntimeNamespaceDescription("TestTools_Tests.Structure");
            var typeToVerify = new RuntimeTypeDescription(typeof(object));

            ITypeVerifier verifier1 = Substitute.For<ITypeVerifier>();
            verifier1.Aspects.Returns(new[] { TypeVerificationAspect.AccessLevel });
            ITypeVerifier verifier2 = Substitute.For<ITypeVerifier>();
            verifier2.Aspects.Returns(new[] { TypeVerificationAspect.IsSubclassOf });
            StructureService service = new StructureService(@namespace, @namespace);

            service.VerifyType(typeToVerify, new[] { verifier1, verifier2 });

            verifier1.Received().Verify(typeToVerify, typeToVerify);
            verifier2.Received().Verify(typeToVerify, typeToVerify);
        }

        [TestMethod("TranslateMember uses MemberTranslator if no custom translator is defined on member")]
        public void TranslateMemberUsesMemberTranslatorIfNoCustomTranslatorIsDefinedOnMember()
        {
            var @namespace = new RuntimeNamespaceDescription("TestTools_Tests.Structure");
            var typeToTranslate = new RuntimeTypeDescription(typeof(TestTypeWithoutCustomTranslator));
            var fieldToTranslate = new RuntimeFieldDescription(typeof(TestTypeWithoutCustomTranslator).GetField("FieldWithoutCustomTranslator"));

            ITypeTranslator typeTranslator = Substitute.For<ITypeTranslator>();
            typeTranslator.Translate(typeToTranslate).Returns(typeToTranslate);
            IMemberTranslator memberTranslator = Substitute.For<IMemberTranslator>();
            StructureService service = new StructureService(@namespace, @namespace)
            {
                TypeTranslator = typeTranslator,
                MemberTranslator = memberTranslator
            };

            service.TranslateMember(fieldToTranslate);

            memberTranslator.Received().Translate(fieldToTranslate);
        }

        [TestMethod("TranslateMember does not use MemberTranslator if member's DeclaringType is not within FromNamespace")]
        public void TranslateMemberDoesNotUseMemberTranslatorIfMemberDeclaringTypeIsNotWithinFromNamespace()
        {
            var @namespace = new RuntimeNamespaceDescription("TestTools_Tests.Structure");
            var propertyToVerify = new RuntimePropertyDescription(typeof(string).GetProperty("Length"));

            IMemberTranslator memberTranslator = Substitute.For<IMemberTranslator>();
            StructureService service = new StructureService(@namespace, @namespace)
            {
                MemberTranslator = memberTranslator
            };

            service.TranslateMember(propertyToVerify);

            memberTranslator.DidNotReceive().Translate(propertyToVerify);
        }

        [TestMethod("TranslateMember does not use MemberTranslator if custom translator is defined on member")]
        public void TranslateMemberDoesNotUseMemberTranslatorIfCustomerTranslatorIsDefinedOnMember()
        {
            var @namespace = new RuntimeNamespaceDescription("TestTools_Tests.Structure");
            var typeToTranslate = new RuntimeTypeDescription(typeof(TestTypeWithoutCustomTranslator));
            var fieldToTranslate = new RuntimeFieldDescription(typeof(TestTypeWithoutCustomTranslator).GetField("FieldWithCustomTranslator"));

            ITypeTranslator typeTranslator = Substitute.For<ITypeTranslator>();
            typeTranslator.Translate(typeToTranslate).Returns(typeToTranslate);
            IMemberTranslator memberTranslator = Substitute.For<IMemberTranslator>();
            StructureService service = new StructureService(@namespace, @namespace)
            {
                TypeTranslator = typeTranslator,
                MemberTranslator = memberTranslator
            };

            service.TranslateMember(fieldToTranslate);

            memberTranslator.DidNotReceive().Translate(fieldToTranslate);
        }

        [TestMethod("VerifyMember(MemberInfo, IMemberVerifier[]) uses all member verifiers")]
        public void VerifyMemberOverloadUsesAllMemberVerifiers()
        {
            var @namespace = new RuntimeNamespaceDescription("TestTools_Tests.Structure");
            var propertyToVerify = new RuntimePropertyDescription(typeof(string).GetProperty("Length"));

            // The verifiers must have aspects as StructureService depends on aspect to use the verifier
            IMemberVerifier verifier1 = Substitute.For<IMemberVerifier>();
            verifier1.Aspects.Returns(new[] { MemberVerificationAspect.PropertyType });
            IMemberVerifier verifier2 = Substitute.For<IMemberVerifier>();
            verifier2.Aspects.Returns(new[] { MemberVerificationAspect.PropertyIsStatic });
            StructureService service = new StructureService(@namespace, @namespace);

            service.VerifyMember(propertyToVerify, new[] { verifier1, verifier2 });

            verifier1.Received().Verify(propertyToVerify, propertyToVerify);
            verifier2.Received().Verify(propertyToVerify, propertyToVerify);
        }
    }
}
