using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OleVanSanten.TestTools.Structure;
using System.Linq;
using OleVanSanten.TestTools.Syntax;

namespace OleVanSanten.TestTools.Helpers
{
    public static class MemberExtensions
    {
        public static IMemberTranslator GetCustomTranslator(this MemberInfo memberInfo)
        {
            return memberInfo.GetCustomAttributes().OfType<IMemberTranslator>().FirstOrDefault();
        }

        public static IMemberVerifier GetCustomVerifier(this MemberInfo memberInfo, MemberVerificationAspect aspect)
        {
            return memberInfo.GetCustomAttributes().OfType<IMemberVerifier>().FirstOrDefault(ver => ver.Aspects.Contains(aspect));
        }

        public static ISyntaxTransformer GetCustomTransformer(this MemberInfo memberInfo)
        {
            return memberInfo.GetCustomAttributes().OfType<ISyntaxTransformer>().FirstOrDefault();
        }
    }
}
