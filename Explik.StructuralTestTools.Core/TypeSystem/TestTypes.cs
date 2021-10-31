using System;
using System.Collections.Generic;
using System.Text;

namespace TestTypes
{
    public class ConstantArgumentClass { }

    public class OriginalArgumentClass { }

    public class TranslatedArgumentClass { }

    public class ConstantFieldClass { }

    public class OriginalFieldClass { }

    public class TranslatedFieldClass { }

    public class ConstantMethodReturnClass { }

    public class OriginalMethodReturnClass { }

    public class TranslatedMethodReturnClass { }

    public class ConstantPropertyClass { }

    public class OriginalPropertyClass { }

    public class TranslatedPropertyClass { }

    public class ConstantTypeArgumentClass { }

    public class OriginalTypeArgumentClass { }

    public class TranslatedTypeArgumentClass { }

    public struct ConstantStruct { }

    public struct OriginalStruct { }

    public struct TranslatedStruct { }

    public class ConstantClass
    {
        public ConstantClass() { }

        public ConstantClass(object arg) { }

        public event EventHandler Event;

        public static event EventHandler StaticEvent;

        public object Field;

        public static object StaticField;

        public object this[object arg]
        {
            get { return null; }
            set { }
        }

        public class NestedClass { }
        
        public object Method() { return null; }

        public object Method(object arg) { return null; }

        public T GenericMethod<T>() { return default(T); }

        public T GenericMethod<T>(T arg) { return default(T); }

        public static object StaticMethod() { return null; }

        public static T StaticGenericMethod<T>() { return default(T); }

        public object Property { get; set; }

        public static object StaticProperty { get; set; }
    }

    public class OriginalClass
    {
        public OriginalClass() { }

        public OriginalClass(ConstantArgumentClass arg) { }

        public OriginalClass(OriginalArgumentClass arg) { }

        public event EventHandler Event;

        public event EventHandler EventWithOriginalName;

        public static event EventHandler StaticEvent;

        public static event EventHandler StaticEventWithOriginalName;

        public object Field;

        public object FieldWithOriginalName;

        public ConstantFieldClass FieldWithConstantType;

        public OriginalFieldClass FieldWithVariableType;

        public static object StaticField;

        public static object StaticFieldWithOriginalName;

        public object this[object arg]
        {
            get { return null; }
            set { }
        }

        public object this[ConstantArgumentClass arg]
        {
            get { return null; }
            set { }
        }

        public object this[OriginalArgumentClass arg]
        {
            get { return null; }
            set { }
        }

        public class NestedClass { }
        
        public object Method() { return null; }

        public object Method(ConstantArgumentClass arg) { return null; }

        public object Method(OriginalArgumentClass arg) { return null; }

        public object MethodWithOriginalName() { return null; }

        public ConstantMethodReturnClass MethodWithConstantReturnType() { return null; }

        public OriginalMethodReturnClass MethodWithVariableReturnType() { return null; }

        public T GenericMethod<T>() { return default(T); }

        public T GenericMethod<T>(T arg) { return default(T); }

        public static object StaticMethod() { return null; }

        public static object StaticMethodWithOriginalName() { return null; }

        public static T StaticGenericMethod<T>() { return default(T); }

        public object Property { get; set; }

        public object PropertyWithOriginalName { get; set; }

        public ConstantPropertyClass PropertyWithConstantType { get; set; }

        public OriginalPropertyClass PropertyWithVariableType { get; set; }

        public static object StaticProperty { get; set; }

        public static object StaticPropertyWithOriginalName { get; set; }
    }

    public class TranslatedClass
    {
        public TranslatedClass() { }

        public TranslatedClass(ConstantArgumentClass arg) { }

        public TranslatedClass(OriginalArgumentClass arg) { }

        public event EventHandler Event;

        public event EventHandler EventWithTranslatedName;

        public static event EventHandler StaticEvent;

        public static event EventHandler StaticEventWithTranslatedName;

        public object Field;

        public object FieldWithTranslatedName;

        public ConstantFieldClass FieldWithConstantType;

        public TranslatedFieldClass FieldWithVariableType;

        public static object StaticField;

        public static object StaticFieldWithTranslatedName;

        public object this[object arg]
        {
            get { return null; }
            set { }
        }

        public object this[ConstantArgumentClass arg]
        {
            get { return null; }
            set { }
        }

        public object this[OriginalArgumentClass arg]
        {
            get { return null; }
            set { }
        }

        public class NestedClass { }

        public object Method() { return null; }

        public object Method(ConstantArgumentClass arg) { return null; }

        public object Method(OriginalArgumentClass arg) { return null; }

        public object MethodWithTranslatedName() { return null; }

        public ConstantMethodReturnClass MethodWithConstantReturnType() { return null; }

        public TranslatedMethodReturnClass MethodWithVariableReturnType() { return null; }

        public T GenericMethod<T>() { return default(T); }

        public T GenericMethod<T>(T arg) { return default(T); }

        public static object StaticMethod() { return null; }

        public static object StaticMethodWithTranslatedName() { return null; }

        public static T StaticGenericMethod<T>() { return default(T); }

        public object Property { get; set; }

        public object PropertyWithTranslatedName { get; set; }

        public ConstantPropertyClass PropertyWithConstantType { get; set; }

        public TranslatedPropertyClass PropertyWithVariableType { get; set; }

        public static object StaticProperty { get; set; }

        public static object StaticPropertyWithTranslatedName { get; set; }
    }

    public class ConstantGenericClass<T> 
    {
        public static event EventHandler StaticEvent;

        public static object StaticField;

        public static object StaticMethod() { return null; }

        public static object StaticProperty { get; set; }
    }

    public class OriginalGenericClass<T> 
    {
        public static event EventHandler StaticEvent;

        public static event EventHandler StaticEventWithOriginalName;

        public static object StaticField;

        public static object StaticFieldWithOriginalName;

        public static object StaticMethod() { return null; }

        public static object StaticMethodWithOriginalName() { return null; }

        public static object StaticPropertyWithOriginalName { get; set; }
    }

    public class TranslatedGenericClass<T> 
    {
        public static event EventHandler StaticEvent;

        public static event EventHandler StaticEventWithTranslatedName;

        public static object StaticField;

        public static object StaticFieldWithTranslatedName;

        public static object StaticMethod() { return null; }

        public static object StaticMethodWithTranslatedName() { return null; }

        public static object StaticProperty { get; set; }

        public static object StaticPropertyWithTranslatedName { get; set; }
    }
}