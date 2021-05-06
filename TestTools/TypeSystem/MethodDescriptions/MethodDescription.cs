﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TestTools.TypeSystem
{
    // Unifying class for wrappers of System.Reflection.MethodInfo and Microsoft.CodeAnalysis.IMethodSymbol 
    public abstract class MethodDescription : MethodBaseDescription
    {
        // TODO add attributes

        public override MemberTypes MemberType => MemberTypes.Method;

        public abstract TypeDescription ReturnType { get; }
    }
}
