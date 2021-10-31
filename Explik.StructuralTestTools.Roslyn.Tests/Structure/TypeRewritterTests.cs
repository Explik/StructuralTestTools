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
        // TODO Add tests for verification aspects for field and proprety read/write.

        #region VisitArrayType

        [TestMethod("VisitArrayType does not rewrite type of non-translatable array type")]
        public void VisitArrayType_DoesNotRewriteTypeOfNonTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.ConstantClass[]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitArrayType(node);

            Assert.AreEqual("TestTypes.ConstantClass[]", translatedNode.ToSource());
        }

        [TestMethod("VisitArrayType does not rewrite array type of non-translatable array type")]
        public void VisitArrayType_DoesNotRewriteArrayTypeOfNonTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.ConstantClass[][]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantArrayType).Returns(ConstantArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitArrayType(node);

            Assert.AreEqual("TestTypes.ConstantClass[][]", translatedNode.ToSource());
        }

        [TestMethod("VisitArrayType does not rewrite nested type of non-translatable array type")]
        public void VisitArrayType_DoesNotRewriteNestedTypeOfNonTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.ConstantClass.NestedClass[]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNestedType).Returns(ConstantNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitArrayType(node);

            Assert.AreEqual("TestTypes.ConstantClass.NestedClass[]", translatedNode.ToSource());
        }

        [TestMethod("VisitArrayType does not rewrite nullable type of non-translatable array type")]
        public void VisitArrayType_DoesNotRewriteNullableTypeOfNonTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.ConstantStruct? []").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNullableType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNullableType).Returns(ConstantNullableType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitArrayType(node);

            Assert.AreEqual("TestTypes.ConstantStruct? []", translatedNode.ToSource());
        }

        [TestMethod("VisitArrayType does not verify non-translatable array type")]
        public void VisitArrayType_DoesNotVerifyNonTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.ConstantClass[]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitArrayType(node);

            structureService.DidNotReceive().VerifyType(ConstantType);
        }


        [TestMethod("VisitArrayType rewrites type of translatable array type")]
        public void VisitArrayType_RewritesTypeOfTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass[]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitArrayType(node);

            Assert.AreEqual("TestTypes.TranslatedClass[]", translatedNode.ToSource());
        }

        [TestMethod("VisitArrayType rewrites type of translatable array type with length")]
        public void VisitArrayType_RewritesTypeOfTranslatableArrayTypeWithLength()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass[0]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitArrayType(node);

            Assert.AreEqual("TestTypes.TranslatedClass[0]", translatedNode.ToSource());
        }

        [TestMethod("VisitArrayType rewrites array type of translatable array type")]
        public void VisitArrayType_RewritesArrayTypeOfTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass[][]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalArrayType).Returns(TranslatedArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitArrayType(node);

            Assert.AreEqual("TestTypes.TranslatedClass[][]", translatedNode.ToSource());
        }

        [TestMethod("VisitArrayType rewrites array type of translatable array type with lengths")]
        public void VisitArrayType_RewritesArrayTypeOfTranslatableArrayTypeWithLengths()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass[0][1]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalArrayType).Returns(TranslatedArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitArrayType(node);

            Assert.AreEqual("TestTypes.TranslatedClass[0][1]", translatedNode.ToSource());
        }

        [TestMethod("VisitArrayType rewrites array array type of translatable array type")]
        public void VisitArrayType_RewritesArrayArrayTypeOfTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass[][][]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalArrayArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalArrayArrayType).Returns(TranslatedArrayArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitArrayType(node);

            Assert.AreEqual("TestTypes.TranslatedClass[][][]", translatedNode.ToSource());
        }

        [TestMethod("VisitArrayType rewrites array array type of translatable array type with lengths")]
        public void VisitArrayType_RewritesArrayArrayTypeOfTranslatableArrayTypeWithLengths()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass[0][1][2]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalArrayType).Returns(TranslatedArrayArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitArrayType(node);

            Assert.AreEqual("TestTypes.TranslatedClass[0][1][2]", translatedNode.ToSource());
        }

        [TestMethod("VisitArrayType rewrites nested type of translatable array type")]
        public void VisitArrayType_RewritesNestedTypeOfTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass.NestedClass[]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNestedType).Returns(TranslatedNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitArrayType(node);

            Assert.AreEqual("TestTypes.TranslatedClass.NestedClass[]", translatedNode.ToSource());
        }

        [TestMethod("VisitArrayType rewrites nullable type of translatable array type")]
        public void VisitArrayType_RewritesNullableTypeOfTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalStruct? []").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNullableType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNullableType).Returns(TranslatedNullableType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitArrayType(node);

            Assert.AreEqual("TestTypes.TranslatedStruct? []", translatedNode.ToSource());
        }

        [TestMethod("VisitArrayType verifies translatable array type")]
        public void VisitArrayType_VerifiesTranslatableArrayType()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass[]").GetRoot();
            var node = root.AllDescendantNodes<ArrayTypeSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitArrayType(node);

            structureService.Received().VerifyType(OriginalType);
        }

        #endregion

        #region VisitCastExpression

        [TestMethod("VisitCastExpression does not rewrite type of non-translatable cast")]
        public void VisitCastExpression_DoesNotRewriteTypeOfNonTranslatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.ConstantClass)null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitCastExpression(node);

            Assert.AreEqual("(TestTypes.ConstantClass)null", translatedNode.ToSource());
        }

        [TestMethod("VisitCastExpression does not rewrites nested type of non-translatable cast")]
        public void VisitCastExpression_DoesNotRewriteNestedTypeOfNonTranslatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.ConstantClass[])null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantArrayType).Returns(ConstantArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitCastExpression(node);

            Assert.AreEqual("(TestTypes.ConstantClass[])null", translatedNode.ToSource());
        }

        [TestMethod("VisitCastExpression does not rewrite array type of non-translatable cast")]
        public void VisitCastExpression_DoesNotRewriteArrayTypeOfNonTranslatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.ConstantClass.NestedClass)null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNestedType).Returns(ConstantNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitCastExpression(node);

            Assert.AreEqual("(TestTypes.ConstantClass.NestedClass)null", translatedNode.ToSource());
        }

        [TestMethod("VisitCastExpression does not rewrite nullable type of non-translatable cast")]
        public void VisitCastExpression_DoesNotRewriteNullableTypeOfNonTranslatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.ConstantStruct?)null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNullableType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNullableType).Returns(ConstantNullableType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitCastExpression(node);

            Assert.AreEqual("(TestTypes.ConstantStruct? )null", translatedNode.ToSource());
        }

        [TestMethod("VisitCastExpression does not verify non-translatable cast")]
        public void VisitCastExpression_DoesNotVerifyNonTranslatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.ConstantClass)null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitCastExpression(node);

            structureService.DidNotReceive().VerifyType(ConstantType);
        }


        [TestMethod("VisitCastExpression rewrites type of translatable cast")]
        public void VisitCastExpression_RewritesTypeOfTranslatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.OriginalClass)null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitCastExpression(node);

            Assert.AreEqual("(TestTypes.TranslatedClass)null", translatedNode.ToSource());
        }

        [TestMethod("VisitCastExpression rewrites array type of translatable cast")]
        public void VisitCastExpression_RewritesArrayTypeOfTranlatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.OriginalClass[])null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalArrayType).Returns(TranslatedArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitCastExpression(node);

            Assert.AreEqual("(TestTypes.TranslatedClass[])null", translatedNode.ToSource());
        }

        [TestMethod("VisitCastExpression rewrites nested type of translatable cast")]
        public void VisitCastExpression_RewritesNestedTypeOfTranslatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.OriginalClass.NestedClass)null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNestedType).Returns(TranslatedNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitCastExpression(node);

            Assert.AreEqual("(TestTypes.TranslatedClass.NestedClass)null", translatedNode.ToSource());
        }

        [TestMethod("VisitCastExpression rewrites nullable type of translatable cast")]
        public void VisitCastExpressionRewritesNullableTypeOfTranslatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.OriginalStruct?)null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNullableType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNullableType).Returns(TranslatedNullableType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitCastExpression(node);

            Assert.AreEqual("(TestTypes.TranslatedStruct? )null", translatedNode.ToSource());
        }

        [TestMethod("VisitCastExpression verifies translatable cast")]
        public void VisitCastExpression_VerifiesTranslatableCast()
        {
            var root = SyntaxFactory.ParseSyntaxTree("(TestTypes.OriginalClass)null").GetRoot();
            var node = root.AllDescendantNodes<CastExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitCastExpression(node);

            structureService.Received().VerifyType(OriginalType);
        }

        #endregion

        #region VisitDefaultExpression

        [TestMethod("VisitDefaultExpression does not rewrite type of non-translatable default")]
        public void VisitDefaultExpression_DoesNotRewriteTypeOfNonTranslatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.ConstantClass)").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitDefaultExpression(node);

            Assert.AreEqual("default(TestTypes.ConstantClass)", translatedNode.ToSource());
        }

        [TestMethod("VisitDefaultExpression does not rewrites nested type of non-translatable default")]
        public void VisitDefaultExpression_DoesNotRewriteNestedTypeOfNonTranslatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.ConstantClass[])").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantArrayType).Returns(ConstantArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitDefaultExpression(node);

            Assert.AreEqual("default(TestTypes.ConstantClass[])", translatedNode.ToSource());
        }

        [TestMethod("VisitDefaultExpression does not rewrite array type of non-translatable default")]
        public void VisitDefaultExpression_DoesNotRewriteArrayTypeOfNonTranslatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.ConstantStruct?)").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNullableType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNullableType).Returns(ConstantNullableType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitDefaultExpression(node);

            Assert.AreEqual("default(TestTypes.ConstantStruct? )", translatedNode.ToSource());
        }

        [TestMethod("VisitDefaultExpression does not rewrite nullable type of non-translatable default")]
        public void VisitDefaultExpression_DoesNotRewriteNullableTypeOfNonTranslatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.ConstantClass.NestedClass)").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNestedType).Returns(ConstantNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitDefaultExpression(node);

            Assert.AreEqual("default(TestTypes.ConstantClass.NestedClass)", translatedNode.ToSource());
        }

        [TestMethod("VisitDefaultExpression does not verify non-translatable default")]
        public void VisitDefaultExpression_DoesNotVerifyNonTranslatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.ConstantClass)").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitDefaultExpression(node);

            structureService.DidNotReceive().VerifyType(ConstantType);
        }


        [TestMethod("VisitDefaultExpression rewrites type of translatable default")]
        public void VisitDefaultExpression_RewritesTypeOfTranslatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.OriginalClass)").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitDefaultExpression(node);

            Assert.AreEqual("default(TestTypes.TranslatedClass)", translatedNode.ToSource());
        }

        [TestMethod("VisitDefaultExpression rewrites array type of translatable default")]
        public void VisitDefaultExpression_RewritesArrayTypeOfTranlatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.OriginalClass[])").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalArrayType).Returns(TranslatedArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitDefaultExpression(node);

            Assert.AreEqual("default(TestTypes.TranslatedClass[])", translatedNode.ToSource());
        }

        [TestMethod("VisitDefaultExpression rewrites nested type of translatable default")]
        public void VisitDefaultExpression_RewritesNestedTypeOfTranslatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.OriginalClass.NestedClass)").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNestedType).Returns(TranslatedNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitDefaultExpression(node);

            Assert.AreEqual("default(TestTypes.TranslatedClass.NestedClass)", translatedNode.ToSource());
        }

        [TestMethod("VisitDefaultExpression rewrites nullable type of translatable default")]
        public void VisitDefaultExpressionRewritesNullableTypeOfTranslatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.OriginalStruct?)").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNullableType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNullableType).Returns(TranslatedNullableType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitDefaultExpression(node);

            Assert.AreEqual("default(TestTypes.TranslatedStruct? )", translatedNode.ToSource());
        }

        [TestMethod("VisitDefaultExpression verifies translatable default")]
        public void VisitDefaultExpression_VerifiesTranslatableDefault()
        {
            var root = SyntaxFactory.ParseSyntaxTree("default(TestTypes.OriginalClass)").GetRoot();
            var node = root.AllDescendantNodes<DefaultExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitDefaultExpression(node);

            structureService.Received().VerifyType(OriginalType);
        }

        #endregion

        #region VisitElementAccessExpressionSyntax

        [TestMethod("VisitElementAccessExpressionSyntax does not verify non-translatable indexer")]
        public void VisitElementAccessExpressionSyntax_DoesNotVerifyNonTranslatableIndexer()
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

        [TestMethod("VisitElementAccessExpressionSyntax verifies translatable indexer")]
        public void VisitElementAccessExpressionSyntax_VerifiesTranslatableIndexer()
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

            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(OriginalIndexer, Arg.Any<MemberVerificationAspect[]>());
        }

        #endregion

        #region VisitMemberAccessExpression

        [TestMethod("VisitMemberAccessExpression rewrites expression of non-translatable event")]
        public void VisitMemberAccessExpressionRewritesExpressionOfNonTranslatableEvent()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node2);

            Assert.AreEqual("(new ConstantClass()).Event", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrites expression of non-translatable field")]
        public void VisitMemberAccessExpression_RewritesExpressionOfNonTranslatableField()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node2);

            Assert.AreEqual("(new ConstantClass()).Field", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrites expression of non-translatable method")]
        public void VisitMemberAccessExpression_RewritesExpressionOfNonTranslatableMethod()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node2);

            Assert.AreEqual("(new ConstantClass()).Method", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrites expression of non-translatable property")]
        public void VisitMemberAccessExpression_RewritesExpressionOfNonTranslatableProperty()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node2);

            Assert.AreEqual("(new ConstantClass()).Property", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression does not rewrite name of non-translatable event")]
        public void VisitMemberAccessExpression_DoesNotRewriteNameOfNonTranslatableEvent()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Event").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(ConstantEvent);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(ConstantEvent).Returns(ConstantEvent);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitMemberAccessExpression(node);

            Assert.AreEqual("instance.Event", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression does not rewrite name of non-translatable static event")]
        public void VisitMemberAccessExpression_DoesNotRewriteNameOfNonTranslatableStaticEvent()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node1);

            Assert.AreEqual("TestTypes.ConstantClass.StaticEvent", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression does not rewrite name of non-translatable static event in generic class")]
        public void VisitMemberAccessExpression_DoesNotRewriteNameOfNonTranslatableStaticEventInGenericClass()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node1);

            Assert.AreEqual("TestTypes.ConstantGenericClass<TestTypes.ConstantTypeArgumentClass>.StaticEvent", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression does not rewrite name of non-translatable field")]
        public void VisitMemberAccessExpression_DoesNotRewriteNameOfNonTranslatableField()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Field").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(ConstantField);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(ConstantField).Returns(ConstantField);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitMemberAccessExpression(node);

            Assert.AreEqual("instance.Field", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression does not rewrite name of non-translatable static field")]
        public void VisitMemberAccessExpression_DoesNotRewriteNameOfNonTranslatableStaticField()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node1);

            Assert.AreEqual("TestTypes.ConstantClass.StaticField", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression does not rewrite name of non-translatable static field in generic class")]
        public void VisitMemberAccessExpression_DoesNotRewriteNameOfNonTranslatableStaticFieldInGenericClass()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node1);

            Assert.AreEqual("TestTypes.ConstantGenericClass<TestTypes.ConstantTypeArgumentClass>.StaticField", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression does not rewrite name of non-translatable method")]
        public void VisitMemberAccessExpression_DoesNotRewriteNameOfNonTranslableMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Method").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(ConstantMethod);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(ConstantMethod).Returns(ConstantMethod);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitMemberAccessExpression(node);

            Assert.AreEqual("instance.Method", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression does not rewrite name of non-translatable static method")]
        public void VisitMemberAccessExpression_DoesNotRewriteNameOfNonTranslableStaticMethod()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node1);

            Assert.AreEqual("TestTypes.ConstantClass.StaticMethod", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression does not rewrite name of non-translatable static method in generic class")]
        public void VisitMemberAccessExpression_DoesNotRewriteNameOfNonTranslableStaticMethodInGenericClass()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node1);

            Assert.AreEqual("TestTypes.ConstantGenericClass<TestTypes.ConstantTypeArgumentClass>.StaticMethod", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression does not rewrite name of non-translatable property")]
        public void VisitMemberAccessExpression_DoesNotRewriteNameOfTranslatableProperty()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Property").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(ConstantProperty);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(ConstantProperty).Returns(ConstantProperty);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitMemberAccessExpression(node);

            Assert.AreEqual("instance.Property", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression does not rewrite name of non-translatable static property")]
        public void VisitMemberAccessExpression_DoesNotRewriteNameOfTranslatableStaticProperty()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node1);

            Assert.AreEqual("TestTypes.ConstantClass.StaticProperty", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression does not rewrite name of non-translatable static property in generic class")]
        public void VisitMemberAccessExpression_DoesNotRewriteNameOfTranslatableStaticPropertyInGenericClass()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node1);

            Assert.AreEqual("TestTypes.ConstantGenericClass<TestTypes.ConstantTypeArgumentClass>.StaticProperty", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrites type arguments of non-translatable method")]
        public void VisitMemberAccessExpression_RewritesTypeArgumentsOfNonTranslatableMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.GenericMethod<TestTypes.OriginalTypeArgumentClass>").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(ConstantGenericMethodWithOriginalTypeArgument);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(ConstantGenericMethodWithOriginalTypeArgument).Returns(ConstantGenericMethodWithTranslatedTypeArgument);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitMemberAccessExpression(node);

            Assert.AreEqual("instance.GenericMethod<TestTypes.TranslatedTypeArgumentClass>", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrites type arguments of non-translatable static method")]
        public void VisitMemberAccessExpression_RewritesTypeArgumentsOfNonTranslatableStaticMethod()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node1);

            Assert.AreEqual("TestTypes.ConstantClass.StaticGenericMethod<TestTypes.TranslatedTypeArgumentClass>", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression does not verify non-translatable event")]
        public void VisitMemberAccessExpression_DoesNotVerifyNonTranslatableEvent()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Event").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(ConstantEvent);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(ConstantEvent).Returns(ConstantEvent);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitMemberAccessExpression(node);

            structureService.DidNotReceive().VerifyType(ConstantType);
            structureService.DidNotReceive().VerifyMember(ConstantEvent, Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("VisitMemberAccessExpression does not verify non-translatable static event")]
        public void VisitMemberAccessExpression_DoesNotVerifyNonTranslatableStaticEvent()
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

            rewriter.VisitMemberAccessExpression(node1);

            structureService.DidNotReceive().VerifyType(ConstantType);
            structureService.DidNotReceive().VerifyMember(ConstantStaticEvent, Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("VisitMemberAccessExpression does not verify non-translatable field")]
        public void VisitMemberAccessExpression_DoesNotVerifyNonTranslableField()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Field").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(ConstantField);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(ConstantField).Returns(ConstantField);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitMemberAccessExpression(node);

            structureService.DidNotReceive().VerifyType(ConstantType);
            structureService.DidNotReceive().VerifyMember(ConstantEvent, Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("VisitMemberAccessExpression does not verify non-translatable static field")]
        public void VisitMemberAccessExpression_DoesNotVerifyNonTranslableStaticField()
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

            rewriter.VisitMemberAccessExpression(node1);

            structureService.DidNotReceive().VerifyType(ConstantType);
            structureService.DidNotReceive().VerifyMember(ConstantStaticEvent, Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("VisitMemberAccessExpression does verify non-translatable method")]
        public void VisitMemberAccessExpression_DoesNotVerifyNonTranslableMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Method").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(ConstantMethod);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(ConstantMethod).Returns(ConstantMethod);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitMemberAccessExpression(node);

            structureService.DidNotReceive().VerifyType(ConstantType);
            structureService.DidNotReceive().VerifyMember(ConstantMethod, Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("VisitMemberAccessExpression does verify non-translatable static method")]
        public void VisitMemberAccessExpression_DoesNotVerifyNonTranslableStaticMethod()
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

            rewriter.VisitMemberAccessExpression(node1);

            structureService.DidNotReceive().VerifyType(ConstantType);
            structureService.DidNotReceive().VerifyMember(ConstantStaticMethod, Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("VisitMemberAccessExpression does not verify non-translatable property")]
        public void VisitMemberAccessExpression_DoesNotVerifyNonTranslatableProperty()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Property").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(ConstantProperty);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(ConstantProperty).Returns(ConstantProperty);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitMemberAccessExpression(node);

            structureService.DidNotReceive().VerifyType(ConstantType);
            structureService.DidNotReceive().VerifyMember(ConstantProperty, Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("VisitMemberAccessExpression does not verify non-translatable static property")]
        public void VisitMemberAccessExpression_DoesNotVerifyNonTranslatableStaticProperty()
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

            rewriter.VisitMemberAccessExpression(node1);

            structureService.DidNotReceive().VerifyType(ConstantType);
            structureService.DidNotReceive().VerifyMember(ConstantStaticProperty, Arg.Any<MemberVerificationAspect[]>());
        }


        [TestMethod("VisitMemberAccessExpression rewrites expression of translatable event")]
        public void VisitMemberAccessExpression_RewritesExpressionOfTranslatableEvent()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node2);

            Assert.AreEqual("(new TestTypes.TranslatedClass()).Event", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrites expression of translatable field")]
        public void VisitMemberAccessExpression_RewritesExpressionOfTranslatanbleField()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node2);

            Assert.AreEqual("(new TestTypes.TranslatedClass()).Field", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrites expression of translatable method")]
        public void VisitMemberAccessExpression_RewritesExpressionOfTranslatableMethod()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node2);

            Assert.AreEqual("(new TestTypes.TranslatedClass()).Method", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrite expression of translatable property")]
        public void VisitMemberAccessExpression_RewriteExpressionOfTranslableProperty()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node2);

            Assert.AreEqual("(new TestTypes.TranslatedClass()).Property", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrite name of translatable event")]
        public void VisitMemberAccessExpression_RewriteNameOfTranslatableEvent()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.EventWithOriginalName").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(OriginalEventWithVariableName);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalEventWithVariableName).Returns(TranslatedEventWithVariableName);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitMemberAccessExpression(node);

            Assert.AreEqual("instance.EventWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrite name of translatable static event")]
        public void VisitMemberAccessExpression_RewriteNameOfTranslatableStaticEvent()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node1);

            Assert.AreEqual("TestTypes.TranslatedClass.StaticEventWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrite name of translatable static event in generic class")]
        public void VisitMemberAccessExpression_RewriteNameOfTranslatableStaticEventInGenericClass()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node1);

            Assert.AreEqual("TestTypes.TranslatedGenericClass<TestTypes.TranslatedTypeArgumentClass>.StaticEventWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrite name of translatable field")]
        public void VisitMemberAccessExpression_RewriteNameOfTranslatableField()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.FieldWithOriginalName").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(OriginalFieldWithVariableName);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalFieldWithVariableName).Returns(TranslatedFieldWithVariableName);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitMemberAccessExpression(node);

            Assert.AreEqual("instance.FieldWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrite name of translatable static field")]
        public void VisitMemberAccessExpression_RewriteNameOfTranslatableStaticField()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node1);

            Assert.AreEqual("TestTypes.TranslatedClass.StaticFieldWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrite name of translatable static field in generic class")]
        public void VisitMemberAccessExpression_RewriteNameOfTranslatableStaticFieldInGenericClass()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node1);

            Assert.AreEqual("TestTypes.TranslatedGenericClass<TestTypes.TranslatedTypeArgumentClass>.StaticFieldWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrites name of translatable method")]
        public void VisitMemberAccessExpression_RewritesNameOfTranslatableMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.MethodWithOriginalName").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(OriginalMethodWithVariableName);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalMethodWithVariableName).Returns(TranslatedMethodWithVariableName);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitMemberAccessExpression(node);

            Assert.AreEqual("instance.MethodWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrites name of translatable static method")]
        public void VisitMemberAccessExpression_RewritesNameOfTranslatableStaticMethod()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node1);

            Assert.AreEqual("TestTypes.TranslatedClass.StaticMethodWithTranslatedName", translatedNode.ToSource());
        }

        // NSubstitute returns an invalid proxy for StructureService.TranslatedMember causing errors.
        //[TestMethod("VisitMemberAccessExpression rewrites name of translatable static method in generic class")]
        public void VisitMemberAccessExpression_RewritesNameOfTranslatableStaticMethodInGenericClass()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node1);

            Assert.AreEqual("TestTypes.TranslatedGenericClass<TestTypes.TranslatedTypeArgumentClass>.StaticMethodWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrite name of translatable property")]
        public void VisitMemberAccessExpression_RewriteNameOfTranslatableProperty()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.PropertyWithOriginalName").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(OriginalPropertyWithVariableName);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalPropertyWithVariableName).Returns(TranslatedPropertyWithVariableName);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitMemberAccessExpression(node);

            Assert.AreEqual("instance.PropertyWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrite name of translatable static property")]
        public void VisitMemberAccessExpression_RewriteNameOfTranslatableStaticProperty()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node1);

            Assert.AreEqual("TestTypes.TranslatedClass.StaticPropertyWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrite name of translatable static property in generic class")]
        public void VisitMemberAccessExpression_RewriteNameOfTranslatableStaticPropertyInGenericClass()
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node1);

            Assert.AreEqual("TestTypes.TranslatedGenericClass<TestTypes.TranslatedTypeArgumentClass>.StaticPropertyWithTranslatedName", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrites type arguments of translatable method")]
        public void VisitiMemberAccessExpression_RewritesTypeArgumentsOfTranslatableMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.GenericMethod<TestTypes.OriginalTypeArgumentClass>").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(OriginalGenericMethodWithVariableTypeArgument);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalGenericMethodWithVariableTypeArgument).Returns(TranslatedGenericMethodWithVariableTypeArgument);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitMemberAccessExpression(node);

            Assert.AreEqual("instance.GenericMethod<TestTypes.TranslatedTypeArgumentClass>", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression rewrites type arguments of translatable static method")]
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

            var translatedNode = rewriter.VisitMemberAccessExpression(node1);

            Assert.AreEqual("TestTypes.TranslatedClass.StaticGenericMethod<TestTypes.TranslatedTypeArgumentClass>", translatedNode.ToSource());
        }

        [TestMethod("VisitMemberAccessExpression verifies translatable event")]
        public void VisitMemberAccessExpression_VerifiesTranslatableEvent()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Event").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalEvent);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalEvent).Returns(TranslatedEvent);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitMemberAccessExpression(node1);

            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalEvent,
                Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("VisitMemberAccessExpression verifies translatable static event")]
        public void VisitMemberAccessExpression_VerifiesTranslatableStaticEvent()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass.StaticEvent").GetRoot();
            var node1 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(0);
            var node2 = root.AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node1).Returns(OriginalStaticEvent);
            resolver.GetDescription(node2).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            structureService.TranslateMember(OriginalStaticEvent).Returns(TranslatedStaticEvent);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitMemberAccessExpression(node1);

            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalStaticEvent,
                Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("VisitMemberAccessExpression verifies translatable field")]
        public void VisitMemberAccessExpression_VerifiesTranslatableField()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Field").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(OriginalField);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalField).Returns(TranslatedField);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitMemberAccessExpression(node);

            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalField,
                Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("VisitMemberAccessExpression verifies translatable static field")]
        public void VisitMemberAccessExpression_VerifiesTranslatableStaticField()
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

            rewriter.VisitMemberAccessExpression(node1);

            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalStaticField,
                Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("VisitMemberAccessExpression verifies translatable method")]
        public void VisitMemberAccessExpression_VerifiesTranslatableMethod()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Method").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(OriginalMethod);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalMethod).Returns(TranslatedMethod);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitMemberAccessExpression(node);

            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalMethod,
                Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("VisitMemberAccessExpression verifies translatable static method")]
        public void VisitMemberAccessExpression_VerifiesTranslatableStaticMethod()
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

            rewriter.VisitMemberAccessExpression(node1);

            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalStaticMethod,
                Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("VisitMemberAccessExpression verifies translatable property")]
        public void VisitMemberAccessExpression_VerifiesTranslatableProperty()
        {
            var root = SyntaxFactory.ParseSyntaxTree("instance.Property").GetRoot();
            var node = root.AllDescendantNodes<MemberAccessExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetDescription(node).Returns(OriginalProperty);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateMember(OriginalProperty).Returns(TranslatedProperty);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitMemberAccessExpression(node);

            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalProperty,
                Arg.Any<MemberVerificationAspect[]>());
        }

        [TestMethod("VisitMemberAccessExpression verifies translatable static property")]
        public void VisitMemberAccessExpression_VerifiesTranslatableStaticProperty()
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

            rewriter.VisitMemberAccessExpression(node1);

            structureService.Received().VerifyType(OriginalType);
            structureService.Received().VerifyMember(
                OriginalStaticProperty,
                Arg.Any<MemberVerificationAspect[]>());
        }

        #endregion

        #region VisitObjectCreationExpression

        [TestMethod("VisitObjectCreationExpression does not rewrite type of non-translatable constructor")]
        public void VisitObjectCreationExpression_DoesNotRewriteTypeOfNonTranslatableConstructor()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.ConstantClass()").GetRoot();
            var node = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node).Returns(ConstantDefaultConstructor);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitObjectCreationExpression(node);

            Assert.AreEqual("new TestTypes.ConstantClass()", translatedNode.ToSource());
        }

        [TestMethod("VisitObjectCreationExpression does not rewrite nested type of non-translatable constructor")]
        public void VisitObjectCreationExpression_DoesNotRewriteNewstedTypeNonTranslatableConstructor()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.ConstantClass.NestedClass()").GetRoot();
            var node = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node).Returns(ConstantNestedConstructor);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNestedType).Returns(ConstantNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitObjectCreationExpression(node);

            Assert.AreEqual("new TestTypes.ConstantClass.NestedClass()", translatedNode.ToSource());
        }

        [TestMethod("VisitObjectCreationExpression rewrites arguments of non-translatable constructor")]
        public void VisitObjectCreationExpression_RewritesArgumentsOfNonTranslatableConstructor()
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

            var translatedNode = rewriter.VisitObjectCreationExpression(node1);

            Assert.AreEqual("new TestTypes.ConstantClass(new TestTypes.TranslatedArgumentClass())", translatedNode.ToSource());
        }

        [TestMethod("VisitObjectCreationExpression rewrites initializers of non-translatable constructor")]
        public void VisitObjectCreationExpression_RewritesInitializersOfNonTranslatableConstructor()
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

            var translatedNode = rewriter.VisitObjectCreationExpression(node1);

            var expected = string.Join(
                Environment.NewLine,
                "new TestTypes.ConstantClass()",
                "{Field = new TestTypes.TranslatedFieldClass(), Property = new TestTypes.TranslatedPropertyClass()}");
            Assert.AreEqual(expected, translatedNode.ToSource());
        }

        [TestMethod("VisitObjectCreationExpression does not verify non-translatable constructor")]
        public void VisitObjectCreationExpression_DoesNotVerifyNonTranslatableConstructor()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.ConstantClass()").GetRoot();
            var node = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node).Returns(ConstantDefaultConstructor);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitObjectCreationExpression(node);

            structureService.VerifyType(ConstantType);
            structureService.VerifyMember(ConstantDefaultConstructor);
        }


        [TestMethod("VisitObjectCreationExpression rewrites type of translatable constructor")]
        public void VisitObjectCreationExpression_RewritesTypeOfTranslatableConstructor()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass()").GetRoot();
            var node = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node).Returns(OriginalDefaultConstructor);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);
            
            var translatedNode = rewriter.VisitObjectCreationExpression(node);

            Assert.AreEqual("new TestTypes.TranslatedClass()", translatedNode.ToSource());
        }

        [TestMethod("VisitObjectCreationExpression rewrites nested type of translatable constructor")]
        public void VisitObjectCreationExpression_RewritesNestedTypeOfTranslatableConstructor()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass.NestedClass()").GetRoot();
            var node = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node).Returns(OriginalNestedConstructor);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNestedType).Returns(TranslatedNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitObjectCreationExpression(node);

            Assert.AreEqual("new TestTypes.TranslatedClass.NestedClass()", translatedNode.ToSource());
        }

        [TestMethod("VisitObjectCreationExpression rewrites type arguments of translatable constructor")]
        public void VisitObjectCreationExpression_RewritesTypeArgumentsOfTranslatableConstructor()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalGenericClass<OriginalTypeArgumentClass>()").GetRoot();
            var node = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().ElementAt(0);

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node).Returns(OriginalGenericConstructorWithVariableTypeArgument);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalGenericTypeWithVariableTypeArgument).Returns(TranslatedGenericTypeWithVariableTypeArgument);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitObjectCreationExpression(node);

            Assert.AreEqual("new TestTypes.TranslatedGenericClass<TestTypes.TranslatedTypeArgumentClass>()", translatedNode.ToSource());
        }

        [TestMethod("VisitObjectCreationExpression rewrites arguments of translatable constructor")]
        public void VisitObjectCreationExpression_RewritesArgumentsOfTranslatableConstructor()
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

            var translatedNode = rewriter.VisitObjectCreationExpression(node1);

            Assert.AreEqual("new TestTypes.TranslatedClass(new TestTypes.TranslatedArgumentClass())", translatedNode.ToSource());
        }
        
        [TestMethod("VisitObjectCreationExpression rewrites initializer of translatable constructor")]
        public void VisitObjectCreationExpression_RewritesInitializerOfTranslatableConstructor()
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

            var translatedNode = rewriter.VisitObjectCreationExpression(node1);

            var expected = string.Join(
                Environment.NewLine,
                "new TestTypes.TranslatedClass()",
                "{Field = new TestTypes.TranslatedFieldClass(), Property = new TestTypes.TranslatedPropertyClass()}");
            Assert.AreEqual(expected, translatedNode.ToSource());
        }

        [TestMethod("VisitObjectCreationExpression verifies translatable constructor")]
        public void VisitObjectCreationExpression_VerifiesTranslableConstructor()
        {
            var root = SyntaxFactory.ParseSyntaxTree("new TestTypes.OriginalClass()").GetRoot();
            var node = root.AllDescendantNodes<ObjectCreationExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetConstructorDescription(node).Returns(OriginalDefaultConstructor);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(OriginalType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitObjectCreationExpression(node);

            structureService.VerifyType(OriginalType);
            structureService.VerifyMember(OriginalDefaultConstructor);
        }

        #endregion

        #region VisitTypeOfExpression

        [TestMethod("VisitTypeOfExpression does not rewrite type of non-translatable typeof")]
        public void VisitTypeOfExpression_DoesNotRewriteTypeOfNonTranslatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.ConstantClass)").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitTypeOfExpression(node);

            Assert.AreEqual("typeof(TestTypes.ConstantClass)", translatedNode.ToSource());
        }

        [TestMethod("VisitTypeOfExpression does not rewrites nested type of non-translatable typeof")]
        public void VisitTypeOfExpression_DoesNotRewriteNestedTypeOfNonTranslatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.ConstantClass.NestedClass)").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNestedType).Returns(ConstantNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitTypeOfExpression(node);

            Assert.AreEqual("typeof(TestTypes.ConstantClass.NestedClass)", translatedNode.ToSource());
        }

        [TestMethod("VisitTypeOfExpression does not rewrite array type of non-translatable typeof")]
        public void VisitTypeOfExpression_DoesNotRewriteArrayTypeOfNonTranslatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.ConstantClass[])").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantArrayType).Returns(ConstantArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitTypeOfExpression(node);

            Assert.AreEqual("typeof(TestTypes.ConstantClass[])", translatedNode.ToSource());
        }

        [TestMethod("VisitTypeOfExpression does not rewrite nullable type of non-translatable typeof")]
        public void VisitTypeOfExpression_DoesNotRewriteNullableTypeOfNonTranslatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.ConstantClass.NestedClass)").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNestedType).Returns(ConstantNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitTypeOfExpression(node);

            Assert.AreEqual("typeof(TestTypes.ConstantClass.NestedClass)", translatedNode.ToSource());
        }

        [TestMethod("VisitTypeOfExpression does not verify non-translatable typeof")]
        public void VisitTypeOfExpression_DoesNotVerifyNonTranslatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.ConstantClass)").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitTypeOfExpression(node);

            structureService.DidNotReceive().VerifyType(ConstantType);
        }


        [TestMethod("VisitTypeOfExpression rewrites type of translatable typeof")]
        public void VisitTypeOfExpression_RewritesTypeOfTranslatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.OriginalClass)").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitTypeOfExpression(node);

            Assert.AreEqual("typeof(TestTypes.TranslatedClass)", translatedNode.ToSource());
        }

        [TestMethod("VisitTypeOfExpression rewrites array type of translatable typeof")]
        public void VisitTypeOfExpression_RewritesArrayTypeOfTranlatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.OriginalClass[])").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalArrayType).Returns(TranslatedArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitTypeOfExpression(node);

            Assert.AreEqual("typeof(TestTypes.TranslatedClass[])", translatedNode.ToSource());
        }

        [TestMethod("VisitTypeOfExpression rewrites nested type of translatable typeof")]
        public void VisitTypeOfExpression_RewritesNestedTypeOfTranslatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.OriginalClass.NestedClass)").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNestedType).Returns(TranslatedNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitTypeOfExpression(node);

            Assert.AreEqual("typeof(TestTypes.TranslatedClass.NestedClass)", translatedNode.ToSource());
        }

        [TestMethod("VisitTypeOfExpression rewrites nullable type of translatable typeof")]
        public void VisitTypeOfExpressionRewritesNullableTypeOfTranslatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.OriginalStruct?)").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNullableType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNullableType).Returns(TranslatedNullableType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitTypeOfExpression(node);

            Assert.AreEqual("typeof(TestTypes.TranslatedStruct? )", translatedNode.ToSource());
        }

        [TestMethod("VisitTypeOfExpression verifies translatable typeof")]
        public void VisitTypeOfExpression_VerifiesTranslatableTypeOf()
        {
            var root = SyntaxFactory.ParseSyntaxTree("typeof(TestTypes.OriginalClass)").GetRoot();
            var node = root.AllDescendantNodes<TypeOfExpressionSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitTypeOfExpression(node);

            structureService.Received().VerifyType(OriginalType);
        }
        
        #endregion VisitTypeOfExpression

        #region VisitVariableDeclaration

        [TestMethod("VisitVariableDeclaration does not rewrite type of non-translatable variable")]
        public void VisitVariableDeclaration_DoesNotRewriteTypeOfNonTranslatableVariable()
        {
            
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitVariableDeclaration(node);

            Assert.AreEqual("TestTypes.ConstantClass instance", translatedNode.ToSource());
        }

        [TestMethod("VisitVariableDeclaration does not rewrite array type of non-translatable variable")]
        public void VisitVariableDeclaration_DoesNotRewriteArrayTypeOfNonTranslatableVariable()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass.NestedClass instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNestedType).Returns(ConstantNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitVariableDeclaration(node);

            Assert.AreEqual("TestTypes.ConstantClass.NestedClass instance", translatedNode.ToSource());
        }

        [TestMethod("VisitVariableDeclaration does not rewrite nested type of non-translatable variable")]
        public void VisitVariableDeclaration_DoesNotRewriteNestedTypeOfNonTranslatableVariable()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass[] instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantArrayType).Returns(ConstantArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitVariableDeclaration(node);

            Assert.AreEqual("TestTypes.ConstantClass[] instance", translatedNode.ToSource());
        }

        [TestMethod("VisitVariableDeclaration does not rewrite nullable type of non-translatable variable")]
        public void VisitVariableDeclaration_DoesNotRewriteNullableTypeOfNonTranslatableVariable()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass.NestedClass instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantNestedType).Returns(ConstantNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitVariableDeclaration(node);

            Assert.AreEqual("TestTypes.ConstantClass.NestedClass instance", translatedNode.ToSource());
        }

        [TestMethod("VisitVariableDeclaration does verify non-translatable variable")]
        public void VisitVariableDeclarationDoesVerifyNonTranslatableVariable()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.ConstantClass instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(ConstantType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(ConstantType).Returns(ConstantType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitVariableDeclaration(node);

            structureService.DidNotReceive().VerifyType(ConstantType);
        }


        [TestMethod("VisitVariableDeclaration rewrites type of translatable variable")]
        public void VisitVariableDeclaration_RewritesTypeOfTranslatableVariable()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitVariableDeclaration(node);

            Assert.AreEqual("TestTypes.TranslatedClass instance", translatedNode.ToSource());
        }

        [TestMethod("VisitVariableDeclaration rewrites array type of translatable variable")]
        public void VisitVariableDeclaration_RewritesArrayTypeOfTranslatableVariable()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass[] instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalArrayType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalArrayType).Returns(TranslatedArrayType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitVariableDeclaration(node);

            Assert.AreEqual("TestTypes.TranslatedClass[] instance", translatedNode.ToSource());
        }

        [TestMethod("VisitVariableDeclaration rewrites nested type of translatable variable")]
        public void VisitVariableDeclaration_RewritesNestedTypeOfTranslatableVariable()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass.NestedClass instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNestedType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNestedType).Returns(TranslatedNestedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitVariableDeclaration(node);

            Assert.AreEqual("TestTypes.TranslatedClass.NestedClass instance", translatedNode.ToSource());
        }

        [TestMethod("VisitVariableDeclaration rewrites nullable type of translatable variable")]
        public void VisitVariableDeclaration_RewritesNullableTypeOfTranslatableVariable()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalStruct? instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalNullableType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalNullableType).Returns(TranslatedNullableType);
            var rewriter = new TypeRewriter(resolver, structureService);

            var translatedNode = rewriter.VisitVariableDeclaration(node);

            Assert.AreEqual("TestTypes.TranslatedStruct? instance", translatedNode.ToSource());
        }

        [TestMethod("VisitVariableDeclaration verifies translatable variable")]
        public void VisitVariableDeclaration_VerifiesTranslatableVariable()
        {
            var root = SyntaxFactory.ParseSyntaxTree("TestTypes.OriginalClass instance").GetRoot();
            var node = root.AllDescendantNodes<VariableDeclarationSyntax>().First();

            var resolver = Substitute.For<ICompileTimeDescriptionResolver>();
            resolver.GetTypeDescription(node).Returns(OriginalType);
            var structureService = Substitute.For<IStructureService>();
            structureService.TranslateType(OriginalType).Returns(TranslatedType);
            var rewriter = new TypeRewriter(resolver, structureService);

            rewriter.VisitVariableDeclaration(node);

            structureService.Received().VerifyType(OriginalType);
        }

        #endregion VisitVariableDeclaration
    }
}