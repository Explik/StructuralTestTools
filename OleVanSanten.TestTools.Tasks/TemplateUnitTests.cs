using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace OleVanSanten.TestTools.Tasks
{
    public class TemplateUnitTest : Task
    {
        [Output]
        public string ConfigPath { get; set; }

        public override bool Execute()
        {
            Log.LogMessage("Hello world");
            return true;
        }
    }
}
