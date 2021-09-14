using Explik.StructuralTestTools.TypeSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Explik.StructuralTestTools
{
    public interface ITypeVerifierService
    {
        void VerifyType(TypeDescription original);
        
        void VerifyType(TypeDescription original, params TypeVerificationAspect[] aspects);
    }
}
