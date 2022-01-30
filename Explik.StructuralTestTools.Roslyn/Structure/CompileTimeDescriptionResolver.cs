using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Explik.StructuralTestTools.TypeSystem;
using System;
using System.Linq;
using System.Reflection;

namespace Explik.StructuralTestTools
{
    public class SyntaxResolutionException : Exception
    {
        public SyntaxResolutionException() : base() { }

        public SyntaxResolutionException(string message) : base(message) { }
    }

    public class CompileTimeDescriptionResolver : ICompileTimeDescriptionResolver
    {
        Compilation _compilation;

        public CompileTimeDescriptionResolver(Compilation compilation)
        {
            _compilation = compilation;
        }

        public ConstructorDescription GetConstructorDescription(ObjectCreationExpressionSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree, ignoreAccessibility: true);
            var methodSymbol = (IMethodSymbol)semanticModel.GetSymbolInfo(node).Symbol;

            if (methodSymbol == null)
                throw new SyntaxResolutionException("Could not resolve method in " + node.ToString());

            return new CompileTimeConstructorDescription(_compilation, methodSymbol);
        }

        public ConstructorDescription GetConstructorDescription(ConstructorDeclarationSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree, ignoreAccessibility: true);
            var methodSymbol = semanticModel.GetDeclaredSymbol(node);

            if (methodSymbol == null)
                throw new SyntaxResolutionException("Could not resolve method in " + node.ToString());

            return new CompileTimeConstructorDescription(_compilation, methodSymbol);
        }

        public object GetDescription(MemberAccessExpressionSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree, ignoreAccessibility: true);
            var memberSymbol = semanticModel.GetSymbolInfo(node).Symbol;

            if (memberSymbol is ITypeSymbol typeSymbol)
                return new CompileTimeTypeDescription(_compilation, typeSymbol);

            if (memberSymbol is IEventSymbol eventSymbol)
                return new CompileTimeEventDescription(_compilation, eventSymbol);

            if (memberSymbol is IFieldSymbol fieldSymbol)
                return new CompileTimeFieldDescription(_compilation, fieldSymbol);

            if (memberSymbol is IMethodSymbol methodSymbol)
                return new CompileTimeMethodDescription(_compilation, methodSymbol);

            if (memberSymbol is IPropertySymbol propertySymbol)
                return new CompileTimePropertyDescription(_compilation, propertySymbol);

            throw new ArgumentException("Node cannot be converted to ITypeSymbol, IEventSymbol, IFieldSymbol, IMethodSymbol or IPropertySymbol");
        }

        public MethodDescription GetMethodDescription(InvocationExpressionSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree, ignoreAccessibility: true);
            var methodSymbol = (IMethodSymbol)semanticModel.GetSymbolInfo(node).Symbol;

            if (methodSymbol == null)
                throw new SyntaxResolutionException("Could not resolve method in " + node.ToString());

            return new CompileTimeMethodDescription(_compilation, methodSymbol);
        }

        public MethodDescription GetMethodDescription(MethodDeclarationSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree, ignoreAccessibility: true);
            var methodSymbol = semanticModel.GetDeclaredSymbol(node);

            if (methodSymbol == null)
                throw new SyntaxResolutionException("Could not resolve method in " + node.ToString());

            return new CompileTimeMethodDescription(_compilation, methodSymbol);
        }

        public PropertyDescription GetPropertyDescription(ElementAccessExpressionSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree, ignoreAccessibility: true);
            var propertySymbol = semanticModel.GetSymbolInfo(node).Symbol as IPropertySymbol;

            return propertySymbol != null ? new CompileTimePropertyDescription(_compilation, propertySymbol) : null;
        }

        public bool IsTemplatedAttribute(AttributeSyntax node)
        {
            var attributeType = GetTypeDescription(node);
            return TemplatedAttributes.IsTemplatedAttribute(attributeType);
        }

        public bool HasTemplatedAttribute(ClassDeclarationSyntax node)
        {
            return node.AttributeLists.Any(l => l.Attributes.Any(IsTemplatedAttribute));
        }

        public bool HasTemplatedAttribute(MethodDeclarationSyntax node)
        {
            return node.AttributeLists.Any(l => l.Attributes.Any(IsTemplatedAttribute));
        }

        public string GetAssociatedAttributeType(AttributeSyntax node)
        {
            var attributeType = GetTypeDescription(node);
            return TemplatedAttributes.GetAssociatedAttributeTypeName(attributeType);
        }

        public string GetAssociatedExceptionType(AttributeSyntax node)
        {
            var attributeType = GetTypeDescription(node);
            return TemplatedAttributes.GetAssociatedExceptionTypeName(attributeType);
        }

        public TypeDescription GetTypeDescription(ArrayTypeSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.ElementType.SyntaxTree, ignoreAccessibility: true);
            var typeModel = semanticModel.GetTypeInfo(node.ElementType).Type;

            if (typeModel == null)
                throw new SyntaxResolutionException("Could not resolve type of " + node.ToString());

            return new CompileTimeTypeDescription(_compilation, typeModel);
        }

        public TypeDescription GetTypeDescription(CastExpressionSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree, ignoreAccessibility: true);
            var typeModel = semanticModel.GetTypeInfo(node.Type).Type;

            if (typeModel == null)
                throw new SyntaxResolutionException("Could not resolve type of " + node.ToString());

            return new CompileTimeTypeDescription(_compilation, typeModel);
        }

        public TypeDescription GetTypeDescription(DefaultExpressionSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree, ignoreAccessibility: true);
            var typeModel = semanticModel.GetTypeInfo(node.Type).Type;

            if (typeModel == null)
                throw new SyntaxResolutionException("Could not resolve type of " + node.ToString());

            return new CompileTimeTypeDescription(_compilation, typeModel);
        }

        public TypeDescription GetTypeDescription(TypeDeclarationSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree, ignoreAccessibility: true);
            var typeModel = semanticModel.GetDeclaredSymbol(node);

            if (typeModel == null)
                throw new SyntaxResolutionException("Could not resolve type of " + node.ToString());

            return new CompileTimeTypeDescription(_compilation, typeModel);
        }

        public TypeDescription GetTypeDescription(TypeOfExpressionSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree, ignoreAccessibility: true);
            var typeModel = semanticModel.GetTypeInfo(node.Type).Type;

            if (typeModel == null)
                throw new SyntaxResolutionException("Could not resolve type of " + node.ToString());

            return new CompileTimeTypeDescription(_compilation, typeModel);
        }

        public TypeDescription GetTypeDescription(VariableDeclarationSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.Type.SyntaxTree, ignoreAccessibility: true);
            var typeSymbol = semanticModel.GetTypeInfo(node.Type).Type;

            if (typeSymbol == null)
                throw new SyntaxResolutionException("Could not resolve type of " + node.ToString());

            return new CompileTimeTypeDescription(_compilation, typeSymbol);
        }

        public TypeDescription GetTypeDescription(AttributeSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree, ignoreAccessibility: true);
            var attributeSymbolInfo = semanticModel.GetSymbolInfo(node);
            var attributeSymbol = attributeSymbolInfo.Symbol ?? attributeSymbolInfo.CandidateSymbols.FirstOrDefault();

            if (attributeSymbol == null)
                throw new SyntaxResolutionException("Could not resolve type of " + node.ToString() + "at " + node.GetLocation().ToString());

            return new CompileTimeTypeDescription(_compilation, attributeSymbol.ContainingType);
        }

        public NamespaceDescription GetNamespaceDescription(UsingDirectiveSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree, ignoreAccessibility: true);
            var namespaceSymbol = semanticModel.GetSymbolInfo(node.Name).Symbol as INamespaceSymbol;

            if (namespaceSymbol == null)
                throw new SyntaxResolutionException("Could not resolve namespace of " + node.ToString());

            return new CompileTimeNamespaceDescription(_compilation, namespaceSymbol);
        }
    }
}
