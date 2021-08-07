using System;
using System.Collections.Generic;
using System.Text;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public class UnchangedMemberIsVirtualVerifier : IMemberVerifier
    {
        public MemberVerificationAspect[] Aspects => new[] {
            MemberVerificationAspect.PropertyGetIsVirtual,
            MemberVerificationAspect.PropertySetIsVirtual,
            MemberVerificationAspect.MethodIsVirtual
        };

        public void Verify(MemberVerifierArgs args)
        {
            if (args.OriginalMember is PropertyDescription originalProperty)
            {
                args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Property });

                if (originalProperty.CanRead)
                    args.Verifier.VerifyIsVirtual((PropertyDescription)args.TranslatedMember, originalProperty.GetMethod.IsVirtual, GetMethod: true);

                if (originalProperty.CanWrite)
                    args.Verifier.VerifyIsVirtual((PropertyDescription)args.TranslatedMember, originalProperty.SetMethod.IsVirtual, SetMethod: true);
            }
            else if (args.TranslatedMember is MethodDescription originalMethod)
            {
                args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Method });
                args.Verifier.VerifyIsStatic((MethodDescription)args.TranslatedMember, originalMethod.IsStatic);
            }
            else throw new NotImplementedException();
        }
    }
}
