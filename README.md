# Structural TestTools
![alt text](https://img.shields.io/nuget/v/OleVanSanten.TestTools?label=TestTools%20nuget "TestTools nuget package")
![alt text](https://img.shields.io/nuget/v/OleVanSanten.TestTools.Generator?label=TestTools%20Generator%20nuget "TestTools Generator nuget package")
![alt text](https://img.shields.io/nuget/v/OleVanSanten.TestTools.MSTest?label=TestTools%20MSTest%20nuget "TestTools MSTest nuget package")

Structural TestTools is a unit test rewriter for C#. It allows for testing of user-implemented types, even though members might be missing or in other ways be invalid. This ensures that if the user's code compiles the test code also will. 

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

See  for [TDD OPP Kursus](https://github.com/OleVanSanten/tdd-oop-exercises/tree/templated-syntax) a show case. 

## Overall Process
<img src="Docs/Assets/OverallProcess.png" alt="drawing" width="450"/>
Structural TestTools rewrites syntax trees representing the original unit tets according to type information of the user-implemented types and configuration object. This rewriting process has the following steps: 
<br></br>

1. Translating type with <code>ITypeTranslator</code>
2. Verifying translated type with <code>ITypeVerifier</code>
3. Translating member with <code>IMemberTranslator</code>
4. Verifying translated member with <code>IMemberVerifier</code>
5. Modifying syntax tree by substituting types/members with translated types/members

The rewriting process is different de

### Runtime Mode
In runtime mode, the tests are written in a custom syntax to accomodate LINQ expressions. LINQ Expressions are used for syntax trees, but are wrapped in <code>TestExpression</code>. Reflection is used for type information, but is wrapped by the <code>RuntimeTypeDescription</code>, <code>RuntimeConstructorDescription</code>, etc. 

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
In compile time mode, the tests are written as usaul, but require the use of the TestTools.Generator package and Visual Studio. A roslyn source generator provides the syntax trees. It also provides the type information, which is wrapped in <code>CompileTimeTypeDescription</code>, <code>CompileTimeConstructorDescription</code>, etc. 

This sample shows how the original unit test would look in compile time mode. 
```C#
[TemplatedTestMethod("Person.Age initializes as 0")]
public void Person_AgeInitializesAsZero() {
  Rewritten.Person person = new Rewritten.Person();
  Assert.AreEqual(0, person.Age);
}
```
