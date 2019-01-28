using System;
using UnityEngine;
using Wth.ModApi.Items;

namespace Items
{
    /// <summary>
    /// This class represents a saveable item instance.
    /// </summary>
    [Serializable]
    public class Item
    {
        public int Level;
        public Vector2 Position;
        public ItemDefinition Definition;
    }
}