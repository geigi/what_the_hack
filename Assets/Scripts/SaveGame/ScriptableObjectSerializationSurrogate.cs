using System.Runtime.Serialization;
using UnityEngine;
using Wth.ModApi.Tools;

namespace SaveGame
{
    /// <summary>
    /// This surrogate handles <see cref="ScriptableObject"/> serialization.
    /// It'll use a lookup dictionary to find keys to each ScriptableObject instance.
    /// </summary>
    sealed class ScriptableObjectSerializationSurrogate: ISerializationSurrogate 
    {
        /// <summary>
        /// Handle serialization.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var so = (ScriptableObject) obj;
            info.AddValue("key", ScriptableObjectManager.Instance.GetKey(so));
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
            return ScriptableObjectManager.Instance.GetObject((string)info.GetValue("key", typeof(string)));
        }
    }
}