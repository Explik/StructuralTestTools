using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Helpers
{
    public static class AttributeDataExtensions
    {
        public static Attribute ConvertToAttribute(this AttributeData attributeData)
        {
            // Trying to acquire Type from attribute data. This might result in  
            // a null value if the attribute is not declared in an assembly
            // targeting .netstandard2.0 
            var namespaceName = GetFullNamespaceName(attributeData.AttributeClass.ContainingNamespace);
            var typeName = attributeData.AttributeClass.Name;
            var runtimeNamespace = new RuntimeNamespaceDescription(namespaceName);
            var runtimeTypeDescription = runtimeNamespace.GetTypes().FirstOrDefault(t => t.Name == typeName);
            var type = (runtimeTypeDescription as RuntimeTypeDescription)?.Type;

            // If the attribute is successfully acquired, it is instantiated
            if (type != null)
            {
                var arguments = attributeData.ConstructorArguments.Select(c => c.Value).ToArray();
                
                try
                {
                    return (Attribute)Activator.CreateInstance(type, arguments);
                }
                catch
                {
                    return null;
                }
            }
            else return null;
        }

        private static string GetFullNamespaceName(INamespaceSymbol namespaceSymbol)
        {
            string output = namespaceSymbol.Name;
            INamespaceSymbol superNamespace = namespaceSymbol.ContainingNamespace;

            while (superNamespace != null)
            {
                if (superNamespace.IsGlobalNamespace)
                    break;

                output = superNamespace.Name + "." + output;
                superNamespace = superNamespace.ContainingNamespace;
            }

            return output;
        }
    }
}
