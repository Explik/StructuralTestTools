using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class MemberIsStaticVerifier : MemberVerifier
    {
        bool _isStatic;

        public MemberIsStaticVerifier(bool isStatic = true)
        {
            _isStatic = isStatic;
        }

        public override MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.FieldIsStatic,
            MemberVerificationAspect.MethodIsStatic,
            MemberVerificationAspect.PropertyIsStatic
        };

        public override void Verify(MemberDescription originalMember, MemberDescription translatedMember)
        {
            Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Field, MemberTypes.Property, MemberTypes.Method });

            if (translatedMember is FieldDescription translatedField)
            {
                Verifier.VerifyIsStatic(translatedField, _isStatic);
            }
            else if (translatedMember is PropertyDescription translatedProperty)
            {
                Verifier.VerifyIsStatic(translatedProperty, _isStatic);
            }
            else if (translatedMember is MethodDescription translatedMethod)
            {
                Verifier.VerifyIsStatic(translatedMethod, _isStatic);
            }
            else throw new NotImplementedException();
        }
    }
}
