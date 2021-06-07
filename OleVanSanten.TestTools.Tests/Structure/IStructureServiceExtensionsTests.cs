using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using OleVanSanten.TestTools.Structure;
using NSubstitute;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OleVanSanten.TestTools.TypeSystem;

namespace TestTools_Tests.Structure
{
    [TestClass]
    public class IStructureServiceExtensionsTests
    {
        // This method fails as it tries to compare a array, which will always fails
        // [TestMethod("VerifyType(Type, ITypeVerifier[], TypeVerificationAspect[]) uses subset of verifiers")]
        public void VerifyTypeOverloadUsesSubsetOfAllTypeVerifiers()
        {
            var typeToVerify = new RuntimeTypeDescription(typeof(object));

            ITypeVerifier verifier1 = Substitute.For<ITypeVerifier>();
            verifier1.Aspects.Returns(new[] { TypeVerificationAspect.AccessLevel });
            ITypeVerifier verifier2 = Substitute.For<ITypeVerifier>();
            verifier2.Aspects.Returns(new[] { TypeVerificationAspect.IsSubclassOf });
            IStructureService service = Substitute.For<IStructureService>();

            service.VerifyType(typeToVerify, new[] { verifier1, verifier2 }, TypeVerificationAspect.AccessLevel);

            service.Received().VerifyType(typeToVerify, new[] { verifier1 });
        }

        // This method fails as it tries to compare a array, which will always fails
        // [TestMethod("VerifyMember(MemberInfo, IMemberVerifier[], MemberVerificationAspect[]) uses subset of verifiers")]
        public void VerifyMemberOverloadUsesSubsetOfAllTypeVerifiers()
        {
            var propertyToVerify = new RuntimePropertyDescription(typeof(string).GetProperty("Length"));

            IMemberVerifier verifier1 = Substitute.For<IMemberVerifier>();
            verifier1.Aspects.Returns(new[] { MemberVerificationAspect.PropertyType });
            IMemberVerifier verifier2 = Substitute.For<IMemberVerifier>();
            verifier2.Aspects.Returns(new[] { MemberVerificationAspect.PropertyIsStatic });
            IStructureService service = Substitute.For<IStructureService>();

            service.VerifyMember(propertyToVerify, new[] { verifier1, verifier2 }, MemberVerificationAspect.PropertyType);

            service.Received().VerifyMember(propertyToVerify, new[] { verifier1 });
        }
    }
}
