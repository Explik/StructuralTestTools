using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using OleVanSanten.TestTools.Structure;
using System.Linq.Expressions;
using System.Reflection;
using static TestTools_Tests.TestHelper;
using OleVanSanten.TestTools.TypeSystem;

namespace TestTools_Tests.Structure
{
    // All tests in this class fails as NSubstitutes only creates proxy objects for declared types, 
    // and not for runtime types, which is required for TypeVisitor to work.
    //[TestClass]
    public class TypeVisitorTests
    {
        class ClassA
        {
            public int Field;
            public int Property { get; set; }
            public event EventHandler Event;
            public void VoidMethod()
            {
            }
        }

        class ClassB
        {
            public int Field;
            public int Property { get; set; }
            public event EventHandler Event;
            public void VoidMethod()
            {
            }
        }

        ConstructorInfo ClassAConstructor = typeof(ClassA).GetConstructor(new Type[0]);
        FieldInfo ClassAField = typeof(ClassA).GetField("Field");
        EventInfo ClassAEvent = typeof(ClassA).GetEvent("Event");
        MethodInfo ClassAVoidMethod = typeof(ClassB).GetMethod("VoidMethod", new Type[0]);
        PropertyInfo ClassAProperty = typeof(ClassA).GetProperty("Property");

        ConstructorInfo ClassBConstructor = typeof(ClassB).GetConstructor(new Type[0]);
        FieldInfo ClassBField = typeof(ClassB).GetField("Field");
        EventInfo ClassBEvent = typeof(ClassB).GetEvent("Event");
        MethodInfo ClassBVoidMethod = typeof(ClassA).GetMethod("VoidMethod", new Type[0]);
        PropertyInfo ClassBProperty = typeof(ClassB).GetProperty("Property");

        [TestMethod("Visit correctly transforms ParameterExpression")]
        public void VisitCorrectlyTransformsParameterExpression()
        {
            // Setting up semantic data
            var originalType = new RuntimeTypeDescription(typeof(ClassA));
            var translatedType = new RuntimeTypeDescription(typeof(ClassB));

            // Setting up structure service to handle semantic data
            IStructureService service = Substitute.For<IStructureService>();
            service.TranslateType(originalType).Returns(translatedType);

            // Setting up and testing visitor
            TypeVisitor visitor = new TypeVisitor(service)
            {
                TypeVerifiers = new ITypeVerifier[0]
            };

            Expression input = Expression.Parameter(typeof(ClassA));
            Expression expected = Expression.Parameter(typeof(ClassB));
            Expression actual = visitor.Visit(input);

            AssertAreEqualExpressions(expected, actual);
        }

        [TestMethod("Visit correctly transforms NewExpression")]
        public void VisitCorrectlyTransformsNewExpression()
        {
            // Setting up semantic data
            var originalType = new RuntimeTypeDescription(typeof(ClassA));
            var translatedType = new RuntimeTypeDescription(typeof(ClassB));
            var originalConstructor = new RuntimeConstructorDescription(ClassAConstructor);
            var translatedConstructor = new RuntimeConstructorDescription(ClassBConstructor);

            // Setting up structure service to handle semantic data
            IStructureService service = Substitute.For<IStructureService>();
            service.TranslateType(originalType).Returns(translatedType);
            service.TranslateMember(originalConstructor).Returns(translatedConstructor);

            // Setting up and testing visitor
            TypeVisitor visitor = new TypeVisitor(service)
            {
                TypeVerifiers = new ITypeVerifier[0],
                MemberVerifiers = new IMemberVerifier[0]
            };

            Expression input = Expression.New(ClassAConstructor);
            Expression expected = Expression.New(ClassBConstructor);
            Expression actual = visitor.Visit(input);

            AssertAreEqualExpressions(expected, actual);
        }

        [TestMethod("Visit correctly transforms MethodCallExpression")]
        public void VisitCorrectlyTransformsMethodCallExpression()
        {
            // Setting up semantic data
            var originalType = new RuntimeTypeDescription(typeof(ClassA));
            var translatedType = new RuntimeTypeDescription(typeof(ClassB));
            var originalMethod = new RuntimeMethodDescription(ClassAVoidMethod);
            var translatedMethod = new RuntimeMethodDescription(ClassBVoidMethod);

            // Setting up structure service to handle semantic data
            IStructureService service = Substitute.For<IStructureService>();
            service.TranslateType(originalType).Returns(translatedType);
            service.TranslateMember(originalMethod).Returns(translatedMethod);

            // Setting up and testing visitor
            TypeVisitor visitor = new TypeVisitor(service)
            {
                TypeVerifiers = new ITypeVerifier[0],
                MemberVerifiers = new IMemberVerifier[0]
            };

            Expression input = Expression.Call(Expression.Parameter(typeof(ClassA)), ClassAVoidMethod);
            Expression expected = Expression.Call(Expression.Parameter(typeof(ClassB)), ClassBVoidMethod);
            Expression actual = visitor.Visit(input);

            AssertAreEqualExpressions(expected, actual);
        }

        [TestMethod("Visit correctly transforms MemberExpression for field")]
        public void VisitCorrectlyTransformMemberExpressionForField()
        {
            // Setting up semantic data
            var originalType = new RuntimeTypeDescription(typeof(ClassA));
            var translatedType = new RuntimeTypeDescription(typeof(ClassB));
            var originalField = new RuntimeFieldDescription(ClassAField);
            var translatedField = new RuntimeFieldDescription(ClassBField);

            // Setting up structure service to handle semantic data
            IStructureService service = Substitute.For<IStructureService>();
            service.TranslateType(originalType).Returns(translatedType);
            service.TranslateMember(originalField).Returns(translatedField);

            // Setting up and testing visitor
            TypeVisitor visitor = new TypeVisitor(service)
            {
                TypeVerifiers = new ITypeVerifier[0],
                MemberVerifiers = new IMemberVerifier[0]
            };

            Expression input = Expression.Field(Expression.Parameter(typeof(ClassA)), ClassAField);
            Expression expected = Expression.Field(Expression.Parameter(typeof(ClassB)), ClassBField);
            Expression actual = visitor.Visit(input);

            AssertAreEqualExpressions(expected, actual);
        }

        [TestMethod("Visit correctly transforms MemberExpression for property")]
        public void VisitCorrectlyTransformMemberExpressionForProperty()
        {
            // Setting up semantic data
            var originalType = new RuntimeTypeDescription(typeof(ClassA));
            var translatedType = new RuntimeTypeDescription(typeof(ClassB));
            var originalProperty = new RuntimePropertyDescription(ClassAProperty);
            var translatedProperty = new RuntimePropertyDescription(ClassBProperty);

            // Setting up structure service to handle semantic data
            IStructureService service = Substitute.For<IStructureService>();
            service.TranslateType(originalType).Returns(translatedType);
            service.TranslateMember(originalProperty).Returns(translatedProperty);

            // Setting up and testing visitor
            TypeVisitor visitor = new TypeVisitor(service)
            {
                TypeVerifiers = new ITypeVerifier[0],
                MemberVerifiers = new IMemberVerifier[0]
            };

            Expression input = Expression.Property(Expression.Parameter(typeof(ClassA)), ClassAProperty);
            Expression expected = Expression.Property(Expression.Parameter(typeof(ClassB)), ClassBProperty);
            Expression actual = visitor.Visit(input);

            AssertAreEqualExpressions(expected, actual);
        }
        
        //TODO write tests to verify verifications
    }
}
