using System;
using System.Collections.Generic;
using System.Text;

namespace Lecture_2_Solutions
{
    public class PersonPrinter
    {
        public void PrintPerson(Person p)
        {
            Console.WriteLine(p.FirstName + " " + p.LastName + " (" + p.Age + ")");
        }

        public void PrintFamily(Person p)
        {
            PrintFamilyWithIndent(p, 0);
        }

        private void PrintFamilyWithIndent(Person p, int indent)
        {
            for (int i = 0; i < indent; i++)
                Console.Write("  ");

            PrintPerson(p);

            if(p.Father != null)
                PrintFamilyWithIndent(p.Father, indent + 1);
            if(p.Mother != null)
                PrintFamilyWithIndent(p.Mother, indent + 1);
        }
    }
}
