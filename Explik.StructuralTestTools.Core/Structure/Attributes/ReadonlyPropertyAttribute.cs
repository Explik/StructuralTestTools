using System;
using System.Collections.Generic;
using System.Text;
using Explik.StructuralTestTools.Helpers;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class ReadonlyPropertyAttribute : Attribute, IMemberVerifier
    {
        public VerifierService Verifier { get; set; }

        public IStructureService Service { get; set; }

        public MemberVerificationAspect[] Aspects => new[] {  
            MemberVerificationAspect.MemberType,
            MemberVerificationAspect.PropertyGetAccessLevel 
        };

        public void Verify(MemberVerifierArgs args)
        {
            Verifier.VerifyMemberType(args.TranslatedMember, new[] { MemberTypes.Property });

            var originalProperty = (PropertyDescription)args.OriginalMember;
            var translatedProperty = (PropertyDescription)args.TranslatedMember;
            Verifier.VerifyIsReadonly(translatedProperty, DescriptionHelper.GetAccessLevel(originalProperty.GetMethod));
        }
    }
}
