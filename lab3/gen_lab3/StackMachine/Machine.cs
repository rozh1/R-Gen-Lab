using System;
using System.Collections.Generic;
using System.Globalization;
using gen_lab3.StackMachine.Data;
using gen_lab3.Utils;

namespace gen_lab3.StackMachine
{
    internal class Machine
    {
        private readonly Operations _operations;
        private Stack<int> _stack;

        public Machine()
        {
            _operations = new Operations();
        }

        public ComputeResult Run(string[] program, bool trace = false)
        {
            _stack = new Stack<int>();
            for (int i = 0; i < program.Length; i++)
            {
                string comand = program[i];
                int number;
                if (int.TryParse(comand, out number))
                {
                    _operations.Push(_stack, number);
                }
                else
                {
                    ComputeResult computeResult = null;
                    switch (comand)
                    {
                        case Data.Operations.Div:
                            computeResult = _operations.Div(_stack);
                            break;
                        case Data.Operations.Drop:
                            computeResult = _operations.Drop(_stack);
                            break;
                        case Data.Operations.Dup:
                            computeResult = _operations.Dup(_stack);
                            break;
                        case Data.Operations.Minus:
                            computeResult = _operations.Minus(_stack);
                            break;
                        case Data.Operations.Mul:
                            computeResult = _operations.Mul(_stack);
                            break;
                        case Data.Operations.Nop:
                            break;
                        case Data.Operations.Over:
                            computeResult = _operations.Over(_stack);
                            break;
                        case Data.Operations.Plus:
                            computeResult = _operations.Plus(_stack);
                            break;
                        case Data.Operations.Swap:
                            computeResult = _operations.Swap(_stack);
                            break;
                    }
                    if (computeResult != null)
                    {
                        return computeResult;
                    }
                }
                if (trace)
                {
                    Writer.WriteLine(comand + "\t" + String.Join(" ", _stack.ToArray()));
                }
            }

            return new ComputeResult
            {
                Error = 0,
                Lenght = _stack.Count,
                Val = _stack.Count > 0 ? _stack.Peek().ToString(CultureInfo.InvariantCulture) : "NA"
            };
        }
    }
}