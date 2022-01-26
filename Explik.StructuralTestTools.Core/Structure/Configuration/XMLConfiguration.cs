using Explik.StructuralTestTools.TypeSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Explik.StructuralTestTools
{
    public class XMLConfiguration : IConfiguration
    {
        private readonly XElement root;

        public XMLConfiguration(string xml)
        {
            root = XElement.Parse(xml);
        }

        public FileInfo ProjectFile { get; set; }

        public NamespaceDescription GlobalNamespace { get; set; }

        public NamespaceDescription FromNamespace
        {
            get
            {
                var fromNamespaceElement = root.Element("FromNamespace");

                if (fromNamespaceElement == null)
                    return null;

                return GlobalNamespace?.GetNamespace(fromNamespaceElement.Value);
            }
        }

        public NamespaceDescription ToNamespace
        {
            get
            {
                var toNamespaceElement = root.Element("ToNamespace");

                if (toNamespaceElement == null)
                    return null;

                return GlobalNamespace?.GetNamespace(toNamespaceElement.Value);
            }
        }

        public FileInfo[] TemplateFiles 
        { 
            get 
            {
                var output = new List<FileInfo>();
                var templatesElement = root.Element("TemplateFiles");
                var templateElements = templatesElement?.Elements("TemplateFile");

                if (templateElements == null)
                    return null;

                foreach(var templateElement in templateElements)
                {
                    string templateAbsolutePath;
                    if (!Path.IsPathRooted(templateElement.Value))
                    {
                        templateAbsolutePath = Path.Combine(ProjectFile?.DirectoryName ?? "", templateElement.Value);
                    }
                    else templateAbsolutePath = templateElement.Value;

                    output.Add(new FileInfo(templateAbsolutePath));
                }
                return output.ToArray();
            } 
        }

        public ITypeTranslator TypeTranslator
        {
            get
            {
                var typeTranslatorElement = root.Element("TypeTranslator");
                var typeTranslatorTypeAttribute = typeTranslatorElement?.Attribute("Type");

                if (typeTranslatorElement == null)
                    return null;

                if (typeTranslatorTypeAttribute == null)
                    throw new ArgumentException("The TypeTranslator tag must have attribute Type");

                return CreateInstance(typeTranslatorTypeAttribute.Value) as ITypeTranslator;
            }
        }

        public IMemberTranslator MemberTranslator
        {
            get
            {
                var memberTranslatorElement = root.Element("MemberTranslator");
                var memberTranslatorTypeAttribute = memberTranslatorElement?.Attribute("Type");

                if (memberTranslatorElement == null)
                    return null;

                if (memberTranslatorTypeAttribute == null)
                    throw new ArgumentException("The MemberTranslator tag must have attribute Type");

                return CreateInstance(memberTranslatorTypeAttribute.Value) as IMemberTranslator;
            }
        }

        public ITypeVerifier[] TypeVerifiers
        {
            get
            {
                List<ITypeVerifier> output = new List<ITypeVerifier>();
                var typeVerifiersElement = root.Element("TypeVerifiers");
                var typeVerifierElements = typeVerifiersElement?.Elements("TypeVerifier");

                if (typeVerifiersElement == null)
                    return null;

                foreach (var typeVerifierElement in typeVerifierElements)
                {
                    var typeVerifierTypeAttribute = typeVerifierElement.Attribute("Type");

                    if (typeVerifierTypeAttribute == null)
                        throw new ArgumentException("The TypeVerifier tag must have attribute Type");

                    output.Add(CreateInstance(typeVerifierTypeAttribute.Value) as ITypeVerifier);
                }

                if (output.All(v => v == null))
                    return null;

                return output.ToArray();
            }
        }

        public TypeVerificationAspect[] TypeVerificationOrder { get; } = null;
        
        public IMemberVerifier[] MemberVerifiers
        {
            get
            {
                List<IMemberVerifier> output = new List<IMemberVerifier>();
                var memberVerifiersElement = root.Element("MemberVerifiers");
                var memberVerifierElements = memberVerifiersElement?.Elements("MemberVerifier");

                if (memberVerifiersElement == null)
                    return null;

                foreach (var memberVerifierElement in memberVerifierElements)
                {
                    var memberVerifierTypeAttribute = memberVerifierElement.Attribute("Type");

                    if (memberVerifierTypeAttribute == null)
                        throw new ArgumentException("The MemberVerifier tag must have attribute Type");

                    output.Add(CreateInstance(memberVerifierTypeAttribute.Value) as IMemberVerifier);
                }

                if (output.All(v => v == null))
                    return null;

                return output.ToArray();
            }
        }

        public MemberVerificationAspect[] MemberVerificationOrder { get; } = null;
        
        private object CreateInstance(string fullTypename)
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
    }
}