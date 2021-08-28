using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OleVanSanten.TestTools.TypeSystem
{
    public class RuntimeParameterDescription : ParameterDescription
    {
        public RuntimeParameterDescription(ParameterInfo parameterInfo)
        {
            ParameterInfo = parameterInfo;
        }

        public ParameterInfo ParameterInfo { get; }

        public override object DefaultValue => ParameterInfo.DefaultValue;

        public override bool HasDefaultValue => ParameterInfo.HasDefaultValue;

        public override bool IsIn => ParameterInfo.IsIn;

        public override bool IsOut => ParameterInfo.IsOut;

        public override string Name => ParameterInfo.Name;

        public override TypeDescription ParameterType => new RuntimeTypeDescription(ParameterInfo.ParameterType);
    }
}
