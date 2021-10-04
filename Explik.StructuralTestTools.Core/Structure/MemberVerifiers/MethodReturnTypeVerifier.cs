using Explik.StructuralTestTools.TypeSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Explik.StructuralTestTools
{
    public class MethodReturnTypeVerifier : IMemberVerifier
    {
        private readonly TypeDescription _returnType;

        public MethodReturnTypeVerifier(Type returnType) : this(new RuntimeTypeDescription(returnType))
        {
        }

        public MethodReturnTypeVerifier(TypeDescription returnType)
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
            TypeDescription translatedReturnType = args.TypeTranslatorService.TranslateType(_returnType);

            args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Method });

            if (args.TranslatedMember is MethodDescription methodDescription)
            {
                args.Verifier.VerifyReturnType(methodDescription, translatedReturnType);
            }
            else throw new NotImplementedException();
        }
    }
}
