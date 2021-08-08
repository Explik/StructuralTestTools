using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Linq;
using OleVanSanten.TestTools.Helpers;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class TypeVisitor : ExpressionVisitor
    {
        IStructureService _structureService;

        Dictionary<ParameterExpression, ParameterExpression> _variableCache = new Dictionary<ParameterExpression, ParameterExpression>();

        public ITypeVerifier[] TypeVerifiers { get; set; } = new ITypeVerifier[]
        {
            new UnchangedTypeAccessLevelVerifier(),
            new UnchangedTypeIsAbstractVerifier(),
            new UnchangedTypeIsStaticVerifier()
        };

        public IMemberVerifier[] MemberVerifiers { get; set; } = new IMemberVerifier[]
        {
            new UnchangedFieldTypeVerifier(),
            new UnchangedMemberAccessLevelVerifier(),
            new UnchangedMemberDeclaringType(),
            new UnchangedMemberIsStaticVerifier(),
            new UnchangedMemberIsVirtualVerifier(),
            new UnchangedMemberTypeVerifier(),
            new UnchangedPropertyTypeVerifier()
        };

        public TypeVisitor(IStructureService structureService)
        {
            _structureService = structureService;
        }

        protected override Expression VisitNew(NewExpression node)
        {
            var originalType = new RuntimeTypeDescription(node.Type);
            var originalConstructor = new RuntimeConstructorDescription(node.Constructor);
            var translatedConstructor = _structureService.TranslateMember(originalConstructor) as RuntimeConstructorDescription;

            if (originalConstructor == translatedConstructor)
                return base.VisitNew(node);

            _structureService.VerifyType(originalType, TypeVerifiers);
            _structureService.VerifyMember(
                originalConstructor,
                MemberVerifiers,
                MemberVerificationAspect.MemberType,
                MemberVerificationAspect.ConstructorAccessLevel);

            return Expression.New(translatedConstructor.ConstructorInfo, node.Arguments.Select(Visit));
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var originalType = new RuntimeTypeDescription(node.Type);
            var originalMethod = new RuntimeMethodDescription(node.Method);
            var translatedMethod = _structureService.TranslateMember(originalMethod) as RuntimeMethodDescription;

            if (originalMethod == translatedMethod)
                return base.VisitMethodCall(node);

            _structureService.VerifyType(originalType, TypeVerifiers);
            _structureService.VerifyMember(
                originalMethod,
                MemberVerifiers,
                MemberVerificationAspect.MemberType,
                MemberVerificationAspect.MethodDeclaringType,
                MemberVerificationAspect.MethodReturnType,
                MemberVerificationAspect.MethodIsStatic,
                MemberVerificationAspect.MethodIsAbstract,
                MemberVerificationAspect.MethodIsVirtual,
                MemberVerificationAspect.MethodAccessLevel);

            MethodInfo methodInfo = translatedMethod.MethodInfo;
            ParameterInfo[] methodPars = methodInfo.GetParameters();
            Expression[] methodArgs = node.Arguments.Select(Visit).ToArray();
            
            // Constructing generic methods need to be constructed based on argument types
            if (methodInfo.IsGenericMethod)
            {
                Type[] typeArguments = methodInfo.GetGenericArguments();

                for (int i = 0; i < typeArguments.Length; i++) { 
                    for (int j = 0; j < methodArgs.Length; j++)
                    {
                        Type temp = methodArgs[j].Type.GetGenericArguments(methodPars[j].ParameterType, typeArguments[i]);

                        if (temp != null)
                        {
                            typeArguments[i] = temp;
                            break;
                        }
                    }
                }
                methodInfo = methodInfo.MakeGenericMethod(typeArguments);
            }
            return Expression.Call(Visit(node.Object), methodInfo, methodArgs);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var originalType = new RuntimeTypeDescription(node.Type);
            var factory = new RuntimeDescriptionFactory();
            var originalMember = factory.Create(node.Member);
            var translatedMember = _structureService.TranslateMember(originalMember);

            if (originalMember == translatedMember)
                return base.VisitMember(node);

            _structureService.VerifyType(originalType, TypeVerifiers);
            _structureService.VerifyMember(originalMember, MemberVerifiers, MemberVerificationAspect.MemberType);

            if (translatedMember is FieldDescription fieldDescription)
            {
                _structureService.VerifyMember(
                    fieldDescription,
                    MemberVerifiers,
                    MemberVerificationAspect.FieldType,
                    MemberVerificationAspect.FieldIsStatic,
                    MemberVerificationAspect.FieldAccessLevel);

                var fieldInfo = ((RuntimeFieldDescription)fieldDescription).FieldInfo;
                return Expression.Field(Visit(node.Expression), fieldInfo);
            }
            else if (translatedMember is PropertyDescription propertyDescription)
            {
                _structureService.VerifyMember(
                       propertyDescription,
                       MemberVerifiers,
                       MemberVerificationAspect.PropertyType,
                       MemberVerificationAspect.PropertyIsStatic,
                       MemberVerificationAspect.PropertyGetDeclaringType,
                       MemberVerificationAspect.PropertyGetIsAbstract,
                       MemberVerificationAspect.PropertyGetIsVirtual,
                       MemberVerificationAspect.PropertyGetAccessLevel);
                var propertyInfo = ((RuntimePropertyDescription)propertyDescription).PropertyInfo;
                return Expression.Property(Visit(node.Expression), propertyInfo);
            }
            else throw new ArgumentException("Member was not translated to FieldInfo or PropertyInfo");
        }

        protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            var originalType = new RuntimeTypeDescription(node.Member.DeclaringType);
            var factory = new RuntimeDescriptionFactory();
            var originalMember = factory.Create(node.Member);
            var translatedMember = _structureService.TranslateMember(originalMember);

            if (originalMember == translatedMember)
                return base.VisitMemberAssignment(node);

            _structureService.VerifyType(originalType, TypeVerifiers);
            _structureService.VerifyMember(originalMember, MemberVerifiers, MemberVerificationAspect.MemberType);

            if (translatedMember is FieldDescription fieldDescription)
            {
                _structureService.VerifyMember(
                    translatedMember,
                    MemberVerifiers,
                    MemberVerificationAspect.FieldType,
                    MemberVerificationAspect.FieldIsStatic,
                    MemberVerificationAspect.FieldWriteability,
                    MemberVerificationAspect.FieldAccessLevel);

                var fieldInfo = ((RuntimeFieldDescription)fieldDescription).FieldInfo;
                return Expression.Bind(fieldInfo, node.Expression);
            }
            else if (translatedMember is PropertyDescription propertyDescription)
            {
                _structureService.VerifyMember(
                       translatedMember,
                       MemberVerifiers,
                       MemberVerificationAspect.PropertyType,
                       MemberVerificationAspect.PropertyIsStatic,
                       MemberVerificationAspect.PropertySetDeclaringType,
                       MemberVerificationAspect.PropertySetIsAbstract,
                       MemberVerificationAspect.PropertySetIsVirtual,
                       MemberVerificationAspect.PropertySetAccessLevel);

                var propertyInfo = ((RuntimePropertyDescription)propertyDescription).PropertyInfo;
                return Expression.Bind(propertyInfo, node.Expression);
            }
            else throw new ArgumentException("Member was not translated to FieldInfo or PropertyInfo");
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            var originalType = new RuntimeTypeDescription(node.Type);
            var translatedType = _structureService.TranslateType(originalType) as RuntimeTypeDescription;

            if (originalType == translatedType)
                return base.VisitParameter(node);

            _structureService.VerifyType(originalType, TypeVerifiers);

            // To preserve the referential equality of parameter expressions 
            // the function must return the exact same output for the same input.
            // Not doing this would result in the expression not being compile-able.
            if (!_variableCache.ContainsKey(node))
            {
                Random random = new Random();
                ParameterExpression newParameter = Expression.Parameter(translatedType.Type, random.Next().ToString());

                _variableCache.Add(node, newParameter);

                return newParameter;
            }
            return _variableCache[node];
        }
    }
}
