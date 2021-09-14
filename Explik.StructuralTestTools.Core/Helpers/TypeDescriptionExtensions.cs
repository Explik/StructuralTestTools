using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Explik.StructuralTestTools;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools.Helpers
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
            return GetCustomVerifiers(type).FirstOrDefault(v => v.Aspects.Contains(aspect));
        }

        public static ITypeVerifier[] GetCustomVerifiers(this TypeDescription type)
        {
            return type.GetCustomAttributes().OfType<ITypeVerifier>().ToArray();
        }
    }
}
