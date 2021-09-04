using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools
{
    public class MemberAccessLevelVerifier : IMemberVerifier
    {
        readonly AccessLevels[] _accessLevels;

        public MemberAccessLevelVerifier(AccessLevels accessLevel) : this(new[] { accessLevel })
        {
        }

        public MemberAccessLevelVerifier(AccessLevels[] accessLevels)
        {
            _accessLevels = accessLevels;
        }

        public MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.ConstructorAccessLevel,
            MemberVerificationAspect.EventAddAccessLevel,
            MemberVerificationAspect.EventRemoveAccessLevel,
            MemberVerificationAspect.FieldAccessLevel,
            MemberVerificationAspect.PropertyGetAccessLevel,
            MemberVerificationAspect.PropertySetAccessLevel,
            MemberVerificationAspect.MethodAccessLevel
        };

        public void Verify(MemberVerifierArgs args)
        {
            if (args.TranslatedMember is ConstructorDescription translatedConstructor)
            {
                args.Verifier.VerifyAccessLevel(translatedConstructor, _accessLevels);
            }
            else if (args.TranslatedMember is EventDescription translatedEvent)
            {
                args.Verifier.VerifyAccessLevel(translatedEvent, _accessLevels, AddMethod: true, RemoveMethod: true);
            }
            else if (args.TranslatedMember is FieldDescription translatedField)
            {
                args.Verifier.VerifyAccessLevel(translatedField, _accessLevels);
            }
            else if (args.TranslatedMember is PropertyDescription translatedProperty)
            {
                args.Verifier.VerifyAccessLevel(translatedProperty, _accessLevels, GetMethod: true, SetMethod: true);
            }
            else if (args.TranslatedMember is MethodDescription translatedMethod)
            {
                args.Verifier.VerifyAccessLevel(translatedMethod, _accessLevels);
            }
            else throw new NotImplementedException();
        }
    }
}
