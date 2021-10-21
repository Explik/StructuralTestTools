using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public interface ICompileTimeDescriptionResolver
    {
        ConstructorDescription GetConstructorDescription(ObjectCreationExpressionSyntax node);

        ConstructorDescription GetConstructorDescription(ConstructorDeclarationSyntax node);

        MemberDescription GetMemberDescription(MemberAccessExpressionSyntax node);

        MethodDescription GetMethodDescription(InvocationExpressionSyntax node);

        MethodDescription GetMethodDescription(MethodDeclarationSyntax node);

        bool IsTemplatedAttribute(AttributeSyntax node);

        bool HasTemplatedAttribute(ClassDeclarationSyntax node);

        bool HasTemplatedAttribute(MethodDeclarationSyntax node);

        string GetAssociatedAttributeType(AttributeSyntax node);

        string GetAssociatedExceptionType(AttributeSyntax node);

        TypeDescription GetTypeDescription(TypeDeclarationSyntax node);

        TypeDescription GetTypeDescription(VariableDeclarationSyntax node);

        TypeDescription GetTypeDescription(AttributeSyntax node);
    }
}
