﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using TestTools.ConsoleSession;
using TestTools.Integrated;
using Lecture_3_Solutions;

namespace Lecture_3_Tests
{
    [TestClass]
    public class Exercise_6_Tests
    {
        TestFactory factory = new TestFactory("Lecture_3");

        [TestMethod("FileExplorer.PrintDirectory(DirectoryInfo info) prints correct output"), TestCategory("Exercise 6A")]
        public void FileExplorerPrintDirectoryPrintsCorrectOutput()
        {
            static string ProduceExpected(DirectoryInfo info)
            {
                string buffer = $"Directory of {info.FullName}\n\n";
                
                foreach (DirectoryInfo subDirectory in info.GetDirectories())
                    buffer += $"{subDirectory.LastWriteTime}\t<DIR>\t{subDirectory.Name}\n";
                
                foreach (FileInfo file in info.GetFiles())
                    buffer += $"{file.LastWriteTime}\t{file.Length / 1000.0}kb\t{file.Name}\n";
                
                return buffer; 
            }

            Test test = factory.CreateTest();
            TestObject<FileExplorer> explorer = test.Create<FileExplorer>();
            DirectoryInfo directoryInfo = new DirectoryInfo("../../../");

            test.Arrange(explorer, () => new FileExplorer());
            test.Act(explorer, e => e.PrintDirectory(directoryInfo));
            test.AssertWriteOut(ProduceExpected(directoryInfo));

            test.Execute();
        }
    }
}
