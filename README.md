# Structural TestTools
![alt text](https://img.shields.io/nuget/v/OleVanSanten.TestTools?label=TestTools%20nuget "TestTools nuget package")
![alt text](https://img.shields.io/nuget/v/OleVanSanten.TestTools.Generator?label=TestTools%20Generator%20nuget "TestTools Generator nuget package")
![alt text](https://img.shields.io/nuget/v/OleVanSanten.TestTools.MSTest?label=TestTools%20MSTest%20nuget "TestTools MSTest nuget package")

Structural TestTools is a unit test rewriter for C#. It allows for testing of user-implemented types, even though members might be missing or in other ways be invalid. This ensures that if the user's code compiles the test code will as well. 

```C#
// Original unit test 
public void Person_AgeInitializesAsZero() {
  Original.Person person = new Original.Person();
  Assert.AreEqual(0, person.Age);
}

// Rewritten unit test (if successful)
public void Person_AgeInitializesAsZero() {
  Rewritten.Person person = new Rewritten.Person();
  Assert.AreEqual(0, person.Age);
}

// Rewritten unit test (if unsuccessful)
public void Person_AgeInitializesAsZero() {
  throw new AssertFailedException("Class Person does not contain member Age");
}
```

See [TDD OPP Kursus](https://github.com/OleVanSanten/tdd-oop-exercises/tree/templated-syntax) for a show case. 

## Overall Process
<img src="Docs/Assets/OverallProcess.png" alt="drawing" width="450"/>
Structural TestTools rewrites syntax trees representing the original unit tests according to type information about the user-implemented types and a configuration object. This rewriting process has the following steps: 
<br></br>

1. Translating type with <code>ITypeTranslator</code>
2. Verifying translated type with <code>ITypeVerifier</code>
3. Translating member with <code>IMemberTranslator</code>
4. Verifying translated member with <code>IMemberVerifier</code>
5. Modifying syntax tree by substituting types/members with translated types/members

### Runtime Mode
In runtime mode, the tests are written in a custom syntax to accomodate the LINQ expressions. LINQ Expressions are internally used for syntax trees, but are wrapped in <code>TestExpression</code> in the custom syntax. Reflection is internally used for type information, but is wrapped by the classes <code>RuntimeTypeDescription</code>, <code>RuntimeConstructorDescription</code>, etc. 

This sample shows how the original unit test would look in runtime mode. 
```C#
[TestMethod("Person.Age initializes as 0")]
public void Person_AgeInitializesAsZero() {
  UnitTest test = Factory.CreateTest();
  TestVariable<Person> _person = test.CreateVariable<Person>(nameof(_person));
  test.Arrange(_person, Expr(() => new Person()));
  test.Assert.AreEqual(Const(0), Expr(_person, p => p.Age));
  test.Execute();
}
```

### Compile Time Mode
In compile time mode, the tests are written nearly as usaul, but require the use of the TestTools.Generator package and Visual Studio. A Roslyn source generator provides the syntax trees and also provides the type information, which is wrapped in the classes <code>CompileTimeTypeDescription</code>, <code>CompileTimeConstructorDescription</code>, etc. 

This sample shows how the original unit test would look in compile time mode. 
```C#
[TemplatedTestMethod("Person.Age initializes as 0")]
public void Person_AgeInitializesAsZero() {
  Original.Person person = new Original.Person();
  Assert.AreEqual(0, person.Age);
}
```

## Configuration
Structural TestTools already come pre-configured, however this configuration can be easily be overwritten. 

### Project-wide customization
The pre-defined configuration can be overwritten for a whole test project using an XML file. The settings in the configuration can be overwritten one-by-one through including each of them in the XML file. 
```XML
<?xml version="1.0" encoding="utf-8" ?>
<Config>
  <!-- Defining the namespaces from which classes are translated from and to -->
  <FromNamespace>Original</FromNamespace>
  <ToNamespace>Rewritten</ToNamespace>
  
  <!-- Overwritting all rewriting process objects --> 
  <TypeTranslator Type="TestTools.Structure.SameNameTypeTranslator"/>
  <MemberTranslator Type="TestTools.Structure.SameNameMemberTranslator"/>
  <TypeVerifiers>
    <TypeVerifier Type="TestTools.Structure.UnchangedTypeAccessLevelVerifier"/>
    <TypeVerifier Type="TestTools.Structure.UnchangedTypeIsAbstractVerifier"/>
    <TypeVerifier Type="TestTools.Structure.UnchangedTypeIsStaticVerifier"/>
  </TypeVerifiers>
  <MemberVerifiers>
    <MemberVerifier Type="TestTools.Structure.UnchangedFieldTypeVerifier"/>
    <MemberVerifier Type="TestTools.Structure.UnchangedMemberAccessLevelVerifier"/>
    <MemberVerifier Type="TestTools.Structure.UnchangedMemberDeclaringType"/>
    <MemberVerifier Type="TestTools.Structure.UnchangedMemberIsStaticVerifier"/>
    <MemberVerifier Type="TestTools.Structure.UnchangedMemberIsVirtualVerifier"/>
    <MemberVerifier Type="TestTools.Structure.UnchangedMemberTypeVerifier"/>
    <MemberVerifier Type="TestTools.Structure.UnchangedPropertyTypeVerifier"/>
  </MemberVerifiers>
</Config>
```

In runtime mode, the XML file is specified for each test (or in a test helper used by each test). 
```C#
TestFactory testFactory = TestFactory.CreateFromConfigurationFile("./TestToolsConfig.xml");
UnitTest test = Factory.CreateTest();
```

In compile time mode, the XML file is specified in the test project file. 
```XML
<Project Sdk="Microsoft.NET.Sdk">
  <ItemGroup>
    <AdditionalFiles Include="TestToolsConfig.xml" UnitTestGenerator_IsConfig="true" />
  </ItemGroup>
</Project>
```

### Type/Member-specific customization
The project-wide configuration can be overwritten for a single type/member using attributes. 

The project-wide TypeTranslator can be overwritten for a single type, if the type is associated with an attribute implementing <code>ITtypeTranslator</code>. The following example shows how the default type translation approach can be overwritten. The default approach is to translate the type Customer to a type Customer in a different namespace. The new approach is to translate the type Customer to either Customer or Client in the other namespace. 
```C#
[AlternateNames("Client")]
public class Customer {}
```

The project-wide TypeVerifier can be overwritten in the same way as TypeTranslator. 

The project-wide MemberTranslator can be overwritten for a single member, if the member is associated with an attribute implemeting <code>IMemberTranslator</code>. The following shows how the default member translation approach can be overwritten. The default approach is to translate the member CalculateSalary to CalculateSalary on the translated type. The new approach is to translate CalculateSalary to either CalculateSalary or CalculateYearlySalary on the translated type.
```C#
public class Employee {
  [AlternateNames("CalculateYearlySalary")]
  public decimal CalculateSalary() { ... }
}
```

The project-wide MemberVerifier can be overwritten in the same as MemberTranslator. The following shows how the default member verification approach can be overwritten. The default approach is to verify that the field ID stays a field during translation. The new approach is to verify that the field ID is translated into either a field or a property. 
```C#
public class Person {
  [FieldOrProperty]
  public int ID;
}
```
