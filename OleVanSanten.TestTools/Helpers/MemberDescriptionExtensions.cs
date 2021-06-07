using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OleVanSanten.TestTools.Structure;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Helpers
{
    public static class MemberDescriptionExtensions
    {
        public static IMemberTranslator GetCustomTranslator(this MemberDescription memberInfo)
        {
            return memberInfo.GetCustomAttributes().OfType<IMemberTranslator>().FirstOrDefault();
        }

        public static IMemberVerifier GetCustomVerifier(this MemberDescription memberInfo, MemberVerificationAspect aspect)
        {
            return memberInfo.GetCustomAttributes().OfType<IMemberVerifier>().FirstOrDefault(ver => ver.Aspects.Contains(aspect));
        }
    }
}
