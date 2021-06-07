using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class UnchangedMemberDeclaringType : MemberVerifier
    {
        public override MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.MethodDeclaringType,
            MemberVerificationAspect.PropertyGetDeclaringType,
            MemberVerificationAspect.PropertySetDeclaringType
        };

        public override void Verify(MemberDescription originalMember, MemberDescription translatedMember)
        {
            if (originalMember is MethodDescription originalMethod)
            {
                var type = Service.TranslateType(originalMethod.DeclaringType);
                Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Method });
                Verifier.VerifyDeclaringType((MethodDescription)translatedMember, type);
            }
            else if (originalMember is PropertyDescription originalProperty)
            {
                var type1 = originalProperty.CanRead ? Service.TranslateType(originalProperty.GetMethod.DeclaringType) : null;
                var type2 = originalProperty.CanWrite ? Service.TranslateType(originalProperty.SetMethod.DeclaringType) : null;

                Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Field, MemberTypes.Property });

                if (translatedMember is FieldDescription translatedField)
                {
                    Verifier.VerifyDeclaringType(translatedField, type1 ?? type2);
                }
                else if (translatedMember is PropertyDescription translatedProperty)
                {
                    if (type1 != null)
                        Verifier.VerifyDeclaringType(translatedProperty, type1, GetMethod: true);
                    
                    if (type2 != null)
                        Verifier.VerifyDeclaringType(translatedProperty, type2, SetMethod: true);
                }
            }
            else throw new NotImplementedException();
        }
    }
}
