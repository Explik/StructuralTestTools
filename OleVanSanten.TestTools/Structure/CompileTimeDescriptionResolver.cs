using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OleVanSanten.TestTools.Helpers;
using OleVanSanten.TestTools.Structure;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
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

        // Returns TemplatedAttribute object if attribute class is marked with [TemplatedAttribute]
        public AttributeEquivalentAttribute GetTemplatedAttribute(AttributeSyntax node)
        {
            var semanticModel = _compilation.GetSemanticModel(node.SyntaxTree, ignoreAccessibility: true);
            var attributeSymbol = semanticModel.GetSymbolInfo(node).Symbol;
            var attributeClass = attributeSymbol.ContainingType;
            var attributeDescription = new CompileTimeTypeDescription(_compilation, attributeClass);

            // Check if it contains any TemplateEquivalentAttribute at all
            var targetAttribute = new RuntimeTypeDescription(typeof(AttributeEquivalentAttribute));
            if (!attributeDescription.GetCustomAttributeTypes().Contains(targetAttribute))
                return null;

            return attributeDescription.GetCustomAttributes().OfType<AttributeEquivalentAttribute>().FirstOrDefault();
        }

        public AttributeEquivalentAttribute GetTemplatedAttribute(ClassDeclarationSyntax node)
        {
            var type = GetTypeDescription(node);
            var attributesOfAttributes = type.GetCustomAttributeTypes().SelectMany(t => t.GetCustomAttributes());

            return attributesOfAttributes.OfType<AttributeEquivalentAttribute>().FirstOrDefault();
        }

        public AttributeEquivalentAttribute GetTemplatedAttribute(MethodDeclarationSyntax node)
        {
            var method = GetMethodDescription(node);
            var attributesOfAttributes = method.GetCustomAttributeTypes().SelectMany(t => t.GetCustomAttributes());

            return attributesOfAttributes.OfType<AttributeEquivalentAttribute>().FirstOrDefault();
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
    }
}
