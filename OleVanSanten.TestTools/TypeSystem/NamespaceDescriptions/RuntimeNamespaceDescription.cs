using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OleVanSanten.TestTools.TypeSystem
{
    public class RuntimeNamespaceDescription : NamespaceDescription
    {
        public RuntimeNamespaceDescription(string @namespace)
        {
            if (string.IsNullOrEmpty(@namespace))
                throw new ArgumentException("Global namespaces are not supported");

            Name = @namespace;
        }

        public override NamespaceDescription ContainingNamespace
        {
            get
            {
                if (!Name.Contains("."))
                    throw new NotImplementedException("Global namespace is not yet supported");

                string baseNamespace = Name.Substring(0, Name.LastIndexOf(".") - 1);
                return new RuntimeNamespaceDescription(baseNamespace);
            }
        }

        public override string Name { get; }

        public override NamespaceDescription[] GetNamespaces()
        {
            var allTypesInAllSubnamespaces = GetTypes(t => t.Namespace == null || t.Namespace.StartsWith(Name) || string.IsNullOrEmpty(Name)); 
            var allSubnamespaces = allTypesInAllSubnamespaces.Select(t => t.Namespace).Distinct();
            var directSubnamespaces = allSubnamespaces.Where(s => CountPeriods(s) + 1 == CountPeriods(Name));

            return directSubnamespaces.Select(t => new RuntimeNamespaceDescription(t)).ToArray();
        }

        private int CountPeriods(string str)
        {
            if (str == null)
                return 0;

            return str.Count(c => c == '.');
        }

        public override TypeDescription[] GetTypes()
        {
            return GetTypes(t => t.Namespace == Name);
        }

        private TypeDescription[] GetTypes(Func<Type, bool> predicate)
        {
            var output = new List<TypeDescription>();

            // The try-catch block is a little hack for accessing as many assemblies as possible,
            // because some assemblies always fail to load, like Microsoft.CodeAnalysis assemblies.
            // This might be a result of an unresolved configuration issues in one of the projects.
            var assemblyEnumerator = AppDomain.CurrentDomain.GetAssemblies().Reverse().GetEnumerator();
            while (assemblyEnumerator.MoveNext())
            {
                try
                {
                    var assembly = assemblyEnumerator.Current;
                    var typesInNamespace = assembly.GetTypes().Where(predicate);
                    var typeDescriptions = typesInNamespace.Select(t => new RuntimeTypeDescription(t));
                    output.AddRange(typeDescriptions);
                }
                catch (ReflectionTypeLoadException) { }
            }
            return output.ToArray();
        }
    }
}
