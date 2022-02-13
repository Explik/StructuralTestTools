# Structural Tests

## Limitations of feedback 
Regular unit tests can only provide feedback on whether a correct value has bene produced, so to see its limitations as a teaching tool let's propose a simple assignment for some students new to C#. The assignment will be to create a class Person with 3 properties FirstName, LastName and FullName, where FullName will be a combination of FirstName and LastName. This assignment will be evaluated by the simple unit test shown below. 

```C# 
// The test that evaluates the students' answer
[TestMethod("Person.FullName returns combination of FirstName and LastName")]
public void Person_FullNameReturnsCombinationOfFirstNameAndLastName() {
  Person person = new Person() {
    FirstName = "Thomas",
    LastName = "Stevens"
  };
  Assert.AreEqual("Thomas Stevens")
}
```

Student A is a real A student and has read up on all the litterature prior to class, so he tweaks an example he remembers and writes it down. The unit tests runs without any errors.
```C#
public class Person {
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string FullName { get { return FirstName + " " + LastName; }
}
```

Student B does a lot of programming in her spare time, including C#, so she quickly jots down a few lines of code. However, student B made a small mistake that he easily corrected with the feedback from the unit test: expected "Thomas Stevens" but got "ThomasStevens".
```C#
public class Person {
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string FullName => FirstName + LastName;
}
```

Student C is very eager to learn C# and has recently seen a YouTube video about initilization in the constructor. So he includes a constructor in his answer not knowing that no default constructor will not be provided for him, so the unit tests fails to compile. The unit test does not compile even though he did exactly as the assignment asked, leaving him with little feedback.
```C#
public class Person {
  public Person(string firstName, string lastName) {
    FirstName = firstName;
    LastName = lastName;
  }

  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string FullName {
    get { return FirstName + " " + LastName; }
  }
}
```

Student D has done a little java programming in the past, but is new to C#. She answers the question and the test runs without any errors. However, her solution is incorrect as of the assignment as FirstName and LastName are fields instead of properties, so she never receives any feedback on this and never asks as she thinks she is right.
```C#
public class Person {
  public string FirstName;
  public string Lastname;
  public string FullName { get { return FirstName + " " + LastName; }
}
```

Student E has always a hard time reading, so he carefully reads the assignment but still misunderstands the assignment. He gives an answer without FirstName, so the unit tests cannot compile, and he is left with little useful feedback on how to continue. 
```C#
public class Person { 
  public string LastName { get; set; }
  public string FullName { get { return "FirstName" + " " + LastName; }
}
```

As shown above, unit tests lack in certain areas  several flaws or 

To summerize, unit tests are not 

As shown above, unit test are fairly limited in the amount of feedback that they are able to give. The primary faults being two.
1. Unit tests do not differentiate between similiar concepts in cases where they are functionally and syntactically equavilent. This is a problem due to some concepts like fields and properties having large overlaps, but still being important for students to be able differentiate.
2. Unit tests may not be able to compile due to student error even when the student's own code compiles perfectly fine on its own. This is a problem due to error being displayed on the tests instead of in the students code where it originated. 
3. A single unit test having an compile-time error will block all other unit tests from compilling. This can make it exceedingly difficult to find the errors, and blocks any possibilitý of intermittent feedback as anything less than full implementation will result in no feedback at all. 

# A naive solution


# A proper solution 
