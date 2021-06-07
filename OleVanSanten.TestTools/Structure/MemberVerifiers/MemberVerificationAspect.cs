using System;
using System.Collections.Generic;
using System.Text;

namespace OleVanSanten.TestTools.Structure
{
    public class MemberVerificationAspect
    {
        string _name;

        public MemberVerificationAspect(string name)
        {
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }

        public static ICollection<MemberVerificationAspect> DefaultOrder { get; set; } = new[]
        {
            MemberType,
            FieldType,
            FieldAccessLevel,
            FieldWriteability,
            PropertyType,
            PropertyGetIsAbstract,
            PropertyGetIsVirtual,
            PropertyGetDeclaringType,
            PropertyGetAccessLevel,
            PropertySetIsAbstract,
            PropertySetIsVirtual,
            PropertySetDeclaringType,
            PropertySetAccessLevel,
            MethodReturnType,
            MethodIsAbstract,
            MethodIsVirtual,
            MethodDeclaringType,
            MethodAccessLevel
        };

        public static MemberVerificationAspect MemberType { get; } = new MemberVerificationAspect(nameof(MemberType));

        public static MemberVerificationAspect ConstructorAccessLevel { get; } = new MemberVerificationAspect(nameof(ConstructorAccessLevel));

        public static MemberVerificationAspect EventHandlerType { get; } = new MemberVerificationAspect(nameof(EventHandlerType));
        public static MemberVerificationAspect EventAddAccessLevel { get; } = new MemberVerificationAspect(nameof(EventAddAccessLevel));
        public static MemberVerificationAspect EventRemoveAccessLevel { get; } = new MemberVerificationAspect(nameof(EventRemoveAccessLevel));

        public static MemberVerificationAspect FieldType { get; } = new MemberVerificationAspect(nameof(FieldType));
        public static MemberVerificationAspect FieldAccessLevel { get; } = new MemberVerificationAspect(nameof(FieldAccessLevel));
        public static MemberVerificationAspect FieldWriteability { get; } = new MemberVerificationAspect(nameof(FieldWriteability));
        public static MemberVerificationAspect FieldIsStatic { get; } = new MemberVerificationAspect(nameof(FieldIsStatic));

        public static MemberVerificationAspect MethodReturnType { get; } = new MemberVerificationAspect(nameof(MethodReturnType));
        public static MemberVerificationAspect MethodIsVirtual { get; } = new MemberVerificationAspect(nameof(MethodIsVirtual));
        public static MemberVerificationAspect MethodIsAbstract { get; } = new MemberVerificationAspect(nameof(MethodIsAbstract));
        public static MemberVerificationAspect MethodIsStatic { get; } = new MemberVerificationAspect(nameof(MethodIsStatic));
        public static MemberVerificationAspect MethodDeclaringType { get; } = new MemberVerificationAspect(nameof(MethodDeclaringType));
        public static MemberVerificationAspect MethodAccessLevel { get; } = new MemberVerificationAspect(nameof(MethodAccessLevel));

        public static MemberVerificationAspect PropertyType { get; } = new MemberVerificationAspect(nameof(PropertyType));
        public static MemberVerificationAspect PropertyIsStatic { get; } = new MemberVerificationAspect(nameof(PropertyIsStatic));
        public static MemberVerificationAspect PropertyGetIsAbstract { get; } = new MemberVerificationAspect(nameof(PropertyGetIsAbstract));
        public static MemberVerificationAspect PropertyGetIsVirtual { get; } = new MemberVerificationAspect(nameof(PropertyGetIsVirtual));
        public static MemberVerificationAspect PropertyGetDeclaringType { get; } = new MemberVerificationAspect(nameof(PropertyGetDeclaringType));
        public static MemberVerificationAspect PropertyGetAccessLevel { get; } = new MemberVerificationAspect(nameof(PropertyGetAccessLevel));
        public static MemberVerificationAspect PropertySetIsAbstract { get; } = new MemberVerificationAspect(nameof(PropertySetIsAbstract));
        public static MemberVerificationAspect PropertySetIsVirtual { get; } = new MemberVerificationAspect(nameof(PropertySetIsVirtual));
        public static MemberVerificationAspect PropertySetDeclaringType { get; } = new MemberVerificationAspect(nameof(PropertySetDeclaringType));
        public static MemberVerificationAspect PropertySetAccessLevel { get; } = new MemberVerificationAspect(nameof(PropertySetAccessLevel));
    }
}
