using Explik.StructuralTestTools.TypeSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;
using static Explik.StructuralTestTools.RuntimeTestTypes;

namespace Explik.StructuralTestTools
{
    [TestClass]
    public class PropertyTypeVerifierTests
    {
        private readonly ITypeTranslatorService typeTranslatorService;

        public PropertyTypeVerifierTests()
        {
            typeTranslatorService = Substitute.For<ITypeTranslatorService>();
            typeTranslatorService.TranslateType(ConstantPropertyType).Returns(ConstantPropertyType);
            typeTranslatorService.TranslateType(OriginalPropertyType).Returns(TranslatedPropertyType);
        }

        [TestMethod("Verify verifies member type equals property")]
        public void Verify_VerifiesMemberTypeEqualsProperty()
        {
            var expectedMemberTypes = new[] { MemberTypes.Property };
            var verifierService = Substitute.For<VerifierService>();
            var propertyVerifier = new PropertyTypeVerifier(typeof(object));

            propertyVerifier.Verify(new MemberVerifierArgs()
            {
                Verifier = verifierService,
                TypeTranslatorService = typeTranslatorService,
                OriginalMember = OriginalProperty,
                TranslatedMember = TranslatedProperty
            });

            verifierService.Received().VerifyMemberType(
                TranslatedProperty,
                Arg.Is<MemberTypes[]>(args => args.SequenceEqual(expectedMemberTypes)));
        }

        [TestMethod("Verify verifies constant property type")]
        public void Verify_VerifiesNonTranslatedPropertyType()
        {
            var verifierService = Substitute.For<VerifierService>();
            var propertyVerifier = new PropertyTypeVerifier(ConstantPropertyType);

            propertyVerifier.Verify(new MemberVerifierArgs()
            {
                Verifier = verifierService,
                TypeTranslatorService = typeTranslatorService,
                OriginalMember = OriginalPropertyWithConstantType,
                TranslatedMember = TranslatedPropertyWithConstantType
            });

            verifierService.Received().VerifyPropertyType(
                TranslatedPropertyWithConstantType, 
                ConstantPropertyType);
        }

        [TestMethod("Verify verifies variable property type")]
        public void Verify_VerifiesTranslatedPropertyType()
        {
            var verifierService = Substitute.For<VerifierService>();
            var propertyVerifier = new PropertyTypeVerifier(OriginalPropertyType);

            propertyVerifier.Verify(new MemberVerifierArgs()
            {
                Verifier = verifierService,
                TypeTranslatorService = typeTranslatorService,
                OriginalMember = OriginalPropertyWithVariableType,
                TranslatedMember = TranslatedPropertyWithVariableType
            });

            verifierService.Received().VerifyPropertyType(
                TranslatedPropertyWithVariableType,
                TranslatedPropertyType);
        }
    }
}
