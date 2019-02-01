using System.Runtime.Serialization;
using UnityEngine;

namespace SaveGame
{
    /// <summary>
    /// This surrogate handles <see cref="Vector2"/> serialization.
    /// </summary>
    sealed class ColorSerializationSurrogate: ISerializationSurrogate 
    {
        /// <summary>
        /// Handle serialization.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var color = (Color32) obj;
            info.AddValue("r", color.r);
            info.AddValue("g", color.g);
            info.AddValue("b", color.b);
            info.AddValue("a", color.a);
        }

        /// <summary>
        /// Handle deserialization.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            var color = (Color32) obj;
            color.r = (byte) info.GetValue("r", typeof(byte));
            color.g = (byte)info.GetValue("g", typeof(byte));
            color.b = (byte)info.GetValue("b", typeof(byte));
            color.a = (byte)info.GetValue("a", typeof(byte));
            return null;
        }
    }
}