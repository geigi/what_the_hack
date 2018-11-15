using System;

namespace Wth.ModApi.Tools
{
    /// <summary>
    /// This class is currently not used but can be used as a template for singletons.
    /// </summary>
    public sealed class Singleton
    {
        /// <summary>
        /// The lazy instance.
        /// </summary>
        private static readonly Lazy<Singleton> lazy = new Lazy<Singleton>(() => new Singleton());
    
        /// <summary>
        /// The instance.
        /// </summary>
        public static Singleton Instance { get { return lazy.Value; } }

        /// <summary>
        /// Private constructor.
        /// </summary>
        private Singleton()
        {
        }
    }
}