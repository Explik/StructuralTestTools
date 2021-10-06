using Explik.StructuralTestTools.TypeSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;
using static Explik.StructuralTestTools.RuntimeTypeSystemHelper;

namespace Explik.StructuralTestTools
{
    [TestClass]
    public class PropertyIsReadonlyVerifierTests
    {
        [TestMethod("Verify verifies member type equals property")]
        public void Verify_VerifiesMemberTypeEqualsProperty()
        {
            var expectedMemberTypes = new[] { MemberTypes.Property };
            var verifierService = Substitute.For<VerifierService>();
            var isReadonlyVerifier = new PropertyIsReadonlyVerifier(AccessLevels.Public);

            isReadonlyVerifier.Verify(new MemberVerifierArgs()
            {
                Verifier = verifierService,
                OriginalMember = OriginalProperty,
                TranslatedMember = TranslatedProperty
            });

            verifierService.Received().VerifyMemberType(
                TranslatedProperty,
                Arg.Is<MemberTypes[]>(args => args.SequenceEqual(expectedMemberTypes)));
        }

        [DataRow(AccessLevels.Private)]
        [DataRow(AccessLevels.Protected)]
        [DataRow(AccessLevels.Public)]
        [TestMethod("Verify verifies member accessibility")]
        public void Verify_VerifiesMemberAccessility(AccessLevels accessLevel)
        {
            var verifierService = Substitute.For<VerifierService>();
            var isReadonlyVerifier = new PropertyIsReadonlyVerifier(accessLevel);

            isReadonlyVerifier.Verify(new MemberVerifierArgs()
            {
                Verifier = verifierService,
                OriginalMember = OriginalProperty,
                TranslatedMember = TranslatedProperty
            });

            verifierService.Received().VerifyIsReadonly(
                TranslatedProperty,
                accessLevel);
        }
    }
}
