using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OleVanSanten.TestTools.Structure;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Helpers
{
    public static class TypeDescriptionExtensions
    {
        public static bool IsStatic(this TypeDescription type)
        {
            return type.IsAbstract && type.IsSealed;
        }

        public static ITypeTranslator GetCustomTranslator(this TypeDescription type)
        {
            return type.GetCustomAttributes().OfType<ITypeTranslator>().FirstOrDefault();
        }

        public static ITypeVerifier GetCustomVerifier(this TypeDescription type, TypeVerificationAspect aspect)
        {
            return type.GetCustomAttributes().OfType<ITypeVerifier>().FirstOrDefault(v => v.Aspects.Contains(aspect));
        }
    }
}
