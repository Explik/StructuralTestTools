using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using OleVanSanten.TestTools.Helpers;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools
{
    public class AlternateNameMemberTranslator : IMemberTranslator
    {
        string[] _alternateNames;

        public AlternateNameMemberTranslator(string[] alternateNames)
        {
            _alternateNames = alternateNames;
        }

        public MemberDescription Translate(MemberTranslatorArgs args)
        {
            string[] names = _alternateNames.Union(new[] { args.OriginalMember.Name }).ToArray();

            IEnumerable<MemberDescription> members = args.TargetType.GetMembers().Where(m => names.Contains(m.Name));

            if (!members.Any())
                args.Verifier.FailMemberNotFound(args.TargetType, names);

            // Multiple MethodBase members may have the same name and only differ in argument list
            if (args.OriginalMember is MethodBaseDescription methodBase1)
            {
                foreach (var methodBase2 in members.OfType<MethodBaseDescription>())
                {
                    var parameterTypes1 = methodBase1.GetParameters().Select(p => args.TypeTranslatorService.TranslateType(p.ParameterType));
                    var parameterTypes2 = methodBase2.GetParameters().Select(p => p.ParameterType);

                    if (parameterTypes1.SequenceEqual(parameterTypes2))
                        return methodBase2;
                }
                throw new NotImplementedException();
            }
            return members.First();
        }
    }
}
