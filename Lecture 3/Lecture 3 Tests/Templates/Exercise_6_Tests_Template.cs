using Lecture_3_Solutions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using OleVanSanten.TestTools.Expressions;
using OleVanSanten.TestTools.MSTest;
using static Lecture_3_Tests.TestHelper;
using static OleVanSanten.TestTools.Expressions.TestExpression;

namespace Lecture_3_Tests
{
    [TestClass]
    public class Exercise_6_Tests_Template
    {
        private string ProduceExpectedPrintDirectoryOutput(DirectoryInfo info)
        {
            string buffer = $"Directory of {info.FullName}\n\n";

            foreach (DirectoryInfo subDirectory in info.GetDirectories())
                buffer += $"{subDirectory.LastWriteTime}\t<DIR>\t{subDirectory.Name}\n";

            foreach (FileInfo file in info.GetFiles())
                buffer += $"{file.LastWriteTime}\t{file.Length / 1000.0}kb\t{file.Name}\n";

            return buffer;
        }

        private string ProduceExpectedPrintTreeOutput(DirectoryInfo info, int depth)
        {
            string buffer = "";

            if (depth == 0)
                buffer += $"{info.FullName}\n";

            foreach (DirectoryInfo subDirectory in info.GetDirectories())
            {
                buffer += ProducePadding(depth + 1);
                buffer += $"{subDirectory.Name}\n";
                buffer += ProduceExpectedPrintTreeOutput(subDirectory, depth + 1);
            }
            foreach (FileInfo file in info.GetFiles())
            {
                buffer += ProducePadding(depth + 1);
                buffer += $"{file.Name}\n";
            }
            return buffer;
        }

        private string ProducePadding(int indent)
        {
            string buffer = "";

            for (int i = 0; i < indent; i++)
            {
                if (i == indent - 1)
                    buffer += "|---";
                else buffer += "|   ";
            }
            return buffer;
        }

        #region Exercise 6A
        [TemplatedTestMethod("FileExplorer.PrintDirectory(DirectoryInfo info) prints correct output"), TestCategory("Exercise 6A")]
        public void FileExplorerPrintDirectoryPrintsCorrectOutput()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo("../../../directory/");
            FileExplorer explorer = new FileExplorer();

            string expectedOutput = ProduceExpectedPrintDirectoryOutput(directoryInfo);
            ConsoleAssert.WritesOut(() => explorer.PrintDirectory(directoryInfo), expectedOutput);
        }
        #endregion

        #region Exercise 6B
        [TemplatedTestMethod("FileExplorer.PrintTree(DirectoryInfo info) prints correct output"), TestCategory("Exercise 6B")]
        public void FileExplorerPrintTreePrintsCorrectOutput()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo("../../../directory/");
            FileExplorer explorer = new FileExplorer();

            string expectedOutput = ProduceExpectedPrintTreeOutput(directoryInfo, 0);
            ConsoleAssert.WritesOut(() => explorer.PrintTree(directoryInfo), expectedOutput);
        }
        #endregion
    }
}
