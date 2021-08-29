using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace OleVanSanten.TestTools.Tasks
{
    public class TemplateUnitTest : Task
    {
        [Required]
        public string ConfigPath { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        public string SolutionPath { get; set; }

        public override bool Execute()
        {
            Log.LogMessage(MessageImportance.High, ConfigPath);
            Log.LogMessage(MessageImportance.High, ProjectName);
            Log.LogMessage(MessageImportance.High, SolutionPath);

            try
            {
                UnitTestTemplator.TemplateUnitTests(SolutionPath, ProjectName, ConfigPath);
                return true;
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
                return false;
            }
        }
    }
}
