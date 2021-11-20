using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTools_Tests
{
    public static class TestHelper
    {
        public static void AssertAreEqualSource(string expected, string actual)
        {
            var expectedLines = expected.Split(Environment.NewLine);
            var actualLines = actual.Split(Environment.NewLine);

            if (expectedLines.Length != actualLines.Length)
                throw new AssertFailedException($"Assertion failed: The number of lines differ. \r\n Expected<{expected}> \r\n Actual<{actual}>");

            for(int i = 0; i < expectedLines.Length; i++)
            {
                var expectedLine = expectedLines[i].TrimEnd();
                var actualLine = actualLines[i].TrimEnd();
                var lineLength = Math.Min(expectedLine.Length, actualLine.Length);

                for (int j = 0; j < lineLength; j++)
                {
                    if (expectedLine[j] != actualLine[j])
                        throw new AssertFailedException($"Assertion failed: The line {i} at characther {j}.\r\n Expected<{expected}> \r\n Actual<{actual}>");
                }
            }
        }
    }
}
