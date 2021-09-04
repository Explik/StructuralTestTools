using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools
{
    public class MemberIsAbstractVerifier : IMemberVerifier
    {
        bool _isAbstract;

        public MemberIsAbstractVerifier(bool isAbstract = true)
        {
            _isAbstract = isAbstract;
        }

        public MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.PropertyGetIsAbstract,
            MemberVerificationAspect.PropertySetIsAbstract,
            MemberVerificationAspect.MethodIsAbstract
        };

        public void Verify(MemberVerifierArgs args)
        {
            args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Property, MemberTypes.Method });

            if (args.TranslatedMember is PropertyDescription translatedProperty)
            {
                args.Verifier.VerifyIsAbstract(translatedProperty, _isAbstract);
            }
            else if (args.TranslatedMember is MethodDescription translatedMethod)
            {
                args.Verifier.VerifyIsAbstract(translatedMethod, _isAbstract);
            }
            else throw new NotImplementedException();
        }
    }
}
