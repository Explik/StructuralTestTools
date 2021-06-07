using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using OleVanSanten.TestTools.Structure;
using OleVanSanten.TestTools.TypeSystem;

namespace TestTools_Tests.Structure
{
    [TestClass]
    public class CompileTimeDescriptionResolverTests
    {
        [TestMethod("GetConstructorDescription correctly returns for object instantiation")]
        public void GetConstructorDescription_CorrectlyReturnsforObjectInstantiation()
        {
            var compilation = CreateCompilation("new System.Text.StringBuilder()");
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var input = Get<ObjectCreationExpressionSyntax>(compilation);
            var output = resolver.GetConstructorDescription(input);

            Assert.AreEqual("StringBuilder", output.DeclaringType.Name);
            Assert.AreEqual(0, output.GetParameters().Length);
        }

        [TestMethod("GetConstructorDescription correctly returns for class declaration")]
        public void GetConstructorDescription_CorrectlyReturnsForClassDeclaration()
        {
            var compilation = CreateCompilation("class ClassName { ClassName() {} }");
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var input = Get<ConstructorDeclarationSyntax>(compilation);
            var output = resolver.GetConstructorDescription(input);

            Assert.AreEqual("ClassName", output.DeclaringType.Name);
            Assert.AreEqual(0, output.GetParameters().Length);
        }

        [TestMethod("GetMemberDescription correctly returns for event access")]
        public void GetMemberDescription_CorrectlyReturnsForEventAccess()
        {
            var compilation = CreateCompilation("class ClassName { event EventHandler EventName; ClassName () { this.EventName += null; } }");
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var input = Get<MemberAccessExpressionSyntax>(compilation);
            var output = resolver.GetMemberDescription(input);

            Assert.IsInstanceOfType(output, typeof(EventDescription));
            Assert.AreEqual("ClassName", output.DeclaringType.Name);
            Assert.AreEqual("EventName", output.Name);
        }

        [TestMethod("GetMemberDescription correctly returns for field access")]
        public void GetMemberDescription_CorrectlyReturnsForFieldAccess()
        {
            var compilation = CreateCompilation("class ClassName { int FieldName; ClassName () { this.FieldName = 5; } }");
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var input = Get<MemberAccessExpressionSyntax>(compilation);
            var output = resolver.GetMemberDescription(input);

            Assert.IsInstanceOfType(output, typeof(FieldDescription));
            Assert.AreEqual("ClassName", output.DeclaringType.Name);
            Assert.AreEqual("FieldName", output.Name);
        }

        [TestMethod("GetMemberDescription correctly returns for method access")]
        public void GetMemberDescription_CorrectlyReturnsForMethodAccess()
        {
            var compilation = CreateCompilation("class ClassName { int MethodName () {} ClassName () { this.MethodName(); } }");
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var input = Get<MemberAccessExpressionSyntax>(compilation);
            var output = resolver.GetMemberDescription(input);

            Assert.IsInstanceOfType(output, typeof(MethodDescription));
            Assert.AreEqual("ClassName", output.DeclaringType.Name);
            Assert.AreEqual("MethodName", output.Name);
            Assert.AreEqual(0, ((MethodDescription)output).GetParameters().Length);
        }

        [TestMethod("GetMemberDescription correctly returns for property access")]
        public void GetMemberDescription_CorrectlyReturnsForPropertyAccess()
        {
            var compilation = CreateCompilation("class ClassName { int PropertyName { get; } ClassName () { this.PropertyName = 5; } }");
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var input = Get<MemberAccessExpressionSyntax>(compilation);
            var output = resolver.GetMemberDescription(input);

            Assert.IsInstanceOfType(output, typeof(PropertyDescription));
            Assert.AreEqual("ClassName", output.DeclaringType.Name);
            Assert.AreEqual("PropertyName", output.Name);
        }

        [TestMethod("GetMethodDescription correctly returns for method access")]
        public void GetMethodDescription_CorrectlyReturnsForMethodAccess()
        {
            var compilation = CreateCompilation("class ClassName { int MethodName () {} ClassName () { this.MethodName(); } }");
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var input = Get<InvocationExpressionSyntax>(compilation);
            var output = resolver.GetMethodDescription(input);

            Assert.IsInstanceOfType(output, typeof(MethodDescription));
            Assert.AreEqual("ClassName", output.DeclaringType.Name);
            Assert.AreEqual("MethodName", output.Name);
            Assert.AreEqual(0, ((MethodDescription)output).GetParameters().Length);
        }

        [TestMethod("GetMethodDescription correctly returns for method declaration")]
        public void GetMethodDescription_CorrectlyReturnsForMethodDeclaration()
        {
            var compilation = CreateCompilation("class ClassName { int MethodName () }");
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var input = Get<MethodDeclarationSyntax>(compilation);
            var output = resolver.GetMethodDescription(input);

            Assert.IsInstanceOfType(output, typeof(MethodDescription));
            Assert.AreEqual("ClassName", output.DeclaringType.Name);
            Assert.AreEqual("MethodName", output.Name);
            Assert.AreEqual(0, ((MethodDescription)output).GetParameters().Length);
        }

        // This test won't work right as is, because TemplateAttribute is not recognized as an attribute
        // [TestMethod("GetTemplatedAttribute correctly returns for attribute applied on class")]
        public void GetTemplatedAttributeCorrectlyReturnsForAttribute()
        {
            string source = string.Join(
                Environment.NewLine,
                "[TemplateAttribute]",
                "class ClassName { }",
                "[AttributeEquivalentAttribute(\"AttributeName\")]",
                "class TemplateAttribute : Attribute { }");
            var compilation = CreateCompilation(source);
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var input = Get<AttributeSyntax>(compilation);
            var output = resolver.GetTemplatedAttribute(input);

            Assert.AreEqual("AttributeName", output.EquavilentAttribute);
        }

        // TODO test for TemplatedAttribute for attribute applied on method 

        // TODO test for TemplatedAttribute for class declaration

        // TODO test for TemplatedAttribute for method declaration

        [TestMethod("GetTypeDescription correctly returns for type declaration")]
        public void GetTypeDescription_CorrectlyReturnsForTypeDeclaration()
        {
            var compilation = CreateCompilation("class ClassName { }");
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var input = Get<TypeDeclarationSyntax>(compilation);
            var output = resolver.GetTypeDescription(input);

            Assert.AreEqual("ClassName", output.Name);
        }

        [TestMethod("GetTypeDescription correctly returns for variable declaration")]
        public void GetTypeDescription_CorrectlyReturnsForVariableDeclaration()
        {
            var compilation = CreateCompilation("class ClassName { int MethodName() { int value; } }");
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var input = Get<VariableDeclarationSyntax>(compilation);
            var output = resolver.GetTypeDescription(input);

            Assert.AreEqual("Int32", output.Name);
        }

        private Compilation CreateCompilation(string source)
        {
            return CSharpCompilation.Create("compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));
        }

        private T Get<T>(Compilation compilation) where T : SyntaxNode
        {
            var syntaxTree = compilation.SyntaxTrees.First();
            
            var root = syntaxTree.GetRoot();

            return root.DescendantNodes().OfType<T>().First();
        }
    }
}
