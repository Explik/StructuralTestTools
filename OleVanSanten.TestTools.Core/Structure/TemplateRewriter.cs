using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OleVanSanten.TestTools;
using Microsoft.CodeAnalysis.Text;
using System.IO;

namespace OleVanSanten.TestTools
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

            // Potentially rewritting the class members
            var newMembers = new SyntaxList<SyntaxNode>(node.Members.Select(Visit));

            // Potentially rewritting attributes
            var newAttributeLists = new SyntaxList<AttributeListSyntax>(node.AttributeLists.Select(Visit).OfType<AttributeListSyntax>());

            return node.WithIdentifier(newIdentifier).WithMembers(newMembers).WithAttributeLists(newAttributeLists);
        }

        public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            // Potentially rewritting the class name by removing _Templated from it
            var newClassName = node.Identifier.Text.Replace("_Template", "");
            var newIdentifier = SyntaxFactory.IdentifierName(newClassName).Identifier;

            return node.WithIdentifier(newIdentifier);
        }

        public override SyntaxNode VisitAttribute(AttributeSyntax node)
        {
            if (!_resolver.IsTemplatedAttribute(node))
                return node;

            // Rewritting templated-attribute type to non-templated-attribute type
            var attributeTypeName = _resolver.GetAssociatedAttributeType(node);
            var newName = SyntaxFactory.IdentifierName(attributeTypeName);

            return node.WithName(newName);
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
            BlockSyntax newBody;
            var newAttributeLists = new SyntaxList<AttributeListSyntax>(node.AttributeLists.Select(Visit).OfType<AttributeListSyntax>());

            try
            {
                var newStatements = new SyntaxList<StatementSyntax>(node.Body.Statements.Select(_statementRewriter.Visit).OfType<StatementSyntax>());
                newBody = SyntaxFactory.Block(newStatements);
            }
            catch (VerifierServiceException ex)
            {
                var newBodySource = string.Join(
                    Environment.NewLine,
                    "{",
                    "// == Failed To Compile ==",
                    CreateComments(node),
                    CreateThrowStatement(node, ex),
                    "}");
                newBody = (BlockSyntax)SyntaxFactory.ParseStatement(newBodySource);
            }
            return node.WithBody(newBody).WithAttributeLists(newAttributeLists);
        }

        private string CreateComments(MethodDeclarationSyntax node)
        {
            return string.Join(Environment.NewLine, node.Body.Statements.Select(CreateComment));
        }

        private string CreateComment(SyntaxNode node)
        {
            var builder = new StringBuilder();
            var source = SourceText.From(node.NormalizeWhitespace().ToFullString(), Encoding.UTF8).ToString();
            
            using(StringReader sr = new StringReader(source))
            {
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    builder.Append("// " + line + Environment.NewLine);
                }
            }
            return builder.ToString();
        }

        private string CreateThrowStatement(MethodDeclarationSyntax node, VerifierServiceException ex)
        {
            var templatedAttribute = node.AttributeLists.SelectMany(l => l.Attributes).First(_resolver.IsTemplatedAttribute);
            var exceptionTypeName = _resolver.GetAssociatedExceptionType(templatedAttribute);
            var throwStatement = $"throw new {exceptionTypeName}(\"{ex.Message}\");";
            return throwStatement;
        }
    }
}
