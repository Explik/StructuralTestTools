using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OleVanSanten.TestTools.TypeSystem
{
    public class RuntimeDescriptionFactory
    {
        public MemberDescription Create(MemberInfo memberInfo)
        {
            if (memberInfo is ConstructorInfo constructorInfo)
                return new RuntimeConstructorDescription(constructorInfo);

            if (memberInfo is EventInfo eventInfo)
                return new RuntimeEventDescription(eventInfo);
            
            if (memberInfo is FieldInfo fieldInfo)
                return new RuntimeFieldDescription(fieldInfo);
            
            if (memberInfo is MethodInfo methodInfo)
                return new RuntimeMethodDescription(methodInfo);
            
            if (memberInfo is PropertyInfo propertyDescription)
                return new RuntimePropertyDescription(propertyDescription);
            
            else throw new NotImplementedException();
        }
    }
}
