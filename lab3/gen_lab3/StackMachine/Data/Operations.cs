namespace gen_lab3.StackMachine.Data
{
    public static class Operations
    {
        public const string Plus = "+";
        public const string Minus = "-";
        public const string Mul = "*";
        public const string Div = "/";
        public const string Drop = "D";
        public const string Dup = "Dup";
        public const string Swap = "S";
        public const string Over = "O";
        public const string Nop = "NOP";

        public static string[] OperationStrings = {Plus, Minus, Mul, Div, Drop, Dup, Swap, Over, Nop};
    }
}