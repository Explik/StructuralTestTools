using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class SameNameTypeTranslator : ITypeTranslator
    {
        public TypeDescription Translate(TypeTranslateArgs args)
        {
            var translatedType = args.TargetNamespace.GetTypes().FirstOrDefault(t => t.Name == args.OriginalType.Name);

            if (translatedType != null)
                return translatedType;

            // TODO fix the following lines as they give an unclear program flow
            args.Verifier.FailTypeNotFound(args.TargetNamespace, args.OriginalType);

            // Should never get to here as FailTypeNotFound() should throw an exception
            throw new NotImplementedException();
        }
    }
}
