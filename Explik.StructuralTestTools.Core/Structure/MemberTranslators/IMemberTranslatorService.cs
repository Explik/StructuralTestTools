using Explik.StructuralTestTools.TypeSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Explik.StructuralTestTools
{
    public interface IMemberTranslatorService
    {
        MemberDescription TranslateMember(MemberDescription memberInfo);
    }
}
