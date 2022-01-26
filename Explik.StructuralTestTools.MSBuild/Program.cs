using System;
using Explik.StructuralTestTools.MSBuild;
using System.Threading.Tasks;
using Explik.StructuralTestTools.TypeSystem;

namespace Explik.StructuralTestTools
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 3)
                throw new ArgumentException();

            Run(args[0], args[1], args[2]).Wait();
        }

        public static async Task Run(string solutionPath, string projectPath, string configPath)
        {
            // Setting up base services
            var fileService = new FileService(solutionPath, projectPath, configPath);
            var logService = new LogService(fileService);

            // Generate templated unit tests if project compiles
            try
            {
                var configurationService = new ConfigurationService(fileService, logService);
                var compilationService = new CompilationService(configurationService, fileService, logService);
                var templateService = new TemplateService(fileService, logService);

                var compilation = await compilationService.GetCompilation();
                if (compilation != null)
                {
                    var globalNamespace = new CompileTimeNamespaceDescription(compilation, compilation.GlobalNamespace);
                    var configuration = await configurationService.GetConfiguration(globalNamespace);
                    templateService.TemplateUnitTests(compilation, configuration);
                }
            }
            catch (Exception ex)
            {
                logService.LogException(ex);
            }
        }
    }
}
