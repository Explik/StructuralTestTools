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
        private readonly ICompileTimeDescriptionResolver _resolver;
        private readonly IStructureService _structureService;

        private readonly MemberVerificationAspect[] _eventAddAspects = new[]
        {
            MemberVerificationAspect.EventAddAccessLevel,
            MemberVerificationAspect.EventHandlerType,
            MemberVerificationAspect.MemberType
        };
        private readonly MemberVerificationAspect[] _eventRemoveAspects = new[]
        {
            MemberVerificationAspect.EventHandlerType,
            MemberVerificationAspect.EventRemoveAccessLevel,
            MemberVerificationAspect.MemberType
        };
        private readonly MemberVerificationAspect[] _fieldGetAspects = new[]
        {
            MemberVerificationAspect.FieldAccessLevel,
            MemberVerificationAspect.FieldIsStatic,
            MemberVerificationAspect.FieldType,
            MemberVerificationAspect.MemberType
        };
        private readonly MemberVerificationAspect[] _fieldSetAspects = new[]
        {
            MemberVerificationAspect.FieldAccessLevel,
            MemberVerificationAspect.FieldIsStatic,
            MemberVerificationAspect.FieldType,
            MemberVerificationAspect.FieldWriteability,
            MemberVerificationAspect.MemberType
        };
        private readonly MemberVerificationAspect[] _methodAspects = new[]
        {
            MemberVerificationAspect.MemberType,
            MemberVerificationAspect.MethodAccessLevel,
            MemberVerificationAspect.MethodDeclaringType,
            MemberVerificationAspect.MethodIsAbstract,
            MemberVerificationAspect.MethodIsStatic,
            MemberVerificationAspect.MethodIsVirtual,
            MemberVerificationAspect.MethodReturnType
        };
        private readonly MemberVerificationAspect[] _propertyGetAspects = new[]
        {
            MemberVerificationAspect.MemberType,
            MemberVerificationAspect.PropertyIsStatic,
            MemberVerificationAspect.PropertyGetAccessLevel,
            MemberVerificationAspect.PropertyGetDeclaringType,
            MemberVerificationAspect.PropertyGetIsAbstract,
            MemberVerificationAspect.PropertyGetIsVirtual,
            MemberVerificationAspect.PropertyType
        };
        private readonly MemberVerificationAspect[] _propertySetAspects = new[]
        {
            MemberVerificationAspect.MemberType,
            MemberVerificationAspect.PropertyIsStatic,
            MemberVerificationAspect.PropertySetAccessLevel,
            MemberVerificationAspect.PropertySetDeclaringType,
            MemberVerificationAspect.PropertySetIsAbstract,
            MemberVerificationAspect.PropertySetIsVirtual,
            MemberVerificationAspect.PropertyType
        };

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

            // _resolver.GetPropertyDescription(node) returns null on array access. 
            if (originalMember == null) return base.VisitElementAccessExpression(node);

            var originalType = originalMember.DeclaringType;
            var translatedMember = _structureService.TranslateMember(originalMember);

            if (originalMember != translatedMember)
            {
                var isAssignment = IsAssigment(node.Ancestors().First());
                var aspects = !isAssignment ? _propertyGetAspects : _propertySetAspects;

                _structureService.VerifyType(originalType);
                _structureService.VerifyMember(originalMember, aspects);
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
                var isUnsubscription = node.Ancestors().First().IsKind(SyntaxKind.SubtractAssignmentExpression);
                var aspects = !isUnsubscription ? _eventAddAspects : _eventRemoveAspects;

                _structureService.VerifyType(eventDescription1.DeclaringType);
                _structureService.VerifyMember(eventDescription1, aspects);
            }
            else if (originalMember is FieldDescription fieldDescription2)
            {
                var isAssignment = IsAssigment(node.Ancestors().First());
                var aspects = !isAssignment ? _fieldGetAspects : _fieldSetAspects;

                _structureService.VerifyType(fieldDescription2.DeclaringType);
                _structureService.VerifyMember(fieldDescription2, aspects);
            }
            else if (originalMember is MethodDescription methodDescription2)
            {
                _structureService.VerifyType(methodDescription2.DeclaringType);
                _structureService.VerifyMember(methodDescription2, _methodAspects);
            }
            else if (originalMember is PropertyDescription propertyDescription2)
            {
                var isAssignment = IsAssigment(node.Ancestors().First());
                var aspects = !isAssignment ? _propertyGetAspects : _propertySetAspects;

                _structureService.VerifyType(propertyDescription2.DeclaringType);
                _structureService.VerifyMember(propertyDescription2, aspects);
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

        private bool IsAssigment(SyntaxNode node)
        {
            var assignmentKinds = new[]
            {
                SyntaxKind.AddAssignmentExpression,
                SyntaxKind.AndAssignmentExpression,
                SyntaxKind.CoalesceAssignmentExpression,
                SyntaxKind.DivideAssignmentExpression,
                SyntaxKind.ExclusiveOrAssignmentExpression,
                SyntaxKind.LeftShiftAssignmentExpression,
                SyntaxKind.ModuloAssignmentExpression,
                SyntaxKind.MultiplyAssignmentExpression,
                SyntaxKind.OrAssignmentExpression,
                SyntaxKind.RightShiftAssignmentExpression,
                SyntaxKind.SimpleAssignmentExpression,
                SyntaxKind.SubtractAssignmentExpression
            };
            return assignmentKinds.Contains(node.Kind());
        }
    }
}