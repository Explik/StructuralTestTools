using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class MemberIsAbstractVerifier : MemberVerifier
    {
        bool _isAbstract;

        public MemberIsAbstractVerifier(bool isAbstract = true)
        {
            _isAbstract = isAbstract;
        }

        public override MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.PropertyGetIsAbstract,
            MemberVerificationAspect.PropertySetIsAbstract,
            MemberVerificationAspect.MethodIsAbstract
        };

        public override void Verify(MemberDescription originalMember, MemberDescription translatedMember)
        {
            Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Property, MemberTypes.Method });

            if (translatedMember is PropertyDescription translatedProperty)
            {
                Verifier.VerifyIsAbstract(translatedProperty, _isAbstract);
            }
            else if (translatedMember is MethodDescription translatedMethod)
            {
                Verifier.VerifyIsAbstract(translatedMethod, _isAbstract);
            }
            else throw new NotImplementedException();
        }
    }
}
