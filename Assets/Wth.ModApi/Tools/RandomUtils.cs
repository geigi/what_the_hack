using UnityEngine;

namespace Wth.ModApi.Tools
{
    public static class RandomUtils
    {
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