using System;
using System.Collections.Generic;
using System.Text;

namespace OleVanSanten.TestTools.TypeSystem
{
    public abstract class ParameterDescription
    {
        public abstract object DefaultValue { get; }

        public abstract bool HasDefaultValue { get; }

        public abstract bool IsIn { get; }

        public abstract bool IsOut { get; }

        public abstract string Name { get; }

        public abstract TypeDescription ParameterType { get; }
    }
}
