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
            return Random.Range(-v, v);
        }
        
        public static float var(float v) {
            return Random.Range(-v, v);
        }
        
        public static float mult_var(float v) {
            return 1 + Random.Range(-v, v);
        }
    }
}