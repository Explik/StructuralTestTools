using OleVanSanten.TestTools.TypeSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace OleVanSanten.TestTools
{
    public interface ITypeTranslatorService
    {
        TypeDescription TranslateType(TypeDescription type);
    }
}
