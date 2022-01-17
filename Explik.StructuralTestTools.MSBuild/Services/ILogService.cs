using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explik.StructuralTestTools.MSBuild
{
    public interface ILogService
    {
        void LogDiagnostic(WorkspaceDiagnostic diagnostic);

        void LogException(Exception exception);

        void LogInfo(string info);
    }
}
