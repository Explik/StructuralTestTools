using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace TestTools_Tests.TypeSystem
{
    [TestClass]
    public class CompileTimeTypeDescriptionTests
    {
        [TestMethod("GetElementType correctly returns for array type")]
        public void GetElementType_CorrectlyReturnsForArrayType()
        {
            var type = GetTypeDescription("int[]");
            var elementType = GetTypeDescription("int");

            Assert.AreEqual(elementType, type.GetElementType());
        }

        [TestMethod("GetElementType returns null for non-array type")]
        public void GetElementType_ReturnsNullForNonArrayType()
        {
            var type = GetTypeDescription("int");

            Assert.IsNull(type.GetElementType());
        }

        [TestMethod("GetGenericArguments correctly returns for generic type")]
        public void GetGenericArguments_CorrectlyReturnsForGenericType()
        {
            var type = GetTypeDescription("Nullable<int>");
            var typeArgument1 = GetTypeDescription("int");

            Assert.AreEqual(typeArgument1, type.GetGenericArguments()[0]);
        }

        [TestMethod("GetGenericArguments returns empty array for non-generic type")]
        public void GetGenericArguments_ReturnsEmptyArrayForNonGenericType()
        {
            var type = GetTypeDescription("int");

            Assert.AreEqual(0, type.GetGenericArguments().Length);
        }

        [TestMethod("GetGenericTypeDefinition correctly returns for generic type")]
        public void GetGenericTypeDefinition_CorrectlyReturnsForGenericType()
        {
            var type = GetTypeDescription("Nullable<int>");

            var genericTypeDefinition = type.GetGenericTypeDefinition();

            Assert.AreEqual("Nullable", genericTypeDefinition.Name);
        }

        [TestMethod("GetGenericTypeDefinition throws InvalidOperationException for non-generic type")]
        public void GetGenericTypeDefinition_ThrowsInvalidOperationExceptionForNonGenericType()
        {
            var type = GetTypeDescription("int");
            Assert.ThrowsException<InvalidOperationException>(() => type.GetGenericTypeDefinition());
        }

        [TestMethod("IsArray returns false for non-array type")]
        public void IsArray_ReturnsFalseForNonArrayTypes()
        {
            var type = GetTypeDescription("int");

            Assert.IsFalse(type.IsArray);
        }

        [TestMethod("IsArray returns true for array type")]
        public void IsArray_ReturnsTrueForArrayTypes()
        {
            var type = GetTypeDescription("int[]");

            Assert.IsTrue(type.IsArray);
        }

        [TestMethod("IsArray returns false for non-generic type")]
        public void IsArray_ReturnsFalseForNonGenericTypes()
        {
            var type = GetTypeDescription("int");

            Assert.IsFalse(type.IsGenericType);
        }

        [TestMethod("IsArray returns true for generic type")]
        public void IsArray_ReturnsTrueForGenericTypes()
        {
            var type = GetTypeDescription("Nullable<int>");

            Assert.IsTrue(type.IsGenericType);
        }

        [TestMethod("MakeArrayType returns correctly")]
        public void MakeArrayType_ReturnsCorrectly()
        {
            var type = GetTypeDescription("int");
            var arrayType = GetTypeDescription("int[]");

            Assert.AreEqual(arrayType, type.MakeArrayType());
        }

        [TestMethod("MakeGenericType returns correctly")]
        public void MakeGenericType_ReturnsCorrectly()
        {
            var genericType = GetTypeDescription("Nullable<int>");
            var genericTypeDefinition = genericType.GetGenericTypeDefinition();
            var typeArguments = genericType.GetGenericArguments();

            Assert.AreEqual(genericType, genericTypeDefinition.MakeGenericType(typeArguments));
        }

        private TypeDescription GetTypeDescription(string typeName)
        {
            // Compile typeName as variable type
            var source = $"{typeName} variable";
            var compilation = CSharpCompilation.Create("compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location) },
                new CSharpCompilationOptions(OutputKind.ConsoleApplication));
           
            // Acquiring variable node
            var syntaxTree = compilation.SyntaxTrees.First();
            var root = syntaxTree.GetRoot();
            var node = root.DescendantNodes().OfType<VariableDeclarationSyntax>().First();

            // Acquiring type info
            var semanticModel = compilation.GetSemanticModel(syntaxTree);
            var typeSymbol = semanticModel.GetTypeInfo(node.Type).Type ?? throw new ArgumentException("Type is not predefined");
            
            return new CompileTimeTypeDescription(compilation, typeSymbol);
        }
    }
}