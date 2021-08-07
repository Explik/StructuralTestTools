using OleVanSanten.TestTools.TypeSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace OleVanSanten.TestTools.Structure
{
    public interface ITypeTranslatorService
    {
        bool IsTranslatableType(TypeDescription type);

        TypeDescription TranslateType(TypeDescription type);
    }
}
