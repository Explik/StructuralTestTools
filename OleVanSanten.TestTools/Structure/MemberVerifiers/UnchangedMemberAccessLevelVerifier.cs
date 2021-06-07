using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Helpers;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class UnchangedMemberAccessLevelVerifier : MemberVerifier
    {
        public override MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.ConstructorAccessLevel,
            MemberVerificationAspect.FieldAccessLevel,
            MemberVerificationAspect.PropertyGetAccessLevel,
            MemberVerificationAspect.PropertySetAccessLevel,
            MemberVerificationAspect.MethodAccessLevel
        };

        public override void Verify(MemberDescription originalMember, MemberDescription translatedMember)
        {
            if (originalMember is ConstructorDescription originalConstructor)
            {
                AccessLevels accessLevel = DescriptionHelper.GetAccessLevel(originalConstructor);
                Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Constructor });
                Verifier.VerifyAccessLevel((ConstructorDescription)translatedMember, new[] { accessLevel });
            }
            else if (originalMember is FieldDescription originalField)
            {
                AccessLevels accessLevel = DescriptionHelper.GetAccessLevel(originalField);

                Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Field, MemberTypes.Property });

                if (translatedMember is FieldDescription translatedField)
                {
                    Verifier.VerifyAccessLevel(translatedField, new[] { accessLevel });
                }
                else if (translatedMember is PropertyDescription translatedProperty)
                {
                    Verifier.VerifyAccessLevel(translatedProperty, new[] { accessLevel }, GetMethod: true, SetMethod: true);
                }   
            }
            else if (originalMember is PropertyDescription originalProperty)
            {
                AccessLevels? accessLevel1 = originalProperty.CanRead ? DescriptionHelper.GetAccessLevel(originalProperty.GetMethod) : (AccessLevels?)null;
                AccessLevels? accessLevel2 = originalProperty.CanWrite ? DescriptionHelper.GetAccessLevel(originalProperty.SetMethod) : (AccessLevels?)null;

                Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Field, MemberTypes.Property });

                if (translatedMember is FieldDescription translatedField)
                {
                    if (accessLevel1 != null && accessLevel2 != null)
                    {
                        Verifier.VerifyAccessLevel(translatedField, new[] { (AccessLevels)accessLevel1, (AccessLevels)accessLevel2 });
                    }
                    else if (accessLevel1 != null)
                    {
                        Verifier.VerifyAccessLevel(translatedField, new[] { (AccessLevels)accessLevel1 });
                    }
                    else if (accessLevel2 != null)
                    {
                        Verifier.VerifyAccessLevel(translatedField, new[] { (AccessLevels)accessLevel2 });
                    }
                }
                else if (translatedMember is PropertyDescription translatedProperty)
                {
                    if (accessLevel1 != null)
                        Verifier.VerifyAccessLevel(translatedProperty, new[] { (AccessLevels)accessLevel1 }, GetMethod: true);
                    if (accessLevel2 != null)
                    Verifier.VerifyAccessLevel(translatedProperty, new[] { (AccessLevels)accessLevel2 }, SetMethod: true);
                }
            }
            else if (translatedMember is MethodDescription originalMethod)
            {
                AccessLevels accessLevel = DescriptionHelper.GetAccessLevel(originalMethod);
                Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Method });
                Verifier.VerifyAccessLevel((MethodDescription)translatedMember, new[] { accessLevel });
            }
            else throw new NotImplementedException();
        }
    }
}
