using Explik.StructuralTestTools.TypeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explik.StructuralTestTools
{
    /// <summary>
    /// A class containing sample data for runtime type system
    /// </summary>
    public static class RuntimeTypeSystemHelper
    {
        public class ConstantPropertyClass { }

        public class OriginalPropertyClass { }

        public class TranslatedPropertyClass { }

        public class OriginalClass
        {
            public object Property { get; set; }

            public ConstantPropertyClass PropertyWithConstantType { get; set; }

            public OriginalPropertyClass PropertyWithVariableType { get; set; }
        }

        public class TranslatedClass
        {
            public object Property { get; set; }

            public ConstantPropertyClass PropertyWithConstantType { get; set; }

            public TranslatedPropertyClass PropertyWithVariableType { get; set; }
        }

        public static readonly RuntimeTypeDescription OriginalType = new RuntimeTypeDescription(typeof(OriginalClass));
        public static readonly RuntimeTypeDescription TranslatedType = new RuntimeTypeDescription(typeof(TranslatedClass));

        public static readonly RuntimeTypeDescription ConstantPropertyType = new RuntimeTypeDescription(typeof(ConstantPropertyClass));
        public static readonly RuntimeTypeDescription OriginalPropertyType = new RuntimeTypeDescription(typeof(OriginalPropertyClass));
        public static readonly RuntimeTypeDescription TranslatedPropertyType = new RuntimeTypeDescription(typeof(TranslatedPropertyClass));

        public static readonly RuntimePropertyDescription OriginalProperty = new RuntimePropertyDescription(typeof(OriginalClass).GetProperty("Property"));
        public static readonly RuntimePropertyDescription OriginalPropertyWithConstantType = new RuntimePropertyDescription(typeof(OriginalClass).GetProperty("PropertyWithConstantType"));
        public static readonly RuntimePropertyDescription OriginalPropertyWithVariableType = new RuntimePropertyDescription(typeof(OriginalClass).GetProperty("PropertyWithVariableType"));
        public static readonly RuntimePropertyDescription TranslatedProperty = new RuntimePropertyDescription(typeof(TranslatedClass).GetProperty("Property"));
        public static readonly RuntimePropertyDescription TranslatedPropertyWithConstantType = new RuntimePropertyDescription(typeof(TranslatedClass).GetProperty("PropertyWithConstantType"));
        public static readonly RuntimePropertyDescription TranslatedPropertyWithVariableType = new RuntimePropertyDescription(typeof(TranslatedClass).GetProperty("PropertyWithVariableType"));
    }
}
