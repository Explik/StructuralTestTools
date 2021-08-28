using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using OleVanSanten.TestTools.Structure;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Helpers
{
    public static class FormatHelper
    {
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

            Dictionary<Type, string> BuiltinTypes = new Dictionary<Type, string>()
            {
                [typeof(void)] = "void",
                [typeof(bool)] = "bool",
                [typeof(int)] = "int",
                [typeof(byte)] = "byte",
                [typeof(sbyte)] = "sbyte",
                [typeof(char)] = "char",
                [typeof(decimal)] = "decimal",
                [typeof(double)] = "double",
                [typeof(float)] = "float",
                [typeof(int)] = "int",
                [typeof(uint)] = "uint",
                [typeof(long)] = "long",
                [typeof(ulong)] = "ulong",
                [typeof(short)] = "short",
                [typeof(ushort)] = "ushort",
                [typeof(string)] = "string",
                [typeof(object)] = "object"
            };

            /*if (BuiltinTypes.ContainsKey(type))
            {
                return BuiltinTypes[type];
            }*/
            /*else if (type.IsGenericType)
            {
                // This is based on https://stackoverflow.com/questions/1533115/get-generictype-name-in-good-format-using-reflection-on-c-sharp
                string typeName = type.Name.Substring(0, type.Name.IndexOf('`'));
                string[] typeArguments = type.GenericTypeArguments.Select(FormatType).ToArray();

                return string.Format("{0}<{1}>", typeName, string.Join(", ", typeArguments));
            }*/
            return type.Name;
        }
    }
}
