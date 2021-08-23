using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OleVanSanten.TestTools.TypeSystem
{
    public class RuntimeMethodDescription : MethodDescription
    {
        public RuntimeMethodDescription(MethodInfo methodInfo)
        {
            if (methodInfo == null)
                throw new ArgumentNullException("methodInfo");

            MethodInfo = methodInfo;
        }

        public MethodInfo MethodInfo { get; }

        public override TypeDescription DeclaringType => new RuntimeTypeDescription(MethodInfo.DeclaringType);

        public override bool IsAbstract => MethodInfo.IsAbstract;

        public override bool IsAssembly => MethodInfo.IsAssembly;

        public override bool IsFamily => MethodInfo.IsFamily;

        public override bool IsPrivate => MethodInfo.IsPrivate;

        public override bool IsPublic => MethodInfo.IsPublic;

        public override bool IsStatic => MethodInfo.IsStatic;

        public override bool IsVirtual => MethodInfo.IsVirtual;

        public override string Name => MethodInfo.Name;

        public override TypeDescription ReturnType => new RuntimeTypeDescription(MethodInfo.ReturnType);

        public override bool IsGenericMethod => MethodInfo.IsGenericMethod;

        public override Attribute[] GetCustomAttributes()
        {
            return MethodInfo.GetCustomAttributes().ToArray();
        }
        public override TypeDescription[] GetCustomAttributeTypes()
        {
            var attributes = MethodInfo.GetCustomAttributes();
            return attributes.Select(t => new RuntimeTypeDescription(t.GetType())).ToArray();
        }

        public override TypeDescription[] GetGenericArguments()
        {
            return MethodInfo.GetGenericArguments().Select(t => new RuntimeTypeDescription(t)).ToArray();
        }

        public override ParameterDescription[] GetParameters()
        {
            return MethodInfo.GetParameters().Select(i => new RuntimeParameterDescription(i)).ToArray();
        }

        public override MethodDescription MakeGenericMethod(params TypeDescription[] typeArguments)
        {
            if (MethodInfo.IsGenericMethod)
            {
                var args = typeArguments.OfType<RuntimeTypeDescription>().Select(t => t.Type).ToArray();
                return new RuntimeMethodDescription(MethodInfo.MakeGenericMethod(args));
            }
            throw new InvalidOperationException("MakeGenericMethod cannot be performed on non-generic method");
        }
    }
}
