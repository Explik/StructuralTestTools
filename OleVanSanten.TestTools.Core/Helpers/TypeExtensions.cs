using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using OleVanSanten.TestTools.Structure;

namespace OleVanSanten.TestTools.Helpers
{
    public static class TypeExtensions
    {
        public static MemberInfo[] GetAllMembers(this Type type)
        {
            return type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        }

        public static bool IsStatic(this Type type)
        {
            return type.IsAbstract && type.IsSealed;
        }

        public static ITypeTranslator GetCustomTranslator(this Type type)
        {
            return type.GetCustomAttributes().OfType<ITypeTranslator>().FirstOrDefault();
        }

        public static ITypeVerifier GetCustomVerifier(this Type type, TypeVerificationAspect aspect)
        {
            return type.GetCustomAttributes().OfType<ITypeVerifier>().FirstOrDefault(v => v.Aspects.Contains(aspect));
        }

        // Inspired by https://bradhe.wordpress.com/2010/07/27/how-to-tell-if-a-type-implements-an-interface-in-net/
        public static bool IsImplementationOf(this Type type, Type interfaceType)
        {
            return type.GetInterfaces().Any(interfaceType.Equals);
        }

        // Inspired by https://stackoverflow.com/questions/429552/can-i-get-the-signature-of-a-c-sharp-delegate-by-its-type
        public static bool IsDelegateOf(this Type type, MethodInfo methodInfo)
        {
            if (!type.IsSubclassOf(typeof(Delegate)))
                throw new ArgumentException("type is not a delegate type");

            MethodInfo invoke = type.GetMethod("Invoke");
            ParameterInfo[] invokeParameters = invoke.GetParameters();
            ParameterInfo[] methodParameters = methodInfo.GetParameters();

            if (invoke.ReturnType != methodInfo.ReturnType)
                return false;
            if (invokeParameters.Length != methodParameters.Length)
                return false;
            for(int i = 0; i < invokeParameters.Length; i++)
            {
                if (invokeParameters[i].ParameterType != methodParameters[i].ParameterType)
                    return false;
            }
            return true;
        }

        public static Type GetGenericArguments(this Type type, Type typeTemplate, Type typeArgument)
        {
            if (typeTemplate == typeArgument)
                return type;

            // Example, int[] & T[] => T = int
            if (type.IsArray && type.IsArray)
            {
                return GetGenericArguments(type.GetElementType(), typeTemplate.GetElementType(), typeArgument);
            }

            if (type.IsGenericType && typeTemplate.IsGenericType)
            {
                Type result; 
                Type[] typeArgs = type.GetGenericArguments();
                Type[] typeTemplateArgs = typeTemplate.GetGenericArguments();

                for(int i = 0; i < typeArgs.Length; i++)
                {
                    result = GetGenericArguments(typeArgs[i], typeTemplateArgs[i], typeArgument);

                    if (result != null)
                        return result;
                }
            }
            return null;
        }
    }
}
