using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OleVanSanten.TestTools.Helpers;

namespace OleVanSanten.TestTools.TypeSystem
{
    public class CompileTimeConstructorDescription : ConstructorDescription
    {
        public CompileTimeConstructorDescription(Compilation compilation, IMethodSymbol methodSymbol)
        {
            if (compilation == null)
                throw new ArgumentNullException("compilation");
            if (methodSymbol == null)
                throw new ArgumentNullException("methodSymbol");

            Compilation = compilation;
            MethodSymbol = methodSymbol;
        }

        public Compilation Compilation { get; }

        public IMethodSymbol MethodSymbol { get; }

        public override TypeDescription DeclaringType => new CompileTimeTypeDescription(Compilation, MethodSymbol.ContainingType);

        public override bool IsAssembly
        {
            get
            {
                bool isInternal = MethodSymbol.DeclaredAccessibility == Accessibility.Internal;
                bool isProtectedAndInternal = MethodSymbol.DeclaredAccessibility == Accessibility.ProtectedAndInternal;
                bool isProtectedOrInternal = MethodSymbol.DeclaredAccessibility == Accessibility.ProtectedOrInternal;

                return isInternal || isProtectedAndInternal || isProtectedOrInternal;
            }
        }

        public override bool IsFamily
        {
            get
            {
                bool isProtected = MethodSymbol.DeclaredAccessibility == Accessibility.Protected;
                bool isProtectedAndFriend = MethodSymbol.DeclaredAccessibility == Accessibility.ProtectedAndFriend;

                return isProtected || isProtectedAndFriend;
            }
        }

        public override bool IsPrivate
        {
            get
            {
                bool isNotApplicable = MethodSymbol.DeclaredAccessibility == Accessibility.NotApplicable;
                bool isPrivate = MethodSymbol.DeclaredAccessibility == Accessibility.Private;

                return isNotApplicable || isPrivate;
            }
        }

        public override bool IsPublic => MethodSymbol.DeclaredAccessibility == Accessibility.Public;

        public override string Name => MethodSymbol.Name;

        public override bool IsAbstract => MethodSymbol.IsAbstract;

        public override bool IsStatic => MethodSymbol.IsStatic;

        public override bool IsVirtual => MethodSymbol.IsVirtual;

        // Please note, GetCustomAttributes might return fewer results than
        // GetCustomAttributeTypes, because attributes cannot be loaded from 
        // non .netstandard2.0 targeting assemblies
        public override Attribute[] GetCustomAttributes()
        {
            return MethodSymbol
                .GetAttributes()
                .Select(attributeData => attributeData.ConvertToAttribute())
                .Where(attribute => attribute != null)
                .ToArray();
        }

        public override TypeDescription[] GetCustomAttributeTypes()
        {
            var attributes = MethodSymbol.GetAttributes();
            var output = new List<TypeDescription>();

            for (int i = 0; i < attributes.Length; i++)
            {
                output.Add(new CompileTimeTypeDescription(Compilation, attributes[i].AttributeClass));
            }

            return output.ToArray();
        }


        public override ParameterDescription[] GetParameters()
        {
            var parameters = MethodSymbol.Parameters;
            var output = new List<CompileTimeParameterDescription>();

            for (int i = 0; i < parameters.Length; i++)
            {
                output.Add(new CompileTimeParameterDescription(Compilation, parameters[i]));
            }

            return output.ToArray();
        }
    }
}
