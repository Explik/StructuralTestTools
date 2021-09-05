using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools
{
    public interface ICompileTimeDescriptionResolver
    {
        ConstructorDescription GetConstructorDescription(ObjectCreationExpressionSyntax node);

        ConstructorDescription GetConstructorDescription(ConstructorDeclarationSyntax node);

        MemberDescription GetMemberDescription(MemberAccessExpressionSyntax node);

        MethodDescription GetMethodDescription(InvocationExpressionSyntax node);

        MethodDescription GetMethodDescription(MethodDeclarationSyntax node);

        EquivalentAttributeAttribute GetEquivalentAttribute(AttributeSyntax node);

        EquivalentAttributeAttribute GetEquivalentAttribute(ClassDeclarationSyntax node);

        EquivalentAttributeAttribute GetEquivalentAttribute(MethodDeclarationSyntax node);

        EquivalentExceptionAttribute GetEquivalentException(MethodDeclarationSyntax node);

        TypeDescription GetTypeDescription(TypeDeclarationSyntax node);

        TypeDescription GetTypeDescription(VariableDeclarationSyntax node);
    }
}
