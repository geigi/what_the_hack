using System.Collections.Generic;
using UnityEngine;
using UE.Common;
using Wth.ModApi.Tools;

namespace Extensions
{
    public static class CommonExtensions
    {
        /// <summary>
        /// Return a random element of this list.
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T RandomElement<T>(List<T> list)
        {
            return list[RandomUtils.RollDice(list.Count) - 1];
        }
        
        /// <summary>
        /// Convert a Vector3Int to a Vector2Int by dropping the z value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector2Int ToVector2Int(this Vector3Int value)
        {
            return new Vector2Int(value.x, value.y);
        }
        
        /// <summary>
        /// Convert a Vector3 to a Vector2Int by dropping the z value and casting to int.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector2Int ToVector2Int(this Vector3 value)
        {
            return new Vector2Int((int)value.x, (int)value.y);
        }
        
        /// <summary>
        /// Convert a Vector2Int to a Vector2.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector2 ToVector2(this Vector2Int value)
        {
            return new Vector2(value.x, value.y);
        }
        
        /// <summary>
        /// Convert a Vector2 to a Vector3 by setting z = 0f.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3 ToVector3(this Vector2 value)
        {
            return new Vector3(value.x, value.y, 0f);
        }

        public static RectTransform ResetPosition(this RectTransform rect)
        {
            rect.SetLeft(0.0f);
            rect.SetRight(0.0f);
            rect.localPosition = Vector3.zero;
            rect.SetLeft(0.0f);
            return rect;
        }
        
        public static bool IsTrueNull(this UnityEngine.Object obj)
        {
            // ReSharper disable once RedundantCast.0
            return (object)obj == null;
        }
    }
}