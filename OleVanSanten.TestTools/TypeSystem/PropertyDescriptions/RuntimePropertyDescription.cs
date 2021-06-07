using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OleVanSanten.TestTools.TypeSystem
{
    public class RuntimePropertyDescription : PropertyDescription
    {
        public RuntimePropertyDescription(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }

        public PropertyInfo PropertyInfo { get; }

        public override bool CanRead => PropertyInfo.CanRead;

        public override bool CanWrite => PropertyInfo.CanWrite;

        public override TypeDescription DeclaringType => new RuntimeTypeDescription(PropertyInfo.DeclaringType);

        public override MethodDescription GetMethod => PropertyInfo.CanRead ? new RuntimeMethodDescription(PropertyInfo.GetMethod) : null;

        public override string Name => PropertyInfo.Name;

        public override TypeDescription PropertyType => new RuntimeTypeDescription(PropertyInfo.PropertyType);

        public override MethodDescription SetMethod => PropertyInfo.CanWrite ? new RuntimeMethodDescription(PropertyInfo.SetMethod) : null;

        public override Attribute[] GetCustomAttributes()
        {
            return PropertyInfo.GetCustomAttributes().ToArray();
        }
        public override TypeDescription[] GetCustomAttributeTypes()
        {
            var attributes = PropertyInfo.GetCustomAttributes();
            return attributes.Select(t => new RuntimeTypeDescription(t.GetType())).ToArray();
        }
    }
}
