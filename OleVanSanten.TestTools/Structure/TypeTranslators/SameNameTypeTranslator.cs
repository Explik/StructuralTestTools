using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class SameNameTypeTranslator : TypeTranslator
    {
        public override TypeDescription Translate(TypeDescription type)
        {
            var translatedType = TargetNamespace.GetTypes().FirstOrDefault(t => t.Name == type.Name);

            if (translatedType != null)
                return translatedType;

            // TODO fix the following lines as they give an unclear program flow
            Verifier.FailTypeNotFound(TargetNamespace, type);

            // Should never get to here as FailTypeNotFound() should throw an exception
            throw new NotImplementedException();
        }
    }
}
