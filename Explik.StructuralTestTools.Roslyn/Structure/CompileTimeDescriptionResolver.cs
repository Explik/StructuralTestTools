using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Explik.StructuralTestTools.TypeSystem;
using System;
using System.Linq;
using System.Reflection;

namespace Explik.StructuralTestTools
{
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

            return new CompileTimeConstructorDescription(_compilation, methodSymbol);
        }

        public ConstructorDescription GetConstructorDescription(ConstructorDeclarationSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree, ignoreAccessibility: true);
            var methodSymbol = semanticModel.GetDeclaredSymbol(node);

            return new CompileTimeConstructorDescription(_compilation, methodSymbol);
        }

        public MemberDescription GetMemberDescription(MemberAccessExpressionSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree, ignoreAccessibility: true);
            var memberSymbol = semanticModel.GetSymbolInfo(node).Symbol;

            if (memberSymbol is IEventSymbol eventSymbol)
                return new CompileTimeEventDescription(_compilation, eventSymbol);

            if (memberSymbol is IFieldSymbol fieldSymbol)
                return new CompileTimeFieldDescription(_compilation, fieldSymbol);

            if (memberSymbol is IMethodSymbol methodSymbol)
                return new CompileTimeMethodDescription(_compilation, methodSymbol);

            if (memberSymbol is IPropertySymbol propertySymbol)
                return new CompileTimePropertyDescription(_compilation, propertySymbol);

            throw new ArgumentException("Node cannot be converted to IEventSymbol, IFieldSymbol, IMethodSymbol, or IPropertySymbol");
        }

        public MethodDescription GetMethodDescription(InvocationExpressionSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree, ignoreAccessibility: true);
            var methodSymbol = (IMethodSymbol)semanticModel.GetSymbolInfo(node).Symbol;

            return new CompileTimeMethodDescription(_compilation, methodSymbol);
        }

        public MethodDescription GetMethodDescription(MethodDeclarationSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree, ignoreAccessibility: true);
            var methodSymbol = semanticModel.GetDeclaredSymbol(node);

            return new CompileTimeMethodDescription(_compilation, methodSymbol);
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

        public TypeDescription GetTypeDescription(TypeDeclarationSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree, ignoreAccessibility: true);
            var typeModel = semanticModel.GetDeclaredSymbol(node);

            return new CompileTimeTypeDescription(_compilation, typeModel);
        }

        public TypeDescription GetTypeDescription(VariableDeclarationSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.Type.SyntaxTree, ignoreAccessibility: true);
            var typeSymbol = semanticModel.GetTypeInfo(node.Type).Type;

            return new CompileTimeTypeDescription(_compilation, typeSymbol);
        }

        public TypeDescription GetTypeDescription(AttributeSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree, ignoreAccessibility: true);
            var attributeSymbolInfo = semanticModel.GetSymbolInfo(node);
            var attributeSymbol = attributeSymbolInfo.Symbol ?? attributeSymbolInfo.CandidateSymbols.First();
            
            return new CompileTimeTypeDescription(_compilation, attributeSymbol.ContainingType);
        }
    }
}
