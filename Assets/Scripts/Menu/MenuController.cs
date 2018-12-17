using SaveGame;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MenuController : MonoBehaviour
    {
        public Button ResumeButton;
    
        // Start is called before the first frame update
        void Start()
        {
            if (SaveGameSystem.DoesSaveGameExist(SaveGameSystem.DEFAULT_SAVE_GAME_NAME))
                ResumeButton.gameObject.SetActive(true);
        }
    }
}
