using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using OleVanSanten.TestTools.Structure;
using OleVanSanten.TestTools.TypeSystem;

namespace TestTools_Tests
{
    [TestClass]
    public class TypeRewriterTests
    {
        class Class
        {
            public void GenericMethod<T>()
            {
            }
        }

        class ClassA
        {
            public int Field_A;
            public int Property_A { get; set; }
            public event EventHandler Event_A;
            public void Method_A()
            {
            }
            public void GenericMethod_A<T>()
            {
            }
        }

        class ClassB
        {
            public int Field_B;
            public int Property_B { get; set; }
            public event EventHandler Event_B;
            public void Method_B()
            {
            }
            public void GenericMethod_B<T>()
            {
            }
        }

        class GenericClass<T>
        {
        }

        [TestMethod("VisitObjectCreationExpression transforms constructor type")]
        public void VisitObjectCreationExpression_TransformsConstructorType()
        {
            // Setting up semantic data
            var originalType = new RuntimeTypeDescription(typeof(ClassA));
            var translatedType = new RuntimeTypeDescription(typeof(ClassB));
            var originalConstructor = new RuntimeConstructorDescription(typeof(ClassB).GetConstructor(new Type[0]));
            var translatedConstructor = new RuntimeConstructorDescription(typeof(ClassA).GetConstructor(new Type[0]));

            // Setting up services to handle semantic data
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            var structureService = Substitute.For<IStructureService>();

            syntaxResolver.GetConstructorDescription((ObjectCreationExpressionSyntax)null).ReturnsForAnyArgs(translatedConstructor);
            structureService.IsTranslatableType(originalType).Returns(true);
            structureService.TranslateType(originalType).Returns(translatedType);
            structureService.TranslateMember(translatedConstructor).Returns(originalConstructor);

            // Setting up and testing rewriter ouput
            var rewriter = new TypeRewriter(syntaxResolver, structureService);
            var input = Parse<ObjectCreationExpressionSyntax>("new TestTools_Tests.ClassA()");

            var output = rewriter.VisitObjectCreationExpression(input);

            Assert.AreEqual("new TestTools_Tests.ClassB()", output.ToFullString());
        }

        [TestMethod("VisitInvocationExpression transforms method name")]
        public void VisitInvocationExpression_TransformsMethodName()
        {
            // Setting up semantic data
            var originalType = new RuntimeTypeDescription(typeof(ClassA));
            var translatedType = new RuntimeTypeDescription(typeof(ClassB));
            var originalMethod = new RuntimeMethodDescription(typeof(ClassA).GetMethod("Method_A", new Type[0]));
            var translatedMethod = new RuntimeMethodDescription(typeof(ClassB).GetMethod("Method_B", new Type[0]));

            // Setting up services to handle semantic data
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            var structureService = Substitute.For<IStructureService>();

            syntaxResolver.GetMethodDescription((InvocationExpressionSyntax)null).ReturnsForAnyArgs(originalMethod);
            structureService.IsTranslatableType(originalType).Returns(true);
            structureService.TranslateType(originalType).Returns(translatedType);
            structureService.TranslateMember(originalMethod).Returns(translatedMethod);

            // Setting up and testing rewriter ouput
            var rewriter = new TypeRewriter(syntaxResolver, structureService);
            var input = Parse<InvocationExpressionSyntax>("value.Method_A()");

            var output = rewriter.VisitInvocationExpression(input);

            Assert.AreEqual("value.Method_B()", output.ToFullString());
        }

        [TestMethod("VisitInvocationExpression transforms name of generic method")]
        public void VisitInvocationExpression_TransformsNameOfGenericMethod()
        {
            // Setting up semantic data
            var originalType = new RuntimeTypeDescription(typeof(ClassA));
            var translatedType = new RuntimeTypeDescription(typeof(ClassB));
            var originalMethod = new RuntimeMethodDescription(typeof(ClassA).GetMethod("GenericMethod_A", new Type[0]).MakeGenericMethod(typeof(int)));
            var translatedMethod = new RuntimeMethodDescription(typeof(ClassB).GetMethod("GenericMethod_B", new Type[0]).MakeGenericMethod(typeof(int)));

            // Setting up services to handle semantic data
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            var structureService = Substitute.For<IStructureService>();

            syntaxResolver.GetMethodDescription((InvocationExpressionSyntax)null).ReturnsForAnyArgs(originalMethod);
            structureService.IsTranslatableType(originalType).Returns(true);
            structureService.TranslateType(originalType).Returns(translatedType);
            structureService.TranslateMember(originalMethod).Returns(translatedMethod);

            // Setting up and testing rewriter ouput
            var rewriter = new TypeRewriter(syntaxResolver, structureService);
            var input = Parse<InvocationExpressionSyntax>("value.GenericMethod_A<int>()");

            var output = rewriter.VisitInvocationExpression(input);

            Assert.AreEqual("value.GenericMethod_B<Int32>()", output.ToFullString());
        }

        [TestMethod("VisitInvocationExpression transforms type arguments of generic method ")]
        public void VisitInvocationExpression_TransformsTypeArgumentsOfGenericMethod()
        {
            // Setting up semantic data
            var type = new RuntimeTypeDescription(typeof(Class));
            var typeArgument = new RuntimeTypeDescription(typeof(ClassA));
            var originalMethod = new RuntimeMethodDescription(typeof(Class).GetMethod("GenericMethod", new Type[0]).MakeGenericMethod(typeof(ClassA)));
            var translatedMethod = new RuntimeMethodDescription(typeof(Class).GetMethod("GenericMethod", new Type[0]).MakeGenericMethod(typeof(ClassB)));

            // Setting up services to handle semantic data
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            var structureService = Substitute.For<IStructureService>();

            syntaxResolver.GetMethodDescription((InvocationExpressionSyntax)null).ReturnsForAnyArgs(originalMethod);
            structureService.IsTranslatableType(type).Returns(true);
            structureService.IsTranslatableType(typeArgument).Returns(true);
            structureService.TranslateType(type).Returns(type);
            structureService.TranslateMember(originalMethod).Returns(translatedMethod);

            // Setting up and testing rewriter ouput
            var rewriter = new TypeRewriter(syntaxResolver, structureService);
            var input = Parse<InvocationExpressionSyntax>("value.GenericMethod<ClassA>()");

            var output = rewriter.VisitInvocationExpression(input);

            Assert.AreEqual("value.GenericMethod<ClassB>()", output.ToFullString());
        }

        [TestMethod("VisitMemberAccessExpression transforms event name")]
        public void VisitMemberAccessExpression_TransformsEventName()
        {
            // Setting up semantic data
            var originalType = new RuntimeTypeDescription(typeof(ClassA));
            var translatedType = new RuntimeTypeDescription(typeof(ClassB));
            var originalEvent = new RuntimeEventDescription(typeof(ClassA).GetEvent("Event_A"));
            var translatedEvent = new RuntimeEventDescription(typeof(ClassB).GetEvent("Event_B"));

            // Setting up services to handle semantic data
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            var structureService = Substitute.For<IStructureService>();

            syntaxResolver.GetMemberDescription(null).ReturnsForAnyArgs(originalEvent);
            structureService.IsTranslatableType(originalType).Returns(true);
            structureService.TranslateType(originalType).Returns(translatedType);
            structureService.TranslateMember(originalEvent).Returns(translatedEvent);

            // Setting up and testing rewriter
            var rewriter = new TypeRewriter(syntaxResolver, structureService);
            var input = Parse<MemberAccessExpressionSyntax>("value.Event_A");

            var output = rewriter.VisitMemberAccessExpression(input);

            Assert.AreEqual("value.Event_B", output.ToFullString());
        }

        [TestMethod("VisitMemberAccessExpression transforms field name")]
        public void VisitMemberAccessExpression_TransformsFieldName()
        {
            // Setting up semantic data
            var originalType = new RuntimeTypeDescription(typeof(ClassA));
            var translatedType = new RuntimeTypeDescription(typeof(ClassB));
            var originalField = new RuntimeFieldDescription(typeof(ClassA).GetField("Field_A"));
            var translatedField = new RuntimeFieldDescription(typeof(ClassB).GetField("Field_B"));

            // Setting up services to handle semantic data
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            var structureService = Substitute.For<IStructureService>();

            syntaxResolver.GetMemberDescription(null).ReturnsForAnyArgs(originalField);
            structureService.IsTranslatableType(originalType).Returns(true);
            structureService.TranslateType(originalType).Returns(translatedType);
            structureService.TranslateMember(originalField).Returns(translatedField);

            // Setting up and testing rewriter ouput
            var rewriter = new TypeRewriter(syntaxResolver, structureService);
            var input = Parse<MemberAccessExpressionSyntax>("value.Field_A");

            var output = rewriter.VisitMemberAccessExpression(input);

            Assert.AreEqual("value.Field_B", output.ToFullString());
        }

        [TestMethod("VisitMemberAccessExpression transforms property name")]
        public void VisitMemberAccessExpression_TransformsPropertyName()
        {
            // Setting up semantic data
            var originalType = new RuntimeTypeDescription(typeof(ClassA));
            var translatedType = new RuntimeTypeDescription(typeof(ClassB));
            var originalProperty = new RuntimePropertyDescription(typeof(ClassA).GetProperty("Property_A"));
            var translatedProperty = new RuntimePropertyDescription(typeof(ClassB).GetProperty("Property_B"));

            // Setting up services to handle semantic data
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            var structureService = Substitute.For<IStructureService>();

            syntaxResolver.GetMemberDescription(null).ReturnsForAnyArgs(originalProperty);
            structureService.IsTranslatableType(originalType).Returns(true);
            structureService.TranslateType(originalType).Returns(translatedType);
            structureService.TranslateMember(originalProperty).Returns(translatedProperty);

            // Setting up and testing rewriter ouput
            var rewriter = new TypeRewriter(syntaxResolver, structureService);
            var input = Parse<MemberAccessExpressionSyntax>("value.Property_A");

            var output = rewriter.VisitMemberAccessExpression(input);

            Assert.AreEqual("value.Property_B", output.ToFullString());
        }
 
        [TestMethod("VisitVariableDeclaration transforms variable type")]
        public void VisitVariableDeclaration_TransformsVariableType()
        {
            // Setting up semantic data VariableDeclarationSyntax.ToString() does not add whitespace
            var originalType = new RuntimeTypeDescription(typeof(ClassA));
            var translatedType = new RuntimeTypeDescription(typeof(ClassB));

            // Setting up services to handle semantic data
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            var structureService = Substitute.For<IStructureService>();

            syntaxResolver.GetTypeDescription((VariableDeclarationSyntax)null).ReturnsForAnyArgs(originalType);
            structureService.IsTranslatableType(originalType).Returns(true);
            structureService.TranslateType(originalType).Returns(translatedType);

            // Setting up and testing rewriter ouput
            var rewriter = new TypeRewriter(syntaxResolver, structureService);
            var input = Parse<VariableDeclarationSyntax>("ClassA value");

            var output = (VariableDeclarationSyntax)rewriter.VisitVariableDeclaration(input);

            Assert.AreEqual("ClassB", output.Type.ToFullString());
            Assert.AreEqual("value", output.Variables.ToFullString());
        }

        [TestMethod("VisitVariableDeclaration transforms element type of array variable type")]
        public void VisitVariableDeclaration_TransformsElementTypeOfGenericVariableType()
        {
            // Setting up semantic data
            var originalType = new RuntimeTypeDescription(typeof(ClassA[]));
            var translatedType = new RuntimeTypeDescription(typeof(ClassB[]));

            // Setting up services to handle semantic data
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            var structureService = Substitute.For<IStructureService>();

            syntaxResolver.GetTypeDescription((VariableDeclarationSyntax)null).ReturnsForAnyArgs(originalType);
            structureService.IsTranslatableType(originalType).Returns(true);
            structureService.TranslateType(originalType).Returns(translatedType);

            // Setting up and testing rewriter output
            var rewriter = new TypeRewriter(syntaxResolver, structureService);
            var input = Parse<VariableDeclarationSyntax>("ClassA[] value");

            var output = (VariableDeclarationSyntax)rewriter.VisitVariableDeclaration(input);

            Assert.AreEqual("ClassB[]", output.Type.ToFullString());
            Assert.AreEqual("value", output.Variables.ToFullString());
        }

        [TestMethod("VisitVariableDeclaration transforms type arguments of generic variable type")]
        public void VisitVariableDeclaration_TransformsTypeArgumentsOfGenericVariableType()
        {
            // Setting up semantic data
            var originalType = new RuntimeTypeDescription(typeof(GenericClass<ClassA>));
            var translatedType = new RuntimeTypeDescription(typeof(GenericClass<ClassB>));

            // Setting up services to handle semantic data
            var syntaxResolver = Substitute.For<ICompileTimeDescriptionResolver>();
            var structureService = Substitute.For<IStructureService>();

            syntaxResolver.GetTypeDescription((VariableDeclarationSyntax)null).ReturnsForAnyArgs(originalType);
            structureService.IsTranslatableType(originalType).Returns(true);
            structureService.TranslateType(originalType).Returns(translatedType);

            // Setting up and testing rewriter output
            var rewriter = new TypeRewriter(syntaxResolver, structureService);
            var input = Parse<VariableDeclarationSyntax>("GenericClass<ClassA> value");

            var output = (VariableDeclarationSyntax)rewriter.VisitVariableDeclaration(input);

            Assert.AreEqual("GenericClass<ClassB>", output.Type.ToFullString());
            Assert.AreEqual("value", output.Variables.ToFullString());
        }

        private T Parse<T>(string source) where T : SyntaxNode
        {
            var root = SyntaxFactory.ParseSyntaxTree(source).GetRoot();
            return (T)root.DescendantNodes().First(n => n is T);
        }
    }
}
