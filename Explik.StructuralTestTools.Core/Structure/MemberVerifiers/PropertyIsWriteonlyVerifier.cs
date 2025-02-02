﻿using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools.Helpers;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class PropertyIsWriteonlyVerifier : IMemberVerifier
    {
        public MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.MemberType,
            MemberVerificationAspect.PropertyGetAccessLevel,
            MemberVerificationAspect.PropertySetAccessLevel
        };

        public void Verify(MemberVerifierArgs args)
        {
            args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Property });
            args.Verifier.VerifyIsWriteonly((PropertyDescription)args.TranslatedMember, DescriptionHelper.GetAccessLevel(args.TranslatedMember));
        }
    }
}
