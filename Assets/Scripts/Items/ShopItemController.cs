using UnityEngine;
using UnityEngine.UI;
using UnityScript.Steps;

namespace Items
{
    public class ShopItemController : MonoBehaviour
    {
        public Text Name;
        public Text Description;
        public Image sprite;
        public Button button;
        public Text Price;
        public Text LevelText;
        public Text LevelValue;
        
        private Item item;

        public void SetItem(Item item)
        {
            this.item = item;

            Name.text = item.Definition.Name;
            Description.text = item.Definition.Description;
            sprite.sprite = item.Definition.PreviewSprite;
            Price.text = item.GetPrice().ToString();
            LevelValue.text = item.Level.ToString();
        }
    }
}