using Explik.StructuralTestTools;
using Explik.StructuralTestTools.TypeSystem;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using static Explik.StructuralTestTools.RuntimeTestTypes;

namespace TestTools_Tests.Structure
{
    [TestClass]
    public class TypeRewritterTests
    {
        private ref MemberVerificationAspect[] CreateEquvilantArg(MemberVerificationAspect[] expected)
        {
            var orderedExpected = expected.OrderBy(a => a.ToString());

            return ref Arg.Is<MemberVerificationAspect[]>(actual =>
                actual.OrderBy(a => a.ToString()).SequenceEqual(orderedExpected)
            );
        }

        #region VisitArrayType

        [TestMethod("Visit does not rewrite type of non-translatable array type")]
        public void Visit_DoesNotRewriteTypeOfNonTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.ConstantClass[]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.ConstantClass[]", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite array type of non-translatable array type")]
        public void Visit_DoesNotRewriteArrayTypeOfNonTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.ConstantClass[][]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantArrayType).Returns(ConstantArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.ConstantClass[][]", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite nested type of non-translatable array type")]
        public void Visit_DoesNotRewriteNestedTypeOfNonTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.ConstantClass.NestedClass[]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNestedType).Returns(ConstantNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.ConstantClass.NestedClass[]", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite nullable type of non-translatable array type")]
        public void Visit_DoesNotRewriteNullableTypeOfNonTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.ConstantStruct? []").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNullableType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNullableType).Returns(ConstantNullableType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.ConstantStruct? []", translatedNode.ToSource());
        }

        [TestMethod("Visit does not verify non-translatable array type")]
        public void Visit_DoesNotVerifyNonTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.ConstantClass[]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            structureService.DidNotReceive().VerifyType(ConstantType);
        }


        [TestMethod("Visit rewrites type of translatable array type")]
        public void Visit_RewritesTypeOfTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass[]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.TranslatedClass[]", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites type of translatable array type with length")]
        public void Visit_RewritesTypeOfTranslatableArrayTypeWithLength()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass[0]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.TranslatedClass[0]", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites array type of translatable array type")]
        public void Visit_RewritesArrayTypeOfTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass[][]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalArrayType).Returns(TranslatedArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.TranslatedClass[][]", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites array type of translatable array type with lengths")]
        public void Visit_RewritesArrayTypeOfTranslatableArrayTypeWithLengths()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass[0][1]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalArrayType).Returns(TranslatedArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.TranslatedClass[0][1]", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites array array type of translatable array type")]
        public void Visit_RewritesArrayArrayTypeOfTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass[][][]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalArrayArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalArrayArrayType).Returns(TranslatedArrayArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.TranslatedClass[][][]", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites array array type of translatable array type with lengths")]
        public void Visit_RewritesArrayArrayTypeOfTranslatableArrayTypeWithLengths()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass[0][1][2]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalArrayType).Returns(TranslatedArrayArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.TranslatedClass[0][1][2]", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites nested type of translatable array type")]
        public void Visit_RewritesNestedTypeOfTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass.NestedClass[]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNestedType).Returns(TranslatedNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.TranslatedClass.NestedClass[]", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites nullable type of translatable array type")]
        public void Visit_RewritesNullableTypeOfTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalStruct? []").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNullableType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNullableType).Returns(TranslatedNullableType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.TranslatedStruct? []", translatedNode.ToSource());
        }

        [TestMethod("Visit verifies translatable array type")]
        public void Visit_VerifiesTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass[]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            structureService.Received().VerifyType(OriginalType);
        }

        #endregion

        #region VisitCastExpression

        [TestMethod("Visit does not rewrite type of non-translatable cast")]
        public void Visit_DoesNotRewriteTypeOfNonTranslatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.ConstantClass)null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("(TestTypes.ConstantClass)null", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrites nested type of non-translatable cast")]
        public void Visit_DoesNotRewriteNestedTypeOfNonTranslatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.ConstantClass[])null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantArrayType).Returns(ConstantArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("(TestTypes.ConstantClass[])null", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite array type of non-translatable cast")]
        public void Visit_DoesNotRewriteArrayTypeOfNonTranslatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.ConstantClass.NestedClass)null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNestedType).Returns(ConstantNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("(TestTypes.ConstantClass.NestedClass)null", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite nullable type of non-translatable cast")]
        public void Visit_DoesNotRewriteNullableTypeOfNonTranslatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.ConstantStruct?)null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNullableType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNullableType).Returns(ConstantNullableType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("(TestTypes.ConstantStruct? )null", translatedNode.ToSource());
        }

        [TestMethod("Visit does not verify non-translatable cast")]
        public void Visit_DoesNotVerifyNonTranslatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.ConstantClass)null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            structureService.DidNotReceive().VerifyType(ConstantType);
        }


        [TestMethod("Visit rewrites type of translatable cast")]
        public void Visit_RewritesTypeOfTranslatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.OriginalClass)null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("(TestTypes.TranslatedClass)null", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites array type of translatable cast")]
        public void Visit_RewritesArrayTypeOfTranlatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.OriginalClass[])null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalArrayType).Returns(TranslatedArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("(TestTypes.TranslatedClass[])null", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites nested type of translatable cast")]
        public void Visit_RewritesNestedTypeOfTranslatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.OriginalClass.NestedClass)null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNestedType).Returns(TranslatedNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("(TestTypes.TranslatedClass.NestedClass)null", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites nullable type of translatable cast")]
        public void VisitRewritesNullableTypeOfTranslatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.OriginalStruct?)null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNullableType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNullableType).Returns(TranslatedNullableType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("(TestTypes.TranslatedStruct? )null", translatedNode.ToSource());
        }

        [TestMethod("Visit verifies translatable cast")]
        public void Visit_VerifiesTranslatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.OriginalClass)null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            structureService.Received().VerifyType(OriginalType);
        }

        #endregion

        #region VisitDefaultExpression

        [TestMethod("Visit does not rewrite type of non-translatable default")]
        public void Visit_DoesNotRewriteTypeOfNonTranslatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.ConstantClass)").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("default(TestTypes.ConstantClass)", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrites nested type of non-translatable default")]
        public void Visit_DoesNotRewriteNestedTypeOfNonTranslatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.ConstantClass[])").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantArrayType).Returns(ConstantArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("default(TestTypes.ConstantClass[])", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite array type of non-translatable default")]
        public void Visit_DoesNotRewriteArrayTypeOfNonTranslatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.ConstantStruct?)").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNullableType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNullableType).Returns(ConstantNullableType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("default(TestTypes.ConstantStruct? )", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite nullable type of non-translatable default")]
        public void Visit_DoesNotRewriteNullableTypeOfNonTranslatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.ConstantClass.NestedClass)").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNestedType).Returns(ConstantNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("default(TestTypes.ConstantClass.NestedClass)", translatedNode.ToSource());
        }

        [TestMethod("Visit does not verify non-translatable default")]
        public void Visit_DoesNotVerifyNonTranslatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.ConstantClass)").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            structureService.DidNotReceive().VerifyType(ConstantType);
        }


        [TestMethod("Visit rewrites type of translatable default")]
        public void Visit_RewritesTypeOfTranslatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.OriginalClass)").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("default(TestTypes.TranslatedClass)", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites array type of translatable default")]
        public void Visit_RewritesArrayTypeOfTranlatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.OriginalClass[])").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalArrayType).Returns(TranslatedArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("default(TestTypes.TranslatedClass[])", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites nested type of translatable default")]
        public void Visit_RewritesNestedTypeOfTranslatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.OriginalClass.NestedClass)").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNestedType).Returns(TranslatedNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("default(TestTypes.TranslatedClass.NestedClass)", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites nullable type of translatable default")]
        public void VisitRewritesNullableTypeOfTranslatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.OriginalStruct?)").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNullableType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNullableType).Returns(TranslatedNullableType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("default(TestTypes.TranslatedStruct? )", translatedNode.ToSource());
        }

        [TestMethod("Visit verifies translatable default")]
        public void Visit_VerifiesTranslatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.OriginalClass)").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            structureService.Received().VerifyType(OriginalType);
        }

        #endregion

        #region VisitElementAccessExpressionSyntax

        [TestMethod("Visit does not translate array access")]
        public void Visit_DoesNotTranslateArrayAccess()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(new int[] { 1, 2, 3 })[0]").GetRoot();
            var node1 = root.AllDescendantNodes<ElementAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetPropertyDescription(node1).Returns(null as PropertyDescription);
            var structureService = Substitute.For<IStructureService>();
            
            var rewriter = new TypeRewriter(resolver, structureService);
            rewriter.VisitElementAccessExpression(node1);

            structureService.DidNotReceiveWithAnyArgs().TranslateMember(null);
        }

        [TestMethod("Visit does not verify non-translatable indexer")]
        public void Visit_DoesNotVerifyNonTranslatableIndexer()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass[new TestTypes.OriginalArgumentClass()]").GetRoot();
            var node1 = root.AllDescendantNodes<ElementAccessExpressionSyntax>().First();
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();
            var node3 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetPropertyDescription(node1).Returns(ConstantIndexer);
            resolver.GetDescription(node2).Returns(ConstantType);
            resolver.GetConstructorDescription(node3).Returns(OriginalArgumentConstructor);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            structureService.TranslateType(OriginalArgumentType).Returns(TranslatedArgumentType);
            structureService.TranslateMember(ConstantIndexer).Returns(ConstantIndexer);
            
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitElementAccessExpression(node1);

            structureService.DidNotReceive().VerifyType(ConstantType);
            structureService.DidNotReceive().VerifyMember(ConstantIndexer);
        }

        [TestMethod("Visit verifies translatable indexer")]
        public void Visit_VerifiesTranslatableIndexer()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass[new TestTypes.OriginalArgumentClass()]").GetRoot();
            var node1 = root.AllDescendantNodes<ElementAccessExpressionSyntax>().First();
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();
            var node3 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetPropertyDescription(node1).Returns(OriginalIndexer);
            resolver.GetDescription(node2).Returns(OriginalType);
            resolver.GetConstructorDescription(node3).Returns(OriginalArgumentConstructor);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateType(OriginalArgumentType).Returns(TranslatedArgumentType);
            structureService.TranslateMember(OriginalIndexer).Returns(TranslatedIndexer);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitElementAccessExpression(node1);

            var expectedAspects = new[]
            {
                MemberVerificationAspect.MemberType,
                MemberVerificationAspect.PropertyIsStatic,
                MemberVerificationAspect.PropertyGetAccessLevel,
                MemberVerificationAspect.PropertyGetDeclaringType,
                MemberVerificationAspect.PropertyGetIsAbstract,
                MemberVerificationAspect.PropertyGetIsVirtual,
                MemberVerificationAspect.PropertyType
            };
            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalIndexer,
                CreateEquvilantArg(expectedAspects));
        }

        [TestMethod("Visit verifies translatable indexer assignment")]
        public void Visit_VerifiesTranslatableIndexerAssignment()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass[new TestTypes.OriginalArgumentClass()] = null").GetRoot();
            var node1 = root.AllDescendantNodes<ElementAccessExpressionSyntax>().First();
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();
            var node3 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetPropertyDescription(node1).Returns(OriginalIndexer);
            resolver.GetDescription(node2).Returns(OriginalType);
            resolver.GetConstructorDescription(node3).Returns(OriginalArgumentConstructor);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateType(OriginalArgumentType).Returns(TranslatedArgumentType);
            structureService.TranslateMember(OriginalIndexer).Returns(TranslatedIndexer);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitElementAccessExpression(node1);

            var expectedAspects = new[]
            {
                MemberVerificationAspect.MemberType,
                MemberVerificationAspect.PropertyIsStatic,
                MemberVerificationAspect.PropertySetAccessLevel,
                MemberVerificationAspect.PropertySetDeclaringType,
                MemberVerificationAspect.PropertySetIsAbstract,
                MemberVerificationAspect.PropertySetIsVirtual,
                MemberVerificationAspect.PropertyType,
            };
            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalIndexer,
                CreateEquvilantArg(expectedAspects));
        }

        #endregion

        #region VisitMemberAccessExpression

        [TestMethod("Visit rewrites expression of non-translatable event")]
        public void VisitRewritesExpressionOfNonTranslatableEvent()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(new ConstantClass()).Event").GetRoot();
            var node1 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node1).Returns(ConstantDefaultConstructor);
            resolver.GetDescription(node2).Returns(ConstantEvent);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            structureService.TranslateMember(ConstantEvent).Returns(ConstantEvent);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node2);

            Assert.AreEqual("(new ConstantClass()).Event", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites expression of non-translatable field")]
        public void Visit_RewritesExpressionOfNonTranslatableField()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(new ConstantClass()).Field").GetRoot();
            var node1 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node1).Returns(ConstantDefaultConstructor);
            resolver.GetDescription(node2).Returns(ConstantField);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            structureService.TranslateMember(ConstantField).Returns(ConstantField);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node2);

            Assert.AreEqual("(new ConstantClass()).Field", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites expression of non-translatable method")]
        public void Visit_RewritesExpressionOfNonTranslatableMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(new ConstantClass()).Method").GetRoot();
            var node1 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node1).Returns(ConstantDefaultConstructor);
            resolver.GetDescription(node2).Returns(ConstantMethod);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            structureService.TranslateMember(ConstantMethod).Returns(ConstantMethod);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node2);

            Assert.AreEqual("(new ConstantClass()).Method", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites expression of non-translatable property")]
        public void Visit_RewritesExpressionOfNonTranslatableProperty()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(new ConstantClass()).Property").GetRoot();
            var node1 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node1).Returns(ConstantDefaultConstructor);
            resolver.GetDescription(node2).Returns(ConstantProperty);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            structureService.TranslateMember(ConstantProperty).Returns(ConstantProperty);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node2);

            Assert.AreEqual("(new ConstantClass()).Property", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite name of non-translatable event")]
        public void Visit_DoesNotRewriteNameOfNonTranslatableEvent()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Event").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(ConstantEvent);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(ConstantEvent).Returns(ConstantEvent);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("instance.Event", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite name of non-translatable static event")]
        public void Visit_DoesNotRewriteNameOfNonTranslatableStaticEvent()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass.StaticEvent").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(ConstantStaticEvent);
            resolver.GetDescription(node2).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            structureService.TranslateMember(ConstantStaticEvent).Returns(ConstantStaticEvent);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("TestTypes.ConstantClass.StaticEvent", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite name of non-translatable static event in generic class")]
        public void Visit_DoesNotRewriteNameOfNonTranslatableStaticEventInGenericClass()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantGenericClass<TestTypes.ConstantTypeArgumentClass>.StaticEvent").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(ConstantStaticEventInGenericType);
            resolver.GetDescription(node2).Returns(ConstantGenericTypeWithConstantTypeArgument);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantGenericTypeWithConstantTypeArgument).Returns(ConstantGenericTypeWithConstantTypeArgument);
            structureService.TranslateMember(ConstantStaticEventInGenericType).Returns(ConstantStaticEventInGenericType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("TestTypes.ConstantGenericClass<TestTypes.ConstantTypeArgumentClass>.StaticEvent", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite name of non-translatable field")]
        public void Visit_DoesNotRewriteNameOfNonTranslatableField()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Field").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(ConstantField);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(ConstantField).Returns(ConstantField);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("instance.Field", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite name of non-translatable static field")]
        public void Visit_DoesNotRewriteNameOfNonTranslatableStaticField()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass.StaticField").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(ConstantStaticField);
            resolver.GetDescription(node2).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            structureService.TranslateMember(ConstantStaticField).Returns(ConstantStaticField);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("TestTypes.ConstantClass.StaticField", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite name of non-translatable static field in generic class")]
        public void Visit_DoesNotRewriteNameOfNonTranslatableStaticFieldInGenericClass()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantGenericClass<TestTypes.ConstantTypeArgumentClass>.StaticField").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(ConstantStaticFieldInGenericType);
            resolver.GetDescription(node2).Returns(ConstantGenericTypeWithConstantTypeArgument);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantGenericTypeWithConstantTypeArgument).Returns(ConstantGenericTypeWithConstantTypeArgument);
            structureService.TranslateMember(ConstantStaticFieldInGenericType).Returns(ConstantStaticFieldInGenericType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("TestTypes.ConstantGenericClass<TestTypes.ConstantTypeArgumentClass>.StaticField", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite name of non-translatable method")]
        public void Visit_DoesNotRewriteNameOfNonTranslableMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Method").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(ConstantMethod);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(ConstantMethod).Returns(ConstantMethod);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("instance.Method", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite name of non-translatable static method")]
        public void Visit_DoesNotRewriteNameOfNonTranslableStaticMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass.StaticMethod").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(ConstantStaticMethod);
            resolver.GetDescription(node2).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            structureService.TranslateMember(ConstantStaticMethod).Returns(ConstantStaticMethod);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("TestTypes.ConstantClass.StaticMethod", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite name of non-translatable static method in generic class")]
        public void Visit_DoesNotRewriteNameOfNonTranslableStaticMethodInGenericClass()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantGenericClass<TestTypes.ConstantTypeArgumentClass>.StaticMethod").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(ConstantStaticMethodInGenericType);
            resolver.GetDescription(node2).Returns(ConstantGenericTypeWithConstantTypeArgument);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantGenericTypeWithConstantTypeArgument).Returns(ConstantGenericTypeWithConstantTypeArgument);
            structureService.TranslateMember(ConstantStaticMethodInGenericType).Returns(ConstantStaticMethodInGenericType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("TestTypes.ConstantGenericClass<TestTypes.ConstantTypeArgumentClass>.StaticMethod", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite name of non-translatable property")]
        public void Visit_DoesNotRewriteNameOfTranslatableProperty()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Property").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(ConstantProperty);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(ConstantProperty).Returns(ConstantProperty);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("instance.Property", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite name of non-translatable static property")]
        public void Visit_DoesNotRewriteNameOfTranslatableStaticProperty()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass.StaticProperty").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(ConstantStaticProperty);
            resolver.GetDescription(node2).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            structureService.TranslateMember(ConstantStaticProperty).Returns(ConstantStaticProperty);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("TestTypes.ConstantClass.StaticProperty", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite name of non-translatable static property in generic class")]
        public void Visit_DoesNotRewriteNameOfTranslatableStaticPropertyInGenericClass()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantGenericClass<TestTypes.ConstantTypeArgumentClass>.StaticProperty").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(ConstantStaticPropertyInGenericType);
            resolver.GetDescription(node2).Returns(ConstantGenericTypeWithConstantTypeArgument);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantGenericTypeWithConstantTypeArgument).Returns(ConstantGenericTypeWithConstantTypeArgument);
            structureService.TranslateMember(ConstantStaticPropertyInGenericType).Returns(ConstantStaticPropertyInGenericType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("TestTypes.ConstantGenericClass<TestTypes.ConstantTypeArgumentClass>.StaticProperty", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites type arguments of non-translatable method")]
        public void Visit_RewritesTypeArgumentsOfNonTranslatableMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.GenericMethod<TestTypes.OriginalTypeArgumentClass>").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(ConstantGenericMethodWithOriginalTypeArgument);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(ConstantGenericMethodWithOriginalTypeArgument).Returns(ConstantGenericMethodWithTranslatedTypeArgument);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("instance.GenericMethod<TestTypes.TranslatedTypeArgumentClass>", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites type arguments of non-translatable static method")]
        public void Visit_RewritesTypeArgumentsOfNonTranslatableStaticMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass.StaticGenericMethod<TestTypes.OriginalTypeArgumentClass>").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(ConstantStaticGenericMethodWithOriginalTypeArgument);
            resolver.GetDescription(node2).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            structureService.TranslateMember(ConstantStaticGenericMethodWithOriginalTypeArgument).Returns(ConstantStaticGenericMethodWithTranslatedTypeArgument);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("TestTypes.ConstantClass.StaticGenericMethod<TestTypes.TranslatedTypeArgumentClass>", translatedNode.ToSource());
        }

        [TestMethod("Visit does not verify non-translatable event")]
        public void Visit_DoesNotVerifyNonTranslatableEvent()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Event").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(ConstantEvent);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(ConstantEvent).Returns(ConstantEvent);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            structureService.DidNotReceive().VerifyType(ConstantType);
            structureService.DidNotReceive().VerifyMember(ConstantEvent, Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("Visit does not verify non-translatable static event")]
        public void Visit_DoesNotVerifyNonTranslatableStaticEvent()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass.StaticEvent").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(ConstantStaticEvent);
            resolver.GetDescription(node2).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            structureService.TranslateMember(ConstantStaticEvent).Returns(ConstantStaticEvent);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node1);

            structureService.DidNotReceive().VerifyType(ConstantType);
            structureService.DidNotReceive().VerifyMember(ConstantStaticEvent, Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("Visit does not verify non-translatable field")]
        public void Visit_DoesNotVerifyNonTranslableField()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Field").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(ConstantField);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(ConstantField).Returns(ConstantField);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            structureService.DidNotReceive().VerifyType(ConstantType);
            structureService.DidNotReceive().VerifyMember(ConstantEvent, Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("Visit does not verify non-translatable static field")]
        public void Visit_DoesNotVerifyNonTranslableStaticField()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass.StaticField").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(ConstantStaticField);
            resolver.GetDescription(node2).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            structureService.TranslateMember(ConstantStaticField).Returns(ConstantStaticField);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node1);

            structureService.DidNotReceive().VerifyType(ConstantType);
            structureService.DidNotReceive().VerifyMember(ConstantStaticEvent, Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("Visit does verify non-translatable method")]
        public void Visit_DoesNotVerifyNonTranslableMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Method").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(ConstantMethod);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(ConstantMethod).Returns(ConstantMethod);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            structureService.DidNotReceive().VerifyType(ConstantType);
            structureService.DidNotReceive().VerifyMember(ConstantMethod, Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("Visit does verify non-translatable static method")]
        public void Visit_DoesNotVerifyNonTranslableStaticMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass.StaticMethod").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(ConstantStaticMethod);
            resolver.GetDescription(node2).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            structureService.TranslateMember(ConstantStaticMethod).Returns(ConstantStaticMethod);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node1);

            structureService.DidNotReceive().VerifyType(ConstantType);
            structureService.DidNotReceive().VerifyMember(ConstantStaticMethod, Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("Visit does not verify non-translatable property")]
        public void Visit_DoesNotVerifyNonTranslatableProperty()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Property").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(ConstantProperty);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(ConstantProperty).Returns(ConstantProperty);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            structureService.DidNotReceive().VerifyType(ConstantType);
            structureService.DidNotReceive().VerifyMember(ConstantProperty, Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("Visit does not verify non-translatable static property")]
        public void Visit_DoesNotVerifyNonTranslatableStaticProperty()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass.StaticProperty").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(ConstantStaticProperty);
            resolver.GetDescription(node2).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            structureService.TranslateMember(ConstantStaticProperty).Returns(ConstantStaticProperty);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node1);

            structureService.DidNotReceive().VerifyType(ConstantType);
            structureService.DidNotReceive().VerifyMember(ConstantStaticProperty, Arg.Any<MemberVerificationAspect[]>());
        }


        [TestMethod("Visit rewrites expression of translatable event")]
        public void Visit_RewritesExpressionOfTranslatableEvent()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(new TestTypes.OriginalClass()).Event").GetRoot();
            var node1 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node1).Returns(OriginalDefaultConstructor);
            resolver.GetDescription(node2).Returns(OriginalEvent);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateMember(OriginalEvent).Returns(TranslatedEvent);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node2);

            Assert.AreEqual("(new TestTypes.TranslatedClass()).Event", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites expression of translatable field")]
        public void Visit_RewritesExpressionOfTranslatanbleField()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(new TestTypes.OriginalClass()).Field").GetRoot();
            var node1 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node1).Returns(OriginalDefaultConstructor);
            resolver.GetDescription(node2).Returns(OriginalField);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateMember(OriginalField).Returns(TranslatedField);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node2);

            Assert.AreEqual("(new TestTypes.TranslatedClass()).Field", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites expression of translatable method")]
        public void Visit_RewritesExpressionOfTranslatableMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(new TestTypes.OriginalClass()).Method").GetRoot();
            var node1 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node1).Returns(OriginalDefaultConstructor);
            resolver.GetDescription(node2).Returns(OriginalMethod);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateMember(OriginalMethod).Returns(TranslatedMethod);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node2);

            Assert.AreEqual("(new TestTypes.TranslatedClass()).Method", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrite expression of translatable property")]
        public void Visit_RewriteExpressionOfTranslableProperty()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(new TestTypes.OriginalClass()).Property").GetRoot();
            var node1 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node1).Returns(OriginalDefaultConstructor);
            resolver.GetDescription(node2).Returns(OriginalProperty);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateMember(OriginalProperty).Returns(TranslatedProperty);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node2);

            Assert.AreEqual("(new TestTypes.TranslatedClass()).Property", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrite name of translatable event")]
        public void Visit_RewriteNameOfTranslatableEvent()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.EventWithOriginalName").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(OriginalEventWithVariableName);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalEventWithVariableName).Returns(TranslatedEventWithVariableName);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("instance.EventWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrite name of translatable static event")]
        public void Visit_RewriteNameOfTranslatableStaticEvent()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass.StaticEventWithOriginalName").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalStaticEventWithVariableName);
            resolver.GetDescription(node2).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateMember(OriginalStaticEventWithVariableName).Returns(TranslatedStaticEventWithVariableName);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("TestTypes.TranslatedClass.StaticEventWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrite name of translatable static event in generic class")]
        public void Visit_RewriteNameOfTranslatableStaticEventInGenericClass()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalGenericClass<TestTypes.OriginalTypeArgumentClass>.StaticEventWithOriginalName").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalStaticEventWithVariableNameInGenericType);
            resolver.GetDescription(node2).Returns(OriginalGenericTypeWithVariableTypeArgument);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalGenericTypeWithVariableTypeArgument).Returns(TranslatedGenericTypeWithVariableTypeArgument);
            structureService.TranslateMember(OriginalStaticEventWithVariableNameInGenericType).Returns(TranslatedStaticEventWithVariableNameInGenericType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("TestTypes.TranslatedGenericClass<TestTypes.TranslatedTypeArgumentClass>.StaticEventWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrite name of translatable field")]
        public void Visit_RewriteNameOfTranslatableField()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.FieldWithOriginalName").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(OriginalFieldWithVariableName);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalFieldWithVariableName).Returns(TranslatedFieldWithVariableName);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("instance.FieldWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrite name of translatable static field")]
        public void Visit_RewriteNameOfTranslatableStaticField()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass.StaticFieldWithOriginalName").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalStaticFieldWithVariableName);
            resolver.GetDescription(node2).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateMember(OriginalStaticFieldWithVariableName).Returns(TranslatedStaticFieldWithVariableName);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("TestTypes.TranslatedClass.StaticFieldWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrite name of translatable static field in generic class")]
        public void Visit_RewriteNameOfTranslatableStaticFieldInGenericClass()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalGenericClass<TestTypes.OriginalTypeArgumentClass>.StaticFieldWithOriginalName").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalStaticFieldWithVariableNameInGenericType);
            resolver.GetDescription(node2).Returns(OriginalGenericTypeWithVariableTypeArgument);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalGenericTypeWithVariableTypeArgument).Returns(TranslatedGenericTypeWithVariableTypeArgument);
            structureService.TranslateMember(OriginalStaticFieldWithVariableNameInGenericType).Returns(TranslatedStaticFieldWithVariableNameInGenericType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("TestTypes.TranslatedGenericClass<TestTypes.TranslatedTypeArgumentClass>.StaticFieldWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites name of translatable method")]
        public void Visit_RewritesNameOfTranslatableMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.MethodWithOriginalName").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(OriginalMethodWithVariableName);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalMethodWithVariableName).Returns(TranslatedMethodWithVariableName);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("instance.MethodWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites name of translatable static method")]
        public void Visit_RewritesNameOfTranslatableStaticMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass.StaticMethodWithOriginalName").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalStaticMethodWithVariableName);
            resolver.GetDescription(node2).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateMember(OriginalStaticMethodWithVariableName).Returns(TranslatedStaticMethodWithVariableName);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("TestTypes.TranslatedClass.StaticMethodWithTranslatedName", translatedNode.ToSource());
        }

        // NSubstitute returns an invalid proxy for StructureService.TranslatedMember causing errors.
        //[TestMethod("Visit rewrites name of translatable static method in generic class")]
        public void Visit_RewritesNameOfTranslatableStaticMethodInGenericClass()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalGenericClass<OriginalTypeArgumentClass>.StaticMethodWithOriginalName").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalStaticMethodWithVariableNameInGenericType);
            resolver.GetDescription(node2).Returns(OriginalGenericTypeWithVariableTypeArgument);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalGenericTypeWithVariableTypeArgument).Returns(TranslatedGenericTypeWithVariableTypeArgument);
            structureService.TranslateMember(OriginalStaticMethodWithVariableName).Returns(TranslatedStaticMethodWithVariableName);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("TestTypes.TranslatedGenericClass<TestTypes.TranslatedTypeArgumentClass>.StaticMethodWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrite name of translatable property")]
        public void Visit_RewriteNameOfTranslatableProperty()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.PropertyWithOriginalName").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(OriginalPropertyWithVariableName);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalPropertyWithVariableName).Returns(TranslatedPropertyWithVariableName);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("instance.PropertyWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrite name of translatable static property")]
        public void Visit_RewriteNameOfTranslatableStaticProperty()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass.StaticPropertyWithOriginalName").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalStaticPropertyWithVariableName);
            resolver.GetDescription(node2).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateMember(OriginalStaticPropertyWithVariableName).Returns(TranslatedStaticPropertyWithVariableName);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("TestTypes.TranslatedClass.StaticPropertyWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrite name of translatable static property in generic class")]
        public void Visit_RewriteNameOfTranslatableStaticPropertyInGenericClass()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalGenericClass<OriginalTypeArgumentClass>.StaticPropertyWithOriginalName").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalStaticPropertyWithVariableNameInGenericType);
            resolver.GetDescription(node2).Returns(OriginalGenericTypeWithVariableTypeArgument);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalGenericTypeWithVariableTypeArgument).Returns(TranslatedGenericTypeWithVariableTypeArgument);
            structureService.TranslateMember(OriginalStaticPropertyWithVariableNameInGenericType).Returns(TranslatedStaticPropertyWithVariableNameInGenericType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("TestTypes.TranslatedGenericClass<TestTypes.TranslatedTypeArgumentClass>.StaticPropertyWithTranslatedName", translatedNode.ToSource());
        }
        
        [TestMethod("Visit rewrites type arguments of translatable method")]
        public void VisitiMemberAccessExpression_RewritesTypeArgumentsOfTranslatableMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.GenericMethod<TestTypes.OriginalTypeArgumentClass>").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(OriginalGenericMethodWithVariableTypeArgument);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalGenericMethodWithVariableTypeArgument).Returns(TranslatedGenericMethodWithVariableTypeArgument);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("instance.GenericMethod<TestTypes.TranslatedTypeArgumentClass>", translatedNode.ToSource());
        }

        // TODO Make Visit rewrites infered type arguments of translatable method

        [TestMethod("Visit rewrites type arguments of translatable static method")]
        public void VisitiMemberAccessExpression_RewritesTypeArgumentsOfTranslatableStaticMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass.StaticGenericMethod<TestTypes.OriginalTypeArgumentClass>").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalStaticGenericMethodWithVariableTypeArgument);
            resolver.GetDescription(node2).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateMember(OriginalStaticGenericMethodWithVariableTypeArgument).Returns(TranslatedStaticGenericMethodWithVariableTypeArgument);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("TestTypes.TranslatedClass.StaticGenericMethod<TestTypes.TranslatedTypeArgumentClass>", translatedNode.ToSource());
        }

        // TODO Make Visit rewrites infered type arguments of translatable method

        [TestMethod("Visit verifies translatable event subscription")]
        public void Visit_VerifiesTranslatableEventSubscription()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Event += null").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalEvent);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalEvent).Returns(TranslatedEvent);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node1);

            var expectedAspects = new[]
            {
                MemberVerificationAspect.EventAddAccessLevel,
                MemberVerificationAspect.EventHandlerType,
                MemberVerificationAspect.MemberType
            };
            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalEvent,
                CreateEquvilantArg(expectedAspects));
        }

        [TestMethod("Visit verifies translatable static event subscription")]
        public void Visit_VerifiesTranslatableStaticEventSubscription()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass.StaticEvent += null").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalStaticEvent);
            resolver.GetDescription(node2).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateMember(OriginalStaticEvent).Returns(TranslatedStaticEvent);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node1);

            var expectedAspects = new[]
           {
                MemberVerificationAspect.EventAddAccessLevel,
                MemberVerificationAspect.EventHandlerType,
                MemberVerificationAspect.MemberType
            };
            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalStaticEvent,
                CreateEquvilantArg(expectedAspects));
        }

        [TestMethod("Visit verifies translatable field")]
        public void Visit_VerifiesTranslatableField()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Field").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(OriginalField);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalField).Returns(TranslatedField);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            var expectedAspects = new[]
            {
                MemberVerificationAspect.FieldAccessLevel,
                MemberVerificationAspect.FieldIsStatic,
                MemberVerificationAspect.FieldType,
                MemberVerificationAspect.MemberType,
            };
            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalField,
                CreateEquvilantArg(expectedAspects));
        }

        [TestMethod("Visit verifies translatable static field")]
        public void Visit_VerifiesTranslatableStaticField()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass.StaticField").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalStaticField);
            resolver.GetDescription(node2).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateMember(OriginalStaticField).Returns(TranslatedStaticField);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node1);

            var expectedAspects = new[]
            {
                MemberVerificationAspect.FieldAccessLevel,
                MemberVerificationAspect.FieldIsStatic,
                MemberVerificationAspect.FieldType,
                MemberVerificationAspect.MemberType
            };
            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalStaticField,
                CreateEquvilantArg(expectedAspects));
        }

        [TestMethod("Visit verifies translatable method")]
        public void Visit_VerifiesTranslatableMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Method").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(OriginalMethod);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalMethod).Returns(TranslatedMethod);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            var expectedAspects = new[]
            {
                MemberVerificationAspect.MemberType,
                MemberVerificationAspect.MethodAccessLevel,
                MemberVerificationAspect.MethodDeclaringType,
                MemberVerificationAspect.MethodIsAbstract,
                MemberVerificationAspect.MethodIsStatic,
                MemberVerificationAspect.MethodIsVirtual,
                MemberVerificationAspect.MethodReturnType
            };
            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalMethod,
                CreateEquvilantArg(expectedAspects));
        }

        [TestMethod("Visit verifies translatable static method")]
        public void Visit_VerifiesTranslatableStaticMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass.StaticMethod").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalStaticMethod);
            resolver.GetDescription(node2).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateMember(OriginalStaticMethod).Returns(TranslatedStaticMethod);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node1);

            var expectedAspects = new[]
            {
                MemberVerificationAspect.MemberType,
                MemberVerificationAspect.MethodAccessLevel,
                MemberVerificationAspect.MethodDeclaringType,
                MemberVerificationAspect.MethodIsAbstract,
                MemberVerificationAspect.MethodIsStatic,
                MemberVerificationAspect.MethodIsVirtual,
                MemberVerificationAspect.MethodReturnType
            };
            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalStaticMethod,
                CreateEquvilantArg(expectedAspects));
        }

        [TestMethod("Visit verifies translatable property")]
        public void Visit_VerifiesTranslatableProperty()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Property").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(OriginalProperty);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalProperty).Returns(TranslatedProperty);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            var expectedAspects = new[]
            {
                MemberVerificationAspect.MemberType,
                MemberVerificationAspect.PropertyIsStatic,
                MemberVerificationAspect.PropertyGetAccessLevel,
                MemberVerificationAspect.PropertyGetDeclaringType,
                MemberVerificationAspect.PropertyGetIsAbstract,
                MemberVerificationAspect.PropertyGetIsVirtual,
                MemberVerificationAspect.PropertyType
            };
            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalProperty,
                CreateEquvilantArg(expectedAspects));
        }

        [TestMethod("Visit verifies translatable static property")]
        public void Visit_VerifiesTranslatableStaticProperty()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass.StaticProperty").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalStaticProperty);
            resolver.GetDescription(node2).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateMember(OriginalStaticProperty).Returns(TranslatedStaticProperty);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node1);

            var expectedAspects = new[]
            {
                MemberVerificationAspect.MemberType,
                MemberVerificationAspect.PropertyGetAccessLevel,
                MemberVerificationAspect.PropertyGetDeclaringType,
                MemberVerificationAspect.PropertyGetIsAbstract,
                MemberVerificationAspect.PropertyGetIsVirtual,
                MemberVerificationAspect.PropertyIsStatic,
                MemberVerificationAspect.PropertyType,
            };
            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalStaticProperty,
                CreateEquvilantArg(expectedAspects));
        }

        [TestMethod("Visit verifies translatable event unsubscription")]
        public void Visit_VerifiesTranslatableEventUnsubscription()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Event -= null").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalEvent);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalEvent).Returns(TranslatedEvent);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node1);

            var expectedAspects = new[]
            {
                MemberVerificationAspect.EventHandlerType,
                MemberVerificationAspect.EventRemoveAccessLevel,
                MemberVerificationAspect.MemberType
            };
            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalEvent,
                CreateEquvilantArg(expectedAspects));
        }

        [TestMethod("Visit verifies translatable static event unsubscription")]
        public void Visit_VerifiesTranslatableStaticEventUnsubscription()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass.StaticEvent -= null").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalStaticEvent);
            resolver.GetDescription(node2).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateMember(OriginalStaticEvent).Returns(TranslatedStaticEvent);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node1);

            var expectedAspects = new[]
            {
                MemberVerificationAspect.EventHandlerType,
                MemberVerificationAspect.EventRemoveAccessLevel,
                MemberVerificationAspect.MemberType
            };
            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalStaticEvent,
                CreateEquvilantArg(expectedAspects));
        }

        [TestMethod("Visit verifies translatable field assignment")]
        public void Visit_VerifiesTranslatableFieldAssignment()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Field = null").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(OriginalField);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalField).Returns(TranslatedField);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            var expectedAspects = new[]
            {
                MemberVerificationAspect.FieldAccessLevel,
                MemberVerificationAspect.FieldIsStatic,
                MemberVerificationAspect.FieldType,
                MemberVerificationAspect.FieldWriteability,
                MemberVerificationAspect.MemberType
            };
            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalField,
                CreateEquvilantArg(expectedAspects));
        }

        [TestMethod("Visit verifies translatable static field assignment")]
        public void Visit_VerifiesTranslatableStaticFieldAssignment()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass.StaticField = null").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalStaticField);
            resolver.GetDescription(node2).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateMember(OriginalStaticField).Returns(TranslatedStaticField);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node1);

            var expectedAspects = new[]
            {
                MemberVerificationAspect.FieldAccessLevel,
                MemberVerificationAspect.FieldIsStatic,
                MemberVerificationAspect.FieldType,
                MemberVerificationAspect.FieldWriteability,
                MemberVerificationAspect.MemberType
            };
            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalStaticField,
                CreateEquvilantArg(expectedAspects));
        }

        [TestMethod("Visit verifies translatable property assignment")]
        public void Visit_VerifiesTranslatablePropertyAssignment()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Property = null").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(OriginalProperty);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalProperty).Returns(TranslatedProperty);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            var expectedAspects = new[]
            {
                MemberVerificationAspect.MemberType,
                MemberVerificationAspect.PropertyIsStatic,
                MemberVerificationAspect.PropertySetAccessLevel,
                MemberVerificationAspect.PropertySetDeclaringType,
                MemberVerificationAspect.PropertySetIsAbstract,
                MemberVerificationAspect.PropertySetIsVirtual,
                MemberVerificationAspect.PropertyType,
            };
            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalProperty,
                CreateEquvilantArg(expectedAspects));
        }

        [TestMethod("Visit verifies translatable static property assignment")]
        public void Visit_VerifiesTranslatableStaticPropertyAssignment()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass.StaticProperty = null").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalStaticProperty);
            resolver.GetDescription(node2).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateMember(OriginalStaticProperty).Returns(TranslatedStaticProperty);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node1);

            var expectedAspects = new[]
            {
                MemberVerificationAspect.MemberType,
                MemberVerificationAspect.PropertyIsStatic,
                MemberVerificationAspect.PropertySetAccessLevel,
                MemberVerificationAspect.PropertySetDeclaringType,
                MemberVerificationAspect.PropertySetIsAbstract,
                MemberVerificationAspect.PropertySetIsVirtual,
                MemberVerificationAspect.PropertyType
            };
            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalStaticProperty,
                CreateEquvilantArg(expectedAspects));
        }

        #endregion

        #region VisitObjectCreationExpression

        [TestMethod("Visit does not rewrite type of non-translatable constructor")]
        public void Visit_DoesNotRewriteTypeOfNonTranslatableConstructor()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.ConstantClass()").GetRoot();
            var node = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node).Returns(ConstantDefaultConstructor);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("new TestTypes.ConstantClass()", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite nested type of non-translatable constructor")]
        public void Visit_DoesNotRewriteNewstedTypeNonTranslatableConstructor()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.ConstantClass.NestedClass()").GetRoot();
            var node = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node).Returns(ConstantNestedConstructor);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNestedType).Returns(ConstantNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("new TestTypes.ConstantClass.NestedClass()", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites arguments of non-translatable constructor")]
        public void Visit_RewritesArgumentsOfNonTranslatableConstructor()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.ConstantClass(new TestTypes.OriginalArgumentClass())").GetRoot();
            var node1 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node1).Returns(ConstantConstructorWithArgument);
            resolver.GetConstructorDescription(node2).Returns(OriginalArgumentConstructor);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            structureService.TranslateType(OriginalArgumentType).Returns(TranslatedArgumentType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("new TestTypes.ConstantClass(new TestTypes.TranslatedArgumentClass())", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites initializers of non-translatable constructor")]
        public void Visit_RewritesInitializersOfNonTranslatableConstructor()
        {
            var source = string.Join(
                Environment.NewLine,
                "new TestTypes.ConstantClass()",
                "{Field = new TestTypes.OriginalFieldClass(), Property = new TestTypes.OriginalPropertyClass()}");
            var root = SyntaxFactory.ParseSyntaxTree(source).GetRoot();
            var node1 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().ElementAt(1);
            var node3 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().ElementAt(2);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node1).Returns(ConstantDefaultConstructor);
            resolver.GetConstructorDescription(node2).Returns(OriginalFieldConstructor);
            resolver.GetConstructorDescription(node3).Returns(OriginalPropertyConstructor);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            structureService.TranslateType(OriginalFieldType).Returns(TranslatedFieldType);
            structureService.TranslateType(OriginalPropertyType).Returns(TranslatedPropertyType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            var expected = string.Join(
                Environment.NewLine,
                "new TestTypes.ConstantClass()",
                "{Field = new TestTypes.TranslatedFieldClass(), Property = new TestTypes.TranslatedPropertyClass()}");
            Assert.AreEqual(expected, translatedNode.ToSource());
        }

        [TestMethod("Visit does not verify non-translatable constructor")]
        public void Visit_DoesNotVerifyNonTranslatableConstructor()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.ConstantClass()").GetRoot();
            var node = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node).Returns(ConstantDefaultConstructor);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            structureService.VerifyType(ConstantType);
            structureService.VerifyMember(ConstantDefaultConstructor);
        }


        [TestMethod("Visit rewrites type of translatable constructor")]
        public void Visit_RewritesTypeOfTranslatableConstructor()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass()").GetRoot();
            var node = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node).Returns(OriginalDefaultConstructor);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);
            
            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("new TestTypes.TranslatedClass()", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites nested type of translatable constructor")]
        public void Visit_RewritesNestedTypeOfTranslatableConstructor()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass.NestedClass()").GetRoot();
            var node = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node).Returns(OriginalNestedConstructor);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNestedType).Returns(TranslatedNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("new TestTypes.TranslatedClass.NestedClass()", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites type arguments of translatable constructor")]
        public void Visit_RewritesTypeArgumentsOfTranslatableConstructor()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalGenericClass<OriginalTypeArgumentClass>()").GetRoot();
            var node = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().ElementAt(0);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node).Returns(OriginalGenericConstructorWithVariableTypeArgument);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalGenericTypeWithVariableTypeArgument).Returns(TranslatedGenericTypeWithVariableTypeArgument);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("new TestTypes.TranslatedGenericClass<TestTypes.TranslatedTypeArgumentClass>()", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites arguments of translatable constructor")]
        public void Visit_RewritesArgumentsOfTranslatableConstructor()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass(new TestTypes.OriginalArgumentClass())").GetRoot();
            var node1 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node1).Returns(OriginalConstructorWithVariableArgument);
            resolver.GetConstructorDescription(node2).Returns(OriginalArgumentConstructor);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateType(OriginalArgumentType).Returns(TranslatedArgumentType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            Assert.AreEqual("new TestTypes.TranslatedClass(new TestTypes.TranslatedArgumentClass())", translatedNode.ToSource());
        }
        
        [TestMethod("Visit rewrites initializer of translatable constructor")]
        public void Visit_RewritesInitializerOfTranslatableConstructor()
        {
            var source = string.Join(
                Environment.NewLine,
                "new TestTypes.OriginalClass()",
                "{Field = new TestTypes.OriginalFieldClass(), Property = new TestTypes.OriginalPropertyClass()}");
            var root = SyntaxFactory.ParseSyntaxTree(source).GetRoot();
            var node1 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().ElementAt(1);
            var node3 = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().ElementAt(2);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node1).Returns(OriginalDefaultConstructor);
            resolver.GetConstructorDescription(node2).Returns(OriginalFieldConstructor);
            resolver.GetConstructorDescription(node3).Returns(OriginalPropertyConstructor);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateType(OriginalFieldType).Returns(TranslatedFieldType);
            structureService.TranslateType(OriginalPropertyType).Returns(TranslatedPropertyType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node1);

            var expected = string.Join(
                Environment.NewLine,
                "new TestTypes.TranslatedClass()",
                "{Field = new TestTypes.TranslatedFieldClass(), Property = new TestTypes.TranslatedPropertyClass()}");
            Assert.AreEqual(expected, translatedNode.ToSource());
        }

        [TestMethod("Visit verifies translatable constructor")]
        public void Visit_VerifiesTranslableConstructor()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass()").GetRoot();
            var node = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node).Returns(OriginalDefaultConstructor);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(OriginalType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            var expectedAspects = new[]
            {
                MemberVerificationAspect.ConstructorAccessLevel,
                MemberVerificationAspect.MemberType
            };
            structureService.VerifyType(OriginalType);
            structureService.VerifyMember(OriginalDefaultConstructor, expectedAspects);
        }

        #endregion

        #region VisitTypeOfExpression

        [TestMethod("Visit does not rewrite type of non-translatable typeof")]
        public void Visit_DoesNotRewriteTypeOfNonTranslatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.ConstantClass)").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("typeof(TestTypes.ConstantClass)", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrites nested type of non-translatable typeof")]
        public void Visit_DoesNotRewriteNestedTypeOfNonTranslatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.ConstantClass.NestedClass)").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNestedType).Returns(ConstantNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("typeof(TestTypes.ConstantClass.NestedClass)", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite array type of non-translatable typeof")]
        public void Visit_DoesNotRewriteArrayTypeOfNonTranslatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.ConstantClass[])").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantArrayType).Returns(ConstantArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("typeof(TestTypes.ConstantClass[])", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite nullable type of non-translatable typeof")]
        public void Visit_DoesNotRewriteNullableTypeOfNonTranslatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.ConstantClass.NestedClass)").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNestedType).Returns(ConstantNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("typeof(TestTypes.ConstantClass.NestedClass)", translatedNode.ToSource());
        }

        [TestMethod("Visit does not verify non-translatable typeof")]
        public void Visit_DoesNotVerifyNonTranslatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.ConstantClass)").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            structureService.DidNotReceive().VerifyType(ConstantType);
        }


        [TestMethod("Visit rewrites type of translatable typeof")]
        public void Visit_RewritesTypeOfTranslatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.OriginalClass)").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("typeof(TestTypes.TranslatedClass)", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites type of translatable typeof with using directives")]
        public void Visit_RewritesTypeOfTranslatableTypeOfWithUsingDirectives()
        {
            var root = SyntaxFactory.ParseSyntaxTree("using TestTypes; typeof(OriginalClass)").GetRoot();
            var node1 = root.AllDescendantNodes<UsingDirectiveSyntax>().First();
            var node2 = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetNamespaceDescription(node1).Returns(ConstantNamespace);
            resolver.GetTypeDescription(node2).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateNamespace(ConstantNamespace).Returns(ConstantNamespace);
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(root);

            Assert.AreEqual("using TestTypes; typeof(TranslatedClass)", translatedNode.ToString());
        }

        [TestMethod("Visit rewrites array type of translatable typeof")]
        public void Visit_RewritesArrayTypeOfTranlatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.OriginalClass[])").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalArrayType).Returns(TranslatedArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("typeof(TestTypes.TranslatedClass[])", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites nested type of translatable typeof")]
        public void Visit_RewritesNestedTypeOfTranslatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.OriginalClass.NestedClass)").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNestedType).Returns(TranslatedNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("typeof(TestTypes.TranslatedClass.NestedClass)", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites nullable type of translatable typeof")]
        public void VisitRewritesNullableTypeOfTranslatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.OriginalStruct?)").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNullableType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNullableType).Returns(TranslatedNullableType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("typeof(TestTypes.TranslatedStruct? )", translatedNode.ToSource());
        }

        [TestMethod("Visit verifies translatable typeof")]
        public void Visit_VerifiesTranslatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.OriginalClass)").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            structureService.Received().VerifyType(OriginalType);
        }
        
        #endregion

        #region VisitVariableDeclaration

        [TestMethod("Visit does not rewrite type of non-translatable variable")]
        public void Visit_DoesNotRewriteTypeOfNonTranslatableVariable()
        {
            
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.ConstantClass instance", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite array type of non-translatable variable")]
        public void Visit_DoesNotRewriteArrayTypeOfNonTranslatableVariable()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass.NestedClass instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNestedType).Returns(ConstantNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.ConstantClass.NestedClass instance", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite nested type of non-translatable variable")]
        public void Visit_DoesNotRewriteNestedTypeOfNonTranslatableVariable()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass[] instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantArrayType).Returns(ConstantArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.ConstantClass[] instance", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite nullable type of non-translatable variable")]
        public void Visit_DoesNotRewriteNullableTypeOfNonTranslatableVariable()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass.NestedClass instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNestedType).Returns(ConstantNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.ConstantClass.NestedClass instance", translatedNode.ToSource());
        }

        [TestMethod("Visit does verify non-translatable variable")]
        public void VisitDoesVerifyNonTranslatableVariable()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            structureService.DidNotReceive().VerifyType(ConstantType);
        }


        [TestMethod("Visit rewrites type of translatable variable")]
        public void Visit_RewritesTypeOfTranslatableVariable()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.TranslatedClass instance", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites type of translatable variable with using directives")]
        public void Visit_RewritesTypeOfTranslatableVariableWithUsingDirectives()
        {
            var root = SyntaxFactory.ParseSyntaxTree("using TestTypes; OriginalClass instance").GetRoot();
            var node1 = root.AllDescendantNodes<UsingDirectiveSyntax>().First();
            var node2 = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetNamespaceDescription(node1).Returns(ConstantNamespace);
            resolver.GetTypeDescription(node2).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateNamespace(ConstantNamespace).Returns(ConstantNamespace);
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(root);

            Assert.AreEqual("using TestTypes; TranslatedClass instance", translatedNode.ToFullString());
        }

        [TestMethod("Visit rewrites array type of translatable variable")]
        public void Visit_RewritesArrayTypeOfTranslatableVariable()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass[] instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalArrayType).Returns(TranslatedArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.TranslatedClass[] instance", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites nested type of translatable variable")]
        public void Visit_RewritesNestedTypeOfTranslatableVariable()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass.NestedClass instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNestedType).Returns(TranslatedNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.TranslatedClass.NestedClass instance", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites nullable type of translatable variable")]
        public void Visit_RewritesNullableTypeOfTranslatableVariable()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalStruct? instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNullableType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNullableType).Returns(TranslatedNullableType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("TestTypes.TranslatedStruct? instance", translatedNode.ToSource());
        }

        [TestMethod("Visit verifies translatable variable")]
        public void Visit_VerifiesTranslatableVariable()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.Visit(node);

            structureService.Received().VerifyType(OriginalType);
        }


        [TestMethod("Visit preserves whitespace when rewriting type of translatable variable")]
        public void Visit_PreservesWhitespaceWhenRewritingTypeOfTranslatableVariable()
        {
            var root = SyntaxFactory.ParseSyntaxTree("     TestTypes.OriginalClass   instance  ").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("     TestTypes.TranslatedClass   instance  ", translatedNode.ToFullString());
        }
        
        #endregion

        #region VisitUsingDirective

        [TestMethod("Visit does not rewrite non-translatable namespace in using directive")]
        public void Visit_DoesNotRewriteNonTranslatableNamespaceInUsingDirective()
        {

            var root = SyntaxFactory.ParseSyntaxTree("using ConstantNamespace").GetRoot();
            var node = root.AllDescendantNodes<UsingDirectiveSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetNamespaceDescription(node).Returns(ConstantNamespace);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateNamespace(ConstantNamespace).Returns(ConstantNamespace);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("using ConstantNamespace", translatedNode.ToSource());
        }

        [TestMethod("Visit does not rewrite type in static using directive")]
        public void Visit_DoesNotRewriteTypeInStaticUsingDirective()
        {
            var root = SyntaxFactory.ParseSyntaxTree("using static System.Object").GetRoot();
            var node = root.AllDescendantNodes<UsingDirectiveSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetNamespaceDescription(node).Returns(null as NamespaceDescription);
            var structureService = Substitute.For<IStructureService>();
            //structureService.TranslateNamespace(ConstantNamespace).Returns(ConstantNamespace);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);
            
            Assert.AreEqual("using static System.Object", translatedNode.ToSource());
        }

        [TestMethod("Visit rewrites translatable namespace in using directive")]
        public void VisitRewritesTranslatableNamespaceInUsingDirective()
        {
            var root = SyntaxFactory.ParseSyntaxTree("using OriginalNamespace").GetRoot();
            var node = root.AllDescendantNodes<UsingDirectiveSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetNamespaceDescription(node).Returns(OriginalNamespace);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateNamespace(OriginalNamespace).Returns(TranslatedNamespace);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual("using TranslatedNamespace", translatedNode.ToSource());
        }

        [TestMethod("Visit preserves whitespace when rewriting namespace in using directive")]
        public void Visit_PreservcesWhitespaceWhenRewritingNamespaceInUsingDirective()
        {
            var root = SyntaxFactory.ParseSyntaxTree(" using  OriginalNamespace   ").GetRoot();
            var node = root.AllDescendantNodes<UsingDirectiveSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetNamespaceDescription(node).Returns(OriginalNamespace);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateNamespace(OriginalNamespace).Returns(TranslatedNamespace);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.Visit(node);

            Assert.AreEqual(" using  TranslatedNamespace   ", translatedNode.ToFullString());
        }
        
        #endregion
    }
}