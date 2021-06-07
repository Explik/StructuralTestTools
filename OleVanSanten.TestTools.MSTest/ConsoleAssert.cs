using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OleVanSanten.TestTools.MSTest
{
    public static class ConsoleAssert
    {
        public static void WritesOut(Action action, string writeout)
        {
            // Capturing Console output
            TextWriter oldWriter = Console.Out;
            LoggingTextWriter newWriter = new LoggingTextWriter();
            
            Console.SetOut(newWriter);
            action();
            Console.SetOut(oldWriter);

            // Reporting potential error
            string actualWriteout = newWriter.Log;
            
            if (!AreStringsApproximatelyEqual(writeout, actualWriteout))
            {
                Console.WriteLine("# Expected (Console.Out): ");
                Console.WriteLine(!string.IsNullOrEmpty(writeout) ? writeout.TrimEnd() : "no output");
                Console.WriteLine();
                Console.WriteLine("# Actual (Console.Out): ");
                Console.WriteLine(!string.IsNullOrEmpty(actualWriteout) ? actualWriteout.TrimEnd() : "no output");

                throw new AssertFailedException("Expected and actual output(might) differ");
            }
        }

        public static void WritesErr(Action action, string writeout)
        {
            // Capturing Console output
            TextWriter oldWriter = Console.Out;
            LoggingTextWriter newWriter = new LoggingTextWriter();

            Console.SetOut(newWriter);
            action();
            Console.SetOut(oldWriter);

            // Reporting potential error
            string actualWriteout = newWriter.Log;

            if (!AreStringsApproximatelyEqual(writeout, actualWriteout))
            {
                Console.WriteLine("# Expected (Console.Err): ");
                Console.WriteLine(!string.IsNullOrEmpty(writeout) ? writeout.TrimEnd() : "no output");
                Console.WriteLine();
                Console.WriteLine("# Actual (Console.Err): ");
                Console.WriteLine(!string.IsNullOrEmpty(actualWriteout) ? actualWriteout.TrimEnd() : "no output");

                throw new AssertFailedException("Expected and actual output(might) differ");
            }
        }

        // An string comparison method that does not look much on whitespace
        private static bool AreStringsApproximatelyEqual(string expected, string actual)
        {
            string[] splitIntoLines(string input) => input.Trim().Split('\n').Select(l => l.Trim()).ToArray();

            string[] expectedLines = splitIntoLines(expected);
            string[] actualLines = splitIntoLines(actual);

            if (expectedLines.Length != actualLines.Length)
                return false;

            for (int i = 0; i < expectedLines.Length; i++)
            {
                if (expectedLines[i] != actualLines[i])
                    return false;
            }

            return true;
        }

        private class LoggingTextWriter : TextWriter
        {
            StringBuilder _logger = new StringBuilder();

            public string Log 
            {
                get => _logger.ToString();
            }

            public override Encoding Encoding => Encoding.Default;

            public override void Write(char value)
            {
                _logger.Append(value);
            }
        }
    }
}
