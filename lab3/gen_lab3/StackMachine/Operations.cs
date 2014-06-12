using System.Collections.Generic;
using gen_lab3.StackMachine.Data;

namespace gen_lab3.StackMachine
{
    internal class Operations
    {
        public ComputeResult Plus(Stack<int> stack)
        {
            if (stack.Count > 1)
            {
                int val1 = stack.Pop();
                int val2 = stack.Pop();
                stack.Push(val1 + val2);
            }
            else
            {
                return (new ComputeResult {Error = 100, Lenght = stack.Count, Val = "NA"});
            }
            return null;
        }

        public ComputeResult Minus(Stack<int> stack)
        {
            if (stack.Count > 1)
            {
                int val1 = stack.Pop();
                int val2 = stack.Pop();
                stack.Push(val1 - val2);
            }
            else
            {
                return (new ComputeResult {Error = 100, Lenght = stack.Count, Val = "NA"});
            }
            return null;
        }

        public ComputeResult Mul(Stack<int> stack)
        {
            if (stack.Count > 1)
            {
                int val1 = stack.Pop();
                int val2 = stack.Pop();
                stack.Push(val1*val2);
            }
            else
            {
                return (new ComputeResult {Error = 100, Lenght = stack.Count, Val = "NA"});
            }
            return null;
        }

        public ComputeResult Div(Stack<int> stack)
        {
            if (stack.Count > 1)
            {
                int val1 = stack.Pop();
                int val2 = stack.Pop();
                if (val2 != 0)
                    stack.Push(val1/val2);
                else
                {
                    return (new ComputeResult {Error = 100, Lenght = stack.Count, Val = "NA"});
                }
            }
            else
            {
                return (new ComputeResult {Error = 100, Lenght = stack.Count, Val = "NA"});
            }
            return null;
        }

        public ComputeResult Push(Stack<int> stack, int item)
        {
            stack.Push(item);
            return null;
        }

        public ComputeResult Drop(Stack<int> stack)
        {
            if (stack.Count > 0)
            {
                stack.Pop();
            }
            else
            {
                return (new ComputeResult {Error = 100, Lenght = stack.Count, Val = "NA"});
            }
            return null;
        }

        public ComputeResult Dup(Stack<int> stack)
        {
            if (stack.Count > 0)
            {
                stack.Push(stack.Peek());
            }
            else
            {
                return (new ComputeResult {Error = 100, Lenght = stack.Count, Val = "NA"});
            }
            return null;
        }

        public ComputeResult Swap(Stack<int> stack)
        {
            if (stack.Count > 1)
            {
                int val1 = stack.Pop();
                int val2 = stack.Pop();
                stack.Push(val1);
                stack.Push(val2);
            }
            else
            {
                return (new ComputeResult {Error = 100, Lenght = stack.Count, Val = "NA"});
            }
            return null;
        }

        public ComputeResult Over(Stack<int> stack)
        {
            if (stack.Count > 2)
            {
                int val1 = stack.Pop();
                int val2 = stack.Pop();
                int val3 = stack.Pop();
                stack.Push(val2);
                stack.Push(val1);
                stack.Push(val3);
            }
            else
            {
                return (new ComputeResult {Error = 100, Lenght = stack.Count, Val = "NA"});
            }
            return null;
        }
    }
}