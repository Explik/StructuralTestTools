using Explik.StructuralTestTools.TypeSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;
using static Explik.StructuralTestTools.RuntimeTypeSystemHelper;

namespace Explik.StructuralTestTools
{
    [TestClass]
    public class MethodReturnTypeVerifierTests
    {
        private readonly ITypeTranslatorService typeTranslatorService;

        public MethodReturnTypeVerifierTests()
        {
            typeTranslatorService = Substitute.For<ITypeTranslatorService>();
            typeTranslatorService.TranslateType(ConstantMethodReturnType).Returns(ConstantMethodReturnType);
            typeTranslatorService.TranslateType(OriginalMethodReturnType).Returns(TranslatedMethodReturnType);
        }

        [TestMethod("Verify verifies member type equals property")]
        public void Verify_VerifiesMemberTypeEqualsMethod()
        {
            var expectedMemberTypes = new[] { MemberTypes.Method };
            var verifierService = Substitute.For<VerifierService>();
            var propertyVerifier = new MethodReturnTypeVerifier(typeof(object));

            propertyVerifier.Verify(new MemberVerifierArgs()
            {
                Verifier = verifierService,
                TypeTranslatorService = typeTranslatorService,
                OriginalMember = OriginalMethod,
                TranslatedMember = TranslatedMethod
            });

            verifierService.Received().VerifyMemberType(
                TranslatedMethod,
                Arg.Is<MemberTypes[]>(args => args.SequenceEqual(expectedMemberTypes)));
        }

        [TestMethod("Verify verifies constant property type")]
        public void Verify_VerifiesNonTranslatedMethodReturnType()
        {
            var verifierService = Substitute.For<VerifierService>();
            var propertyVerifier = new MethodReturnTypeVerifier(ConstantMethodReturnType);

            propertyVerifier.Verify(new MemberVerifierArgs()
            {
                Verifier = verifierService,
                TypeTranslatorService = typeTranslatorService,
                OriginalMember = OriginalMethodWithConstantReturnType,
                TranslatedMember = TranslatedMethodWithConstantReturnType
            });

            verifierService.Received().VerifyReturnType(
                TranslatedMethodWithConstantReturnType, 
                ConstantMethodReturnType);
        }

        [TestMethod("Verify verifies variable property type")]
        public void Verify_VerifiesTranslatedMethodReturnType()
        {
            var verifierService = Substitute.For<VerifierService>();
            var propertyVerifier = new MethodReturnTypeVerifier(OriginalMethodReturnType);

            propertyVerifier.Verify(new MemberVerifierArgs()
            {
                Verifier = verifierService,
                TypeTranslatorService = typeTranslatorService,
                OriginalMember = OriginalMethodWithVariableReturnType,
                TranslatedMember = TranslatedMethodWithVariableReturnType
            });

            verifierService.Received().VerifyReturnType(
                TranslatedMethodWithVariableReturnType,
                TranslatedMethodReturnType);
        }
    }
}
