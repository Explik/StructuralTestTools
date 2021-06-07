using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace TestTools_Tests.TypeSystem
{
    [TestClass]
    public class RuntimeTypeDescriptionTests
    {
        [TestMethod("GetElementType correctly returns for array type")]
        public void GetElementType_CorrectlyReturnsForArrayType()
        {
            var type = new RuntimeTypeDescription(typeof(int[]));
            var elementType = new RuntimeTypeDescription(typeof(int));

            Assert.AreEqual(elementType, type.GetElementType());
        }

        [TestMethod("GetElementType returns null for non-array type")]
        public void GetElementType_ReturnsNullForNonArrayType()
        {
            var type = new RuntimeTypeDescription(typeof(int));
            Assert.IsNull(type.GetElementType());
        }

        [TestMethod("GetGenericArguments correctly returns for generic type")]
        public void GetGenericArguments_CorrectlyReturnsForGenericType()
        {
            var type = new RuntimeTypeDescription(typeof(Nullable<int>));
            var typeArgument1 = new RuntimeTypeDescription(typeof(int));

            Assert.AreEqual(typeArgument1, type.GetGenericArguments()[0]);
        }

        [TestMethod("GetGenericArguments returns empty array for non-generic type")]
        public void GetGenericArguments_ReturnsEmptryArrayForNonGenericType()
        {
            var type = new RuntimeTypeDescription(typeof(int));
            Assert.AreEqual(0, type.GetGenericArguments().Length);
        }

        [TestMethod("GetGenericTypeDefinition correctly returns for generic type")]
        public void GetGenericTypeDefinition_CorrectlyReturnsForGenericType()
        {
            var type = new RuntimeTypeDescription(typeof(Nullable<int>));

            var genericTypeDefinition = type.GetGenericTypeDefinition();

            Assert.AreEqual("Nullable`1", genericTypeDefinition.Name);
        }

        [TestMethod("GetGenericTypeDefinition throws InvalidOperationException for non-generic type")]
        public void GetGenericTypeDefinition_ThrowsInvalidOperationExceptionForNonGenericType()
        {
            var type = new RuntimeTypeDescription(typeof(int));
            Assert.ThrowsException<InvalidOperationException>(() => type.GetGenericTypeDefinition());
        }

        [TestMethod("IsArray returns false for non-array type")]
        public void IsArray_ReturnsFalseForNonArrayTypes()
        {
            var type = new RuntimeTypeDescription(typeof(int));
            Assert.IsFalse(type.IsArray);
        }

        [TestMethod("IsArray returns true for array type")]
        public void IsArray_ReturnsTrueForArrayTypes()
        {
            var type = new RuntimeTypeDescription(typeof(int[]));
            Assert.IsTrue(type.IsArray);
        }

        [TestMethod("IsArray returns false for non-generic type")]
        public void IsArray_ReturnsFalseForNonGenericType()
        {
            var type = new RuntimeTypeDescription(typeof(int));
            Assert.IsFalse(type.IsGenericType);
        }

        [TestMethod("IsArray returns true for generic type")]
        public void IsArray_ReturnsTrueForGenericType()
        {
            var type = new RuntimeTypeDescription(typeof(Nullable<int>));
            Assert.IsTrue(type.IsGenericType);
        }

        [TestMethod("MakeArrayType returns correctly")]
        public void MakeArrayType_ReturnsCorrectly()
        {
            var type = new RuntimeTypeDescription(typeof(int));
            var arrayType = new RuntimeTypeDescription(typeof(int[]));

            Assert.AreEqual(arrayType, type.MakeArrayType());
        }

        [TestMethod("MakeGenericType returns correctly")]
        public void MakeGenericType_ReturnsCorrectly()
        {
            var openType = new RuntimeTypeDescription(typeof(Nullable<>));
            var typeArgument = new RuntimeTypeDescription(typeof(double));
            var closedType = new RuntimeTypeDescription(typeof(Nullable<double>));

            Assert.AreEqual(closedType, openType.MakeGenericType(typeArgument));
        }
    }
}
