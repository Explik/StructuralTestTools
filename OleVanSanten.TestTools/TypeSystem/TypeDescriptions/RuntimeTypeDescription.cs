using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OleVanSanten.TestTools.TypeSystem
{
    public class RuntimeTypeDescription : TypeDescription
    {
        public RuntimeTypeDescription(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            Type = type;
        }

        public Type Type { get; }

        public override TypeDescription BaseType => new RuntimeTypeDescription(Type.BaseType);

        public override string Name => Type.Name;

        public override bool IsAbstract => Type.IsAbstract;

        public override bool IsArray => Type.IsArray;

        public override bool IsClass => Type.IsClass;

        public override bool IsEnum => Type.IsEnum;

        public override bool IsInterface => Type.IsInterface;

        public override bool IsNotPublic => Type.IsNotPublic;

        public override bool IsPublic => Type.IsPublic;

        public override bool IsSealed => Type.IsSealed;

        public override string Namespace => Type.Namespace;

        public override bool IsGenericType => Type.IsGenericType;

        public override ConstructorDescription[] GetConstructors()
        {
            var allConstructors = Type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return allConstructors.Select(c => new RuntimeConstructorDescription(c)).ToArray();
        }

        public override Attribute[] GetCustomAttributes()
        {
            return Type.GetCustomAttributes().ToArray();
        }

        public override TypeDescription[] GetCustomAttributeTypes()
        {
            var attributes = Type.GetCustomAttributes();
            return attributes.Select(t => new RuntimeTypeDescription(t.GetType())).ToArray();
        }

        public override TypeDescription GetElementType()
        {
            var elementType = Type.GetElementType();
            return elementType != null ? new RuntimeTypeDescription(elementType) : null;
        }

        public override EventDescription[] GetEvents()
        {
            var allEvents = Type.GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            return allEvents.Select(e => new RuntimeEventDescription(e)).ToArray();
        }

        public override FieldDescription[] GetFields()
        {
            var allFields = Type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            return allFields.Select(f => new RuntimeFieldDescription(f)).ToArray();
        }

        public override TypeDescription[] GetGenericArguments()
        {
            return Type.GetGenericArguments().Select(t => new RuntimeTypeDescription(t)).ToArray();
        }

        public override TypeDescription GetGenericTypeDefinition()
        {
            if (Type.IsGenericType)
            {
                return new RuntimeTypeDescription(Type.GetGenericTypeDefinition());
            }
            throw new InvalidOperationException("GetGenericTypeDefinition cannot be performed on non-generic type");
        }

        public override TypeDescription[] GetInterfaces()
        {
            return Type.GetInterfaces().Select(t => new RuntimeTypeDescription(t)).ToArray();
        }

        public override MethodDescription[] GetMethods()
        {
            var allMethods = Type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            return allMethods.Select(m => new RuntimeMethodDescription(m)).ToArray();
        }

        public override TypeDescription[] GetNestedTypes()
        {
            var allNestedTypes = Type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            return allNestedTypes.Select(t => new RuntimeTypeDescription(t)).ToArray();
        }

        public override PropertyDescription[] GetProperties()
        {
            var allProperties = Type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Instance);
            return allProperties.Select(p => new RuntimePropertyDescription(p)).ToArray();
        }

        public override TypeDescription MakeArrayType()
        {
            return new RuntimeTypeDescription(Type.MakeArrayType());
        }

        public override TypeDescription MakeGenericType(params TypeDescription[] typeArguments)
        {
            if (Type.IsGenericType)
            {
                var args = typeArguments.OfType<RuntimeTypeDescription>().Select(t => t.Type).ToArray();
                return new RuntimeTypeDescription(Type.MakeGenericType(args));
            }
            throw new InvalidOperationException("MakeGenericType cannot be performed on non-generic type");
        }
    }
}
