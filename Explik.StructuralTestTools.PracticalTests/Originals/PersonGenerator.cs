using System;
using System.Collections.Generic;
using System.Text;

namespace Lecture_2_Solutions
{
    public class PersonGenerator
    {
        public Person GeneratePerson()
        {
            Person person = new Person();
            person.FirstName = "Adam";
            person.LastName = "Smith";
            person.Age = 36;

            return person;
        }

        public Person GenerateFamily()
        {
            Person gustav = new Person();
            gustav.FirstName = "Gustav";
            gustav.LastName = "Rich";
            gustav.Age = 66;

            Person elsa = new Person();
            elsa.FirstName = "Elsa";
            elsa.LastName = "Johnson";
            elsa.Age = 65;

            Person warren = new Person(elsa, gustav);
            warren.FirstName = "Warren";
            warren.LastName = "Rich";
            warren.Age = 36;

            Person anna = new Person();
            anna.FirstName = "Anna";
            anna.LastName = "Smith";
            anna.Age = 38;

            Person robin = new Person(anna, warren);
            robin.FirstName = "Robin";
            robin.LastName = "Rich";
            robin.Age = 10;

            return robin;
        }
    }
}
