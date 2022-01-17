using Explik.StructuralTestTools.TypeSystem;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explik.StructuralTestTools.MSBuild
{
    public interface IConfigurationService
    {
        Task<IConfiguration> GetConfiguration(NamespaceDescription globalNamespace);
    }
}
