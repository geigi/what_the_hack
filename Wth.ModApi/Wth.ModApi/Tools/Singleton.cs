using System;

namespace Wth.ModApi.Tools
{
    public sealed class Singleton
    {
        private static readonly Lazy<Singleton> lazy = new Lazy<Singleton>(() => new Singleton());
    
        public static Singleton Instance { get { return lazy.Value; } }

        private Singleton()
        {
        }
    }
}