using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace OleVanSanten.TestTools.TypeSystem
{
    public class CompileTimeParameterDescription : ParameterDescription
    {
        public CompileTimeParameterDescription(Compilation compilation, IParameterSymbol parameterSymbol)
        {
            if (compilation == null)
                throw new ArgumentNullException("compilation");
            if (parameterSymbol == null)
                throw new ArgumentNullException("parameterSymbol");

            Compilation = compilation;
            ParameterSymbol = parameterSymbol;
        }

        public Compilation Compilation { get; }

        public IParameterSymbol ParameterSymbol;

        public override object DefaultValue => ParameterSymbol.ExplicitDefaultValue;

        public override bool HasDefaultValue => ParameterSymbol.HasExplicitDefaultValue;

        public override bool IsIn => ParameterSymbol.RefKind == RefKind.In;

        public override bool IsOut => ParameterSymbol.RefKind == RefKind.Out;

        public override string Name => ParameterSymbol.Name;

        public override TypeDescription ParameterType => new CompileTimeTypeDescription(Compilation, ParameterSymbol.Type);
    }
}
