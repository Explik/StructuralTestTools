using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OleVanSanten.TestTools.TypeSystem
{
    // A wrapper for Microsoft.CodeAnalysis.INameSpaceSymbol and AppDomain.GetAssemblies().SelectMany(t => t.GetTypes())
    public abstract class NamespaceDescription
    {
        public abstract NamespaceDescription ContainingNamespace { get; }

        public abstract string Name { get; }

        public virtual NamespaceDescription GetNamespace(string name)
        {
            // Creating predicate for either non-global pattern ("*.Name") or global pattern ("Name")
            Func<NamespaceDescription, bool> predicate;
            if (!string.IsNullOrEmpty(Name))
                predicate = n => n.Name == $"{Name}.{name}";
            else predicate = n => n.Name == name;

            // Trying to matching namespace
            var @namespace = GetNamespaces().FirstOrDefault(predicate);
            if (@namespace == null)
            {
                string ownNameWithPrefix = !string.IsNullOrEmpty(Name) ? Name + "." : "global::";
                throw new ArgumentException($"Namespace {ownNameWithPrefix}{name} could not be found");
            }
            return @namespace;
        }

        public abstract NamespaceDescription[] GetNamespaces();

        public virtual TypeDescription GetType(string name)
        {
            return GetTypes().First(t => t.Name == name);
        }

        public abstract TypeDescription[] GetTypes();
    }
}
