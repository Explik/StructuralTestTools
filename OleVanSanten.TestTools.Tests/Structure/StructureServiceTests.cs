using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
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
            };
            StructureService service = new StructureService(configuration);

            service.TranslateType(originalType);

            translator.ReceivedWithAnyArgs().Translate(new TypeTranslateArgs());
        }

        [TestMethod("TranslateType does not use TypeTranslator if type is not in FromNamespace")]
        public void TranslateTypeDoesNotUseTypeTranslatorIfTypeIsNotInFromNamespace()
        {
            var @namespace = new RuntimeNamespaceDescription("TestTools_Tests.Structure");
            var originalType = new RuntimeTypeDescription(typeof(object));

            ITypeTranslator translator = Substitute.For<ITypeTranslator>();

            IConfiguration configuration = new MemoryConfiguration()
            {
                FromNamespace = @namespace,
                ToNamespace = @namespace,
                TypeTranslator = translator,
            };
            StructureService service = new StructureService(configuration);

            service.TranslateType(originalType);

            translator.DidNotReceiveWithAnyArgs().Translate(new TypeTranslateArgs());
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
            };
            StructureService service = new StructureService(configuration);

            service.TranslateType(originalType);

            translator.DidNotReceiveWithAnyArgs().Translate(new TypeTranslateArgs());
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
                MemberTranslator = memberTranslator
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
                MemberTranslator = memberTranslator
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
                MemberTranslator = memberTranslator
            };
            StructureService service = new StructureService(configuration);

            service.TranslateMember(fieldToTranslate);

            memberTranslator.DidNotReceiveWithAnyArgs().Translate(new MemberTranslatorArgs());
        }
    }
}
