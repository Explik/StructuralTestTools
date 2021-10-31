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

        public override SyntaxNode VisitArrayType(ArrayTypeSyntax node)
        {
            var originalType = _resolver.GetTypeDescription(node);
            var translatedType = _structureService.TranslateType(originalType);

            if (originalType == translatedType)
                return base.VisitArrayType(node);

            _structureService.VerifyType(originalType);

            // Potentially rewritting type
            var innerMostType = translatedType;
            while (innerMostType.IsArray)
                innerMostType = innerMostType.GetElementType();
            var formattedTranslatedType = FormatHelper.FormatFullTypeName(innerMostType);
            var newElementType = SyntaxFactory.ParseTypeName(formattedTranslatedType);

            return node.WithElementType(newElementType);
        }

        public override SyntaxNode VisitCastExpression(CastExpressionSyntax node)
        {
            var originalType = _resolver.GetTypeDescription(node);
            var translatedType = _structureService.TranslateType(originalType);

            if (originalType == translatedType)
                return base.VisitCastExpression(node);

            _structureService.VerifyType(originalType);

            var formattedTranslatedType = FormatHelper.FormatFullTypeName(translatedType);
            var newType = SyntaxFactory.ParseTypeName(formattedTranslatedType);

            return node.WithType(newType);
        }

        public override SyntaxNode VisitDefaultExpression(DefaultExpressionSyntax node)
        {
            var originalType = _resolver.GetTypeDescription(node);
            var translatedType = _structureService.TranslateType(originalType);

            if (originalType == translatedType)
                return base.VisitDefaultExpression(node);

            _structureService.VerifyType(originalType);

            var formattedTranslatedType = FormatHelper.FormatFullTypeName(translatedType);
            var newType = SyntaxFactory.ParseTypeName(formattedTranslatedType);

            return node.WithType(newType);
        }

        public override SyntaxNode VisitElementAccessExpression(ElementAccessExpressionSyntax node)
        {
            var originalMember = _resolver.GetPropertyDescription(node);
            var originalType = originalMember.DeclaringType;
            var translatedMember = _structureService.TranslateMember(originalMember);

            if (originalMember != translatedMember)
            {
                _structureService.VerifyType(originalType);
                _structureService.VerifyMember(
                       originalMember,
                       MemberVerificationAspect.PropertyType,
                       MemberVerificationAspect.PropertyIsStatic,
                       MemberVerificationAspect.PropertySetDeclaringType,
                       MemberVerificationAspect.PropertySetIsAbstract,
                       MemberVerificationAspect.PropertySetIsVirtual,
                       MemberVerificationAspect.PropertySetAccessLevel);
            }
            return base.VisitElementAccessExpression(node);
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
            var formattedTranslatedType = FormatHelper.FormatFullTypeName(translatedType);
            var newType = SyntaxFactory.ParseTypeName(formattedTranslatedType);

            // Potentially rewritting method arguments
            var newArguments = SyntaxFactory.SeparatedList(node.ArgumentList.Arguments.Select(Visit).OfType<ArgumentSyntax>());
            var newArgumentList = node.ArgumentList.WithArguments(newArguments);

            // Potentially rewritting object initializer
            var newInitializer = (InitializerExpressionSyntax)this.Visit(node.Initializer);

            return node.WithType(newType).WithArgumentList(newArgumentList).WithInitializer(newInitializer);
        }

        public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            object originalMember = _resolver.GetDescription(node);
            object translatedMember;
            if (originalMember is TypeDescription typeDescription1)
            {
                translatedMember = _structureService.TranslateType(typeDescription1);
            }
            else if (originalMember is MemberDescription memberDescription1)
            {
                translatedMember = _structureService.TranslateMember(memberDescription1);
            }
            else throw new NotImplementedException("originalMember is not a TypeDescription or MemberDescription instance");

            if (originalMember == translatedMember)
                return base.VisitMemberAccessExpression(node);

            if (originalMember is EventDescription eventDescription1)
            {
                _structureService.VerifyType(eventDescription1.DeclaringType);
                _structureService.VerifyMember(
                    eventDescription1,
                    MemberVerificationAspect.EventHandlerType,
                    MemberVerificationAspect.EventAddAccessLevel,
                    MemberVerificationAspect.EventRemoveAccessLevel);
            }
            else if (originalMember is FieldDescription fieldDescription2)
            {
                _structureService.VerifyType(fieldDescription2.DeclaringType);
                _structureService.VerifyMember(
                    fieldDescription2,
                    MemberVerificationAspect.FieldType,
                    MemberVerificationAspect.FieldIsStatic,
                    MemberVerificationAspect.FieldWriteability,
                    MemberVerificationAspect.FieldAccessLevel);
            }
            else if (originalMember is MethodDescription methodDescription2)
            {
                _structureService.VerifyType(methodDescription2.DeclaringType);
                _structureService.VerifyMember(
                    methodDescription2,
                    MemberVerificationAspect.MemberType,
                    MemberVerificationAspect.MethodDeclaringType,
                    MemberVerificationAspect.MethodReturnType,
                    MemberVerificationAspect.MethodIsStatic,
                    MemberVerificationAspect.MethodIsAbstract,
                    MemberVerificationAspect.MethodIsVirtual,
                    MemberVerificationAspect.MethodAccessLevel);
            }
            else if (originalMember is PropertyDescription propertyDescription2)
            {
                _structureService.VerifyType(propertyDescription2.DeclaringType);
                _structureService.VerifyMember(
                       propertyDescription2,
                       MemberVerificationAspect.PropertyType,
                       MemberVerificationAspect.PropertyIsStatic,
                       MemberVerificationAspect.PropertySetDeclaringType,
                       MemberVerificationAspect.PropertySetIsAbstract,
                       MemberVerificationAspect.PropertySetIsVirtual,
                       MemberVerificationAspect.PropertySetAccessLevel);
            }

            // Potentially rewritting member name
            SimpleNameSyntax newName;
            if (translatedMember is TypeDescription typeDescription2 && typeDescription2.IsGenericType)
            {
                var name = typeDescription2.Name.Substring(0, typeDescription2.Name.IndexOf('`'));
                var identifier = SyntaxFactory.Identifier(name); 
                var typeArguments = typeDescription2.GetGenericArguments().Select(typeArgument =>
                {
                    var formattedTypeArgumentName = FormatHelper.FormatFullTypeName(typeArgument);
                    return SyntaxFactory.ParseTypeName(formattedTypeArgumentName);
                });
                var typeArgumentList = SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(typeArguments));
                newName = SyntaxFactory.GenericName(identifier, typeArgumentList);
            }
            else if (translatedMember is TypeDescription typeDescription3)
            {
                newName = SyntaxFactory.IdentifierName(typeDescription3.Name);
            }
            else if (translatedMember is EventDescription eventDescription2)
            {
                newName = SyntaxFactory.IdentifierName(eventDescription2.Name);
            }
            else if (translatedMember is FieldDescription fieldDescription2)
            {
                newName = SyntaxFactory.IdentifierName(fieldDescription2.Name);
            }
            else if (translatedMember is MethodDescription methodDescription2 && methodDescription2.IsGenericMethod)
            {
                var identifier = SyntaxFactory.Identifier(methodDescription2.Name);
                var typeArguments = methodDescription2.GetGenericArguments().Select(typeArgument =>
                {
                    var formattedTypeArgumentName = FormatHelper.FormatFullTypeName(typeArgument);
                    return SyntaxFactory.ParseTypeName(formattedTypeArgumentName);
                });
                var typeArgumentList = SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(typeArguments));
                newName = SyntaxFactory.GenericName(identifier, typeArgumentList);
            }
            else if (translatedMember is MethodDescription methodDescription3)
            {
                newName = SyntaxFactory.IdentifierName(methodDescription3.Name);
            }
            else if (translatedMember is PropertyDescription propertyDescription2)
            {
                newName = SyntaxFactory.IdentifierName(propertyDescription2.Name);
            }
            else throw new NotImplementedException("originalMember is not a TypeDescription, EventDescription, FieldDescription, MethodDescription or PropertyDescription instance");

            // Potentially rewritting expression
            ExpressionSyntax newExpression = newExpression = (ExpressionSyntax)Visit(node.Expression);

            return node.WithName(newName).WithExpression(newExpression);
        }

        public override SyntaxNode VisitTypeOfExpression(TypeOfExpressionSyntax node)
        {
            var originalType = _resolver.GetTypeDescription(node);
            var translatedType = _structureService.TranslateType(originalType);

            if (originalType == translatedType)
                return base.VisitTypeOfExpression(node);

            _structureService.VerifyType(originalType);

            var formattedTranslatedType = FormatHelper.FormatFullTypeName(translatedType);
            var newType = SyntaxFactory.ParseTypeName(formattedTranslatedType);

            return node.WithType(newType);
        }

        public override SyntaxNode VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            var originalType = _resolver.GetTypeDescription(node);
            var translatedType = _structureService.TranslateType(originalType);

            if (originalType == translatedType)
                return base.VisitVariableDeclaration(node);

            _structureService.VerifyType(originalType);

            // Potentially rewritting type
            var formattedTranslatedType = FormatHelper.FormatFullTypeName(translatedType);
            var newType = SyntaxFactory.ParseTypeName(formattedTranslatedType);

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