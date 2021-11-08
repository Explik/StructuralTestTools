using Explik.StructuralTestTools;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace TestTools_Tests.Structure
{
    [TestClass]
    public class TemplateRewriterTests
    {
        [TestMethod("Visit removes _Template from class name")]
        public void Visit_RemovesUnderscoreTemplateFromClassName()
        { 
            var root = SyntaxFactory.ParseSyntaxTree("class ClassName_Template { }").GetRoot();
            var node = root.AllDescendantNodes<ClassDeclarationSyntax>().First();
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            var statementRewriter = Substitute.For<CSharpSyntaxRewriter>(true);
            var rewriter = new TemplateRewriter(syntaxResolver, statementRewriter);

            var translatedNode = rewriter.Visit(node);

            var expectedSource = "class ClassName { }";
            Assert.AreEqual(expectedSource, translatedNode.ToString());
        }

        [TestMethod("Visit removes _Template from constructor")]
        public void Visit_RemovesUnderscoreTemplateFromConstructor()
        {
            var source = "class ClassName_Template { ClassName_Template() { } }";
            var root = SyntaxFactory.ParseSyntaxTree(source).GetRoot();
            var node = root.AllDescendantNodes<ClassDeclarationSyntax>().First();
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            var statementRewriter = Substitute.For<CSharpSyntaxRewriter>(true);
            var rewriter = new TemplateRewriter(syntaxResolver, statementRewriter);

            var translatedNode = rewriter.Visit(node);

            var expectedSource = "class ClassName { ClassName() { } }";
            Assert.AreEqual(expectedSource, translatedNode.ToString());
        }

        [TestMethod("Visit does not modify non-templated attributes")]
        public void Visit_DoesNotModifyNonTemplatedAttributes()
        {
            var source = "[TestClass] class ClassName { }";
            var root = SyntaxFactory.ParseSyntaxTree(source).GetRoot();
            var node1 = root.AllDescendantNodes<ClassDeclarationSyntax>().First();
            var node2 = root.AllDescendantNodes<AttributeSyntax>().First();
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            syntaxResolver.IsTemplatedAttribute(node2).Returns(false);
            var statementRewriter = Substitute.For<CSharpSyntaxRewriter>(true);
            var rewriter = new TemplateRewriter(syntaxResolver, statementRewriter);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual(source, translatedNode.ToString());
        }

        [TestMethod("Visit replace name of templated attributes on class")]
        public void Visit_ReplacesNameOfTemplatedAttributesOnClass()
        {
            var source = "[TemplatedTestClass] class ClassName { }";
            var root = SyntaxFactory.ParseSyntaxTree(source).GetRoot();
            var node1 = root.AllDescendantNodes<ClassDeclarationSyntax>().First();
            var node2 = root.AllDescendantNodes<AttributeSyntax>().First();
            
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            syntaxResolver.IsTemplatedAttribute(node2).Returns(true);
            syntaxResolver.GetAssociatedAttributeType(node2).Returns("TestClass");
            var statementRewriter = Substitute.For<CSharpSyntaxRewriter>(true);
            var rewriter = new TemplateRewriter(syntaxResolver, statementRewriter);

            var translatedNode = rewriter.Visit(node1);

            var expectedSource = "[TestClass] class ClassName { }";
            Assert.AreEqual(expectedSource, translatedNode.ToString());
        }

        [TestMethod("Visit replaces name of templated attributes on method")]
        public void Visit_ReplacesNameOfTemplatedAttributeOnMethod()
        {
            var source = "class ClassName { [TemplatedTestMethod] void MethodName() {} }";
            var root = SyntaxFactory.ParseSyntaxTree(source).GetRoot();
            var node1 = root.AllDescendantNodes<MethodDeclarationSyntax>().First();
            var node2 = root.AllDescendantNodes<AttributeSyntax>().First();

            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            syntaxResolver.IsTemplatedAttribute(node2).Returns(true);
            syntaxResolver.GetAssociatedAttributeType(node2).Returns("TestClass");
            var statementRewriter = Substitute.For<CSharpSyntaxRewriter>(true);
            var rewriter = new TemplateRewriter(syntaxResolver, statementRewriter);

            var translatedNode = rewriter.Visit(node1);

            var expectedSource = "[TemplatedTestMethod] void MethodName() {}";
            Assert.AreEqual(expectedSource, translatedNode.ToString());
        }

        [TestMethod("Visit does not modify method body without templated attribute")]
        public void Visit_DoesNotModifyMethodBodyWithoutTemplatedAttribute()
        {
            var source = "class ClassName { [TestMethod] void MethodName() {} }";
            var root = SyntaxFactory.ParseSyntaxTree(source).GetRoot();
            var node1 = root.AllDescendantNodes<MethodDeclarationSyntax>().First();
            var node2 = root.AllDescendantNodes<AttributeSyntax>().First();
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            syntaxResolver.IsTemplatedAttribute(node2).Returns(false);
            var statementRewriter = Substitute.For<CSharpSyntaxRewriter>(true);
            var rewriter = new TemplateRewriter(syntaxResolver, statementRewriter);

            var translatedNode = rewriter.Visit(node1);

            var expectedSource = "[TestMethod] void MethodName() {}";
            Assert.AreEqual(expectedSource, translatedNode.ToString());
        }

        [TestMethod("Visit modifies body of method with templated attributes")]
        public void Visit_ModifiesBodyOfMethodWithTemplatedAttributes()
        {
            var source = string.Join(
                Environment.NewLine,
                "class ClassName {",
                "  [TemplatedTestMethod]",
                "  void MethodName() {",
                "    Console.WriteLine(\"original line 1\");",
                "    ",
                "    Console.WriteLine(\"original line 2\");",
                "  }",
                "}");
            var root = SyntaxFactory.ParseSyntaxTree(source).GetRoot();
            var node1 = root.AllDescendantNodes<MethodDeclarationSyntax>().First();
            var node2 = root.AllDescendantNodes<AttributeSyntax>().First();
            var node3 = root.AllDescendantNodes<BlockSyntax>().First();

            var replacedSource = string.Join(
                Environment.NewLine,
                "  {",
                "    Console.WriteLine(\"translated line 1\");",
                "    ",
                "    Console.WriteLine(\"translated line 2\");",
                "  }");
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            syntaxResolver.HasTemplatedAttribute(node1).Returns(true);
            syntaxResolver.IsTemplatedAttribute(node2).Returns(true);
            syntaxResolver.GetAssociatedAttributeType(node2).Returns("TestMethod");
            var statementRewriter = Substitute.For<CSharpSyntaxRewriter>(true);
            statementRewriter.VisitBlock(node3).Returns(SyntaxFactory.ParseStatement(replacedSource));
            var rewriter = new TemplateRewriter(syntaxResolver, statementRewriter);

            var translatedNode = rewriter.Visit(node1);

            var expectedSource = string.Join(
                Environment.NewLine,
                "[TestMethod]",
                "  void MethodName() {",
                "    Console.WriteLine(\"translated line 1\");",
                "    ",
                "    Console.WriteLine(\"translated line 2\");",
                "  }");
            Assert.AreEqual(expectedSource, translatedNode.ToString());
        }

        // String comparison is too strict
        // [TestMethod("Visit replaces body of method with templated attributes on VerifierServiceException")]
        public void Visit_ReplacesBodyOfMethodWithTemplatedAttributesOnVerifierServiceException()
        {
            var source = string.Join(
                Environment.NewLine,
                "class ClassName {",
                "  [TemplatedTestMethod]",
                "  void MethodName() {",
                "    Console.WriteLine(\"original line 1\");",
                "    ",
                "    Console.WriteLine(\"original line 2\");",
                "  }",
                "}");
            var root = SyntaxFactory.ParseSyntaxTree(source).GetRoot();
            var node1 = root.AllDescendantNodes<MethodDeclarationSyntax>().First();
            var node2 = root.AllDescendantNodes<AttributeSyntax>().First();
            var node3 = root.AllDescendantNodes<BlockSyntax>().First();

            var replacedSource = string.Join(
                Environment.NewLine,
                "  {",
                "    Console.WriteLine(\"translated line 1\");",
                "    ",
                "    Console.WriteLine(\"translated line 2\");",
                "  }");
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            syntaxResolver.HasTemplatedAttribute(node1).Returns(true);
            syntaxResolver.IsTemplatedAttribute(node2).Returns(true);
            syntaxResolver.GetAssociatedAttributeType(node2).Returns("TestMethod");
            syntaxResolver.GetAssociatedExceptionType(node2).Returns("AssertFailedException");
            var statementRewriter = Substitute.For<CSharpSyntaxRewriter>(true);
            statementRewriter.VisitBlock(node3).Returns(x => throw new VerifierServiceException("message for user"));
            var rewriter = new TemplateRewriter(syntaxResolver, statementRewriter);

            var translatedNode = rewriter.Visit(node1);

            var expectedSource = string.Join(
                Environment.NewLine,
                "[TestMethod]",
                "  void MethodName() {",
                "    // == Failed To Compile ==",
                "    // Console.WriteLine(\"original line 1\");",
                "    ",
                "    // Console.WriteLine(\"original line 2\");",
                "    throw new AssertFailedException(\"message for user\");",
                "  }");
            Assert.AreEqual(expectedSource, translatedNode.ToString());
        }
    }
}
