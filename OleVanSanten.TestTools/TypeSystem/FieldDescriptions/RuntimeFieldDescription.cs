using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OleVanSanten.TestTools.TypeSystem
{
    public class RuntimeFieldDescription : FieldDescription
    {
        public RuntimeFieldDescription(FieldInfo fieldInfo)
        {
            FieldInfo = fieldInfo;
        }

        public FieldInfo FieldInfo { get; }

        public override TypeDescription DeclaringType => new RuntimeTypeDescription(FieldInfo.DeclaringType);

        public override TypeDescription FieldType => new RuntimeTypeDescription(FieldInfo.FieldType);

        public override bool IsAssembly => FieldInfo.IsAssembly;

        public override bool IsFamily => FieldInfo.IsFamily;

        public override bool IsInitOnly => FieldInfo.IsInitOnly;

        public override bool IsPrivate => FieldInfo.IsPrivate;

        public override bool IsPublic => FieldInfo.IsPublic;

        public override bool IsStatic => FieldInfo.IsStatic;

        public override string Name => FieldInfo.Name;

        public override Attribute[] GetCustomAttributes()
        {
            return FieldInfo.GetCustomAttributes().ToArray();
        }

        public override TypeDescription[] GetCustomAttributeTypes()
        {
            var attributes = FieldInfo.GetCustomAttributes();
            return attributes.Select(t => new RuntimeTypeDescription(t.GetType())).ToArray();
        }
    }
}
