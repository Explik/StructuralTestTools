using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explik.StructuralTestTools.MSBuild
{
    public class LogService: ILogService
    {
        private IFileService _fileService;
        private StringBuilder _stringBuilder = new StringBuilder();
        
        public LogService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public void LogDiagnostic(WorkspaceDiagnostic diagnostic)
        {
            _stringBuilder.AppendLine(diagnostic.Message);

            // Only generate file if diagnostics is failure
            if (diagnostic.Kind == WorkspaceDiagnosticKind.Failure) GenerateLogFile();
        }

        public void LogException(Exception exception)
        {
            _stringBuilder.AppendLine(exception.ToString());

            // Always generate file for exceptions
            GenerateLogFile();
        }

        public void LogInfo(string info)
        {
            _stringBuilder.AppendLine(info);
        }

        private void GenerateLogFile()
        {
            _fileService.CreateLogFile(_stringBuilder.ToString());
        }
    }
}
