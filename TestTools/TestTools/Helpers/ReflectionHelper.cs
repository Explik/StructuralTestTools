﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using TestTools.Structure;
using TestTools.Structure.Generic;

namespace TestTools.Helpers
{
    public static class ReflectionHelper
    {
        // Manipulation
        public static object GetValue(MemberInfo memberInfo, object instance)
        {
            Func<object> getFunc;

            if (memberInfo is FieldInfo fieldInfo)
                getFunc = () => fieldInfo.GetValue(instance);
            else if (memberInfo is PropertyInfo propertyInfo)
                getFunc = () => propertyInfo.GetValue(instance);
            else throw new ArgumentException("INTERNAL: memberInfo is not field or property");

            try
            {
                return getFunc();
            }
            catch (Exception ex)
            {
                //rethrow of exception makes reflection transparent to user
                throw new AssertFailedException(ex.ToString(), ex);
            }
        }

        public static void SetValue(MemberInfo memberInfo, object instance, object value)
        {
            Action setAction;

            if (memberInfo is FieldInfo fieldInfo)
            {
                AssertObjectIsOfType(fieldInfo.FieldType, value);
                setAction = () => fieldInfo.SetValue(instance, value);
            }
            else if (memberInfo is PropertyInfo propertyInfo)
            {
                AssertObjectIsOfType(propertyInfo.PropertyType, value);
                setAction = () => propertyInfo.SetValue(instance, value);
            }
            else throw new ArgumentException("INTERNAL: memberInfo is not field or property");

            try
            {
                setAction();
            }
            catch (Exception ex)
            {
                //rethrow of exception makes reflection transparent to user
                throw new AssertFailedException(ex.ToString(), ex);
            }
        }

        public static object Invoke(MemberInfo memberInfo, object instance, object[] arguments)
        {
            Func<object> invokeFunc;

            if (memberInfo is MethodInfo methodInfo)
            {
                object[] argumentsInclDefaultValues = GetArgumentsAndDefaults(methodInfo.GetParameters(), arguments);
                invokeFunc = () => methodInfo.Invoke(instance, argumentsInclDefaultValues);
            }
            else if (memberInfo is EventInfo eventInfo)
            {
                throw new NotImplementedException();
            }
            else if (memberInfo is ConstructorInfo constructorInfo)
            {
                object[] argumentsInclDefaultValues = GetArgumentsAndDefaults(constructorInfo.GetParameters(), arguments);
                invokeFunc = () => constructorInfo.Invoke(argumentsInclDefaultValues);
            }
            else throw new ArgumentException("INTERNAL: memberInfo is not method, event or constructor");

            try
            {
                return invokeFunc();
            }
            catch (Exception ex)
            {
                //rethrow of exception makes reflection transparent to user
                throw new AssertFailedException(ex.InnerException.ToString(), ex.InnerException);
            }
        }

        // Infos
        public static FieldInfo GetFieldInfo(Type type, FieldOptions options, bool isStatic = false)
        {
            string typeName = FormatHelper.FormatType(type);
            MemberInfo memberInfo = GetMembers(type, isStatic).FirstOrDefault(m => m.Name == options.Name);
            FieldInfo fieldInfo = memberInfo as FieldInfo;

            AssertMemberExists(type, options.Name, ErrorCodes.MemberIsMissing);
            AssertMemberIsInstanceOrStatic(type, options.Name, isStatic);
            AssertMemberIs<FieldInfo>(type, memberInfo, ErrorCodes.MemberIsWrongMemberType);
            AssertMemberIsOfType(type, memberInfo, options.FieldType, ErrorCodes.FieldIsWrongType);

            if (options.IsInitOnly != null && options.IsInitOnly != fieldInfo.IsInitOnly)
            {
                if (options.IsInitOnly == false)
                    throw new AssertFailedException(string.Format("{0}.{1} is readonly", typeName, options.Name));
                if (options.IsInitOnly == true)
                    throw new AssertFailedException(string.Format("{0}.{1} is not readonly", typeName, options.Name));
            }

            if (options.IsPrivate != null && options.IsPrivate != fieldInfo.IsPrivate)
            {
                if (options.IsPrivate == false)
                    throw new AssertFailedException(string.Format(ErrorCodes.FieldIsPrivate, typeName, options.Name));
                if (options.IsPrivate == true)
                    throw new AssertFailedException(string.Format(ErrorCodes.FieldIsNonPrivate, typeName, options.Name));
            }
            if (options.IsFamily != null && options.IsFamily != fieldInfo.IsFamily)
            {
                if (options.IsFamily == false)
                    throw new AssertFailedException(string.Format(ErrorCodes.FieldIsProtected, typeName, options.Name));
                if (options.IsFamily == true)
                    throw new AssertFailedException(string.Format(ErrorCodes.FieldIsNonProtected, typeName, options.Name));
            }
            if (options.IsPublic != null && options.IsPublic != fieldInfo.IsPublic)
            {
                if (options.IsPublic == false)
                    throw new AssertFailedException(string.Format(ErrorCodes.FieldIsPublic, typeName, options.Name));
                if (options.IsPublic == true)
                    throw new AssertFailedException(string.Format(ErrorCodes.FieldIsNonPublic, typeName, options.Name));
            }
            
            return (FieldInfo)memberInfo;
        }

        public static PropertyInfo GetPropertyInfo(Type type, PropertyOptions options, bool isStatic = false)
        {
            string typeName = FormatHelper.FormatType(type);
            MemberInfo memberInfo = GetMembers(type, isStatic).FirstOrDefault(m => m.Name == options.Name);
            PropertyInfo propertyInfo = memberInfo as PropertyInfo;
            
            if (propertyInfo == null)
                throw new AssertFailedException(string.Format(ErrorCodes.MemberIsMissing, typeName, options.Name));
            
            AssertMemberIsInstanceOrStatic(type, options.Name, isStatic);
            AssertMemberIs<PropertyInfo>(type, memberInfo, ErrorCodes.MemberIsWrongMemberType);
            AssertMemberIsOfType(type, memberInfo, options.PropertyType, ErrorCodes.PropertyIsWrongType);
            
            if (options.GetMethod != null)
            {
                MethodOptions get = (MethodOptions)options.GetMethod;

                if (!propertyInfo.CanRead)
                    throw new AssertFailedException(string.Format(ErrorCodes.PropertyIsMissingGet, typeName, options.Name));
                
                if (get.IsAbstract != null && get.IsAbstract != propertyInfo.GetMethod.IsAbstract)
                {
                    if (get.IsAbstract == false)
                        throw new AssertFailedException(string.Format(ErrorCodes.PropertyGetIsAbstract, typeName, options.Name));
                    if (get.IsAbstract == true)
                        throw new AssertFailedException(string.Format(ErrorCodes.PropertyGetIsNonAbstract, typeName, options.Name));
                }

                if (get.IsVirtual != null && get.IsVirtual == propertyInfo.GetMethod.IsVirtual)
                {
                    if (get.IsVirtual == false)
                        throw new AssertFailedException(string.Format(ErrorCodes.PropertyGetIsAbstract, typeName, options.Name));
                    if (get.IsVirtual == true)
                        throw new AssertFailedException(string.Format(ErrorCodes.PropertyGetIsNonAbstract, typeName, options.Name)); 
                }

                if (get.DeclaringType != null && options.GetMethod?.DeclaringType != propertyInfo.GetMethod.DeclaringType)
                    throw new AssertFailedException(string.Format(ErrorCodes.PropertyGetHasWrongDeclaringType, typeName, options.Name));

                if (get.IsPrivate != null && get.IsPrivate != propertyInfo.GetMethod.IsPrivate) {
                    if (get.IsPrivate == false)
                        throw new AssertFailedException(string.Format(ErrorCodes.PropertyGetIsPrivate, typeName, options.Name));
                    if (get.IsPrivate == true)
                        throw new AssertFailedException(string.Format(ErrorCodes.PropertyGetIsNonPrivate, typeName, options.Name));
                }
                if (get.IsFamily != null && get.IsFamily != propertyInfo.GetMethod.IsFamily)
                {
                    if (get.IsFamily == false)
                        throw new AssertFailedException(string.Format(ErrorCodes.PropertyGetIsProtected, typeName, options.Name));
                    if (get.IsFamily == true)
                        throw new AssertFailedException(string.Format(ErrorCodes.PropertyGetIsNonProtected, typeName, options.Name));
                }
                if (get.IsPublic != null && get.IsPublic != propertyInfo.GetMethod.IsPublic)
                {
                    if (get.IsPublic == false)
                        throw new AssertFailedException(string.Format(ErrorCodes.PropertyGetIsPublic, typeName, options.Name));
                    if (get.IsPublic == true)
                        throw new AssertFailedException(string.Format(ErrorCodes.PropertyGetIsNonPublic, typeName, options.Name));
                }
            }
            if(options.SetMethod != null)
            {
                MethodOptions set = (MethodOptions)options.SetMethod;

                if (!propertyInfo.CanWrite)
                    throw new AssertFailedException(string.Format(ErrorCodes.PropertyIsMissingSet, typeName, options.Name));

                if (set.IsVirtual != null && set.IsAbstract != propertyInfo.SetMethod.IsAbstract)
                {
                    if (set.IsAbstract == false)
                        throw new AssertFailedException(string.Format(ErrorCodes.PropertySetIsAbstract, typeName, options.Name));
                    if (set.IsAbstract == true)
                        throw new AssertFailedException(string.Format(ErrorCodes.PropertySetIsNonAbstract, typeName, options.Name));
                }

                if (set.IsVirtual != null && set.IsVirtual != propertyInfo.SetMethod.IsVirtual)
                {
                    if (set.IsVirtual == false)
                        throw new AssertFailedException(string.Format(ErrorCodes.PropertySetIsVirtual, typeName, options.Name));
                    if (set.IsVirtual == true)
                        throw new AssertFailedException(string.Format(ErrorCodes.ProeprtySetIsNonVirtual, typeName, options.Name));
                }

                if (set.DeclaringType != null && set.DeclaringType != propertyInfo.SetMethod.DeclaringType)
                    throw new AssertFailedException(string.Format(ErrorCodes.PropertySetHasWrongDeclaringType, typeName, options.Name));

                if (set.IsPrivate != null && set.IsPrivate != propertyInfo.SetMethod.IsPrivate)
                {
                    if (set.IsPrivate == false)
                        throw new AssertFailedException(string.Format(ErrorCodes.PropertySetIsPrivate, typeName, options.Name));
                    if (set.IsPrivate == true)
                        throw new AssertFailedException(string.Format(ErrorCodes.PropertySetIsNonPrivate, typeName, options.Name));
                }
                if (set.IsFamily != null && set.IsFamily != propertyInfo.SetMethod.IsFamily)
                {
                    if (set.IsFamily == false)
                        throw new AssertFailedException(string.Format(ErrorCodes.PropertySetIsProtected, typeName, options.Name));
                    if (set.IsFamily == true)
                        throw new AssertFailedException(string.Format(ErrorCodes.PropertySetIsNonProtexted, typeName, options.Name));
                }
                if (set.IsPublic != null && set.IsPublic != propertyInfo.SetMethod.IsPublic)
                {
                    if (set.IsPublic == false)
                        throw new AssertFailedException(string.Format(ErrorCodes.PropertySetIsPublic, typeName, options.Name));
                    if (set.IsPublic == true)
                        throw new AssertFailedException(string.Format(ErrorCodes.PropertySetIsNonPublic, typeName, options.Name));
                }
            }

            return propertyInfo;
        }

        public static MethodInfo GetMethodInfo(Type type, MethodOptions options, bool isStatic = false)
        {
            string typeName = FormatHelper.FormatType(type);
            string methodDeclaration = FormatHelper.FormatMethodAccess(type, options);
            IEnumerable<MemberInfo> memberInfos = GetMembers(type, isStatic).Where(m => m.Name == options.Name);
            
            AssertMemberExists(type, options.Name, ErrorCodes.MemberIsMissing);
            AssertMemberIsInstanceOrStatic(type, options.Name, isStatic);
            AssertMemberIs<MethodInfo>(type, memberInfos.First(), ErrorCodes.MemberIsWrongMemberType);
            AssertMemberIsOfType(type, memberInfos.First(), options.ReturnType, ErrorCodes.MethodIsWrongReturnType);
            
            MethodInfo methodInfo = memberInfos.OfType<MethodInfo>().FirstOrDefault(info => IsEachParameterMatchesType(info.GetParameters(), options.Parameters));
            
            if (methodInfo == null)
                throw new AssertFailedException(string.Format(ErrorCodes.MethodIsMissing, typeName, methodDeclaration));
            
            if (options.IsVirtual != null && options.IsAbstract == methodInfo.IsAbstract)
            {
                if (options.IsAbstract == false)
                    throw new AssertFailedException(string.Format(ErrorCodes.MethodIsAbstract, typeName, methodDeclaration));
                if (options.IsAbstract == true)
                    throw new AssertFailedException(string.Format(ErrorCodes.MethodIsNonAbstract, typeName, methodDeclaration));
            }

            if (options.IsVirtual != null && options.IsVirtual == methodInfo.IsVirtual)
            {
                if (options.IsVirtual == false)
                    throw new AssertFailedException(string.Format(ErrorCodes.MethodIsVirtual, typeName, methodDeclaration));
                if (options.IsVirtual == true)
                    throw new AssertFailedException(string.Format(ErrorCodes.MethodIsNonVirtual, typeName, methodDeclaration));
            }

            if (options.DeclaringType != null && options.DeclaringType == methodInfo.DeclaringType)
                throw new AssertFailedException(string.Format(ErrorCodes.MethodHasWrongDeclaringType, typeName, methodDeclaration));

            if (options.IsPrivate != null && options.IsPrivate != methodInfo.IsPrivate)
            {
                if (options.IsPrivate == false)
                    throw new AssertFailedException(string.Format(ErrorCodes.MethodIsPrivate, typeName, methodDeclaration));
                if (options.IsPrivate == true)
                    throw new AssertFailedException(string.Format(ErrorCodes.MethodIsNonPrivate, typeName, methodDeclaration));
            }
            if (options.IsFamily != null && options.IsFamily != methodInfo.IsFamily)
            {
                if (options.IsFamily == false)
                    throw new AssertFailedException(string.Format(ErrorCodes.MethodIsProtected, typeName, methodDeclaration));
                if (options.IsFamily == true)
                    throw new AssertFailedException(string.Format(ErrorCodes.MethodIsNonProtected, typeName, methodDeclaration));
            }
            if (options.IsPublic != null && options.IsPublic != methodInfo.IsPublic)
            {
                if (options.IsPublic == false)
                    throw new AssertFailedException(string.Format(ErrorCodes.MethodIsPublic, typeName, methodDeclaration));
                if (options.IsPublic == true)
                    throw new AssertFailedException(string.Format(ErrorCodes.MethodIsNonPublic, typeName, methodDeclaration));
            }

            return methodInfo;
        }

        public static ConstructorInfo GetConstructorInfo(Type type, ConstructorOptions options)
        {
            string typeName = FormatHelper.FormatType(type);
            string constructorDeclaration = FormatHelper.FormatConstructorDeclaration(type, options);
            IEnumerable <ConstructorInfo> constructorInfos = GetMembers(type, isStatic: false).OfType<ConstructorInfo>();
            ConstructorInfo constructorInfo = constructorInfos.FirstOrDefault(info => IsEachParameterMatchesType(info.GetParameters(), options.Parameters));

            if (constructorInfo == null)
                throw new AssertFailedException(string.Format(ErrorCodes.ConstructorIsMissing, typeName, constructorDeclaration));

            if (options.IsPrivate != null && options.IsPrivate != constructorInfo.IsPrivate)
            {
                if (options.IsPrivate == false)
                    throw new AssertFailedException(string.Format(ErrorCodes.ConstructorIsPrivate, typeName, constructorDeclaration));
                if (options.IsPrivate == true)
                    throw new AssertFailedException(string.Format(ErrorCodes.ConstructorIsNonPrivate, typeName, constructorDeclaration));
            }
            if (options.IsFamily != null && options.IsFamily != constructorInfo.IsFamily)
            {
                if (options.IsFamily == false)
                    throw new AssertFailedException(string.Format(ErrorCodes.ConstructorIsProtected, typeName, constructorDeclaration));
                if (options.IsFamily == true)
                    throw new AssertFailedException(string.Format(ErrorCodes.ConstructorIsNonProtected, typeName, constructorDeclaration));
            }
            if (options.IsPublic != null && options.IsPublic != constructorInfo.IsPublic)
            {
                if (options.IsPublic == false)
                    throw new AssertFailedException(string.Format(ErrorCodes.ConstructorIsPublic, typeName, constructorDeclaration));
                if (options.IsPublic == true)
                    throw new AssertFailedException(string.Format(ErrorCodes.ConstructorIsNonPublic, typeName, constructorDeclaration));
            }
            
            return constructorInfo;
        }

        public static IEnumerable<MemberInfo> GetMembers(Type type, bool? isStatic = null)
        {
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public;

            if (isStatic == null)
            {
                flags |= BindingFlags.Static;
                flags |= BindingFlags.Instance;
            }
            else flags |= ((bool)isStatic) ? BindingFlags.Static : BindingFlags.Instance;

            return type.GetMembers(flags);
        }

        // Asserts
        private static void AssertMemberExists(Type type, string memberName, string messageTemplate)
        {
            if(GetMembers(type).Any(m => m.Name == memberName))
                return;

            string errorMessage = String.Format(
                messageTemplate,
                FormatHelper.FormatType(type),
                memberName
            );
            throw new AssertFailedException(errorMessage);
        }

        private static void AssertMemberIsInstanceOrStatic(Type type, string memberName, bool isStatic)
        {
            if (GetMembers(type, isStatic).Any(m => m.Name == memberName))
                return;

            string errorMessage = String.Format(
                isStatic ? ErrorCodes.MemberIsNonStaticMember : ErrorCodes.MemberIsNonInstanceMember,
                FormatHelper.FormatType(type),
                memberName
            );
            throw new AssertFailedException(errorMessage);  
        }
        
        private static void AssertMemberIs<TMemberInfo>(Type type, MemberInfo info, string messageTemplate) where TMemberInfo : MemberInfo
        {
            if (TypeHelper.IsOfType(typeof(TMemberInfo), info))
                return;
            
            string errorMessage = String.Format(
                messageTemplate,
                FormatHelper.FormatType(type),
                info.Name,
                $"a {FormatHelper.FormatMemberType(info.GetType())}",
                $"a {FormatHelper.FormatMemberType(typeof(TMemberInfo))}"
            );
            throw new AssertFailedException(errorMessage);
        }

        private static void AssertMemberIsOfType(Type type, MemberInfo memberInfo, Type expectedType, string messageTemplate)
        {
            if (GetType(memberInfo) == expectedType)
                return;

            string errorMessage = String.Format(
                messageTemplate,
                FormatHelper.FormatType(type),
                memberInfo.Name,
                FormatHelper.FormatType(expectedType)
            );
            throw new AssertFailedException(errorMessage);
        }

        private static void AssertMemberIsNonStaticOrStatic(Type type, MethodInfo memberInfo, bool isAbstract)
        {
            if (IsÁbstract(memberInfo) == isAbstract)
                return;

            if ((bool)isAbstract)
            {
                throw new AssertFailedException($"{FormatHelper.FormatType(type)} {memberInfo.Name} was not expected to have an implementation");
            }
            else throw new AssertFailedException($"{FormatHelper.FormatType(type)} {memberInfo.Name} was expected to have an implementation");
        }

        private static void AssertMemberHasAccessLevel(Type type, MemberInfo memberInfo, AccessLevel? accessLevel, string messageTemplate)
        {
            if (accessLevel == null)
                return;
            if (GetAccessLevel(memberInfo) == accessLevel)
                return;

            string errorMessage = String.Format(
                messageTemplate,
                FormatHelper.FormatType(type),
                memberInfo.Name,
                FormatHelper.FormatAccessLevel(accessLevel ?? AccessLevel.Private)
            );
            throw new AssertFailedException(errorMessage);
        }

        private static void AssertObjectIsOfType(Type type, object value)
        {
            if (TypeHelper.IsOfType(type, value))
                return;
            
            string errorMessage = String.Format(
                ErrorCodes.ObjectIsWrongType,
                ObjectMethodRegistry.ToString(value),
                FormatHelper.FormatType(type)
            );
            throw new AssertFailedException(errorMessage);
        }

        // method helpers
        private static bool IsEachParameterMatchesType(ParameterInfo[] parameterInfos, ParameterOptions[] parameterOptions)
        {
            int i = 0; 
            foreach(ParameterInfo info in parameterInfos)
            {
                if (i > parameterOptions.Length - 1)
                    break;
                //too many parameters
                if (i > parameterInfos.Length - 1)
                    return false;
                //wrong parameter type
                if (info.ParameterType != parameterOptions[i].ParameterType && !info.IsOptional)
                    return false;
                //match
                if (info.ParameterType == parameterOptions[i].ParameterType)
                    i++;
            }
            //too few parameters
            if (i < parameterOptions.Length)
                return false;

            return true;
        }

        private static AccessLevel GetAccessLevel(MemberInfo memberInfo)
        {
            if (memberInfo is ConstructorInfo constructorInfo)
            {
                if (constructorInfo.IsPrivate)
                    return AccessLevel.Private;
                if (constructorInfo.IsFamily)
                    return AccessLevel.Protected;
                if (constructorInfo.IsPublic)
                    return AccessLevel.Public;
            }
            if (memberInfo is FieldInfo fieldInfo)
            {
                if (fieldInfo.IsPrivate)
                    return AccessLevel.Private;
                if (fieldInfo.IsFamily)
                    return AccessLevel.Protected;
                if (fieldInfo.IsPublic)
                    return AccessLevel.Public;
            }
            if (memberInfo is MethodInfo methodInfo)
            {
                if (methodInfo.IsPrivate)
                    return AccessLevel.Private;
                if (methodInfo.IsFamily)
                    return AccessLevel.Protected;
                if (methodInfo.IsPublic)
                    return AccessLevel.Public;
            }
            throw new NotImplementedException($"Unsupported MemberInfo type {memberInfo.GetType().Name}");
        }

        private static bool IsÁbstract(MemberInfo memberInfo)
        {
            if (memberInfo is MethodInfo methodInfo)
                return methodInfo.IsAbstract;
            throw new NotImplementedException($"Unsupported MemberInfo type {memberInfo.GetType().Name}");
        }

        private static object[] GetArgumentsAndDefaults(ParameterInfo[] infos, object[] arguments)
        {
            if (infos.Length < arguments.Length)
                throw new ArgumentException("INTERNAL: Too many arguments");

            List<object> newArguments = new List<object>();
            int i = 0; 
            foreach(ParameterInfo parameterInfo in infos)
            {
                if (i < arguments.Length)
                {
                    if (!TypeHelper.IsOfType(parameterInfo.ParameterType, arguments[i]))
                    {
                        string errorMessage = String.Format(
                            "INTERNAL: Parameter {0} argument {1} is not of type {2}",
                            parameterInfo.Name,
                            ObjectMethodRegistry.ToString(arguments[i]),
                            FormatHelper.FormatType(parameterInfo.ParameterType)
                        );
                        throw new ArgumentException(errorMessage);
                    }
                    newArguments.Add(arguments[i]);
                    i++;
                }
                else if (parameterInfo.HasDefaultValue)
                {
                    newArguments.Add(parameterInfo.DefaultValue);
                }
                else throw new ArgumentException("INTERNAL: Too few arguments");
            }
            return newArguments.ToArray();
        }

        private static Type GetType(MemberInfo memberInfo)
        {
            if (memberInfo is FieldInfo fieldInfo)
                return fieldInfo.FieldType;
            if (memberInfo is PropertyInfo propertyInfo)
                return propertyInfo.PropertyType;
            if (memberInfo is MethodInfo methodInfo)
                return methodInfo.ReturnType;

            throw new NotImplementedException($"Unsupported MemberInfo type {memberInfo.GetType().Name}");
        }

        private static bool IsStatic(MemberInfo memberInfo)
        {
            if (memberInfo is FieldInfo fieldInfo)
                return fieldInfo.IsStatic;
            if (memberInfo is PropertyInfo propertyInfo)
            {
                if (propertyInfo.CanRead)
                    return propertyInfo.GetMethod.IsStatic;
                if (propertyInfo.CanWrite)
                    return propertyInfo.SetMethod.IsStatic;
            }
            if (memberInfo is MethodInfo methodInfo)
                return methodInfo.IsStatic;

            throw new NotImplementedException($"Unsupported MemberInfo type {memberInfo.GetType().Name}");
        }
    }
}