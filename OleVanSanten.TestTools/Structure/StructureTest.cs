using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using OleVanSanten.TestTools.Helpers;
using OleVanSanten.TestTools.Structure;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class StructureTest
    {
        Action _executeAction;
        IStructureService _structureService;

        public StructureTest(IStructureService structureService) 
        {
            _structureService = structureService;
        }

        public void AssertType(Type type, params ITypeVerifier[] verifiers)
        {
            _executeAction += () => _structureService.VerifyType(new RuntimeTypeDescription(type), verifiers);
        }

        public void AssertMember(MemberInfo memberInfo, params IMemberVerifier[] verifiers)
        {
            RuntimeDescriptionFactory factory = new RuntimeDescriptionFactory();
            _executeAction += () => _structureService.VerifyMember(factory.Create(memberInfo), verifiers);
        }

        public void Execute()
        {
            _executeAction?.Invoke();
        }
    }
}
