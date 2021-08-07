using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class MemberIsStaticVerifier : IMemberVerifier
    {
        bool _isStatic;

        public MemberIsStaticVerifier(bool isStatic = true)
        {
            _isStatic = isStatic;
        }

        public MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.FieldIsStatic,
            MemberVerificationAspect.MethodIsStatic,
            MemberVerificationAspect.PropertyIsStatic
        };

        public void Verify(MemberVerifierArgs args)
        {
            args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Field, MemberTypes.Property, MemberTypes.Method });

            if (args.TranslatedMember is FieldDescription translatedField)
            {
                args.Verifier.VerifyIsStatic(translatedField, _isStatic);
            }
            else if (args.TranslatedMember is PropertyDescription translatedProperty)
            {
                args.Verifier.VerifyIsStatic(translatedProperty, _isStatic);
            }
            else if (args.TranslatedMember is MethodDescription translatedMethod)
            {
                args.Verifier.VerifyIsStatic(translatedMethod, _isStatic);
            }
            else throw new NotImplementedException();
        }
    }
}
