using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explik.StructuralTestTools.MSBuild
{
    public class CompilationService : ICompilationService
    {
        private readonly IFileService _fileService;
        private readonly ILogService _logService;

        static CompilationService()
        {
            // Finds compatible MSBuild instances 
            var instaces = MSBuildLocator.QueryVisualStudioInstances();
            if (!instaces.Any())
                throw new InvalidOperationException("Failed to find MSBuild instance compatible with Visual Studio");

            // Registers some instance with MSBuildWorkspace to make it work
            MSBuildLocator.RegisterMSBuildPath(instaces.Select(s => s.MSBuildPath).ToArray());
            if (MSBuildLocator.CanRegister)
                throw new InvalidOperationException("Failed to load MSBuild");
        }

        public CompilationService(IFileService fileService): this(fileService, null)
        {
        }
        
        public CompilationService(IFileService fileService, ILogService logService)
        {
            _fileService = fileService;
            _logService = logService;
        }

        public async Task<Compilation> GetCompilation()
        {
            Compilation compilation = null;

            using (var workspace = GetWorkspace())
            {
                var solutionPath = _fileService.GetSolutionFile().FullName;
                var projectPath = _fileService.GetProjectFile().FullName;
                var solution = await workspace.OpenSolutionAsync(solutionPath);

                foreach (Project project in GetProjects(solution, projectPath))
                {
                    AddProjectDocuments(project, skipGeneratedFiles: project.FilePath == projectPath);
                    compilation = await project.GetCompilationAsync();
                }
            }
            return compilation;
        }

        // Creates MSBuildWorkspace based on registered MSbuild
        private MSBuildWorkspace GetWorkspace()
        {
            var workspace = MSBuildWorkspace.Create();
            workspace.WorkspaceFailed += (sender, args) => _logService?.LogDiagnostic(args.Diagnostic);
            return workspace;
        }

        // Gets all projects in solution associated with projectName in build order
        private IEnumerable<Project> GetProjects(Solution solution, string projectPath)
        {
            var dependencyTree = solution.GetProjectDependencyGraph();
            var allProjectIds = dependencyTree.GetTopologicallySortedProjects();
            var allProjects = allProjectIds.Select(id => solution.GetProject(id));

            var project = allProjects.FirstOrDefault(p => p.FilePath == projectPath);
            if (project == null) throw new InvalidOperationException("Solution does not contain project " + projectPath);

            var chainProjectIds = dependencyTree.GetProjectsThatThisProjectTransitivelyDependsOn(project.Id);
            return allProjects.Where(p => p.Id == project.Id || chainProjectIds.Contains(p.Id));
        }

        // Adds documents to project as some project formats do not include documents by default
        private void AddProjectDocuments(Project project, bool skipGeneratedFiles)
        {
            // Some project formats do not include files to compile, so the .cs files contained in the project
            // directory are included by default.
            if (project.DocumentIds.Count == 0)
            {
                var projectFile = new FileInfo(project.FilePath);
                var csFiles = projectFile.Directory.GetFiles("*.cs", SearchOption.AllDirectories);

                foreach (var csFile in csFiles)
                {
                    project = project.AddDocument(csFile.FullName, File.ReadAllText(csFile.FullName)).Project;
                }
            }

            // Removing generated files from earlier run as these might cause the compilation to fail.
            if(skipGeneratedFiles)
            {
                var generatedDocuments = project.Documents.Where(d => d.Name.EndsWith(".g.cs"));
                foreach (var document in generatedDocuments)
                {
                    project = project.RemoveDocument(document.Id);
                }
            }
        }
    }
}
