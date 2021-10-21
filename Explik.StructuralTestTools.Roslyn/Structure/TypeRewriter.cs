using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Explik.StructuralTestTools.Helpers;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    // Source Generator of TypeVisitor
    // TypeRewriter translates and verifies statements
    public class TypeRewriter : CSharpSyntaxRewriter
    {
        private ICompileTimeDescriptionResolver _resolver;
        private IStructureService _structureService;
        
        public TypeRewriter(ICompileTimeDescriptionResolver resolver, IStructureService structureService)
        {
            _resolver = resolver;
            _structureService = structureService;
        }

        public override SyntaxNode VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            var originalConstructor = _resolver.GetConstructorDescription(node);
            var originalType = originalConstructor.DeclaringType;
            var translatedType = _structureService.TranslateType(originalType);

            if (originalType == translatedType)
                return base.VisitObjectCreationExpression(node);

            _structureService.VerifyType(originalType);
            _structureService.VerifyMember(
                originalConstructor,
                MemberVerificationAspect.MemberType,
                MemberVerificationAspect.ConstructorAccessLevel);

            // Potentially rewritting type
            var newType = SyntaxFactory.ParseTypeName(translatedType.FullName);

            // Potentially rewritting method arguments
            var newArguments = SyntaxFactory.SeparatedList(node.ArgumentList.Arguments.Select(Visit).OfType<ArgumentSyntax>());
            var newArgumentList = node.ArgumentList.WithArguments(newArguments);

            // Potentially rewritting object initializer
            var newInitializer = (InitializerExpressionSyntax)this.Visit(node.Initializer);

            return node.WithType(newType).WithArgumentList(newArgumentList).WithInitializer(newInitializer);
        }

        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            var memberExpression = node.Expression as MemberAccessExpressionSyntax;

            if (memberExpression == null)
                return node;

            var originalMethod = _resolver.GetMethodDescription(node);
            var originalType = originalMethod.DeclaringType;
            var translatedMethod = _structureService.TranslateMember(originalMethod);

            if (originalMethod == translatedMethod)
                return base.VisitInvocationExpression(node);

            _structureService.VerifyType(originalType);
            _structureService.VerifyMember(
                originalMethod,
                MemberVerificationAspect.MemberType,
                MemberVerificationAspect.MethodDeclaringType,
                MemberVerificationAspect.MethodReturnType,
                MemberVerificationAspect.MethodIsStatic,
                MemberVerificationAspect.MethodIsAbstract,
                MemberVerificationAspect.MethodIsVirtual,
                MemberVerificationAspect.MethodAccessLevel);

            // Potentially rewritting expression and method name 
            var newName = GetNameSyntax((MethodDescription)translatedMethod);
            var newMemberExpression = (MemberAccessExpressionSyntax)Visit(memberExpression);
            var newExpression = newMemberExpression.WithName(newName);

            // Potentially rewritting method arguments
            var newArguments = SyntaxFactory.SeparatedList(node.ArgumentList.Arguments.Select(Visit).OfType<ArgumentSyntax>());
            var newArgumentList = node.ArgumentList.WithArguments(newArguments);
            
            return node.WithExpression(newExpression).WithArgumentList(newArgumentList);
        }

        
        public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            var originalMember = _resolver.GetMemberDescription(node);
            var originalType = originalMember.DeclaringType;
            var translatedMember = _structureService.TranslateMember(originalMember);

            if (originalMember == translatedMember)
                return base.VisitMemberAccessExpression(node);

            _structureService.VerifyType(originalType);
            if (originalMember is EventDescription)
            {
                _structureService.VerifyMember(
                    originalMember,
                    MemberVerificationAspect.EventHandlerType,
                    MemberVerificationAspect.EventAddAccessLevel,
                    MemberVerificationAspect.EventRemoveAccessLevel);
            }
            else if (originalMember is FieldDescription)
            {
                _structureService.VerifyMember(
                    originalMember,
                    MemberVerificationAspect.FieldType,
                    MemberVerificationAspect.FieldIsStatic,
                    MemberVerificationAspect.FieldWriteability,
                    MemberVerificationAspect.FieldAccessLevel);
            }
            else if (originalMember is PropertyDescription)
            {
                _structureService.VerifyMember(
                       originalMember,
                       MemberVerificationAspect.PropertyType,
                       MemberVerificationAspect.PropertyIsStatic,
                       MemberVerificationAspect.PropertySetDeclaringType,
                       MemberVerificationAspect.PropertySetIsAbstract,
                       MemberVerificationAspect.PropertySetIsVirtual,
                       MemberVerificationAspect.PropertySetAccessLevel);
            }

            // Potentially rewritting member name
            var newName = SyntaxFactory.IdentifierName(translatedMember.Name);

            // Potentially rewritting expression
            var newExpression = (ExpressionSyntax)Visit(node.Expression);

            return node.WithName(newName).WithExpression(newExpression);
        }

        public override SyntaxNode VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            var originalType = _resolver.GetTypeDescription(node);
            var translatedType = _structureService.TranslateType(originalType);

            if (originalType == translatedType)
                return base.VisitVariableDeclaration(node);

            _structureService.VerifyType(originalType);

            // Potentially rewritting type
            var newType = GetTypeSyntax(translatedType);

            // Potentially rewritting variable declators
            var newVariables = SyntaxFactory.SeparatedList(node.Variables.Select(Visit).OfType<VariableDeclaratorSyntax>());
            
            return node.WithType(newType).WithVariables(newVariables);
        }

        private SimpleNameSyntax GetNameSyntax(MethodDescription methodDescription)
        {
            if (methodDescription.IsGenericMethod)
            {
                var methodName = SyntaxFactory.Identifier(methodDescription.Name);
                var typeArguments = methodDescription.GetGenericArguments().Select(t => GetTypeSyntax(t)).ToArray();
                var seperatedList = SyntaxFactory.SeparatedList(typeArguments);
                var typeArgumentList = SyntaxFactory.TypeArgumentList(seperatedList);

                return SyntaxFactory.GenericName(methodName, typeArgumentList);
            }
            return SyntaxFactory.IdentifierName(methodDescription.Name); ;
        }

        private TypeSyntax GetTypeSyntax(TypeDescription typeDescription)
        {
            if (typeDescription.IsGenericType)
            {
                // Compiler is to add ` to indicate arity for generic types, however as this is not part
                // of return C# syntax it and all subsquent characters are removed
                var litteralTypeName = typeDescription.Name.Split('`').First();

                var typeName = SyntaxFactory.Identifier(litteralTypeName);
                var typeArguments = typeDescription.GetGenericArguments().Select(t => GetTypeSyntax(t)).ToArray();
                var seperatedList = SyntaxFactory.SeparatedList(typeArguments);
                var typeArgumentList = SyntaxFactory.TypeArgumentList(seperatedList);

                return SyntaxFactory.GenericName(typeName, typeArgumentList);
            }
            if (typeDescription.IsArray)
            {
                var elementType = GetTypeSyntax(typeDescription.GetElementType());
                SyntaxFactory.ArrayType(elementType);
            }
            return SyntaxFactory.ParseTypeName(typeDescription.FullName);
        }
    }
}