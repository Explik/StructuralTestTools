using Explik.StructuralTestTools.TypeSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;
using static Explik.StructuralTestTools.RuntimeTypeSystemHelper;

namespace Explik.StructuralTestTools
{
    [TestClass]
    public class FieldTypeVerifierTests
    {
        private readonly ITypeTranslatorService typeTranslatorService;

        public FieldTypeVerifierTests()
        {
            typeTranslatorService = Substitute.For<ITypeTranslatorService>();
            typeTranslatorService.TranslateType(ConstantFieldType).Returns(ConstantFieldType);
            typeTranslatorService.TranslateType(OriginalFieldType).Returns(TranslatedFieldType);
        }

        [TestMethod("Verify verifies member type equals field")]
        public void Verify_VerifiesMemberTypeEqualsField()
        {
            var expectedMemberTypes = new[] { MemberTypes.Field };
            var verifierService = Substitute.For<VerifierService>();
            var FieldVerifier = new FieldTypeVerifier(typeof(object));

            FieldVerifier.Verify(new MemberVerifierArgs()
            {
                Verifier = verifierService,
                TypeTranslatorService = typeTranslatorService,
                OriginalMember = OriginalField,
                TranslatedMember = TranslatedField
            });

            verifierService.Received().VerifyMemberType(
                TranslatedField,
                Arg.Is<MemberTypes[]>(args => args.SequenceEqual(expectedMemberTypes)));
        }

        [TestMethod("Verify verifies constant field type")]
        public void Verify_VerifiesNonTranslatedFieldType()
        {
            var verifierService = Substitute.For<VerifierService>();
            var FieldVerifier = new FieldTypeVerifier(ConstantFieldType);

            FieldVerifier.Verify(new MemberVerifierArgs()
            {
                Verifier = verifierService,
                TypeTranslatorService = typeTranslatorService,
                OriginalMember = OriginalFieldWithConstantType,
                TranslatedMember = TranslatedFieldWithConstantType
            });

            verifierService.Received().VerifyFieldType(
                TranslatedFieldWithConstantType, 
                ConstantFieldType);
        }

        [TestMethod("Verify verifies variable field type")]
        public void Verify_VerifiesTranslatedFieldType()
        {
            var verifierService = Substitute.For<VerifierService>();
            var FieldVerifier = new FieldTypeVerifier(OriginalFieldType);

            FieldVerifier.Verify(new MemberVerifierArgs()
            {
                Verifier = verifierService,
                TypeTranslatorService = typeTranslatorService,
                OriginalMember = OriginalFieldWithVariableType,
                TranslatedMember = TranslatedFieldWithVariableType
            });

            verifierService.Received().VerifyFieldType(
                TranslatedFieldWithVariableType,
                TranslatedFieldType);
        }
    }
}
