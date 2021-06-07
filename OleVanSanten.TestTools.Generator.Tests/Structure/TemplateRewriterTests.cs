using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OleVanSanten.TestTools.Structure;
using OleVanSanten.TestTools.TypeSystem;

namespace TestTools_Tests.Structure
{
    [TestClass]
    public class TemplateRewriterTests
    {
        [TestMethod("Visit removes \"_Template\" from class name")]
        public void Visit_RemovesTemplateFromClassName()
        {
            // Setting up services to handle semantic data
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            var attribute = new AttributeEquivalentAttribute("Attribute");
            syntaxResolver.GetTemplatedAttribute((ClassDeclarationSyntax)null).ReturnsForAnyArgs(attribute);

            // Setting up and testing rewriter ouput
            var rewriter = new TemplateRewriter(syntaxResolver, null);
            var input = Parse<ClassDeclarationSyntax>("class ClassName_Template{}");
            var output = rewriter.Visit(input);

            Assert.AreEqual("class ClassName{}", output.ToFullString());
        }

        [TestMethod("Visit removes \"_Template\" from constructor name")]
        public void Visit_RemovesTemplateFromConstructorName()
        {
            // Setting up services to handle semantic data
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            var attribute = new AttributeEquivalentAttribute("Attribute");
            syntaxResolver.GetTemplatedAttribute((ClassDeclarationSyntax)null).ReturnsForAnyArgs(attribute);

            // Setting up and testing rewriter ouput
            var rewriter = new TemplateRewriter(syntaxResolver, null);
            var input = Parse<ConstructorDeclarationSyntax>("class ClassName_Template{ClassName_Template(){}}");
            var output = rewriter.Visit(input);

            Assert.AreEqual("ClassName(){}", output.ToFullString());
        }

        [TestMethod("Visit transforms templated attribute to equivalent attribute")]
        public void Visit_TransformsTemplatedAttributeToEquivalentAttribute()
        {
            // Setting up services to handle semantic data
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            var attribute = new AttributeEquivalentAttribute("Attribute");
            syntaxResolver.GetTemplatedAttribute((AttributeSyntax)null).ReturnsForAnyArgs(attribute);

            // Setting up and testing rewriter ouput
            var rewriter = new TemplateRewriter(syntaxResolver, null);
            var input = Parse<AttributeListSyntax>("[TemplatedAttribute]");
            var output = rewriter.Visit(input);

            Assert.AreEqual("[Attribute]", output.ToFullString());
        }

        [TestMethod("Visit transforms body of templated method with StatementRewriter")]
        public void Visit_TransformsBodyOfTemplatedMethod()
        {
            // Setting up services to handle semantic data
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            var attribute = new AttributeEquivalentAttribute("Attribute");
            syntaxResolver.GetTemplatedAttribute((MethodDeclarationSyntax)null).ReturnsForAnyArgs(attribute);
            
            var statementRewriter = Substitute.For<CSharpSyntaxRewriter>(new object[] { true });
            statementRewriter.Visit(null).ReturnsForAnyArgs(x => (SyntaxNode)x[0]);

            // Setting up and testing rewriter ouput
            var rewriter = new TemplateRewriter(syntaxResolver, statementRewriter);
            var input = Parse<MethodDeclarationSyntax>("class ClassName { void TemplatedMethod(){ Console.WriteLine(); } }");
            var output = rewriter.Visit(input);

            statementRewriter.ReceivedWithAnyArgs().Visit(null);
        }

        [TestMethod("Visit does not transforms body of non-templated method")]
        public void Visit_DoesNotTransformsBodyOfNonTemplatedMethod()
        {
            // Setting up services to handle semantic data
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            syntaxResolver.GetTemplatedAttribute((MethodDeclarationSyntax)null).ReturnsForAnyArgs((AttributeEquivalentAttribute)null);

            var statementRewriter = Substitute.For<CSharpSyntaxRewriter>(new object[] { true });
            statementRewriter.Visit(null).ReturnsForAnyArgs(x => (SyntaxNode)x[0]);

            // Setting up and testing rewriter ouput
            var rewriter = new TemplateRewriter(syntaxResolver, statementRewriter);
            var input = Parse<MethodDeclarationSyntax>("class ClassName { void NonTemplatedMethod(){ Console.WriteLine(); } }");
            var output = rewriter.Visit(input);

            statementRewriter.DidNotReceiveWithAnyArgs().Visit(null);
        }

        private T Parse<T>(string source) where T : SyntaxNode
        {
            var root = SyntaxFactory.ParseSyntaxTree(source).GetRoot();
            return (T)root.DescendantNodes().First(n => n is T);
        }
    }
}
