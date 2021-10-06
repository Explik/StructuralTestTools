using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools.Helpers;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class ReadonlyPropertyAttribute : Attribute, IMemberVerifier
    {
        public MemberVerificationAspect[] Aspects => new[] {  
            MemberVerificationAspect.MemberType,
            MemberVerificationAspect.PropertyGetAccessLevel 
        };

        public void Verify(MemberVerifierArgs args)
        {
            args.Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Property });

            var originalProperty = (PropertyDescription)args.OriginalMember;
            var translatedProperty = (PropertyDescription)args.TranslatedMember;
            args.Verifier.VerifyIsReadonly(translatedProperty, DescriptionHelper.GetAccessLevel(originalProperty.GetMethod));
        }
    }
}
