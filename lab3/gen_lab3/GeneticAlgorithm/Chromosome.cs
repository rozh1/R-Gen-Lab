using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using gen_lab3.StackMachine.Data;
using gen_lab3.Utils;

namespace gen_lab3.GeneticAlgorithm.Data
{
    internal class Individual : IComparable<Individual>
    {
        public List<string> IndividualGenesList;

        public Individual()
        {
            IndividualGenesList = new List<string>();
        }

        public Individual(int length)
        {
            IndividualGenesList = new List<string>(length);
            for (int i = 0; i < length; i++)
            {
                IndividualGenesList.Add(
                    Operations.OperationStrings[
                        RandomHelper.RandomNumber(
                            0,
                            Operations.OperationStrings.Length -1
                            )]);
            }
        }

        public int Length
        {
            get { return IndividualGenesList.Count; }
        }

        public int Health { get; set; }

        public int CompareTo(Individual other)
        {
            if (Health > other.Health)
                return 1;
            if (Health < other.Health)
                return -1;
            return 0;
        }

        public string[] Program(int[] inputs)
        {
            List<string> program = inputs.Select(t => t.ToString(CultureInfo.InvariantCulture)).ToList();
            program.AddRange(IndividualGenesList);
            return program.ToArray();
        }

        ~Individual()
        {
            IndividualGenesList.Clear();
        }
    }
}