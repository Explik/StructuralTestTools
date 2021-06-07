using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OleVanSanten.TestTools.TypeSystem
{
    // Unifying class for wrappers of System.Type and Microsoft.CodeAnalysis.ITypeSymbol 
    public abstract class TypeDescription
    {
        public abstract TypeDescription BaseType { get; }

        public virtual string FullName => $"{Namespace}.{Name}";

        public abstract Attribute[] GetCustomAttributes();

        public abstract TypeDescription[] GetCustomAttributeTypes();

        public abstract ConstructorDescription[] GetConstructors();

        public abstract TypeDescription GetElementType();

        public abstract EventDescription[] GetEvents();

        public abstract FieldDescription[] GetFields();

        public abstract TypeDescription[] GetGenericArguments();

        public abstract TypeDescription GetGenericTypeDefinition();

        public abstract TypeDescription[] GetInterfaces();

        public virtual MemberDescription[] GetMembers()
        {
            return Enumerable
                .Empty<MemberDescription>()
                .Union(GetConstructors())
                .Union(GetEvents())
                .Union(GetFields())
                .Union(GetMethods())
                .Union(GetProperties())
                .ToArray();
        }

        public abstract MethodDescription[] GetMethods();

        public abstract TypeDescription[] GetNestedTypes();

        public abstract PropertyDescription[] GetProperties();

        public abstract string Name { get; }

        public abstract string Namespace { get; }

        public abstract bool IsAbstract { get; }

        public abstract bool IsArray { get; }

        public abstract bool IsClass { get; }

        public abstract bool IsEnum { get; }

        public abstract bool IsGenericType { get; }

        public abstract bool IsInterface { get; }

        public abstract bool IsNotPublic { get; }

        public abstract bool IsPublic { get; }

        public abstract bool IsSealed { get; }

        public bool IsSubclassOf(TypeDescription typeDescription)
        {
            bool result = this.Equals(typeDescription);

            result = result || BaseType.IsSubclassOf(typeDescription);

            result = result || GetInterfaces().Any(i => i.IsSubclassOf(typeDescription));

            return result;
        }

        public abstract TypeDescription MakeArrayType();

        public abstract TypeDescription MakeGenericType(params TypeDescription[] typeArguments);

        public override bool Equals(object obj)
        {
            TypeDescription other = obj as TypeDescription;
            return FullName == other?.FullName;
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

        // System.Type overloads == operator, so to fully compatible this is as well 
        public static bool operator== (TypeDescription t1, TypeDescription t2)
        {
            if (t1 is null || t2 is null)
                return t1 is null && t2 is null;

            return t1.Equals(t2);
        }

        // System.Type overloads != operator, so to fully compatible this is as well 
        public static bool operator!= (TypeDescription t1, TypeDescription t2)
        {
            if (t1 is null || t2 is null)
                return !(t1 is null && t2 is null);

            return !t1.Equals(t2);
        }
    }
}
