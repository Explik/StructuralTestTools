using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public interface IStructureService : ITypeTranslatorService, ITypeVerifierService, IMemberTranslatorService, IMemberVerifierService
    {
    }
}
