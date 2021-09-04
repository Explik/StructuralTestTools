using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class AlternateNameTypeTranslator : ITypeTranslator
    {
        string[] _alternateNames;

        public AlternateNameTypeTranslator(params string[] alternateNames)
        {
            _alternateNames = alternateNames;
        }

        public TypeDescription Translate(TypeTranslateArgs args)
        {
            string[] names = _alternateNames.Union(new[] { args.OriginalType.Name }).ToArray();

            var translatedType = args.TargetNamespace.GetTypes().FirstOrDefault(t => names.Contains(t.Name));

            if (translatedType != null)
                return translatedType;

            // TODO fix the following lines as they give an unclear program flow
            args.Verifier.FailTypeNotFound(args.TargetNamespace, names);
            
            // Should never get to here as FailTypeNotFound() should throw an exception
            throw new NotImplementedException(); 
        }
    }
}
