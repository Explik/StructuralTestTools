using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Explik.StructuralTestTools;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools.Helpers
{
    public static class FormatHelper
    {
        private static Dictionary<TypeDescription, string> BuiltinTypes = new Dictionary<TypeDescription, string>()
        {
            [new RuntimeTypeDescription(typeof(void))] = "void",
            [new RuntimeTypeDescription(typeof(bool))] = "bool",
            [new RuntimeTypeDescription(typeof(int))] = "int",
            [new RuntimeTypeDescription(typeof(byte))] = "byte",
            [new RuntimeTypeDescription(typeof(sbyte))] = "sbyte",
            [new RuntimeTypeDescription(typeof(char))] = "char",
            [new RuntimeTypeDescription(typeof(decimal))] = "decimal",
            [new RuntimeTypeDescription(typeof(double))] = "double",
            [new RuntimeTypeDescription(typeof(float))] = "float",
            [new RuntimeTypeDescription(typeof(int))] = "int",
            [new RuntimeTypeDescription(typeof(uint))] = "uint",
            [new RuntimeTypeDescription(typeof(long))] = "long",
            [new RuntimeTypeDescription(typeof(ulong))] = "ulong",
            [new RuntimeTypeDescription(typeof(short))] = "short",
            [new RuntimeTypeDescription(typeof(ushort))] = "ushort",
            [new RuntimeTypeDescription(typeof(string))] = "string",
            [new RuntimeTypeDescription(typeof(object))] = "object"
        };

        private readonly static Dictionary<Type, Func<object, string>> LiteralRepresentations = new Dictionary<Type, Func<object, string>>()
        {
            //[null] = (o) => "null",
            [typeof(bool)] = (o) => ((bool)o).ToString(),
            [typeof(byte)] = (o) => $"(byte){(byte)o}",
            [typeof(char)] = (o) => $"'{(char)o}'",
            [typeof(decimal)] = (o) => $"{(decimal)o}M",
            [typeof(float)] = (o) => $"{(float)o}F",
            [typeof(int)] = (o) => ((int)o).ToString(),
            [typeof(long)] = (o) =>  $"{(long)o}L",
            [typeof(short)] = (o) => $"(short){(short)o}",
            [typeof(string)] = (o) => $"\"{(string)o}\"",
            [typeof(sbyte)] = (o) => $"(sbyte){(sbyte)o}",
            [typeof(uint)] = (o) => $"{(uint)o}U",
            [typeof(ulong)] = (o) => $"{(ulong)o}UL",
            [typeof(ushort)] = (o) => $"(ushort){(ushort)o}"
        };

        public static bool HasLiteralRepresentation(object value)
        {
            return HasLiteralRepresentation(value?.GetType());
        }

        public static bool HasLiteralRepresentation(Type type)
        {
            return LiteralRepresentations.ContainsKey(type);
        }

        public static string FormatAsLiteral(object value)
        {
            Type type = value?.GetType();

            if (LiteralRepresentations.ContainsKey(type))
                throw new ArgumentException($"INTERNAL: {value} cannot be represented as literal");
            return LiteralRepresentations[type](value);
        }
        
        public static string FormatAndList(IEnumerable<string> list)
        {
            int i = 0; 
            int count = list.Count();
            StringBuilder builder = new StringBuilder();

            if (count == 1)
                return list.First();

            foreach(var item in list)
            {
                builder.Append(item);
                builder.Append((i < count - 1) ? ", " : "and ");
            }
            return builder.ToString();
        }

        public static string FormatOrList(IEnumerable<string> list)
        {
            int i = 0;
            int count = list.Count();
            StringBuilder builder = new StringBuilder();

            if (count == 1)
                return list.First();

            foreach (var item in list)
            {
                builder.Append(item);
                builder.Append((i < count - 1) ? ", " : "or ");
            }
            return builder.ToString();
        }

        public static string FormatAccessLevel(AccessLevels accessLevel)
        {
            switch (accessLevel)
            {
                case AccessLevels.Private:
                    return "private";
                case AccessLevels.Protected:
                    return "protected";
                case AccessLevels.Public:
                    return "public";
                case AccessLevels.InternalPrivate:
                    return "internal private";
                case AccessLevels.InternalProtected:
                    return "internal protected";
                case AccessLevels.InternalPublic:
                    return "internal public";
                default: throw new NotImplementedException();
            }
        }

        public static string FormatMethod(MethodDescription methodInfo)
        {
            return methodInfo.Name + "(" + FormatParameters(methodInfo.GetParameters()) + ")";
        }

        public static string FormatSignature(TypeDescription type, string name, ParameterDescription[] parameterInfos)
        {
            return FormatType(type) + " " + name + "(" + FormatParameters(parameterInfos) + ")";
        }

        public static string FormatConstructor(ConstructorDescription constructorInfo)
        {
            return FormatType(constructorInfo.DeclaringType) + "(" + FormatParameters(constructorInfo.GetParameters()) + ")";
        }

        private static string FormatParameters(ParameterDescription[] parameters)
        {
            StringBuilder builder = new StringBuilder();

            int i = 0; 
            foreach(ParameterDescription parameter in parameters)
            {
                if (i != 0)
                    builder.Append(", ");

                if (parameter.Name != null)
                {
                    builder.Append(string.Format("{0} {1}", FormatType(parameter.ParameterType), parameter.Name));
                }
                else builder.Append(string.Format("{0} par{1}", FormatType(parameter.ParameterType), i + 1));
                i++;
            }

            return builder.ToString();
        }

        public static string FormatMemberType(TypeSystem.MemberTypes memberType)
        {
            return memberType.ToString().ToLower();
        }

        public static string FormatType(TypeDescription type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (BuiltinTypes.ContainsKey(type))
            {
                return BuiltinTypes[type];
            }
            else if (type.IsGenericType)
            {
                if (type.FullName.Contains("System.Nullable"))
                {
                    string typeArgument = FormatFullTypeName(type.GetGenericArguments().Single());
                    return typeArgument + "?";
                }
                // This is based on https://stackoverflow.com/questions/1533115/get-generictype-name-in-good-format-using-reflection-on-c-sharp
                string typeName = type.FullName.Substring(0, type.FullName.IndexOf('`')).Replace("+", ".").Replace(" ", "");
                string[] typeArguments = type.GetGenericArguments().Select(FormatFullTypeName).ToArray();

                return string.Format("{0}<{1}>", typeName, string.Join(", ", typeArguments));
            }
            return type.Name.Replace("+", ".").Replace(" ", "");
        }

        public static string FormatFullTypeName(TypeDescription type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            

            if (BuiltinTypes.ContainsKey(type))
            {
                return BuiltinTypes[type];
            }
            else if (type.IsGenericType)
            {
                if (type.FullName.Contains("System.Nullable"))
                {
                    string typeArgument = FormatFullTypeName(type.GetGenericArguments().Single());
                    return typeArgument + "?";
                }
                // This is based on https://stackoverflow.com/questions/1533115/get-generictype-name-in-good-format-using-reflection-on-c-sharp
                string typeName = type.FullName.Substring(0, type.FullName.IndexOf('`')).Replace("+", ".").Replace(" ", "");
                string[] typeArguments = type.GetGenericArguments().Select(FormatFullTypeName).ToArray();

                return string.Format("{0}<{1}>", typeName, string.Join(", ", typeArguments));
            }
            return type.FullName.Replace("+", ".").Replace(" ", "");
        }
    }
}
