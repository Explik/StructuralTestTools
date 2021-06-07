using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using OleVanSanten.TestTools.TypeSystem;
using OleVanSanten.TestTools.Structure;
using System.Linq;
using System.Reflection;

namespace OleVanSanten.TestTools
{
    public class XMLConfiguration
    {
        readonly XElement root;

        public XMLConfiguration(string xml)
        {
            root = XElement.Parse(xml);
        }

        public string GetFromNamespaceName()
        {
            var fromNamespaceElement = root.Element("FromNamespace");

            if (fromNamespaceElement == null)
                return null;

            return fromNamespaceElement.Value;
        }

        public NamespaceDescription GetFromNamespace(NamespaceDescription globalNamespace)
        {
            var fromNamespaceName = GetFromNamespaceName();

            if (fromNamespaceName == null)
                return null;

            return GetNamespace(globalNamespace, fromNamespaceName);
        }

        public string GetToNamespaceName()
        {
            var toNamespaceElement = root.Element("ToNamespace");

            if (toNamespaceElement == null)
                return null;

            return toNamespaceElement.Value;
        }

        public NamespaceDescription GetToNamespace(NamespaceDescription globalNamespace)
        {
            var toNamespace = GetToNamespaceName();

            if (toNamespace == null)
                return null;

            return GetNamespace(globalNamespace, toNamespace);
        }

        public ITypeTranslator GetTypeTranslator()
        {
            var typeTranslatorElement = root.Element("TypeTranslator");
            var typeTranslatorTypeAttribute = typeTranslatorElement?.Attribute("Type");

            if (typeTranslatorElement == null)
                return null;

            if (typeTranslatorTypeAttribute == null)
                throw new ArgumentException("The TypeTranslator tag must have attribute Type");

            return GetObject(typeTranslatorTypeAttribute.Value) as ITypeTranslator;
        }

        public ITypeVerifier[] GetTypeVerifiers()
        {
            List<ITypeVerifier> output = new List<ITypeVerifier>();
            var typeVerifiersElement = root.Element("TypeVerifiers");
            var typeVerifierElements = typeVerifiersElement.Elements("TypeVerifier");

            if (typeVerifiersElement == null)
                return null;

            foreach(var typeVerifierElement in typeVerifierElements)
            {
                var typeVerifierTypeAttribute = typeVerifierElement.Attribute("Type");

                if (typeVerifierTypeAttribute == null)
                    throw new ArgumentException("The TypeVerifier tag must have attribute Type");

                output.Add(GetObject(typeVerifierTypeAttribute.Value) as ITypeVerifier);
            }
			
			if (output.All(v => v == null))
				return null;
			
            return output.ToArray();
        }

        public IMemberTranslator GetMemberTranslator()
        {
            var memberTranslatorElement = root.Element("MemberTranslator");
            var memberTranslatorTypeAttribute = memberTranslatorElement?.Attribute("Type");

            if (memberTranslatorElement == null)
                return null;

            if (memberTranslatorTypeAttribute == null)
                throw new ArgumentException("The MemberTranslator tag must have attribute Type");

            return GetObject(memberTranslatorTypeAttribute.Value) as IMemberTranslator;
        }

        public IMemberVerifier[] GetMemberVerifiers()
        {
            List<IMemberVerifier> output = new List<IMemberVerifier>();
            var memberVerifiersElement = root.Element("MemberVerifiers");
            var memberVerifierElements = memberVerifiersElement.Elements("MemberVerifier");

            if (memberVerifiersElement == null)
                return null;

            foreach (var memberVerifierElement in memberVerifierElements)
            {
                var memberVerifierTypeAttribute = memberVerifierElement.Attribute("Type");

                if (memberVerifierTypeAttribute == null)
                    throw new ArgumentException("The MemberVerifier tag must have attribute Type");

                output.Add(GetObject(memberVerifierTypeAttribute.Value) as IMemberVerifier);
            }
			
			if (output.All(v => v == null))
				return null;
			
            return output.ToArray();

        }

        private object GetObject(string fullTypename)
        {
            var fullTypeNameParts = fullTypename.Split('.');
            var namespaceName = string.Join(".", fullTypeNameParts.Take(fullTypeNameParts.Length - 1));
            var typeName = fullTypeNameParts.Last();

            // The try-catch block is a little hack for accessing as many assemblies as possible,
            // because some assemblies always fail to load, like Microsoft.CodeAnalysis assemblies.
            // This might be a result of an unresolved configuration issues in one of the projects.
            var assemblyEnumerator = AppDomain.CurrentDomain.GetAssemblies().Reverse().GetEnumerator();
            while (assemblyEnumerator.MoveNext())
            {
                try
                {
                    var assembly = assemblyEnumerator.Current;
                    var type = assembly.GetTypes().FirstOrDefault(t => t.Namespace == namespaceName && t.Name == typeName);
			
                    if (type != null)
                        return Activator.CreateInstance(type);
                }
                catch (ReflectionTypeLoadException) { }
            }

            // This method is somewhat unreliable at the moment, possibly due to a configuration issue.
            // Therefore, a null value (signifying property not configured) is used instead of throwing an exception
			return null;
        }

        private NamespaceDescription GetNamespace(NamespaceDescription @namespace,  string fullNamepaceName)
        {
            NamespaceDescription subNamespace = @namespace;

            foreach (var namespaceNamePart in fullNamepaceName.Split('.'))
                subNamespace = subNamespace.GetNamespace(namespaceNamePart);

            return subNamespace;
        }
    }
}
