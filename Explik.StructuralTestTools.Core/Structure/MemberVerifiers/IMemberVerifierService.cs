using Explik.StructuralTestTools.TypeSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Explik.StructuralTestTools
{
    public interface IMemberVerifierService
    {
        void VerifyMember(MemberDescription original, params MemberVerificationAspect[] aspects);
    }
}
