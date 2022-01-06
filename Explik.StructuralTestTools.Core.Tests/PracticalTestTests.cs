using Explik.StructuralTestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace TestTools_Tests
{
    [TestClass]
    public class PracticalTestTests
    {
        private string msbuildPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin";
        private string solutionPath = @"C:\Users\OVS\source\repos\Explik\structural-test-tools\Explik.StructuralTestTools.PracticalTests\Explik.StructuralTestTools.PracticalTests.sln";
        private string projectName = @"Explik.StructuralTestTools.PracticalTests";
        private string configPath = @"C:\Users\OVS\source\repos\Explik\structural-test-tools\Explik.StructuralTestTools.PracticalTests\TestToolsConfig.xml";

        [TestMethod]
        public void UnitTestTemplator_Test1()
        {
            Program.Run(msbuildPath, solutionPath, projectName, configPath);
        }

        [TestMethod]
        public void UnitTestTemplator_Test_Debug1()
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
        public void UnitTestTemplator_Test_Debug2()
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

        [TestMethod]
        public void UnitTestTemplator_Test_Debug3()
        {
            string programPath = @"C:\Users\OVS\source\repos\Explik\structural-test-tools\Explik.StructuralTestTools.MSBuild.Tasks\bin\Debug\net46\Explik.StructuralTestTools.MSBuild.exe";
            string solutionPath = @"C:\Users\OVS\source\repos\Explik\expertimental-opp-course\Lecture 2\Lecture 2.sln";
            string projectName = "Lecture 2 Tests";
            string configPath = @"C:\Users\OVS\source\repos\Explik\expertimental-opp-course\Lecture 2\Lecture 2 Tests\TestToolsConfig.xml";

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
        public void UnitTestTemplator_Test_Release1()
        {
            string programPath = @"C:\Users\OVS\source\repos\Explik\structural-test-tools\Explik.StructuralTestTools.MSBuild\bin\Release\net5.0\Explik.StructuralTestTools.MSBuild.exe";

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
        public void UnitTestTemplator_Test_Release2()
        {
            string programPath = @"C:\Users\OVS\source\repos\Explik\structural-test-tools\Explik.StructuralTestTools.MSBuild.Tasks\bin\Release\net46\Explik.StructuralTestTools.MSBuild.exe";

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
        public void UnitTestTemplator_Test_Release3()
        {
            string programPath = @"C:\Users\OVS\source\repos\Explik\structural-test-tools\Explik.StructuralTestTools.MSBuild.Tasks\bin\Release\net46\Explik.StructuralTestTools.MSBuild.exe";
            string solutionPath = @"C:\Users\OVS\source\repos\Explik\expertimental-opp-course\Lecture 2\Lecture 2.sln";
            string projectName = "Lecture 2 Tests";
            string configPath = @"C:\Users\OVS\source\repos\Explik\expertimental-opp-course\Lecture 2\Lecture 2 Tests\TestToolsConfig.xml";

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