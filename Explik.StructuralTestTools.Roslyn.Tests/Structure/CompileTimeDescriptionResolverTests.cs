using Explik.StructuralTestTools;
using Explik.StructuralTestTools.MSTest;
using Explik.StructuralTestTools.TypeSystem;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace TestTools_Tests.Structure
{
    [TestClass]
    public class CompileTimeDescriptionResolverTests
    {
        [TestMethod("GetConstructorDescription returns correctly for ObjectCreationExpressionSyntax")]
        public void GetConstructorDescription_ReturnsCorrectlyForObjectCreationExpressionSyntax()
        {
            var source = @"
new Student();
public class Student
{
    public Student() {}
    public Student(object arg) {}
}";
            var script = CSharpScript.Create(source);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<ObjectCreationExpressionSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var description = resolver.GetConstructorDescription(node);

            Assert.AreEqual("Student", description.DeclaringType.Name);
            Assert.AreEqual(0, description.GetParameters().Length);
        }

        [TestMethod("GetConstructorDescription returns correctly for ConstructorDeclarationSyntax")]
        public void GetConstructorDescription_ReturnsCorrectlyForConstructorDeclarationSyntax()
        {
            var source = @"
public class ClassRoom
{
    public ClassRoom(string name) {}
}";
            var script = CSharpScript.Create(source);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<ConstructorDeclarationSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var description = resolver.GetConstructorDescription(node);

            Assert.AreEqual("ClassRoom", description.DeclaringType.Name);
            Assert.AreEqual(1, description.GetParameters().Length);
            Assert.AreEqual("String", description.GetParameters()[0].ParameterType.Name);
            Assert.AreEqual("name", description.GetParameters()[0].Name);
        }

        [TestMethod("GetDescription returns correctly for type MemberAccessExpressionSyntax")]
        public void GetDescription_ReturnsCorrectlyForTypeMemberAccessExpressionSyntax()
        {
            var script = CSharpScript.Create("System.Int32.MaxValue");
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<MemberAccessExpressionSyntax>().ElementAt(1);
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var typeDescription = (TypeDescription)resolver.GetDescription(node);

            Assert.AreEqual("Int32", typeDescription.Name);
        }

        [TestMethod("GetDescription returns correctly for event MemberAccessExpressionSyntax")]
        public void GetDescription_ReturnsCorrectlyForEventMemberAccessExpressionSyntax()
        {
            var source = @"
FireAlarm.Emergency += Console.Write(""Emergency!"");
public class FireAlarm
{
    public static event EventHandler Emergency;
}";
            var script = CSharpScript.Create(source);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<MemberAccessExpressionSyntax>().First();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var fieldDescription = (EventDescription)resolver.GetDescription(node);

            Assert.AreEqual("FireAlarm", fieldDescription.DeclaringType.Name);
            Assert.AreEqual("EventHandler", fieldDescription.EventHandlerType.Name);
            Assert.AreEqual("Emergency", fieldDescription.Name);
        }

        [TestMethod("GetDescription returns correctly for field MemberAccessExpressionSyntax")]
        public void GetDescription_ReturnsCorrectlyForFieldMemberAccessExpressionSyntax()
        {
            var source = @"
Grade.MinValue;
public class Grade
{
    public static int MinValue;
    public static int MaxValue;
}";
            var script = CSharpScript.Create(source);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<MemberAccessExpressionSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var fieldDescription = (FieldDescription)resolver.GetDescription(node);

            Assert.AreEqual("Grade", fieldDescription.DeclaringType.Name);
            Assert.AreEqual("Int32", fieldDescription.FieldType.Name);
            Assert.AreEqual("MinValue", fieldDescription.Name);
        }

        [TestMethod("GetDescription returns correctly for method MemberAccessExpressionSyntax")]
        public void GetDescription_ReturnsCorrectlyForMethodMemberAccessExpressionSyntax()
        {
            var source = @"
Teacher.Explain();
public class Teacher
{
    public static void Explain() {};
    public static void Explain(int duration) {};
}";
            var script = CSharpScript.Create(source);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<MemberAccessExpressionSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var methodDescription = (MethodDescription)resolver.GetDescription(node);

            Assert.AreEqual("Teacher", methodDescription.DeclaringType.Name);
            Assert.AreEqual("Void", methodDescription.ReturnType.Name);
            Assert.AreEqual("Explain", methodDescription.Name);
            Assert.AreEqual(0, methodDescription.GetParameters().Length);
        }

        [TestMethod("GetDescription returns correctly for property MemberAccessExpressionSyntax")]
        public void GetDescription_ReturnsCorrectlyForPropertyMemberAccessExpressionSyntax()
        {
            var source = @"
Principal.FullName;
public class Principal
{
    public static string FullName { get; set; }
}";
            var script = CSharpScript.Create(source);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<MemberAccessExpressionSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var propertyDescription = (PropertyDescription)resolver.GetDescription(node);

            Assert.AreEqual("Principal", propertyDescription.DeclaringType.Name);
            Assert.AreEqual("FullName", propertyDescription.Name);
        }

        [TestMethod("GetMethodDescription returns correctly for InvocationExpressionSyntax")]
        public void GetMethodDescription_ReturnsCorrectlyForInvocationExpressionSyntax()
        {
            var source = @"
Pupil.MakeHomeWork();
public class Pupil
{
    public static void MakeHomeWork() {};
    public static void MakeHomeWork(int duration) {};
}";
            var script = CSharpScript.Create(source);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<InvocationExpressionSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var methodDescription = resolver.GetMethodDescription(node);

            Assert.AreEqual("Pupil", methodDescription.DeclaringType.Name);
            Assert.AreEqual("Void", methodDescription.ReturnType.Name);
            Assert.AreEqual("MakeHomeWork", methodDescription.Name);
            Assert.AreEqual(0, methodDescription.GetParameters().Length);
        }

        [TestMethod("GetPropertyDescription returns null for array ElementAccessExpressionSyntax")]
        public void GetPropertyDescriptionReturnsNullForArrayElementAccessExpressionSyntax()
        {
            var source = "(new int[] { 1, 2, 3 })[0];";
            var script = CSharpScript.Create(source);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<ElementAccessExpressionSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            Assert.IsNull(resolver.GetPropertyDescription(node));
        }

        [TestMethod("GetPropertyDescription returns correctly for property ElementAccessExpressionSyntax")]
        public void GetPropertyDescriptionReturnsCorrrectlyForPropertyElementAccessExpressionSyntax()
        {
            var source = @"
(new Page())[0];
public class Page
{
    public string this[int n]
    {
        get { return null; }
        set {}
    }
}
";
            var script = CSharpScript.Create(source);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<ElementAccessExpressionSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var propertyDescription = resolver.GetPropertyDescription(node);

            Assert.AreEqual("Page", propertyDescription.DeclaringType.Name);
            Assert.AreEqual("String", propertyDescription.PropertyType.Name);
        }

        [TestMethod("IsTemplatedAttribute returns false for MSTest TestClass")]
        public void IsTemplatedAttribute_ReturnsFalseForMSTestTestClass()
        {
            var source = @"
[Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
public class TeacherTests { }
";
            var options = ScriptOptions.Default.WithReferences(typeof(TestClassAttribute).Assembly);
            var script = CSharpScript.Create(source, options);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<AttributeSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            Assert.IsFalse(resolver.IsTemplatedAttribute(node));
        }

        [TestMethod("IsTemplatedAttribute returns false for MSTest TestMethod")]
        public void IsTemplatedAttribute_ReturnsFalseForMSTestTestMethod()
        {
            var source = @"
public class PrincipleTests {
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
    public void FullName_ReturnsExpectedValue() { }
}
";
            var options = ScriptOptions.Default.WithReferences(typeof(TestMethodAttribute).Assembly);
            var script = CSharpScript.Create(source, options);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<AttributeSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            Assert.IsFalse(resolver.IsTemplatedAttribute(node));
        }

        [TestMethod("IsTemplatedAttribute returns true for TemplatedTestClass")]
        public void IsTemplatedAttribute_ReturnsTrueForTemplatedTestClass()
        {
            var source = @"
[Explik.StructuralTestTools.MSTest.TemplatedTestClass]
public class PupilTests { }
";
            var options = ScriptOptions.Default.WithReferences(typeof(TemplatedTestClassAttribute).Assembly);
            var script = CSharpScript.Create(source, options);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<AttributeSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            Assert.IsTrue(resolver.IsTemplatedAttribute(node));
        }

        [TestMethod("IsTemplatedAttribute returns true for TemplatedTestMethod")]
        public void IsTemplatedAttribute_ReturnsTrueForTemplatedTestMethod()
        {
            var source = @"
public class ClassRoomTests {
    [Explik.StructuralTestTools.MSTest.TemplatedTestMethod]
    public void Capacity_ReturnsExpectedValue() { }
}
";
            var options = ScriptOptions.Default.WithReferences(typeof(TemplatedTestClassAttribute).Assembly);
            var script = CSharpScript.Create(source, options);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<AttributeSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            Assert.IsTrue(resolver.IsTemplatedAttribute(node));
        }

        [TestMethod("HasTemplatedAttribute returns false for MSTest TestClass")]
        public void HasTemplatedAttribute_ReturnsFalseForMSTestTestClass()
        {
            var source = @"
[Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
public class TeacherTests { }
";
            var options = ScriptOptions.Default.WithReferences(typeof(TestClassAttribute).Assembly);
            var script = CSharpScript.Create(source, options);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<ClassDeclarationSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            Assert.IsFalse(resolver.HasTemplatedAttribute(node));
        }

        [TestMethod("HasTemplatedAttribute returns true for TemplatedTestClass")]
        public void HasTemplatedAttribute_ReturnsTrueForTemplatedTestClass()
        {
            var source = @"
[Explik.StructuralTestTools.MSTest.TemplatedTestClass]
public class PupilTests { }
";
            var options = ScriptOptions.Default.WithReferences(typeof(TemplatedTestClassAttribute).Assembly);
            var script = CSharpScript.Create(source, options);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<ClassDeclarationSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            Assert.IsTrue(resolver.HasTemplatedAttribute(node));
        }

        [TestMethod("HasTemplatedAttribute returns false for MSTest TestMethod")]
        public void HasTemplatedAttribute_ReturnsFalseForMSTestTestMethod()
        {
            var source = @"
public class PrincipleTests {
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
    public void FullName_ReturnsExpectedValue() { }
}
";
            var options = ScriptOptions.Default.WithReferences(typeof(TestMethodAttribute).Assembly);
            var script = CSharpScript.Create(source, options);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<MethodDeclarationSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            Assert.IsFalse(resolver.HasTemplatedAttribute(node));
        }

        [TestMethod("HasTemplatedAttribute returns true for TemplatedTestMethod")]
        public void HasTemplatedAttribute_ReturnsTrueForTemplatedTestMethod()
        {
            var source = @"
public class ClassRoomTests {
    [Explik.StructuralTestTools.MSTest.TemplatedTestMethod]
    public void Capacity_ReturnsExpectedValue() { }
}
";
            var options = ScriptOptions.Default.WithReferences(typeof(TemplatedTestMethodAttribute).Assembly);
            var script = CSharpScript.Create(source, options);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<MethodDeclarationSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            Assert.IsTrue(resolver.HasTemplatedAttribute(node));
        }

        [TestMethod("GetAssociatedAttributeType returns correctly for TemplatedTestClass")]
        public void GetAssociatedAttributeType_ReturnsCorrectlyForTemplatedTestClass()
        {
            var source = @"
[Explik.StructuralTestTools.MSTest.TemplatedTestClass]
public class PupilTests { }
";
            var options = ScriptOptions.Default.WithReferences(typeof(TemplatedTestClassAttribute).Assembly);
            var script = CSharpScript.Create(source, options);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<AttributeSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var typeName = resolver.GetAssociatedAttributeType(node);

            Assert.AreEqual("Microsoft.VisualStudio.TestTools.UnitTesting.TestClass", typeName);
        }

        [TestMethod("GetAssociatedAttributeType returns correctly for TemplatedTestMethod")]
        public void GetAssociatedAttributeType_ReturnsCorrectlyForTemplatedTestMethod()
        {
            var source = @"
public class ClassRoomTests {
    [Explik.StructuralTestTools.MSTest.TemplatedTestMethod]
    public void Capacity_ReturnsExpectedValue() { }
}
";
            var options = ScriptOptions.Default.WithReferences(typeof(TemplatedTestMethodAttribute).Assembly);
            var script = CSharpScript.Create(source, options);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<AttributeSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var typeName = resolver.GetAssociatedAttributeType(node);

            Assert.AreEqual("Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod", typeName);
        }

        [TestMethod("GetAssociatedExceptionType returns correctly for TemplatedTestMethod")]
        public void GetAssociatedExceptionType_ReturnsCorrectlyForTemplatedTestMethod()
        {
            var source = @"
public class ClassRoomTests {
    [Explik.StructuralTestTools.MSTest.TemplatedTestMethod]
    public void Capacity_ReturnsExpectedValue() { }
}
";
            var options = ScriptOptions.Default.WithReferences(typeof(TemplatedTestMethodAttribute).Assembly);
            var script = CSharpScript.Create(source, options);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<AttributeSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var typeName = resolver.GetAssociatedExceptionType(node);

            Assert.AreEqual("Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException", typeName);
        }

        [TestMethod("GetTypeDescription returns element type for ArrayTypeSyntax")]
        public void GetTypeDescription_ReturnsElementTypeForArrayTypeSyntax()
        {
            var source = "new int[] { 1, 2, 3 };";
            var script = CSharpScript.Create(source);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<ArrayTypeSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var elementType = resolver.GetTypeDescription(node);

            Assert.AreEqual("Int32", elementType.Name);
        }

        [TestMethod("GetTypeDescription returns type for CastExpressionSyntax")]
        public void GetTypeDescription_ReturnsTypeForCastExpressionSyntax()
        {
            var source = "(string)null";
            var script = CSharpScript.Create(source);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<CastExpressionSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var castType = resolver.GetTypeDescription(node);

            Assert.AreEqual("String", castType.Name);
        }

        [TestMethod("GetTypeDescription returns type for DefaultExpressionSyntax")]
        public void GetTypeDescription_ReturnsTypeForDefaultExpressionSyntax()
        {
            var source = "default(double)";
            var script = CSharpScript.Create(source);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<DefaultExpressionSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var defaultType = resolver.GetTypeDescription(node);

            Assert.AreEqual("Double", defaultType.Name);
        }

        [TestMethod("GetTypeDescription returns type for TypeDeclarationSyntax")]
        public void GetTypeDescription_ReturnsTypeForTypeDeclarationSyntax()
        {
            var source = "public class ClassType {}";
            var script = CSharpScript.Create(source);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<TypeDeclarationSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var typeDeclared = resolver.GetTypeDescription(node);

            Assert.AreEqual("ClassType", typeDeclared.Name);
        }

        [TestMethod("GetTypeDescription returns type for TypeOfExpressionSyntax")]
        public void GetTypeDescription_ReturnsTypeForTypeOfExpressionSyntax()
        {
            var source = "typeof(object)";
            var script = CSharpScript.Create(source);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<TypeOfExpressionSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var typeofType = resolver.GetTypeDescription(node);

            Assert.AreEqual("Object", typeofType.Name);
        }

        [TestMethod("GetTypeDescription returns type for VariableDeclarationSyntax")]
        public void GetTypeDescription_ReturnsTypeForVariableDeclarationSyntax()
        {
            var source = "long timestamp;";
            var script = CSharpScript.Create(source);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<VariableDeclarationSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var variableType = resolver.GetTypeDescription(node);

            Assert.AreEqual("Int64", variableType.Name);
        }

        [TestMethod("GetTypeDescription returns type for AttributeSyntax")]
        public void GetTypeDescription_ReturnsTypeForAttributeSyntax()
        {
            var source = @"
[Explik.StructuralTestTools.MSTest.TemplatedTestClass]
public class PupilTests { }
";
            var options = ScriptOptions.Default.WithReferences(typeof(TemplatedTestClassAttribute).Assembly);
            var script = CSharpScript.Create(source, options);
            var compilation = script.GetCompilation();
            var syntaxTree = compilation.SyntaxTrees.Single();
            var node = syntaxTree.GetRoot().AllDescendantNodes<AttributeSyntax>().Single();
            var resolver = new CompileTimeDescriptionResolver(compilation);

            var attributeType = resolver.GetTypeDescription(node);

            Assert.AreEqual("TemplatedTestClassAttribute", attributeType.Name);
        }
    }
}