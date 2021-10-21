using Explik.StructuralTestTools.TypeSystem;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Explik.StructuralTestTools
{
    public static class TemplatedAttributes
    {
        private class Entry
        {
            public string AssociatedAttribute;
            public string AssociatedException;
            public Type AssociatedAttributeType;
            public Type AssociatedExceptionType;
        }

        private static Dictionary<Type, Entry> _entries = new Dictionary<Type, Entry>()
        {
            [typeof(MSTest.TemplatedTestClassAttribute)] = new Entry()
            {
                AssociatedAttribute = "Microsoft.VisualStudio.TestTools.UnitTesting.TestClass"
            },
            [typeof(MSTest.TemplatedTestMethodAttribute)] = new Entry()
            {
                AssociatedAttribute = "Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod",
                AssociatedException = "Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException"
            }
        };

        public static bool IsTemplatedAttribute(TypeDescription typeDescription)
        {
            return _entries.Any(kv => kv.Key.FullName == typeDescription.FullName);
        }

        public static Type GetAssociatedAttributeType(TypeDescription typeDescription)
        {
            var entry = _entries.First(kv => kv.Key.FullName == typeDescription.FullName);

            if (entry.Value.AssociatedAttributeType == null)
            {
                entry.Value.AssociatedAttributeType = GetType(entry.Value.AssociatedAttribute);
            }
            return entry.Value.AssociatedAttributeType;
        }

        public static string GetAssociatedAttributeTypeName(TypeDescription typeDescription)
        {
            var entry = _entries.First(kv => kv.Key.FullName == typeDescription.FullName);
            return entry.Value.AssociatedAttribute;
        }

        public static Type GetAssociatedExceptionType(TypeDescription typeDescription)
        {
            var entry = _entries.First(kv => kv.Key.FullName == typeDescription.FullName);

            if (entry.Value.AssociatedExceptionType == null)
            {
                entry.Value.AssociatedExceptionType = GetType(entry.Value.AssociatedException);
            }
            return entry.Value.AssociatedExceptionType;
        }

        public static string GetAssociatedExceptionTypeName(TypeDescription typeDescription)
        {
            var entry = _entries.First(kv => kv.Key.FullName == typeDescription.FullName);
            return entry.Value.AssociatedException;
        }

        private static Type GetType(string fullTypeName)
        {
            return RuntimeTypeDescription.Create(fullTypeName).Type;
        }
    }
}
