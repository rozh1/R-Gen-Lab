using System;
using System.IO;

namespace gen_lab3.Utils
{
    internal static class Writer
    {
        public static string OutputFilePath = "";
        public static bool WriteToConsole = true;

        public static void WriteLine(params string[] text)
        {
            string output = string.Join(" ", text);
            if (WriteToConsole) Console.WriteLine(output);
            if (OutputFilePath != "") File.AppendAllText(OutputFilePath, output + "\r\n");
        }

        public static void ClearFile()
        {
            if (File.Exists(OutputFilePath)) File.Delete(OutputFilePath);
        }
    }
}