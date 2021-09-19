using Explik.StructuralTestTools.TypeSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Explik.StructuralTestTools
{
    public class MethodReturnTypeVerifier : IMemberVerifier
    {
        private readonly Type _returnType;

        public MethodReturnTypeVerifier(Type returnType)
        {
            _returnType = returnType;
        }

        public MemberVerificationAspect[] Aspects => new[]
        {
            MemberVerificationAspect.MemberType,
            MemberVerificationAspect.MethodReturnType
        };

        public void Verify(MemberVerifierArgs args)
        {
            args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Method });

            if (args.TranslatedMember is MethodDescription methodDescription)
            {
                args.Verifier.VerifyReturnType(methodDescription, new RuntimeTypeDescription(_returnType));
            }
            else throw new NotImplementedException();
        }
    }
}
