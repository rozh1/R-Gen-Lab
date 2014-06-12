using System;
using System.Collections.Generic;
using System.IO;

namespace gen_lab3.Utils
{
    internal static class DictionaryReader
    {
        public static Dictionary<int[], int> Read(string filePath)
        {
            var tasksDictionary = new Dictionary<int[], int>();
            try
            {
                var sr = new StreamReader(filePath);
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line != null)
                    {
                        string[] numbers = line.Split(';');
                        var inputs = new int[numbers.Length - 1];
                        int output;
                        for (int i = 0; i < numbers.Length - 1; i++)
                        {
                            if (!int.TryParse(numbers[i], out inputs[i]))
                            {
                                Writer.WriteLine("Строка имела неверный формат: " + line);
                                break;
                            }
                        }

                        if (!int.TryParse(numbers[numbers.Length - 1], out output))
                        {
                            Writer.WriteLine("Строка имела неверный формат: " + line);
                            break;
                        }
                        tasksDictionary.Add(inputs, output);
                    }
                }
            }
            catch (Exception e)
            {
                Writer.WriteLine("При файла возникло исключение: " + e.Message);
            }
            return tasksDictionary;
        }
    }
}