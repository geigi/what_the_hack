using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Wth.ModApi.Editor
{
    /// <summary>
    /// Class for representing a KeyFrame in an Animation.
    /// </summary>
    public class KeyFrame
    {
        /// <summary>
        /// The keyFrame.
        /// </summary>
        public ObjectReferenceKeyframe frame;
        /// <summary>
        /// An, possible null, event of this frame
        /// </summary>
        public AnimationEvent evt;
        /// <summary>
        /// How long the sprite should be displayed.
        /// </summary>
        public float showingTime;

        public KeyFrame(ObjectReferenceKeyframe _frame, float showingTime, AnimationEvent _evt = null)
        {
            this.frame = _frame;
            this.showingTime = showingTime; 
            this.evt = _evt;
        }

        /// <summary>
        /// Gets the Animation Event or creates a new one, if it does not exist.
        /// </summary>
        /// <returns>The Animation Event</returns>
        public AnimationEvent getEventOrNew() => evt ?? (evt = new AnimationEvent());

        /// <summary>
        /// Returns an Array of only the frames of a specific List.
        /// </summary>
        /// <param name="list">The List of KeyFrames</param>
        /// <returns>An Array representation of the specified List</returns>
        public static ObjectReferenceKeyframe[] KeyFramesToArray(List<KeyFrame> list)
        {
            return list.ConvertAll((keyFrame) => keyFrame.frame)
                .ToArray();
        }

        /// <summary>
        /// Returns an Array of only the Events of a specific List.
        /// </summary>
        /// <param name="list">The List of KeyFrames</param>
        /// <returns>An Array representation of the specific list</returns>
        public static AnimationEvent[] EventsToArray(List<KeyFrame> list)
        {
            return list.FindAll((keyFrame) => keyFrame.evt != null)
                .ConvertAll((keyFrame) => keyFrame.evt)
                .ToArray();
        }
    }
}
