using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using gen_lab3.GeneticAlgorithm.Data;
using gen_lab3.StackMachine;
using gen_lab3.StackMachine.Data;
using gen_lab3.Utils;
using Operations = gen_lab3.StackMachine.Data.Operations;

namespace gen_lab3.GeneticAlgorithm
{
    internal class GeneticAlgorithmHandler
    {
        private readonly int _individualCount;

        private readonly Dictionary<int[], int> _tasksDictionary;
        private int _crossingChance = 50;
        private int _mutationChance = 50;
        private int _mutationToNumberChance;
        private List<Individual> _population;

        public GeneticAlgorithmHandler(int chromosomeLength, int individualCount, Dictionary<int[], int> tasksDictionary)
        {
            _individualCount = individualCount;
            _tasksDictionary = tasksDictionary;
            _population = new List<Individual>(individualCount);
            for (int i = 0; i < individualCount; i++)
            {
                _population.Add(new Individual(chromosomeLength));
            }
        }

        public int CrossingChance
        {
            get { return _crossingChance/10; }
            set { _crossingChance = value*10; }
        }

        public int MutatingChance
        {
            get { return _mutationChance/10; }
            set { _mutationChance = value*10; }
        }

        public int MutatingToNumberChance
        {
            get { return _mutationToNumberChance/10; }
            set { _mutationToNumberChance = value*10; }
        }

        private void Crossing(List<Individual> population)
        {
            var rand = new Random();
            Parallel.For(0, _individualCount - 1, i =>
            {
                if (rand.Next(0, 1000) < _crossingChance)
                {
                    Individual father = population[i];
                    Individual mother = population[i + 1];
                    Individual child = Crossing(father, mother);
                    population.Add(child);
                }
            });
        }

        private Individual Crossing(Individual father, Individual mother)
        {
            int index = RandomHelper.RandomNumber(0, father.Length - 1);
            var child = new Individual();
            for (int i = 0; i < index; i++) child.IndividualGenesList.Add(father.IndividualGenesList[i]);
            for (int i = index; i < mother.Length; i++) child.IndividualGenesList.Add(mother.IndividualGenesList[i]);
            Debug.Assert(child!=null);
            return child;
        }

        private void Mutation(List<Individual> population)
        {
            Parallel.For(0, _individualCount, i =>
            {
                if (RandomHelper.RandomNumber(0, 1000) < _mutationChance)
                {
                    population[i] = Mutation(population[i]);
                }
            });
        }

        private Individual Mutation(Individual chromosome)
        {
            int mutationsCount = RandomHelper.RandomNumber(1, chromosome.Length/2);
            for (int i = 0; i < mutationsCount; i++)
            {
                if (RandomHelper.RandomNumber(0, 1000) > _mutationToNumberChance)
                {
                    chromosome.IndividualGenesList[RandomHelper.RandomNumber(0, chromosome.Length - 1)] =
                        Operations.OperationStrings[RandomHelper.RandomNumber(0, Operations.OperationStrings.Length - 1)
                            ];
                }
                else
                {
                    chromosome.IndividualGenesList[RandomHelper.RandomNumber(0, chromosome.Length - 1)] =
                        RandomHelper.RandomNumber(-1, 10).ToString(CultureInfo.InvariantCulture);
                }
            }
            return chromosome;
        }

        private void CalculateHealth(List<Individual> population)
        {
            Parallel.For(0, population.Count, i =>
            {
                population[i].Health = 0;
                foreach (var pair in _tasksDictionary)
                {
                    string[] program = population[i].Program(pair.Key);
                    var machine = new Machine();
                    ComputeResult computeResult = machine.Run(program);

                    int output;
                    int.TryParse(computeResult.Val, out output);

                    population[i].Health += computeResult.Error + computeResult.Lenght + Math.Abs(output - pair.Value) -
                                            1;
                }
            });
        }

        public List<string> Run(int generations)
        {
            for (int i = 0; i < generations; i++)
            {
                Crossing(_population);
                Mutation(_population);
                CalculateHealth(_population);
                _population = _population.AsParallel().OrderBy(obj => Math.Abs(obj.Health)).ToList();
                _population.RemoveRange(_individualCount - 1, _population.Count - _individualCount);

                string programm = String.Join(" ", _population[0].IndividualGenesList.ToArray());

                Writer.WriteLine("Generation=" + (i + 1) + "\t BestHealth: " + _population[0].Health + "\n Program: " +
                                 programm);

                if (_population[0].Health == 0)
                {
                    Writer.WriteLine("Solution found!");
                    return _population[0].IndividualGenesList;
                }
            }
            Writer.WriteLine("Maximum generations hit!");
            return _population[0].IndividualGenesList;
        }
    }
}