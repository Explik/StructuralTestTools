﻿using System;
using System.Collections.Generic;
using System.Text;
using TestTools.Helpers;
using TestTools.TypeSystem;

namespace TestTools.Structure
{
    public class PropertyIsWriteonlyVerifier : MemberVerifier
    {
        public override MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.MemberType,
            MemberVerificationAspect.PropertyGetAccessLevel,
            MemberVerificationAspect.PropertySetAccessLevel
        };

        public override void Verify(MemberDescription originalMember, MemberDescription translatedMember)
        {
            Verifier.VerifyMemberType(translatedMember, new[] { MemberTypes.Property });
            Verifier.VerifyIsWriteonly((PropertyDescription)translatedMember, DescriptionHelper.GetAccessLevel(translatedMember));
        }
    }
}
