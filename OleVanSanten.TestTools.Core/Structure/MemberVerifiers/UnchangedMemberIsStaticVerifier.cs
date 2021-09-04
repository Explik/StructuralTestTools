using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class UnchangedMemberIsStaticVerifier : IMemberVerifier
    {
        public MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.FieldIsStatic,
            MemberVerificationAspect.MethodIsStatic,
            MemberVerificationAspect.PropertyIsStatic
        };

        public void Verify(MemberVerifierArgs args)
        {
            if (args.OriginalMember is FieldDescription originalField)
            {
                args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Field, MemberTypes.Property });

                if (args.TranslatedMember is FieldDescription translatedField)
                {
                    args.Verifier.VerifyIsStatic(translatedField, originalField.IsStatic);
                }
                else if (args.TranslatedMember is PropertyDescription translatedProperty)
                {
                    args.Verifier.VerifyIsStatic(translatedProperty, originalField.IsStatic);
                }
            }
            else if (args.OriginalMember is PropertyDescription originalProperty)
            {
                bool isStatic = originalProperty.CanRead ? originalProperty.GetMethod.IsStatic : originalProperty.SetMethod.IsStatic;

                args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Field, MemberTypes.Property });

                if (args.TranslatedMember is FieldDescription translatedField)
                {
                    args.Verifier.VerifyIsStatic(translatedField, isStatic);
                }
                else if (args.TranslatedMember is PropertyDescription translatedProperty)
                {
                    args.Verifier.VerifyIsStatic(translatedProperty, isStatic);
                }
            }
            else if (args.TranslatedMember is MethodDescription originalMethod)
            {
                args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Method });
                args.Verifier.VerifyIsStatic((MethodDescription)args.TranslatedMember, originalMethod.IsStatic);
            }
            else throw new NotImplementedException();
        }
    }
}
