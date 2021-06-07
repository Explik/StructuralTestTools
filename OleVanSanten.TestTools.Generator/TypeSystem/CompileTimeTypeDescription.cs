using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OleVanSanten.TestTools.Helpers;

namespace OleVanSanten.TestTools.TypeSystem
{
    public class CompileTimeTypeDescription : TypeDescription
    {
        public CompileTimeTypeDescription(Compilation compilation, ITypeSymbol typeSymbol)
        {
            if (compilation == null)
                throw new ArgumentNullException("compilation");
            if (typeSymbol == null)
                throw new ArgumentNullException("typeSymbol");

            TypeSymbol = typeSymbol;
            Compilation = compilation;
        }

        public ITypeSymbol TypeSymbol { get; }

        public Compilation Compilation { get; }

        public override TypeDescription BaseType => new CompileTimeTypeDescription(Compilation, TypeSymbol.BaseType);

        public override string Name => TypeSymbol.Name;

        public override string Namespace
        {
            get
            {
                INamespaceSymbol namespaceSymbol = TypeSymbol.ContainingNamespace;

                if (namespaceSymbol == null)
                    return new CompileTimeNamespaceDescription(Compilation, Compilation.GlobalNamespace).Name;

                return new CompileTimeNamespaceDescription(Compilation, namespaceSymbol).Name;
            }
        }

        public override bool IsAbstract => TypeSymbol.IsAbstract;

        public override bool IsArray => TypeSymbol.TypeKind == TypeKind.Array;

        public override bool IsClass => TypeSymbol.TypeKind == TypeKind.Class;

        public override bool IsEnum => TypeSymbol.TypeKind == TypeKind.Enum;

        public override bool IsInterface => TypeSymbol.TypeKind == TypeKind.Interface;

        public override bool IsNotPublic => !IsPublic;

        public override bool IsPublic => TypeSymbol.DeclaredAccessibility == Accessibility.Public;

        public override bool IsSealed => TypeSymbol.IsSealed;

        public override bool IsGenericType
        {
            get
            {
                if (TypeSymbol is INamedTypeSymbol namedTypeSymbol)
                {
                    return namedTypeSymbol.IsGenericType;
                }
                return false;
            }
        }

        public override ConstructorDescription[] GetConstructors()
        {
            var members = TypeSymbol.GetMembers();
            var output = new List<CompileTimeConstructorDescription>();

            for (int i = 0; i < members.Length; i++)
            {
                if (members[i] is IMethodSymbol method && method.MethodKind == MethodKind.Constructor)
                    output.Add(new CompileTimeConstructorDescription(Compilation, method));
            }

            return output.ToArray();
        }

        // Please note, GetCustomAttributes might return fewer results than
        // GetCustomAttributeTypes, because attributes cannot be loaded from 
        // non .netstandard2.0 targeting assemblies
        public override Attribute[] GetCustomAttributes()
        {
            return TypeSymbol
               .GetAttributes()
               .Select(attributeData => attributeData.ConvertToAttribute())
               .Where(attribute => attribute != null)
               .ToArray();
        }

        public override TypeDescription[] GetCustomAttributeTypes()
        {
            var attributes = TypeSymbol.GetAttributes();
            var output = new List<TypeDescription>();

            for(int i = 0; i < attributes.Length; i++)
            {
                output.Add(new CompileTimeTypeDescription(Compilation, attributes[i].AttributeClass));
            }

            return output.ToArray();
        }

        public override TypeDescription GetElementType()
        {
            if (IsArray)
            {
                var arrayType = (IArrayTypeSymbol)TypeSymbol;
                return new CompileTimeTypeDescription(Compilation, arrayType.ElementType);
            }
            return null;
        }

        public override EventDescription[] GetEvents()
        {
            var members = TypeSymbol.GetMembers();
            var output = new List<CompileTimeEventDescription>();

            for (int i = 0; i < members.Length; i++)
            {
                if (members[i] is IEventSymbol @event)
                    output.Add(new CompileTimeEventDescription(Compilation, @event));
            }

            return output.ToArray();
        }

        public override FieldDescription[] GetFields()
        {
            var members = TypeSymbol.GetMembers();
            var output = new List<CompileTimeFieldDescription>();

            for (int i = 0; i < members.Length; i++)
            {
                if (members[i] is IFieldSymbol field)
                    output.Add(new CompileTimeFieldDescription(Compilation, field));
            }

            return output.ToArray();
        }

        public override TypeDescription[] GetGenericArguments()
        {
            if (TypeSymbol is INamedTypeSymbol namedTypeSymbol)
            {
                return namedTypeSymbol.TypeArguments.Select(t => new CompileTimeTypeDescription(Compilation, t)).ToArray();
            }
            return new TypeDescription[0];
        }

        public override TypeDescription GetGenericTypeDefinition()
        {
            if (TypeSymbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType)
            {
                // Based on https://stackoverflow.com/questions/28240167/correct-way-to-check-the-type-of-an-expression-in-roslyn-analyzer
                return new CompileTimeTypeDescription(Compilation, namedTypeSymbol.OriginalDefinition);
            }
            throw new InvalidOperationException("GetGenericTypeDefinition cannot be performed on non-generic type");
        }

        public override TypeDescription[] GetInterfaces()
        {
            var interfaces = TypeSymbol.AllInterfaces;
            var output = new List<CompileTimeTypeDescription>();

            for (int i = 0; i < interfaces.Length; i++)
            {
                output.Add(new CompileTimeTypeDescription(Compilation, interfaces[i]));
            }

            return output.ToArray();
        }

        public override MethodDescription[] GetMethods()
        {
            var members = TypeSymbol.GetMembers();
            var output = new List<CompileTimeMethodDescription>();

            for (int i = 0; i < members.Length; i++)
            {
                if (members[i] is IMethodSymbol method && method.MethodKind == MethodKind.Ordinary)
                    output.Add(new CompileTimeMethodDescription(Compilation, method));
            }

            return output.ToArray();
        }

        public override TypeDescription[] GetNestedTypes()
        {
            var nestedTypes = TypeSymbol.GetTypeMembers();
            var output = new List<CompileTimeTypeDescription>();

            for(int i = 0; i < nestedTypes.Length; i++)
            {
                output.Add(new CompileTimeTypeDescription(Compilation, nestedTypes[i]));
            }

            return output.ToArray();
        }

        public override PropertyDescription[] GetProperties()
        {
            var members = TypeSymbol.GetMembers();
            var output = new List<CompileTimePropertyDescription>();

            for(int i = 0; i < members.Length; i++)
            {
                if (members[i] is IPropertySymbol property)
                    output.Add(new CompileTimePropertyDescription(Compilation, property));
            }

            return output.ToArray();
        }

        public override TypeDescription MakeArrayType()
        {
            var arrayType = Compilation.CreateArrayTypeSymbol(TypeSymbol);
            return new CompileTimeTypeDescription(Compilation, arrayType);
        }

        public override TypeDescription MakeGenericType(params TypeDescription[] typeArguments)
        {
            if (TypeSymbol is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.IsGenericType)
            {
                var args = typeArguments.OfType<CompileTimeTypeDescription>().Select(t => t.TypeSymbol).ToArray();
                return new CompileTimeTypeDescription(Compilation, namedTypeSymbol.Construct(args));
            }
            throw new InvalidOperationException("MakeGenericType cannot be performed on non-generic type");
        }
    }
}
