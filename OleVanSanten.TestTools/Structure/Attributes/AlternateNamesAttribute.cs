using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
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

        public NamespaceDescription TargetNamespace { 
            get => _typeTranslator.TargetNamespace; 
            set => _typeTranslator.TargetNamespace = value;
        }

        public TypeDescription TargetType { 
            get => _memberTranslator.TargetType; 
            set => _memberTranslator.TargetType = value; 
        }

        public VerifierServiceBase Verifier { get; set; }
        public IStructureService Service { get; set; }

        public TypeDescription Translate(TypeDescription type) => _typeTranslator.Translate(type);

        public MemberDescription Translate(MemberDescription member) => _memberTranslator.Translate(member);
    }
}
