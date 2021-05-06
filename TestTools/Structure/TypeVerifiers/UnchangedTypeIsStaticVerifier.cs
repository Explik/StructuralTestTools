﻿using System;
using System.Collections.Generic;
using System.Text;
using TestTools.Helpers;
using TestTools.TypeSystem;

namespace TestTools.Structure
{
    public class UnchangedTypeIsStaticVerifier : TypeVerifier
    {
        public override TypeVerificationAspect[] Aspects => new[] { 
            TypeVerificationAspect.IsStatic
        };

        public override void Verify(TypeDescription originalType, TypeDescription translatedType)
        {
            bool isStatic = originalType.IsAbstract && originalType.IsSealed;
            Verifier.VerifyIsStatic(translatedType, isStatic);
        }
    }
}
