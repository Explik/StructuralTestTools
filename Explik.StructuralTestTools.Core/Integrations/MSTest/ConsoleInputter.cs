using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Explik.StructuralTestTools.MSTest
{
    public static class ConsoleInputter
    {
        private static BufferedReader _internalReader = new BufferedReader();

        public static void Write()
        {
            Write("");
        }

        public static void Write(string input)
        {
            if (!_internalReader.HasCapturedConsoleIn)
                _internalReader.CaptureConsoleIn();

            _internalReader.Buffer += input;
        }

        public static void WriteLine()
        {
            WriteLine("");
        }

        public static void WriteLine(string input)
        {
            Write(input + Environment.NewLine);
        }

        // Erases any Console.In data 
        public static void Clear()
        {
            /* Console.In.ReadToEnd may block if Console.In is an operating system 
             * window */
            if (Console.KeyAvailable)
                Console.In.ReadToEnd();

            /* Console.KeyAvailable does not work for BufferedReader, so this has 
             * to be cleared separately */
            if (_internalReader.HasCapturedConsoleIn)
                _internalReader.Buffer = "";
        }

        private class BufferedReader : TextReader
        {
            TextReader _originalReader;

            public string Buffer { get; set; }

            public bool HasCapturedConsoleIn { get; private set; }

            public override int Read()
            {
                if (Buffer == null || Buffer.Length == 0)
                {
                    // Releasing Console.In again 
                    Console.SetIn(_originalReader);
                    HasCapturedConsoleIn = false;

                    /* Console.In.ReadToEnd() may continue to hit the this reader 
                     * even after the Console.SetIn has been called, 
                     * so all further reads are redirected to the OriginalReader 
                     * which ensures that the Console.In is left empty when 
                     * ReadToEnd returns */
                    return Console.KeyAvailable ? _originalReader.Read() : -1;
                }
                    
                char charToRead = Buffer[0];
                Buffer = Buffer.Substring(1);
                return (int)charToRead;
            }

            public void CaptureConsoleIn()
            {
                /* Read existing Console.In content to buffer, so that existing 
                 * Console.In content is not erased or preceded by the incoming 
                 * buffer content */
                if (Console.KeyAvailable)
                    Buffer = Console.In.ReadToEnd();

                // Capturing Console.In
                _originalReader = Console.In;
                Console.SetIn(this);
                HasCapturedConsoleIn = true;
            }
        }
    }
}
