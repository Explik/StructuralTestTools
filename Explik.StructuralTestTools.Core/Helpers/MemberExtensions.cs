using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Explik.StructuralTestTools;
using System.Linq;

namespace Explik.StructuralTestTools.Helpers
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
    }
}
