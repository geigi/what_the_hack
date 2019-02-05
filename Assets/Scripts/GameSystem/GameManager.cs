using SaveGame;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameSystem
{
	/// <summary>
	/// This class is responsible for the main game management tasks.
	/// </summary>
	public class GameManager : MonoBehaviour
	{
		public GameObject managers;
	
#if UNITY_EDITOR
		// Use this for initialization
		public void Awake ()
		{
			if (EditorPrefs.GetBool("DEBUG_LOAD_GAME"))
				GameSettings.NewGame = false;
		}
#endif

		void OnApplicationPause(bool isPaused)
		{
			if (isPaused)
				ContentHub.Instance.SaveGameSystem.SaveGame(SaveGameSystem.DEFAULT_SAVE_GAME_NAME);
		}
	}
}
