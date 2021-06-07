using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class UnchangedMemberIsStaticVerifier : MemberVerifier
    {
        public override MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.FieldIsStatic,
            MemberVerificationAspect.MethodIsStatic,
            MemberVerificationAspect.PropertyIsStatic
        };

        public override void Verify(MemberDescription originalMember, MemberDescription translatedMember)
        {
            if (originalMember is FieldDescription originalField)
            {
                Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Field, MemberTypes.Property });

                if (translatedMember is FieldDescription translatedField)
                {
                    Verifier.VerifyIsStatic(translatedField, originalField.IsStatic);
                }
                else if (translatedMember is PropertyDescription translatedProperty)
                {
                    Verifier.VerifyIsStatic(translatedProperty, originalField.IsStatic);
                }
            }
            else if (originalMember is PropertyDescription originalProperty)
            {
                bool isStatic = originalProperty.CanRead ? originalProperty.GetMethod.IsStatic : originalProperty.SetMethod.IsStatic;

                Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Field, MemberTypes.Property });

                if (translatedMember is FieldDescription translatedField)
                {
                    Verifier.VerifyIsStatic(translatedField, isStatic);
                }
                else if (translatedMember is PropertyDescription translatedProperty)
                {
                    Verifier.VerifyIsStatic(translatedProperty, isStatic);
                }
            }
            else if (translatedMember is MethodDescription originalMethod)
            {
                Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Method });
                Verifier.VerifyIsStatic((MethodDescription)translatedMember, originalMethod.IsStatic);
            }
            else throw new NotImplementedException();
        }
    }
}
