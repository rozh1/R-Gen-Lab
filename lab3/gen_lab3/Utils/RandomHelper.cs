using System;

namespace gen_lab3.Utils
{
    internal static class RandomHelper
    {
        //public static Random Random = new Random((int)DateTime.Now.ToBinary());
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            {
                // synchronize
                return random.Next(min, max+1);
            }
        }
    }
}