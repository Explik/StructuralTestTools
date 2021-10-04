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
        public class ConstantFieldClass { }

        public class OriginalFieldClass { }

        public class TranslatedFieldClass { }

        public class ConstantPropertyClass { }

        public class OriginalPropertyClass { }

        public class TranslatedPropertyClass { }

        public class OriginalClass
        {
            public object Field;

            public ConstantFieldClass FieldWithConstantType;

            public OriginalFieldClass FieldWithVariableType;

            public object Property { get; set; }

            public ConstantPropertyClass PropertyWithConstantType { get; set; }

            public OriginalPropertyClass PropertyWithVariableType { get; set; }
        }

        public class TranslatedClass
        {
            public object Field;

            public ConstantFieldClass FieldWithConstantType;

            public TranslatedFieldClass FieldWithVariableType;

            public object Property { get; set; }

            public ConstantPropertyClass PropertyWithConstantType { get; set; }

            public TranslatedPropertyClass PropertyWithVariableType { get; set; }
        }

        public static readonly RuntimeTypeDescription OriginalType = new RuntimeTypeDescription(typeof(OriginalClass));
        public static readonly RuntimeTypeDescription TranslatedType = new RuntimeTypeDescription(typeof(TranslatedClass));

        public static readonly RuntimeTypeDescription ConstantFieldType = new RuntimeTypeDescription(typeof(ConstantFieldClass));
        public static readonly RuntimeTypeDescription OriginalFieldType = new RuntimeTypeDescription(typeof(OriginalFieldClass));
        public static readonly RuntimeTypeDescription TranslatedFieldType = new RuntimeTypeDescription(typeof(TranslatedFieldClass));

        public static readonly RuntimeTypeDescription ConstantPropertyType = new RuntimeTypeDescription(typeof(ConstantPropertyClass));
        public static readonly RuntimeTypeDescription OriginalPropertyType = new RuntimeTypeDescription(typeof(OriginalPropertyClass));
        public static readonly RuntimeTypeDescription TranslatedPropertyType = new RuntimeTypeDescription(typeof(TranslatedPropertyClass));

        public static readonly RuntimeFieldDescription OriginalField = new RuntimeFieldDescription(typeof(OriginalClass).GetField("Field"));
        public static readonly RuntimeFieldDescription OriginalFieldWithConstantType = new RuntimeFieldDescription(typeof(OriginalClass).GetField("FieldWithConstantType"));
        public static readonly RuntimeFieldDescription OriginalFieldWithVariableType = new RuntimeFieldDescription(typeof(OriginalClass).GetField("FieldWithVariableType"));
        public static readonly RuntimeFieldDescription TranslatedField = new RuntimeFieldDescription(typeof(TranslatedClass).GetField("Field"));
        public static readonly RuntimeFieldDescription TranslatedFieldWithConstantType = new RuntimeFieldDescription(typeof(TranslatedClass).GetField("FieldWithConstantType"));
        public static readonly RuntimeFieldDescription TranslatedFieldWithVariableType = new RuntimeFieldDescription(typeof(TranslatedClass).GetField("FieldWithVariableType"));

        public static readonly RuntimePropertyDescription OriginalProperty = new RuntimePropertyDescription(typeof(OriginalClass).GetProperty("Property"));
        public static readonly RuntimePropertyDescription OriginalPropertyWithConstantType = new RuntimePropertyDescription(typeof(OriginalClass).GetProperty("PropertyWithConstantType"));
        public static readonly RuntimePropertyDescription OriginalPropertyWithVariableType = new RuntimePropertyDescription(typeof(OriginalClass).GetProperty("PropertyWithVariableType"));
        public static readonly RuntimePropertyDescription TranslatedProperty = new RuntimePropertyDescription(typeof(TranslatedClass).GetProperty("Property"));
        public static readonly RuntimePropertyDescription TranslatedPropertyWithConstantType = new RuntimePropertyDescription(typeof(TranslatedClass).GetProperty("PropertyWithConstantType"));
        public static readonly RuntimePropertyDescription TranslatedPropertyWithVariableType = new RuntimePropertyDescription(typeof(TranslatedClass).GetProperty("PropertyWithVariableType"));
    }
}
