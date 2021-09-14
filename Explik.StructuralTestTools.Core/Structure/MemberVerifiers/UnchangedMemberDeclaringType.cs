using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class UnchangedMemberDeclaringType : IMemberVerifier
    {
        public MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.MethodDeclaringType,
            MemberVerificationAspect.PropertyGetDeclaringType,
            MemberVerificationAspect.PropertySetDeclaringType
        };

        public void Verify(MemberVerifierArgs args)
        {
            if (args.OriginalMember is MethodDescription originalMethod)
            {
                var type = args.TypeTranslatorService.TranslateType(originalMethod.DeclaringType);
                args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Method });
                args.Verifier.VerifyDeclaringType((MethodDescription)args.TranslatedMember, type);
            }
            else if (args.OriginalMember is PropertyDescription originalProperty)
            {
                var type1 = originalProperty.CanRead ? args.TypeTranslatorService.TranslateType(originalProperty.GetMethod.DeclaringType) : null;
                var type2 = originalProperty.CanWrite ? args.TypeTranslatorService.TranslateType(originalProperty.SetMethod.DeclaringType) : null;

                args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Field, MemberTypes.Property });

                if (args.TranslatedMember is FieldDescription translatedField)
                {
                    args.Verifier.VerifyDeclaringType(translatedField, type1 ?? type2);
                }
                else if (args.TranslatedMember is PropertyDescription translatedProperty)
                {
                    if (type1 != null)
                        args.Verifier.VerifyDeclaringType(translatedProperty, type1, GetMethod: true);
                    
                    if (type2 != null)
                        args.Verifier.VerifyDeclaringType(translatedProperty, type2, SetMethod: true);
                }
            }
            else throw new NotImplementedException();
        }
    }
}
