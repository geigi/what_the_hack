using System;
using UnityEngine;
using Wth.ModApi.Items;

namespace Items
{
    /// <summary>
    /// This class represents a saveable item instance.
    /// </summary>
    [Serializable]
    public abstract class Item
    {
        public int Level;
        public Vector2 Position;
        public ItemDefinition Definition;

        public Item(ItemDefinition definition)
        {
            Level = 0;
            Definition = definition;
        }
        
        public abstract int GetPrice();

        public int BuyUpgrade()
        {
            if (Definition.Upgradable)
            {
                Level += 1;
                return Level;
            }
            else if (Level < 1)
            {
                Level = 1;
                return Level;
            }
            return Level;
        }
    }
}