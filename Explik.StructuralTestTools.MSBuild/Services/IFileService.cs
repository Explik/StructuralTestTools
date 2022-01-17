using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explik.StructuralTestTools.MSBuild
{
    public interface IFileService
    {
        public FileInfo GetSolutionFile();

        public FileInfo GetProjectFile();

        public FileInfo GetConfigFile();

        public void CreateLogFile(string fileContent);

        public void CreateGeneratedFile(string fileName, string fileContent);
    }
}
