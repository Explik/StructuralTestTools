using Microsoft.VisualStudio.TestTools.UnitTesting;
using Explik.StructuralTestTools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace TestTools_Tests
{
    [TestClass]
    public class PracticalTestTests
    {
        string msbuildPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin";
        string solutionPath = @"C:\Users\OVS\source\repos\Explik\structural-test-tools\Explik.StructuralTestTools.PracticalTests\Explik.StructuralTestTools.PracticalTests.sln";
        string projectName = @"Explik.StructuralTestTools.PracticalTests";
        string configPath = @"C:\Users\OVS\source\repos\Explik\structural-test-tools\Explik.StructuralTestTools.PracticalTests\TestToolsConfig.xml";

        [TestMethod]
        public void UnitTestTemplator_Debug()
        {
             Program.Run(msbuildPath, solutionPath, projectName, configPath);
        }

        [TestMethod]
        public void UnitTestTemplator_Debug2()
        {
            string programPath = @"C:\Users\OVS\source\repos\Explik\structural-test-tools\Explik.StructuralTestTools.MSBuild\bin\Debug\net5.0\Explik.StructuralTestTools.MSBuild.exe";

            using (var program = new Process())
            {
                program.StartInfo.FileName = programPath;
                program.StartInfo.Arguments = $"\"{msbuildPath}\" \"{solutionPath}\" \"{projectName}\" \"{configPath}\"";
                program.StartInfo.UseShellExecute = false;
                program.StartInfo.RedirectStandardOutput = true;
                program.StartInfo.RedirectStandardError = true;
                program.Start();

                Console.WriteLine(program.StartInfo.FileName + " " + program.StartInfo.Arguments);
                Console.WriteLine(program.StandardOutput.ReadToEnd());
                Console.WriteLine(program.StandardError.ReadToEnd());

                program.WaitForExit();
            }
            throw new Exception();
        }

        [TestMethod]
        public void UnitTestTemplator_Debug3()
        {
            string programPath = @"C:\Users\OVS\source\repos\Explik\structural-test-tools\Explik.StructuralTestTools.MSBuild.Tasks\bin\Debug\net46\Explik.StructuralTestTools.MSBuild.exe";

            using (var program = new Process())
            {
                program.StartInfo.FileName = programPath;
                program.StartInfo.Arguments = $"\"{msbuildPath}\" \"{solutionPath}\" \"{projectName}\" \"{configPath}\"";
                program.StartInfo.UseShellExecute = false;
                program.StartInfo.RedirectStandardOutput = true;
                program.StartInfo.RedirectStandardError = true;
                program.Start();

                Console.WriteLine(program.StartInfo.FileName + " " + program.StartInfo.Arguments);
                Console.WriteLine(program.StandardOutput.ReadToEnd());
                Console.WriteLine(program.StandardError.ReadToEnd());

                program.WaitForExit();
            }
            throw new Exception();
        }
    }
}
