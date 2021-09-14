using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class AlternateNamesAttribute : Attribute, ITypeTranslator, IMemberTranslator
    {
        readonly AlternateNameTypeTranslator _typeTranslator;
        readonly AlternateNameMemberTranslator _memberTranslator;

        public AlternateNamesAttribute(params string[] alternateNames)
        {
            _typeTranslator = new AlternateNameTypeTranslator(alternateNames);
            _memberTranslator = new AlternateNameMemberTranslator(alternateNames);
        }

        public TypeDescription Translate(TypeTranslateArgs args) => _typeTranslator.Translate(args);

        public MemberDescription Translate(MemberTranslatorArgs args) => _memberTranslator.Translate(args);
    }
}
