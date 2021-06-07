using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using OleVanSanten.TestTools.TypeSystem;

namespace OleVanSanten.TestTools.Structure
{
    public static class IStructureServiceExtensions
    {
        // Only uses a subset of the verifiers (keeps order)
        public static void VerifyType(this IStructureService service, TypeDescription original, ITypeVerifier[] verifiers, params TypeVerificationAspect[] aspects)
        {
            List<ITypeVerifier> verifierSubset = new List<ITypeVerifier>();

            foreach (var aspect in aspects)
            {
                IEnumerable<ITypeVerifier> verifiersWithAspect = verifiers.Where(v => v.Aspects.Contains(aspect));

                // Each verifier is only added once as dublicates could potentially hurt performance,
                // because verifiers use reflection (which is slow) when verifying
                foreach (var verifier in verifiersWithAspect)
                {
                    if (!verifierSubset.Contains(verifier))
                        verifierSubset.Add(verifier);
                }
            }

            service.VerifyType(original, verifierSubset.ToArray());
        }

        // Only uses a subset of the verifiers (keeps order)
        public static void VerifyMember(this IStructureService service,  MemberDescription original, IMemberVerifier[] verifiers, params MemberVerificationAspect[] aspects)
        {
            List<IMemberVerifier> verifierSubset = new List<IMemberVerifier>();

            foreach(var aspect in aspects)
            {
                IEnumerable<IMemberVerifier> verifiersWithAspect = verifiers.Where(v => v.Aspects.Contains(aspect));

                // Each verifier is only added once as dublicates could potentially hurt performance,
                // because verifiers use reflection (which is slow) when verifying
                foreach (var verifier in verifiersWithAspect)
                {
                    if (!verifierSubset.Contains(verifier))
                        verifierSubset.Add(verifier);
                }
            }

            service.VerifyMember(original, verifierSubset.ToArray());
        }
    }
}
