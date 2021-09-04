using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OleVanSanten.TestTools.Helpers;

namespace OleVanSanten.TestTools.TypeSystem
{
    public class CompileTimeEventDescription : EventDescription
    {
        public CompileTimeEventDescription(Compilation compilation, IEventSymbol eventSymbol)
        {
            if (compilation == null)
                throw new ArgumentNullException("compilation");
            if (eventSymbol == null)
                throw new ArgumentNullException("eventSymbol");

            Compilation = compilation;
            EventSymbol = eventSymbol;
        }

        public Compilation Compilation { get; }

        public IEventSymbol EventSymbol { get; }

        public override MethodDescription AddMethod => new CompileTimeMethodDescription(Compilation, EventSymbol.AddMethod);

        public override TypeDescription DeclaringType => new CompileTimeTypeDescription(Compilation, EventSymbol.ContainingType);

        public override TypeDescription EventHandlerType => new CompileTimeTypeDescription(Compilation, EventSymbol.Type);

        public override string Name => EventSymbol.Name;

        public override MethodDescription RemoveMethod => new CompileTimeMethodDescription(Compilation, EventSymbol.RemoveMethod);

        // Please note, GetCustomAttributes might return fewer results than
        // GetCustomAttributeTypes, because attributes cannot be loaded from 
        // non .netstandard2.0 targeting assemblies
        public override Attribute[] GetCustomAttributes()
        {
            return EventSymbol
                .GetAttributes()
                .Select(attributeData => attributeData.ConvertToAttribute())
                .Where(attribute => attribute != null)
                .ToArray();
        }

        public override TypeDescription[] GetCustomAttributeTypes()
        {
            var attributes = EventSymbol.GetAttributes();
            var output = new List<TypeDescription>();

            for (int i = 0; i < attributes.Length; i++)
            {
                output.Add(new CompileTimeTypeDescription(Compilation, attributes[i].AttributeClass));
            }

            return output.ToArray();
        }
    }
}
