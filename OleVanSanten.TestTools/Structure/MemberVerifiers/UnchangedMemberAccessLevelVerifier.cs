using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Helpers;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class UnchangedMemberAccessLevelVerifier : IMemberVerifier
    {
        public MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.ConstructorAccessLevel,
            MemberVerificationAspect.FieldAccessLevel,
            MemberVerificationAspect.PropertyGetAccessLevel,
            MemberVerificationAspect.PropertySetAccessLevel,
            MemberVerificationAspect.MethodAccessLevel
        };

        public void Verify(MemberVerifierArgs args)
        {
            if (args.OriginalMember is ConstructorDescription originalConstructor)
            {
                AccessLevels accessLevel = DescriptionHelper.GetAccessLevel(originalConstructor);
                args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Constructor });
                args.Verifier.VerifyAccessLevel(originalConstructor, new[] { accessLevel });
            }
            else if (args.OriginalMember is FieldDescription originalField)
            {
                AccessLevels accessLevel = DescriptionHelper.GetAccessLevel(originalField);

                args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Field, MemberTypes.Property });

                if (args.TranslatedMember is FieldDescription translatedField)
                {
                    args.Verifier.VerifyAccessLevel(translatedField, new[] { accessLevel });
                }
                else if (args.TranslatedMember is PropertyDescription translatedProperty)
                {
                    args.Verifier.VerifyAccessLevel(translatedProperty, new[] { accessLevel }, GetMethod: true, SetMethod: true);
                }   
            }
            else if (args.OriginalMember is PropertyDescription originalProperty)
            {
                AccessLevels? accessLevel1 = originalProperty.CanRead ? DescriptionHelper.GetAccessLevel(originalProperty.GetMethod) : (AccessLevels?)null;
                AccessLevels? accessLevel2 = originalProperty.CanWrite ? DescriptionHelper.GetAccessLevel(originalProperty.SetMethod) : (AccessLevels?)null;

                args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Field, MemberTypes.Property });

                if (args.TranslatedMember is FieldDescription translatedField)
                {
                    if (accessLevel1 != null && accessLevel2 != null)
                    {
                        args.Verifier.VerifyAccessLevel(translatedField, new[] { (AccessLevels)accessLevel1, (AccessLevels)accessLevel2 });
                    }
                    else if (accessLevel1 != null)
                    {
                        args.Verifier.VerifyAccessLevel(translatedField, new[] { (AccessLevels)accessLevel1 });
                    }
                    else if (accessLevel2 != null)
                    {
                        args.Verifier.VerifyAccessLevel(translatedField, new[] { (AccessLevels)accessLevel2 });
                    }
                }
                else if (args.TranslatedMember is PropertyDescription translatedProperty)
                {
                    if (accessLevel1 != null)
                        args.Verifier.VerifyAccessLevel(translatedProperty, new[] { (AccessLevels)accessLevel1 }, GetMethod: true);
                    if (accessLevel2 != null)
                        args.Verifier.VerifyAccessLevel(translatedProperty, new[] { (AccessLevels)accessLevel2 }, SetMethod: true);
                }
            }
            else if (args.OriginalMember is MethodDescription originalMethod)
            {
                AccessLevels accessLevel = DescriptionHelper.GetAccessLevel(originalMethod);
                args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Method });
                args.Verifier.VerifyAccessLevel((MethodDescription)args.TranslatedMember, new[] { accessLevel });
            }
            else throw new NotImplementedException();
        }
    }
}
