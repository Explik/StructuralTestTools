using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using OleVanSanten.TestTools.Helpers;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class AlternateNameMemberTranslator : MemberTranslator
    {
        string[] _alternateNames;

        public AlternateNameMemberTranslator(string[] alternateNames)
        {
            _alternateNames = alternateNames;
        }

        public override MemberDescription Translate(MemberDescription member)
        {
            string[] names = _alternateNames.Union(new[] { member.Name }).ToArray();

            IEnumerable<MemberDescription> members = TargetType.GetMembers().Where(m => names.Contains(m.Name));

            if (!members.Any())
                Verifier.FailMemberNotFound(TargetType, names);

            // Multiple MethodBase members may have the same name and only differ in argument list
            if (member is MethodBaseDescription methodBase1)
            {
                foreach (var methodBase2 in members.OfType<MethodBaseDescription>())
                {
                    var parameterTypes1 = methodBase1.GetParameters().Select(p => Service.TranslateType(p.ParameterType));
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
