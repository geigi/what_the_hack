using UnityEngine;

namespace Wth.ModApi.Tools
{
    public static class RandomUtils
    {
        private static System.Random random = new System.Random();
        
        public static int RollDice(int sides)
        {
            return random.Next(1, sides);
        }
        
        public static int var(int v) {
            return random.Next(-v, v);
        }
        
        public static float var(float v) {
            return (float)(-v + random.NextDouble() * (v + v));
        }
        
        public static float mult_var(float v) {
            return 1 + (float)(-v + random.NextDouble() * (v + v));
        }
    }
}