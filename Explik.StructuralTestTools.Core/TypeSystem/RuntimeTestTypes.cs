using Explik.StructuralTestTools.TypeSystem;
using System;
using System.Linq;
using TestTypes;

namespace Explik.StructuralTestTools
{
    /// <summary>
    /// A class containing sample data for runtime type system
    /// </summary>
    public static class RuntimeTestTypes
    {
        public static readonly RuntimeNamespaceDescription ConstantNamespace = new RuntimeNamespaceDescription("TestTypes");
        public static readonly RuntimeNamespaceDescription OriginalNamespace = new RuntimeNamespaceDescription("OriginalNamespace");
        public static readonly RuntimeNamespaceDescription TranslatedNamespace = new RuntimeNamespaceDescription("TranslatedNamespace");

        public static readonly RuntimeTypeDescription ConstantType = new RuntimeTypeDescription(typeof(ConstantClass));
        public static readonly RuntimeTypeDescription OriginalType = new RuntimeTypeDescription(typeof(OriginalClass));
        public static readonly RuntimeTypeDescription TranslatedType = new RuntimeTypeDescription(typeof(TranslatedClass));

        public static readonly RuntimeTypeDescription ConstantArrayType = new RuntimeTypeDescription(typeof(ConstantClass[]));
        public static readonly RuntimeTypeDescription OriginalArrayType = new RuntimeTypeDescription(typeof(OriginalClass[]));
        public static readonly RuntimeTypeDescription TranslatedArrayType = new RuntimeTypeDescription(typeof(TranslatedClass[]));

        public static readonly RuntimeTypeDescription ConstantArrayArrayType = new RuntimeTypeDescription(typeof(ConstantClass[][]));
        public static readonly RuntimeTypeDescription OriginalArrayArrayType = new RuntimeTypeDescription(typeof(OriginalClass[][]));
        public static readonly RuntimeTypeDescription TranslatedArrayArrayType = new RuntimeTypeDescription(typeof(TranslatedClass[][]));

        public static readonly RuntimeTypeDescription ConstantGenericTypeWithConstantTypeArgument = new RuntimeTypeDescription(typeof(ConstantGenericClass<ConstantTypeArgumentClass>));
        public static readonly RuntimeTypeDescription ConstantGenericTypeWithOriginalTypeArgument = new RuntimeTypeDescription(typeof(ConstantGenericClass<OriginalTypeArgumentClass>));
        public static readonly RuntimeTypeDescription ConstantGenericTypeWithTranslatedTypeArgument = new RuntimeTypeDescription(typeof(ConstantGenericClass<TranslatedTypeArgumentClass>));
        public static readonly RuntimeTypeDescription OriginalGenericTypeWithConstantTypeArgument = new RuntimeTypeDescription(typeof(OriginalGenericClass<ConstantTypeArgumentClass>));
        public static readonly RuntimeTypeDescription OriginalGenericTypeWithVariableTypeArgument = new RuntimeTypeDescription(typeof(OriginalGenericClass<OriginalTypeArgumentClass>));
        public static readonly RuntimeTypeDescription TranslatedGenericTypeWithConstantTypeArgument = new RuntimeTypeDescription(typeof(TranslatedGenericClass<ConstantTypeArgumentClass>));
        public static readonly RuntimeTypeDescription TranslatedGenericTypeWithVariableTypeArgument = new RuntimeTypeDescription(typeof(TranslatedGenericClass<TranslatedTypeArgumentClass>));

        public static readonly RuntimeTypeDescription ConstantArgumentType = new RuntimeTypeDescription(typeof(ConstantArgumentClass));
        public static readonly RuntimeTypeDescription OriginalArgumentType = new RuntimeTypeDescription(typeof(OriginalArgumentClass));
        public static readonly RuntimeTypeDescription TranslatedArgumentType = new RuntimeTypeDescription(typeof(TranslatedArgumentClass));
        
        public static readonly RuntimeTypeDescription ConstantFieldType = new RuntimeTypeDescription(typeof(ConstantFieldClass));
        public static readonly RuntimeTypeDescription OriginalFieldType = new RuntimeTypeDescription(typeof(OriginalFieldClass));
        public static readonly RuntimeTypeDescription TranslatedFieldType = new RuntimeTypeDescription(typeof(TranslatedFieldClass));

        public static readonly RuntimeTypeDescription ConstantNestedType = new RuntimeTypeDescription(typeof(ConstantClass.NestedClass));
        public static readonly RuntimeTypeDescription OriginalNestedType = new RuntimeTypeDescription(typeof(OriginalClass.NestedClass));
        public static readonly RuntimeTypeDescription TranslatedNestedType = new RuntimeTypeDescription(typeof(TranslatedClass.NestedClass));

        public static readonly RuntimeTypeDescription ConstantNestedArrayType = new RuntimeTypeDescription(typeof(ConstantClass.NestedClass[]));
        public static readonly RuntimeTypeDescription OriginalNestedArrayType = new RuntimeTypeDescription(typeof(OriginalClass.NestedClass[]));
        public static readonly RuntimeTypeDescription TranslatedNestedArrayType = new RuntimeTypeDescription(typeof(TranslatedClass.NestedClass[]));

        public static readonly RuntimeTypeDescription ConstantNullableType = new RuntimeTypeDescription(typeof(ConstantStruct?));
        public static readonly RuntimeTypeDescription OriginalNullableType = new RuntimeTypeDescription(typeof(OriginalStruct?));
        public static readonly RuntimeTypeDescription TranslatedNullableType = new RuntimeTypeDescription(typeof(TranslatedStruct?));

        public static readonly RuntimeTypeDescription ConstantNullableArrayType = new RuntimeTypeDescription(typeof(ConstantStruct?[]));
        public static readonly RuntimeTypeDescription OriginalNullableArrayType = new RuntimeTypeDescription(typeof(OriginalStruct?[]));
        public static readonly RuntimeTypeDescription TranslatedNullableArrayType = new RuntimeTypeDescription(typeof(TranslatedStruct?[]));

        public static readonly RuntimeTypeDescription ConstantMethodReturnType = new RuntimeTypeDescription(typeof(ConstantMethodReturnClass));
        public static readonly RuntimeTypeDescription OriginalMethodReturnType = new RuntimeTypeDescription(typeof(ConstantMethodReturnClass));
        public static readonly RuntimeTypeDescription TranslatedMethodReturnType = new RuntimeTypeDescription(typeof(ConstantMethodReturnClass));

        public static readonly RuntimeTypeDescription ConstantPropertyType = new RuntimeTypeDescription(typeof(ConstantPropertyClass));
        public static readonly RuntimeTypeDescription OriginalPropertyType = new RuntimeTypeDescription(typeof(OriginalPropertyClass));
        public static readonly RuntimeTypeDescription TranslatedPropertyType = new RuntimeTypeDescription(typeof(TranslatedPropertyClass));

        public static readonly RuntimeTypeDescription ConstantTypeArgumentType = new RuntimeTypeDescription(typeof(ConstantTypeArgumentClass));
        public static readonly RuntimeTypeDescription OriginalTypeArgumentType = new RuntimeTypeDescription(typeof(OriginalTypeArgumentClass));
        public static readonly RuntimeTypeDescription TranslatedTypeArgumentType = new RuntimeTypeDescription(typeof(TranslatedTypeArgumentClass));

        public static readonly RuntimeConstructorDescription ConstantDefaultConstructor = new RuntimeConstructorDescription(typeof(ConstantClass).GetConstructor(new Type[0]));
        public static readonly RuntimeConstructorDescription ConstantConstructorWithArgument = new RuntimeConstructorDescription(typeof(ConstantClass).GetConstructor(new Type[0]));
        public static readonly RuntimeConstructorDescription OriginalDefaultConstructor = new RuntimeConstructorDescription(typeof(OriginalClass).GetConstructor(new Type[0]));
        public static readonly RuntimeConstructorDescription OriginalConstructorWithConstantArgument = new RuntimeConstructorDescription(typeof(OriginalClass).GetConstructor(new Type[] { typeof(ConstantArgumentClass) }));
        public static readonly RuntimeConstructorDescription OriginalConstructorWithVariableArgument = new RuntimeConstructorDescription(typeof(OriginalClass).GetConstructor(new Type[] { typeof(OriginalArgumentClass) }));
        public static readonly RuntimeConstructorDescription TranslatedDefaultConstructor = new RuntimeConstructorDescription(typeof(TranslatedClass).GetConstructor(new Type[0]));
        public static readonly RuntimeConstructorDescription TranslatedConstructorWithConstantArgument = new RuntimeConstructorDescription(typeof(TranslatedClass).GetConstructor(new Type[] { typeof(ConstantArgumentClass) }));
        public static readonly RuntimeConstructorDescription TranslatedConstructorWithVariableArgument = new RuntimeConstructorDescription(typeof(TranslatedClass).GetConstructor(new Type[] { typeof(TranslatedArgumentClass) }));

        public static readonly RuntimeConstructorDescription OriginalGenericConstructorWithVariableTypeArgument = new RuntimeConstructorDescription(typeof(OriginalGenericClass<OriginalTypeArgumentClass>).GetConstructor(new Type[0]));
        public static readonly RuntimeConstructorDescription TranslatedGenericConstructorWithVariableTypeArgument = new RuntimeConstructorDescription(typeof(TranslatedGenericClass<TranslatedTypeArgumentClass>).GetConstructor(new Type[0]));

        public static readonly RuntimeConstructorDescription ConstantArgumentConstructor = new RuntimeConstructorDescription(typeof(ConstantArgumentClass).GetConstructor(new Type[0]));
        public static readonly RuntimeConstructorDescription OriginalArgumentConstructor = new RuntimeConstructorDescription(typeof(OriginalArgumentClass).GetConstructor(new Type[0]));
        public static readonly RuntimeConstructorDescription TranslatedArgumentConstructor = new RuntimeConstructorDescription(typeof(TranslatedArgumentClass).GetConstructor(new Type[0]));

        public static readonly RuntimeConstructorDescription ConstantFieldConstructor = new RuntimeConstructorDescription(typeof(ConstantFieldClass).GetConstructor(new Type[0]));
        public static readonly RuntimeConstructorDescription OriginalFieldConstructor = new RuntimeConstructorDescription(typeof(OriginalFieldClass).GetConstructor(new Type[0]));
        public static readonly RuntimeConstructorDescription TranslatedFieldConstructor = new RuntimeConstructorDescription(typeof(TranslatedFieldClass).GetConstructor(new Type[0]));

        public static readonly RuntimeConstructorDescription ConstantPropertyConstructor = new RuntimeConstructorDescription(typeof(ConstantPropertyClass).GetConstructor(new Type[0]));
        public static readonly RuntimeConstructorDescription OriginalPropertyConstructor = new RuntimeConstructorDescription(typeof(OriginalPropertyClass).GetConstructor(new Type[0]));
        public static readonly RuntimeConstructorDescription TranslatedPropertyConstructor = new RuntimeConstructorDescription(typeof(TranslatedPropertyClass).GetConstructor(new Type[0]));

        public static readonly RuntimeConstructorDescription ConstantNestedConstructor = new RuntimeConstructorDescription(typeof(ConstantClass.NestedClass).GetConstructor(new Type[0]));
        public static readonly RuntimeConstructorDescription OriginalNestedConstructor = new RuntimeConstructorDescription(typeof(OriginalClass.NestedClass).GetConstructor(new Type[0]));
        public static readonly RuntimeConstructorDescription TranslatedNestedConstructor = new RuntimeConstructorDescription(typeof(TranslatedClass.NestedClass).GetConstructor(new Type[0]));

        public static readonly RuntimeEventDescription ConstantEvent = new RuntimeEventDescription(typeof(ConstantClass).GetEvent("Event"));
        public static readonly RuntimeEventDescription OriginalEvent = new RuntimeEventDescription(typeof(OriginalClass).GetEvent("Event"));
        public static readonly RuntimeEventDescription OriginalEventWithVariableName = new RuntimeEventDescription(typeof(OriginalClass).GetEvent("EventWithOriginalName"));
        public static readonly RuntimeEventDescription TranslatedEvent = new RuntimeEventDescription(typeof(TranslatedClass).GetEvent("Event"));
        public static readonly RuntimeEventDescription TranslatedEventWithVariableName = new RuntimeEventDescription(typeof(TranslatedClass).GetEvent("EventWithTranslatedName"));

        public static readonly RuntimeEventDescription ConstantStaticEvent = new RuntimeEventDescription(typeof(ConstantClass).GetEvent("StaticEvent"));
        public static readonly RuntimeEventDescription OriginalStaticEvent = new RuntimeEventDescription(typeof(OriginalClass).GetEvent("StaticEvent"));
        public static readonly RuntimeEventDescription OriginalStaticEventWithVariableName = new RuntimeEventDescription(typeof(OriginalClass).GetEvent("StaticEventWithOriginalName"));
        public static readonly RuntimeEventDescription TranslatedStaticEvent = new RuntimeEventDescription(typeof(TranslatedClass).GetEvent("StaticEvent"));
        public static readonly RuntimeEventDescription TranslatedStaticEventWithVariableName = new RuntimeEventDescription(typeof(TranslatedClass).GetEvent("StaticEventWithTranslatedName"));

        public static readonly RuntimeEventDescription ConstantStaticEventInGenericType = new RuntimeEventDescription(typeof(ConstantGenericClass<ConstantTypeArgumentClass>).GetEvent("StaticEvent"));
        public static readonly RuntimeEventDescription OriginalStaticEventInGenericType = new RuntimeEventDescription(typeof(OriginalGenericClass<OriginalTypeArgumentClass>).GetEvent("StaticEvent"));
        public static readonly RuntimeEventDescription OriginalStaticEventWithVariableNameInGenericType = new RuntimeEventDescription(typeof(OriginalGenericClass<OriginalTypeArgumentClass>).GetEvent("StaticEventWithOriginalName"));
        public static readonly RuntimeEventDescription TranslatedStaticEventInGenericType = new RuntimeEventDescription(typeof(TranslatedGenericClass<TranslatedArgumentClass>).GetEvent("StaticEvent"));
        public static readonly RuntimeEventDescription TranslatedStaticEventWithVariableNameInGenericType = new RuntimeEventDescription(typeof(TranslatedGenericClass<TranslatedArgumentClass>).GetEvent("StaticEventWithTranslatedName"));

        public static readonly RuntimeFieldDescription ConstantField = new RuntimeFieldDescription(typeof(ConstantClass).GetField("Field"));
        public static readonly RuntimeFieldDescription OriginalField = new RuntimeFieldDescription(typeof(OriginalClass).GetField("Field"));
        public static readonly RuntimeFieldDescription OriginalFieldWithConstantType = new RuntimeFieldDescription(typeof(OriginalClass).GetField("FieldWithConstantType"));
        public static readonly RuntimeFieldDescription OriginalFieldWithVariableName = new RuntimeFieldDescription(typeof(OriginalClass).GetField("FieldWithOriginalName"));
        public static readonly RuntimeFieldDescription OriginalFieldWithVariableType = new RuntimeFieldDescription(typeof(OriginalClass).GetField("FieldWithVariableType"));
        public static readonly RuntimeFieldDescription TranslatedField = new RuntimeFieldDescription(typeof(TranslatedClass).GetField("Field"));
        public static readonly RuntimeFieldDescription TranslatedFieldWithConstantType = new RuntimeFieldDescription(typeof(TranslatedClass).GetField("FieldWithConstantType"));
        public static readonly RuntimeFieldDescription TranslatedFieldWithVariableName = new RuntimeFieldDescription(typeof(TranslatedClass).GetField("FieldWithTranslatedName"));
        public static readonly RuntimeFieldDescription TranslatedFieldWithVariableType = new RuntimeFieldDescription(typeof(TranslatedClass).GetField("FieldWithVariableType"));

        public static readonly RuntimeFieldDescription ConstantStaticField = new RuntimeFieldDescription(typeof(ConstantClass).GetField("StaticField"));
        public static readonly RuntimeFieldDescription OriginalStaticField = new RuntimeFieldDescription(typeof(OriginalClass).GetField("StaticField"));
        public static readonly RuntimeFieldDescription OriginalStaticFieldWithVariableName = new RuntimeFieldDescription(typeof(OriginalClass).GetField("StaticFieldWithOriginalName"));
        public static readonly RuntimeFieldDescription TranslatedStaticField = new RuntimeFieldDescription(typeof(TranslatedClass).GetField("StaticField"));
        public static readonly RuntimeFieldDescription TranslatedStaticFieldWithVariableName = new RuntimeFieldDescription(typeof(TranslatedClass).GetField("StaticFieldWithTranslatedName"));

        public static readonly RuntimeFieldDescription ConstantStaticFieldInGenericType = new RuntimeFieldDescription(typeof(ConstantGenericClass<ConstantTypeArgumentClass>).GetField("StaticField"));
        public static readonly RuntimeFieldDescription OriginalStaticFieldInGenericType = new RuntimeFieldDescription(typeof(OriginalGenericClass<OriginalTypeArgumentClass>).GetField("StaticField"));
        public static readonly RuntimeFieldDescription OriginalStaticFieldWithVariableNameInGenericType = new RuntimeFieldDescription(typeof(OriginalGenericClass<OriginalTypeArgumentClass>).GetField("StaticFieldWithOriginalName"));
        public static readonly RuntimeFieldDescription TranslatedStaticFieldInGenericType = new RuntimeFieldDescription(typeof(TranslatedGenericClass<TranslatedArgumentClass>).GetField("StaticField"));
        public static readonly RuntimeFieldDescription TranslatedStaticFieldWithVariableNameInGenericType = new RuntimeFieldDescription(typeof(TranslatedGenericClass<TranslatedArgumentClass>).GetField("StaticFieldWithTranslatedName"));

        public static readonly RuntimePropertyDescription ConstantIndexer = new RuntimePropertyDescription(typeof(ConstantClass).GetProperties().First(p => p.GetIndexParameters()[0].ParameterType == typeof(object)));
        public static readonly RuntimePropertyDescription OriginalIndexer = new RuntimePropertyDescription(typeof(OriginalClass).GetProperties().First(p => p.GetIndexParameters()[0].ParameterType == typeof(object)));
        public static readonly RuntimePropertyDescription TranslatedIndexer = new RuntimePropertyDescription(typeof(TranslatedClass).GetProperties().First(p => p.GetIndexParameters()[0].ParameterType == typeof(object)));

        public static readonly RuntimeMethodDescription ConstantMethod = new RuntimeMethodDescription(typeof(ConstantClass).GetMethod("Method", new Type[0]));
        public static readonly RuntimeMethodDescription OriginalMethod = new RuntimeMethodDescription(typeof(OriginalClass).GetMethod("Method", new Type[0]));
        public static readonly RuntimeMethodDescription OriginalMethodWithVariableName = new RuntimeMethodDescription(typeof(OriginalClass).GetMethod("MethodWithOriginalName"));
        public static readonly RuntimeMethodDescription OriginalMethodWithConstantArgument = new RuntimeMethodDescription(typeof(OriginalClass).GetMethod("Method", new Type[] { typeof(ConstantArgumentClass) }));
        public static readonly RuntimeMethodDescription OriginalMethodWithVariableArgument = new RuntimeMethodDescription(typeof(OriginalClass).GetMethod("Method", new Type[] { typeof(OriginalArgumentClass) }));
        public static readonly RuntimeMethodDescription OriginalMethodWithConstantReturnType = new RuntimeMethodDescription(typeof(OriginalClass).GetMethod("MethodWithConstantReturnType"));
        public static readonly RuntimeMethodDescription OriginalMethodWithVariableReturnType = new RuntimeMethodDescription(typeof(OriginalClass).GetMethod("MethodWithVariableReturnType"));
        public static readonly RuntimeMethodDescription TranslatedMethod = new RuntimeMethodDescription(typeof(TranslatedClass).GetMethod("Method", new Type[0]));
        public static readonly RuntimeMethodDescription TranslatedMethodWithVariableName = new RuntimeMethodDescription(typeof(TranslatedClass).GetMethod("MethodWithTranslatedName"));
        public static readonly RuntimeMethodDescription TranslatedMethodWithConstantArgument = new RuntimeMethodDescription(typeof(TranslatedClass).GetMethod("Method", new Type[] { typeof(ConstantArgumentClass) }));
        public static readonly RuntimeMethodDescription TranslatedWithVariableArgument = new RuntimeMethodDescription(typeof(TranslatedClass).GetMethod("Method", new Type[] { typeof(OriginalArgumentClass) }));
        public static readonly RuntimeMethodDescription TranslatedMethodWithConstantReturnType = new RuntimeMethodDescription(typeof(TranslatedClass).GetMethod("MethodWithConstantReturnType"));
        public static readonly RuntimeMethodDescription TranslatedMethodWithVariableReturnType = new RuntimeMethodDescription(typeof(TranslatedClass).GetMethod("MethodWithVariableReturnType"));

        public static readonly RuntimeMethodDescription ConstantStaticMethod = new RuntimeMethodDescription(typeof(ConstantClass).GetMethod("StaticMethod", new Type[0]));
        public static readonly RuntimeMethodDescription OriginalStaticMethod = new RuntimeMethodDescription(typeof(OriginalClass).GetMethod("StaticMethod", new Type[0]));
        public static readonly RuntimeMethodDescription OriginalStaticMethodWithVariableName = new RuntimeMethodDescription(typeof(OriginalClass).GetMethod("StaticMethodWithOriginalName"));
        public static readonly RuntimeMethodDescription TranslatedStaticMethod = new RuntimeMethodDescription(typeof(TranslatedClass).GetMethod("StaticMethod", new Type[0]));
        public static readonly RuntimeMethodDescription TranslatedStaticMethodWithVariableName = new RuntimeMethodDescription(typeof(TranslatedClass).GetMethod("StaticMethodWithTranslatedName"));

        public static readonly RuntimeMethodDescription ConstantStaticMethodInGenericType = new RuntimeMethodDescription(typeof(ConstantGenericClass<ConstantTypeArgumentClass>).GetMethod("StaticMethod", new Type[0]));
        public static readonly RuntimeMethodDescription OriginalStaticMethodInGenericType = new RuntimeMethodDescription(typeof(OriginalGenericClass<OriginalTypeArgumentClass>).GetMethod("StaticMethod", new Type[0]));
        public static readonly RuntimeMethodDescription OriginalStaticMethodWithVariableNameInGenericType = new RuntimeMethodDescription(typeof(OriginalGenericClass<OriginalTypeArgumentClass>).GetMethod("StaticMethodWithOriginalName"));
        public static readonly RuntimeMethodDescription TranslatedStaticMethodInGenericType = new RuntimeMethodDescription(typeof(TranslatedGenericClass<TranslatedArgumentClass>).GetMethod("StaticMethod", new Type[0]));
        public static readonly RuntimeMethodDescription TranslatedStaticMethodWithVariableNameInGenericType = new RuntimeMethodDescription(typeof(TranslatedGenericClass<TranslatedArgumentClass>).GetMethod("StaticMethodWithTranslatedName"));

        public static readonly RuntimeMethodDescription ConstantGenericMethodWithOriginalTypeArgument = new RuntimeMethodDescription(typeof(ConstantClass).GetMethod("GenericMethod", new Type[0]).MakeGenericMethod(new Type[] { typeof(OriginalTypeArgumentClass) }));
        public static readonly RuntimeMethodDescription ConstantGenericMethodWithTranslatedTypeArgument = new RuntimeMethodDescription(typeof(ConstantClass).GetMethod("GenericMethod", new Type[0]).MakeGenericMethod(new Type[] { typeof(TranslatedTypeArgumentClass) }));
        public static readonly RuntimeMethodDescription OriginalGenericMethodWithVariableTypeArgument = new RuntimeMethodDescription(typeof(OriginalClass).GetMethod("GenericMethod", new Type[0]).MakeGenericMethod(new Type[] { typeof(OriginalTypeArgumentClass) }));
        public static readonly RuntimeMethodDescription TranslatedGenericMethodWithVariableTypeArgument = new RuntimeMethodDescription(typeof(TranslatedClass).GetMethod("GenericMethod", new Type[0]).MakeGenericMethod(new Type[] { typeof(TranslatedTypeArgumentClass) }));

        public static readonly RuntimeMethodDescription ConstantStaticGenericMethodWithOriginalTypeArgument = new RuntimeMethodDescription(typeof(ConstantClass).GetMethod("StaticGenericMethod", new Type[0]).MakeGenericMethod(new Type[] { typeof(OriginalTypeArgumentClass) }));
        public static readonly RuntimeMethodDescription ConstantStaticGenericMethodWithTranslatedTypeArgument = new RuntimeMethodDescription(typeof(ConstantClass).GetMethod("StaticGenericMethod", new Type[0]).MakeGenericMethod(new Type[] { typeof(TranslatedTypeArgumentClass) }));
        public static readonly RuntimeMethodDescription OriginalStaticGenericMethodWithVariableTypeArgument = new RuntimeMethodDescription(typeof(OriginalClass).GetMethod("StaticGenericMethod", new Type[0]).MakeGenericMethod(new Type[] { typeof(OriginalTypeArgumentClass) }));
        public static readonly RuntimeMethodDescription TranslatedStaticGenericMethodWithVariableTypeArgument = new RuntimeMethodDescription(typeof(TranslatedClass).GetMethod("StaticGenericMethod", new Type[0]).MakeGenericMethod(new Type[] { typeof(TranslatedTypeArgumentClass) }));

        public static readonly RuntimePropertyDescription ConstantProperty = new RuntimePropertyDescription(typeof(ConstantClass).GetProperty("Property"));
        public static readonly RuntimePropertyDescription OriginalProperty = new RuntimePropertyDescription(typeof(OriginalClass).GetProperty("Property"));
        public static readonly RuntimePropertyDescription OriginalPropertyWithVariableName = new RuntimePropertyDescription(typeof(OriginalClass).GetProperty("PropertyWithOriginalName"));
        public static readonly RuntimePropertyDescription OriginalPropertyWithConstantType = new RuntimePropertyDescription(typeof(OriginalClass).GetProperty("PropertyWithConstantType"));
        public static readonly RuntimePropertyDescription OriginalPropertyWithVariableType = new RuntimePropertyDescription(typeof(OriginalClass).GetProperty("PropertyWithVariableType"));
        public static readonly RuntimePropertyDescription TranslatedProperty = new RuntimePropertyDescription(typeof(TranslatedClass).GetProperty("Property"));
        public static readonly RuntimePropertyDescription TranslatedPropertyWithVariableName = new RuntimePropertyDescription(typeof(TranslatedClass).GetProperty("PropertyWithTranslatedName"));
        public static readonly RuntimePropertyDescription TranslatedPropertyWithConstantType = new RuntimePropertyDescription(typeof(TranslatedClass).GetProperty("PropertyWithConstantType"));
        public static readonly RuntimePropertyDescription TranslatedPropertyWithVariableType = new RuntimePropertyDescription(typeof(TranslatedClass).GetProperty("PropertyWithVariableType"));

        public static readonly RuntimePropertyDescription ConstantStaticProperty = new RuntimePropertyDescription(typeof(ConstantClass).GetProperty("StaticProperty"));
        public static readonly RuntimePropertyDescription OriginalStaticProperty = new RuntimePropertyDescription(typeof(OriginalClass).GetProperty("StaticProperty"));
        public static readonly RuntimePropertyDescription OriginalStaticPropertyWithVariableName = new RuntimePropertyDescription(typeof(OriginalClass).GetProperty("StaticPropertyWithOriginalName"));
        public static readonly RuntimePropertyDescription TranslatedStaticProperty = new RuntimePropertyDescription(typeof(TranslatedClass).GetProperty("StaticProperty"));
        public static readonly RuntimePropertyDescription TranslatedStaticPropertyWithVariableName = new RuntimePropertyDescription(typeof(TranslatedClass).GetProperty("StaticPropertyWithTranslatedName"));

        public static readonly RuntimePropertyDescription ConstantStaticPropertyInGenericType = new RuntimePropertyDescription(typeof(ConstantGenericClass<ConstantTypeArgumentClass>).GetProperty("StaticProperty"));
        public static readonly RuntimePropertyDescription OriginalStaticPropertyInGenericType = new RuntimePropertyDescription(typeof(OriginalGenericClass<OriginalTypeArgumentClass>).GetProperty("StaticProperty"));
        public static readonly RuntimePropertyDescription OriginalStaticPropertyWithVariableNameInGenericType = new RuntimePropertyDescription(typeof(OriginalGenericClass<OriginalTypeArgumentClass>).GetProperty("StaticPropertyWithOriginalName"));
        public static readonly RuntimePropertyDescription TranslatedStaticPropertyInGenericType = new RuntimePropertyDescription(typeof(TranslatedGenericClass<TranslatedArgumentClass>).GetProperty("StaticProperty"));
        public static readonly RuntimePropertyDescription TranslatedStaticPropertyWithVariableNameInGenericType = new RuntimePropertyDescription(typeof(TranslatedGenericClass<TranslatedArgumentClass>).GetProperty("StaticPropertyWithTranslatedName"));
    }
}