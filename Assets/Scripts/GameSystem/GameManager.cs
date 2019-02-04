using SaveGame;
using UnityEngine;

namespace GameSystem
{
	/// <summary>
	/// This class is responsible for the main game management tasks.
	/// </summary>
	public class GameManager : MonoBehaviour
	{
#if UNITY_EDITOR
		public bool LoadGame = false;
		
		
#endif
		public GameObject managers;
	
#if UNITY_EDITOR
		// Use this for initialization
		public void Awake ()
		{
			if (LoadGame)
				GameSettings.NewGame = false;
		}
#endif

		public void Start()
		{
		
		}

		void OnApplicationPause(bool isPaused)
		{
			if (isPaused)
				ContentHub.Instance.SaveGameSystem.SaveGame(SaveGameSystem.DEFAULT_SAVE_GAME_NAME);
		}
	}
}
