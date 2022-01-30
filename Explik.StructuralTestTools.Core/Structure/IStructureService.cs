using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public interface IStructureService : ITypeTranslatorService, ITypeVerifierService, IMemberTranslatorService, IMemberVerifierService
    {
        NamespaceDescription TranslateNamespace(NamespaceDescription @namespace);
    }
}
