﻿using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class EventHandlerTypeVerifier : IMemberVerifier
    {
        Type _type;

        public EventHandlerTypeVerifier(Type type)
        {
            _type = type;
        }

        public MemberVerificationAspect[] Aspects => new[] 
        {
            MemberVerificationAspect.EventHandlerType
        };

        public void Verify(MemberVerifierArgs args)
        {
            args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Event });
            args.Verifier.VerifyEventHandlerType((EventDescription)args.TranslatedMember, new RuntimeTypeDescription(_type));
        }
    }
}
