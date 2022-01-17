using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explik.StructuralTestTools.MSBuild
{
    public class FileService : IFileService
    {
        private readonly string _solutionPath;
        private readonly string _projectPath;
        private readonly string _configPath;

        public FileService(string solutionPath, string projectPath, string configPath)
        {
            _solutionPath = solutionPath;
            _projectPath = projectPath;
            _configPath = configPath;
        }

        public FileInfo GetSolutionFile()
        {
            var fileInfo = new FileInfo(_solutionPath);

            if (!fileInfo.Exists)
                throw new InvalidOperationException("No solution file found on path " + _solutionPath);

            return fileInfo;
        }

        public FileInfo GetProjectFile()
        {
            var fileInfo = new FileInfo(_projectPath);

            if (!fileInfo.Exists)
                throw new InvalidOperationException("No project file found on path " + _solutionPath);

            return fileInfo;
        }

        public FileInfo GetConfigFile()
        {
            var fileInfo = new FileInfo(_configPath);
            return fileInfo.Exists ? fileInfo : null;
        }

        public void CreateLogFile(string fileContent)
        {
            // Generated files are placed in the project directory
            var directory = new FileInfo(_projectPath).Directory;
            var filePath = directory + "\\StructuralTestTools-log.txt";

            File.WriteAllText(filePath, fileContent);
        }

        public void CreateGeneratedFile(string fileName, string fileContent)
        {
            // Generated files are placed in the project directory
            var directory = new FileInfo(_projectPath).Directory;
            var filePath = directory + "\\" + fileName;

            // Creates or overwrites a readonly generated file
            if (File.Exists(filePath)) File.SetAttributes(filePath, FileAttributes.Normal);
            File.WriteAllText(filePath, fileContent);
            File.SetAttributes(filePath, FileAttributes.ReadOnly);
        }
    }
}
