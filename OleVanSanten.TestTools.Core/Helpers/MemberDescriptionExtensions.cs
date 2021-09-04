using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OleVanSanten.TestTools;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Helpers
{
    public static class MemberDescriptionExtensions
    {
        public static IMemberTranslator GetCustomTranslator(this MemberDescription memberInfo)
        {
            return memberInfo.GetCustomAttributes().OfType<IMemberTranslator>().FirstOrDefault();
        }

        public static IMemberVerifier GetCustomVerifier(this MemberDescription member, MemberVerificationAspect aspect)
        {
            return GetCustomVerifiers(member).FirstOrDefault(ver => ver.Aspects.Contains(aspect));
        }

        public static IMemberVerifier[] GetCustomVerifiers(this MemberDescription member)
        {
            return member.GetCustomAttributes().OfType<IMemberVerifier>().ToArray();
        }
    }
}
