using UnityEngine;

namespace Wth.ModApi.Items
{
    /// <summary>
    /// This class is an abstract representation of an item.
    /// Create a child class to define a new kind of item.
    /// </summary>
    public abstract class ItemDefinition: ScriptableObject
    {
        public string Name = "";
        public Sprite PreviewSprite;

        public abstract int GetPrice();
    }
}