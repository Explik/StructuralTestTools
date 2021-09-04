using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OleVanSanten.TestTools.Structure;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Helpers
{
    public static class DescriptionHelper
    {
        public static AccessLevels GetAccessLevel(TypeDescription type)
        {
            return type.IsPublic ? AccessLevels.Public : AccessLevels.Private;
        }

        public static AccessLevels GetAccessLevel(MemberDescription memberDescription)
        {
            if (memberDescription is ConstructorDescription constructorDescription)
            {
                if (constructorDescription.IsPrivate)
                {
                    return constructorDescription.IsAssembly ? AccessLevels.InternalPrivate : AccessLevels.Private;
                }
                else if (constructorDescription.IsFamily)
                {
                    return constructorDescription.IsAssembly ? AccessLevels.InternalProtected : AccessLevels.Protected;
                }
                else return constructorDescription.IsAssembly ? AccessLevels.InternalPublic : AccessLevels.Public;
            }
            else if (memberDescription is EventDescription)
            {
                return AccessLevels.Public;
            }
            else if (memberDescription is FieldDescription fieldDescription)
            {
                if (fieldDescription.IsPrivate)
                {
                    return fieldDescription.IsAssembly ? AccessLevels.InternalPrivate : AccessLevels.Private;
                }
                else if (fieldDescription.IsFamily)
                {
                    return fieldDescription.IsAssembly ? AccessLevels.InternalProtected : AccessLevels.Protected;
                }
                else return fieldDescription.IsAssembly ? AccessLevels.InternalPublic : AccessLevels.Public;
            }
            else if (memberDescription is MethodDescription methodDescription)
            {
                if (methodDescription.IsPrivate)
                {
                    return methodDescription.IsAssembly ? AccessLevels.InternalPrivate : AccessLevels.Private;
                }
                else if (methodDescription.IsFamily)
                {
                    return methodDescription.IsAssembly ? AccessLevels.InternalProtected : AccessLevels.Protected;
                }
                else return methodDescription.IsAssembly ? AccessLevels.InternalPublic : AccessLevels.Public;
            }
            throw new NotImplementedException();
        }
    }
}
