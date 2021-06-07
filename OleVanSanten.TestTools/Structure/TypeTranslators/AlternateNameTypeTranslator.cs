using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class AlternateNameTypeTranslator : TypeTranslator
    {
        string[] _alternateNames;

        public AlternateNameTypeTranslator(params string[] alternateNames)
        {
            _alternateNames = alternateNames;
        }

        public override TypeDescription Translate(TypeDescription type)
        {
            string[] names = _alternateNames.Union(new[] { type.Name }).ToArray();

            var translatedType = TargetNamespace.GetTypes().FirstOrDefault(t => names.Contains(t.Name));

            if (translatedType != null)
                return translatedType;

            // TODO fix the following lines as they give an unclear program flow
            Verifier.FailTypeNotFound(TargetNamespace, names);
            
            // Should never get to here as FailTypeNotFound() should throw an exception
            throw new NotImplementedException(); 
        }
    }
}
