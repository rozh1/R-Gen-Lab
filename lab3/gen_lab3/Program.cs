using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using gen_lab3.GeneticAlgorithm;
using gen_lab3.StackMachine;
using gen_lab3.StackMachine.Data;
using gen_lab3.Utils;

namespace gen_lab3
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 5)
            {
                Console.WriteLine("Usage: " + AppDomain.CurrentDomain.FriendlyName + " " +
                                  "TasksFile" + " " +
                                  "LogFile" + " " +
                                  "chromosomeLength" + " " +
                                  "population" + " " +
                                  "generations"
                    );
                Console.WriteLine("Example:");
                Console.WriteLine(AppDomain.CurrentDomain.FriendlyName + " " +
                                  "tasks.csv" + " " +
                                  "log.txt" + " " +
                                  "9" + " " +
                                  "100" + " " +
                                  "50");

                Console.ReadKey();
                return;
            }

            string filePath = args.Length > 0 ? args[0] : "tasks.csv";
            Writer.OutputFilePath = args.Length > 1 ? args[1] : "";
            Writer.ClearFile();

            int chromosomeLength = 10;
            int individualsCount = 100;
            int generationsCount = 1000;

            if (args.Length > 2) int.TryParse(args[2], out chromosomeLength);
            if (args.Length > 3) int.TryParse(args[3], out individualsCount);
            if (args.Length > 4) int.TryParse(args[4], out generationsCount);
            if (args.Length > 5) Writer.WriteToConsole = false;
            
            Writer.WriteLine("Параметры запуска:");
            Writer.WriteLine("\tДлина программы:", chromosomeLength.ToString(CultureInfo.InvariantCulture));
            Writer.WriteLine("\tПопуляция:", individualsCount.ToString(CultureInfo.InvariantCulture));
            Writer.WriteLine("\tМаксимальное количество поколений:", generationsCount.ToString(CultureInfo.InvariantCulture));
            
            var machine = new Machine();

            Dictionary<int[], int> taskDictionary = DictionaryReader.Read(filePath);

            Writer.WriteLine("\tЗадачник: ");
            foreach (KeyValuePair<int[], int> pair in taskDictionary)
            {
                Writer.WriteLine("\t\t" + string.Join("\t", pair.Key) + "\t" + pair.Value);
            }

            Writer.WriteLine("Запуск генетического алгоритма...");

            DateTime startTime = DateTime.Now;

            var ga = new GeneticAlgorithmHandler(chromosomeLength, individualsCount, taskDictionary);
            List<string> strings = ga.Run(generationsCount);

            Writer.WriteLine("\nГенетический алгоритм завершил работу за " + (DateTime.Now - startTime));
            Writer.WriteLine("\nПроверка решения по задачнику:");

            foreach (var pair in taskDictionary)
            {
                Writer.WriteLine("\nInput = " + string.Join(" ", pair.Key) + ". Program trace:");
                List<string> program = pair.Key.Select(t => t.ToString(CultureInfo.InvariantCulture)).ToList();
                program.AddRange(strings);
                ComputeResult computeResult = machine.Run(program.ToArray(), true);

                Writer.WriteLine("\nVal=" + computeResult.Val + " NeedVal=" + pair.Value + " StackLength=" +
                                 computeResult.Lenght + " Error=" +
                                 computeResult.Error);
            }

            if (Writer.WriteToConsole) Console.ReadKey();
        }
    }
}