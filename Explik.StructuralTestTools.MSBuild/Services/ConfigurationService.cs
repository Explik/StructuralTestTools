using Explik.StructuralTestTools.TypeSystem;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explik.StructuralTestTools.MSBuild
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IFileService _fileService;
        private readonly ILogService _logService;

        public ConfigurationService(IFileService fileService): this(fileService, null)
        {
        }

        public ConfigurationService(IFileService fileService, ILogService logService)
        {
            _fileService = fileService;
            _logService = logService;
        }

        public async Task<IConfiguration> GetConfiguration(NamespaceDescription globalNamespace)
        {
            var configFile = _fileService.GetConfigFile();
            
            if(configFile != null)
            {
                _logService?.LogInfo("Using configuration from file " + configFile.FullName);
                
                var configContent = await File.ReadAllTextAsync(configFile.FullName);
                return Configuration.CreateFromXMLWithDefaults(globalNamespace, _fileService.GetProjectFile(), configContent);
            }
            else
            {
                _logService?.LogInfo("Using default configuration due to no config file found");

                return Configuration.CreateDefault(globalNamespace, _fileService.GetProjectFile());
            }
        }
    }
}
