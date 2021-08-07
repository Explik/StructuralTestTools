using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using OleVanSanten.TestTools.Helpers;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class SameNameMemberTranslator : IMemberTranslator
    {
        public MemberDescription Translate(MemberTranslatorArgs args)
        {
            IEnumerable<MemberDescription> allMembers = args.TargetType.GetMembers();
            IEnumerable<MemberDescription> matchingMembers = allMembers.Where(m => m.Name == args.OriginalMember.Name);

            if (!matchingMembers.Any())
                args.Verifier.FailMemberNotFound(args.TargetType, new[] { args.OriginalMember.Name });

            // Multiple MethodBase members may have the same name and only differ in parameter list
            if (args.OriginalMember is MethodBaseDescription methodBase1)
            {
                foreach (var methodBase2 in matchingMembers.OfType<MethodBaseDescription>())
                {
                    // TODO Try to do this somewhat simpler 
                    var methodBase3 = methodBase2;
                    
                    if (methodBase2.IsGenericMethod)
                        methodBase3 = ((MethodDescription)methodBase2).MakeGenericMethod(methodBase1.GetGenericArguments());

                    var parameterTypes1 = methodBase1.GetParameters().Select(p => args.TypeTranslatorService.TranslateType(p.ParameterType));
                    var parameterTypes2 = methodBase3.GetParameters().Select(p => p.ParameterType);

                    if (parameterTypes1.SequenceEqual(parameterTypes2))
                        return methodBase2;
                }

                // Fail if no matching method is found
                if (methodBase1 is MethodDescription methodInfo)
                    args.Verifier.FailMethodNotFound(args.TargetType, methodInfo);
                if (methodBase1 is ConstructorDescription constructorInfo)
                    args.Verifier.FailConstructorNotFound(args.TargetType, constructorInfo);
            }
            return matchingMembers.First();
        }
    }
}
