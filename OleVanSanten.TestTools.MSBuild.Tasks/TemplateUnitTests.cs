using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace OleVanSanten.TestTools.Tasks
{
    public class TemplateUnitTest : Task
    {
        public override bool Execute()
        {
            try
            {
                UnitTestTemplator.TemplateUnitTests("", "");
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
