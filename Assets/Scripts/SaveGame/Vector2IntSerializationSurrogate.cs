using System.Runtime.Serialization;
using UnityEngine;

namespace SaveGame
{
    /// <summary>
    /// This surrogate handles <see cref="Vector2"/> serialization.
    /// </summary>
    sealed class Vector2IntSerializationSurrogate: ISerializationSurrogate 
    {
        /// <summary>
        /// Handle serialization.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var vector2 = (Vector2Int) obj;
            info.AddValue("x", vector2.x);
            info.AddValue("y", vector2.y);
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
            var vector2 = (Vector2Int) obj;
            vector2.x = (int)info.GetValue("x", typeof(int));
            vector2.y = (int)info.GetValue("y", typeof(int));
            return vector2;
        }
    }
}