using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;

namespace OleVanSanten.TestTools.Structure
{
    public static class StructureTestExtensions
    {
        // Design note, methods with qualifiers (like public & abstract) should not be extendable
        // and instead the methods without qualifiers should be used. This is done in an effort to avoid
        // StructureTest.AssertPublicClass<T>(new TypeIsAbstract()) and
        // StructureTest.AssertAbstractClass<T>(new TypeAccessLevel(AccessLevel.Public)),
        // which are fully equavilent however look different.

        public static void AssertClass(this StructureTest test, Type type, params ITypeVerifier[] verifiers)
        {
            test.AssertType(type, verifiers);
        }

        public static void AssertClass<T>(this StructureTest test, params ITypeVerifier[] verifiers)
        {
            test.AssertClass(typeof(T), verifiers);
        }

        public static void AssertAbstractClass<T>(this StructureTest test)
        {
            test.AssertClass<T>(new TypeIsAbstractVerifier(true));
        }

        public static void AssertPublicClass<T>(this StructureTest test)
        {
            test.AssertClass<T>(new TypeAccessLevelVerifier(AccessLevels.Public));
        }

        public static void AssertDelegate<T, TDelegate>(this StructureTest test, params ITypeVerifier[] verifiers) where TDelegate : Delegate
        {
            ITypeVerifier[] additionalVerifiers = new ITypeVerifier[] { 
                new TypeIsDelegateVerifier(),
                new DelegateSignatureVerifier(typeof(TDelegate))
            };
            ITypeVerifier[] allVerifiers = verifiers.Union(additionalVerifiers).ToArray();
            test.AssertType(typeof(T), allVerifiers);
        }

        public static void AssertPublicDelegate<T, TDelegate>(this StructureTest test) where TDelegate : Delegate
        {
            test.AssertDelegate<T, TDelegate>(new TypeAccessLevelVerifier(AccessLevels.Public));
        }

        public static void AssertInterface<T>(this StructureTest test, params ITypeVerifier[] verifiers)
        {
            ITypeVerifier[] allVerifiers = verifiers.Union(new[] { new TypeIsInterfaceVerifier() }).ToArray();
            test.AssertType(typeof(T), allVerifiers);
        }

        public static void AssertPublicInterface<T>(this StructureTest test)
        {
            test.AssertInterface<T>(new TypeAccessLevelVerifier(AccessLevels.Public));
        }

        public static void AssertConstructor(this StructureTest test, ConstructorInfo constructorInfo, params IMemberVerifier[] verifiers)
        {
            IMemberVerifier[] allVerifiers = verifiers.Union(new[] { new MemberTypeVerifier(TypeSystem.MemberTypes.Constructor) }).ToArray();
            test.AssertMember(constructorInfo, allVerifiers);
        }

        public static void AssertConstructor<TReturn>(this StructureTest test, Expression<Func<TReturn>> locator, params IMemberVerifier[] verifiers)
        {
            NewExpression newExpression = (NewExpression)locator.Body;
            ConstructorInfo constructorInfo = newExpression.Constructor;
            test.AssertConstructor(constructorInfo, verifiers);
        }

        public static void AssertConstructor<TPar1, TReturn>(this StructureTest test, Expression<Func<TPar1, TReturn>> locator, params IMemberVerifier[] verifiers)
        {
            NewExpression newExpression = (NewExpression)locator.Body;
            ConstructorInfo constructorInfo = newExpression.Constructor;
            test.AssertConstructor(constructorInfo, verifiers);
        }

        public static void AssertConstructor<TPar1, TPar2, TReturn>(this StructureTest test, Expression<Func<TPar1, TPar2, TReturn>> locator, params IMemberVerifier[] verifiers)
        {
            NewExpression newExpression = (NewExpression)locator.Body;
            ConstructorInfo constructorInfo = newExpression.Constructor;
            test.AssertConstructor(constructorInfo, verifiers);
        }

        public static void AssertPublicConstructor<TReturn>(this StructureTest test, Expression<Func<TReturn>> locator)
        {
            test.AssertConstructor<TReturn>(locator, new MemberAccessLevelVerifier(AccessLevels.Public));
        }

        public static void AssertPublicConstructor<TPar1, TReturn>(this StructureTest test, Expression<Func<TPar1, TReturn>> locator)
        {
            test.AssertConstructor<TPar1, TReturn>(locator, new MemberAccessLevelVerifier(AccessLevels.Public));
        }

        public static void AssertPublicConstructor<TPar1, TPar2, TReturn>(this StructureTest test, Expression<Func<TPar1, TPar2, TReturn>> locator)
        {
            test.AssertConstructor<TPar1, TPar2, TReturn>(locator, new MemberAccessLevelVerifier(AccessLevels.Public));
        }

        public static void AssertEvent(this StructureTest test, EventInfo info, params IMemberVerifier[] verifiers)
        {
            IMemberVerifier[] allVerifiers = verifiers.Union(new[] { new MemberTypeVerifier(TypeSystem.MemberTypes.Event) }).ToArray();
            test.AssertMember(info, allVerifiers);
        }

        public static void AssertPublicEvent(this StructureTest test, EventInfo info)
        {
            test.AssertEvent(info, new MemberAccessLevelVerifier(AccessLevels.Public));
        }

        public static void AssertField(this StructureTest test, FieldInfo fieldInfo, params IMemberVerifier[] verifiers)
        {
            IMemberVerifier[] allVerifiers = verifiers.Union(new[] { new MemberTypeVerifier(TypeSystem.MemberTypes.Field) }).ToArray();
            test.AssertMember(fieldInfo, allVerifiers);
        }

        public static void AssertField<TInstance, TField>(this StructureTest test, Expression<Func<TInstance, TField>> locator, params IMemberVerifier[] verifiers)
        {
            MemberExpression newExpression = (MemberExpression)locator.Body;
            FieldInfo fieldInfo = (FieldInfo)newExpression.Member;
            test.AssertMember(fieldInfo, verifiers);
        }

        public static void AssertPublicField<TInstance, TField>(this StructureTest test, Expression<Func<TInstance, TField>> locator)
        {
            test.AssertField(locator, new MemberAccessLevelVerifier(AccessLevels.Public));
        }

        public static void AssertPublicReadonlyField<TInstance, TField>(this StructureTest test, Expression<Func<TInstance, TField>> locator)
        {
            test.AssertField(locator, new FieldIsInitOnlyVerifier(), new MemberAccessLevelVerifier(AccessLevels.Public));
        }

        public static void AssertProperty(this StructureTest test, PropertyInfo propertyInfo, params IMemberVerifier[] verifiers)
        {
            IMemberVerifier[] allVerifiers = verifiers.Union(new[] { new MemberTypeVerifier(TypeSystem.MemberTypes.Property) }).ToArray();
            test.AssertMember(propertyInfo, allVerifiers);
        }

        public static void AssertProperty<TInstance, TProperty>(this StructureTest test, Expression<Func<TInstance, TProperty>> locator, params IMemberVerifier[] verifiers)
        {
            MemberExpression newExpression = (MemberExpression)locator.Body;
            PropertyInfo propertyInfo = (PropertyInfo)newExpression.Member;
            test.AssertProperty(propertyInfo, verifiers);
        }

        public static void AssertPublicProperty<TInstance, TProperty>(this StructureTest test, PropertyInfo propertyInfo)
        {
            test.AssertProperty(propertyInfo, new MemberAccessLevelVerifier(AccessLevels.Public));
        }

        public static void AssertPublicProperty<TInstance, TProperty>(this StructureTest test, Expression<Func<TInstance, TProperty>> locator)
        {
            test.AssertProperty(locator, new MemberAccessLevelVerifier(AccessLevels.Public));
        }

        public static void AssertPublicReadonlyProperty(this StructureTest test, PropertyInfo propertyInfo)
        {
            test.AssertProperty(propertyInfo, new PropertyIsReadonlyVerifier());
        }

        public static void AssertPublicReadonlyProperty<TInstance, TProperty>(this StructureTest test, Expression<Func<TInstance, TProperty>> locator)
        {
            test.AssertProperty(locator, new PropertyIsReadonlyVerifier());
        }

        public static void AssertPublicWriteonlyProperty<TInstance, TProperty>(this StructureTest test, Expression<Func<TInstance, TProperty>> locator)
        {
            test.AssertProperty(locator, new MemberAccessLevelVerifier(AccessLevels.Public), new PropertyIsWriteonlyVerifier());
        }

        public static void AssertMethod(this StructureTest test, MethodInfo methodInfo, params IMemberVerifier[] verifiers)
        {
            IMemberVerifier[] allVerifiers = verifiers.Union(new[] { new MemberTypeVerifier(TypeSystem.MemberTypes.Method) }).ToArray();
            test.AssertMember(methodInfo, allVerifiers);
        }

        public static void AssertMethod<TInstance, TReturn>(this StructureTest test, Expression<Func<TInstance, TReturn>> locator, params IMemberVerifier[] verifiers)
        {
            MethodCallExpression methodCallExpression = (MethodCallExpression)locator.Body;
            test.AssertMethod(methodCallExpression.Method, verifiers);
        }

        public static void AssertMethod<TInstance, TPar1, TReturn>(this StructureTest test, Expression<Func<TInstance, TPar1, TReturn>> locator, params IMemberVerifier[] verifiers)
        {
            MethodCallExpression methodCallExpression = (MethodCallExpression)locator.Body;
            test.AssertMethod(methodCallExpression.Method, verifiers);
        }

        public static void AssertMethod<TInstance, TPar1, TPar2, TReturn>(this StructureTest test, Expression<Func<TInstance, TPar1, TPar2, TReturn>> locator, params IMemberVerifier[] verifiers)
        {
            MethodCallExpression methodCallExpression = (MethodCallExpression)locator.Body;
            test.AssertMethod(methodCallExpression.Method, verifiers);
        }

        public static void AssertMethod<TInstance>(this StructureTest test, Expression<Action<TInstance>> locator, params IMemberVerifier[] verifiers)
        {
            MethodCallExpression methodCallExpression = (MethodCallExpression)locator.Body;
            test.AssertMethod(methodCallExpression.Method, verifiers);
        }

        public static void AssertMethod<TInstance, TPar1>(this StructureTest test, Expression<Action<TInstance, TPar1>> locator, params IMemberVerifier[] verifiers)
        {
            MethodCallExpression methodCallExpression = (MethodCallExpression)locator.Body;
            test.AssertMethod(methodCallExpression.Method, verifiers);
        }

        public static void AssertMethod<TInstance, TPar1, TPar2>(this StructureTest test, Expression<Action<TInstance, TPar1, TPar2>> locator, params IMemberVerifier[] verifiers)
        {
            MethodCallExpression methodCallExpression = (MethodCallExpression)locator.Body;
            test.AssertMethod(methodCallExpression.Method, verifiers);
        }

        public static void AssertPublicMethod<TInstance, TReturn>(this StructureTest test, Expression<Func<TInstance, TReturn>> locator)
        {
            test.AssertMethod(locator, new MemberAccessLevelVerifier(AccessLevels.Public));
        }

        public static void AssertPublicMethod<TInstance, TPar1, TReturn>(this StructureTest test, Expression<Func<TInstance, TPar1, TReturn>> locator)
        {
            test.AssertMethod(locator, new MemberAccessLevelVerifier(AccessLevels.Public));
        }

        public static void AssertPublicMethod<TInstance, TPar1, TPar2, TReturn>(this StructureTest test, Expression<Func<TInstance, TPar1, TPar2, TReturn>> locator)
        {
            test.AssertMethod(locator, new MemberAccessLevelVerifier(AccessLevels.Public));
        }

        public static void AssertPublicMethod<TInstance>(this StructureTest test, Expression<Action<TInstance>> locator)
        {
            test.AssertMethod(locator, new MemberAccessLevelVerifier(AccessLevels.Public));
        }

        public static void AssertPublicMethod<TInstance, TPar1>(this StructureTest test, Expression<Action<TInstance, TPar1>> locator)
        {
            test.AssertMethod(locator, new MemberAccessLevelVerifier(AccessLevels.Public));
        }

        public static void AssertPublicMethod<TInstance, TPar1, TPar2>(this StructureTest test, Expression<Action<TInstance, TPar1, TPar2>> locator)
        {
            test.AssertMethod(locator, new MemberAccessLevelVerifier(AccessLevels.Public));
        }

        public static void AssertStaticMethod<TReturn>(this StructureTest test, Expression<Func<TReturn>> locator, params IMemberVerifier[] verifiers)
        {
            IMemberVerifier[] allVerifiers = verifiers.Union(new[] { new MemberIsStaticVerifier() }).ToArray();
            MethodCallExpression methodCallExpression = (MethodCallExpression)locator.Body;
            test.AssertMethod(methodCallExpression.Method, allVerifiers);
        }

        public static void AssertStaticMethod<TPar1, TReturn>(this StructureTest test, Expression<Func<TPar1, TReturn>> locator, params IMemberVerifier[] verifiers)
        {
            IMemberVerifier[] allVerifiers = verifiers.Union(new[] { new MemberIsStaticVerifier() }).ToArray();
            MethodCallExpression methodCallExpression = (MethodCallExpression)locator.Body;
            test.AssertMethod(methodCallExpression.Method, allVerifiers);
        }

        public static void AssertStaticMethod<TPar1, TPar2, TReturn>(this StructureTest test, Expression<Func<TPar1, TPar2, TReturn>> locator, params IMemberVerifier[] verifiers)
        {
            IMemberVerifier[] allVerifiers = verifiers.Union(new[] { new MemberIsStaticVerifier() }).ToArray();
            MethodCallExpression methodCallExpression = (MethodCallExpression)locator.Body;
            test.AssertMethod(methodCallExpression.Method, allVerifiers);
        }

        public static void AssertStaticMethod(this StructureTest test, Expression<Action> locator, params IMemberVerifier[] verifiers)
        {
            IMemberVerifier[] allVerifiers = verifiers.Union(new[] { new MemberIsStaticVerifier() }).ToArray();
            MethodCallExpression methodCallExpression = (MethodCallExpression)locator.Body;
            test.AssertMethod(methodCallExpression.Method, allVerifiers);
        }

        public static void AssertStaticMethod<TPar1>(this StructureTest test, Expression<Action<TPar1>> locator, params IMemberVerifier[] verifiers)
        {
            IMemberVerifier[] allVerifiers = verifiers.Union(new[] { new MemberIsStaticVerifier() }).ToArray();
            MethodCallExpression methodCallExpression = (MethodCallExpression)locator.Body;
            test.AssertMethod(methodCallExpression.Method, allVerifiers);
        }

        public static void AssertStaticMethod<TPar1, TPar2>(this StructureTest test, Expression<Action<TPar1, TPar2>> locator, params IMemberVerifier[] verifiers)
        {
            IMemberVerifier[] allVerifiers = verifiers.Union(new[] { new MemberIsStaticVerifier() }).ToArray();
            MethodCallExpression methodCallExpression = (MethodCallExpression)locator.Body;
            test.AssertMethod(methodCallExpression.Method, allVerifiers);
        }
    }
}
