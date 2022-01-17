using Microsoft.CodeAnalysis;
using System.Threading.Tasks;

namespace Explik.StructuralTestTools.MSBuild
{
    public interface ICompilationService
    {
        Task<Compilation> GetCompilation();
    }
}