using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public interface IStructureService
    {
        bool IsTranslatableType(TypeDescription type);

        TypeDescription TranslateType(TypeDescription type);

        MemberDescription TranslateMember(MemberDescription memberInfo);

        void VerifyType(TypeDescription original, ITypeVerifier[] verifiers);

        void VerifyMember(MemberDescription original, IMemberVerifier[] verifiers);
    }
}
