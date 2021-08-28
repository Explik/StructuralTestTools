using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.Structure;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public interface ICompileTimeDescriptionResolver
    {
        ConstructorDescription GetConstructorDescription(ObjectCreationExpressionSyntax node);

        ConstructorDescription GetConstructorDescription(ConstructorDeclarationSyntax node);

        MemberDescription GetMemberDescription(MemberAccessExpressionSyntax node);

        MethodDescription GetMethodDescription(InvocationExpressionSyntax node);

        MethodDescription GetMethodDescription(MethodDeclarationSyntax node);

        AttributeEquivalentAttribute GetTemplatedAttribute(AttributeSyntax node);

        AttributeEquivalentAttribute GetTemplatedAttribute(ClassDeclarationSyntax node);

        AttributeEquivalentAttribute GetTemplatedAttribute(MethodDeclarationSyntax node);

        TypeDescription GetTypeDescription(TypeDeclarationSyntax node);

        TypeDescription GetTypeDescription(VariableDeclarationSyntax node);
    }
}
