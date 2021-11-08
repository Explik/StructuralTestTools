using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Explik.StructuralTestTools;
using Microsoft.CodeAnalysis.Text;
using System.IO;

namespace Explik.StructuralTestTools
{
    // TemplateRewriter translates and verifies templated classes
    public class TemplateRewriter : CSharpSyntaxRewriter
    {
        private ICompileTimeDescriptionResolver _resolver;
        private CSharpSyntaxRewriter _statementRewriter;

        public TemplateRewriter(ICompileTimeDescriptionResolver syntaxResolver, CSharpSyntaxRewriter statementRewriter)
        {
            _resolver = syntaxResolver;
            _statementRewriter = statementRewriter;
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            // Potentially rewritting the class name by removing _Templated from it
            var newClassName = node.Identifier.Text.Replace("_Template", "");
            var newIdentifier = SyntaxFactory.IdentifierName(newClassName).Identifier;
            var newIdentifierWithTrivia = newIdentifier.WithTriviaFrom(node.Identifier);

            // Potentially rewritting the class members
            var newMembers = new SyntaxList<SyntaxNode>(node.Members.Select(Visit));

            // Potentially rewritting attributes
            var newAttributeLists = new SyntaxList<AttributeListSyntax>(node.AttributeLists.Select(Visit).OfType<AttributeListSyntax>());

            return node.WithIdentifier(newIdentifierWithTrivia).WithMembers(newMembers).WithAttributeLists(newAttributeLists);
        }

        public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            // Potentially rewritting the class name by removing _Templated from it
            var newClassName = node.Identifier.Text.Replace("_Template", "");
            var newIdentifier = SyntaxFactory.IdentifierName(newClassName).Identifier;
            var newIdentifierWithTrivia = newIdentifier.WithTriviaFrom(node.Identifier);

            return node.WithIdentifier(newIdentifierWithTrivia);
        }

        public override SyntaxNode VisitAttribute(AttributeSyntax node)
        {
            if (!_resolver.IsTemplatedAttribute(node))
                return node;

            // Rewritting templated-attribute type to non-templated-attribute type
            var attributeTypeName = _resolver.GetAssociatedAttributeType(node);
            var newName = SyntaxFactory.IdentifierName(attributeTypeName);
            var newNameWithTrivia = newName.WithTriviaFrom(node);

            return node.WithName(newNameWithTrivia);
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            // Only methods marked with an attritubes that are marked with TemplatedAttribute should be rewritten
            if (!_resolver.HasTemplatedAttribute(node))
                return node;

            // Rewritting the method body to switch out all FromNamespace members with ToNamespace members
            // If the rewrite is unsuccessful due to validation errors, the entire method body is replaced
            // with an exception 
            // and if the rewrite validation fails replacing the entire body with an exception
            var newAttributeLists = new SyntaxList<AttributeListSyntax>(node.AttributeLists.Select(Visit).OfType<AttributeListSyntax>());

            BlockSyntax newBody;
            try
            {
                newBody = (BlockSyntax)_statementRewriter.VisitBlock(node.Body).WithTriviaFrom(node.Body);
            }
            catch (VerifierServiceException ex)
            {
                var newBodySource = string.Join(
                    "",
                    node.Body.OpenBraceToken.ToFullString(),
                    CreateComment("// == Failed To Compile == ", node.Body.Statements.FirstOrDefault()),
                    CreateCommentedBody(node).TrimEnd(),
                    CreateThrowStatement(node, ex, node.Body.Statements.LastOrDefault()),
                    node.Body.CloseBraceToken.ToFullString());

                newBody = (BlockSyntax)SyntaxFactory.ParseStatement(newBodySource);
            }
            return node.WithBody(newBody).WithAttributeLists(newAttributeLists).WithTriviaFrom(node);
        }

        private string CreateComment(string text, SyntaxNode triviaNode)
        {
            var leadingTrivia = triviaNode?.GetLeadingTrivia().ToString() ?? "";
            var traillingTrivia = triviaNode?.GetTrailingTrivia().First().ToString() ?? "";

            return leadingTrivia + text + traillingTrivia;
        }

        private string CreateCommentedBody(MethodDeclarationSyntax node)
        {
            var builder = new StringBuilder();

            foreach(var statement in node.Body.Statements)
            {
                var leadingTrivia = statement.GetLeadingTrivia().ToString();
                var traillingTrivia = statement.GetTrailingTrivia().First().ToString();
                 
                builder.Append(leadingTrivia + "// " + statement.ToString() + traillingTrivia);
            }
            return builder.ToString();
        }

        private string CreateThrowStatement(MethodDeclarationSyntax node, VerifierServiceException ex, SyntaxNode triviaNode)
        {
            var leadingTrivia = triviaNode?.GetLeadingTrivia().ToString() ?? "";
            var traillingTrivia = triviaNode?.GetTrailingTrivia().First().ToString() ?? "";
            
            if (leadingTrivia.Contains(Environment.NewLine))
                leadingTrivia = Environment.NewLine + leadingTrivia.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Last();

            var templatedAttribute = node.AttributeLists.SelectMany(l => l.Attributes).First(_resolver.IsTemplatedAttribute);
            var exceptionTypeName = _resolver.GetAssociatedExceptionType(templatedAttribute);
            
            return leadingTrivia + $"throw new {exceptionTypeName}(\"{ex.Message}\");" + traillingTrivia;
        }
    }
}
