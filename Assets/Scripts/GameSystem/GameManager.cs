using SaveGame;
using UnityEngine;

namespace GameSystem
{
	/// <summary>
	/// This class is responsible for the main game management tasks.
	/// </summary>
	public class GameManager : MonoBehaviour
	{
		public GameObject managers;
	
		// Use this for initialization
		public void Awake () {
		
		}

		public void Start()
		{
		
		}

		void OnApplicationPause(bool isPaused)
		{
			if (isPaused)
				SaveGameSystem.SaveGame(SaveGameSystem.DEFAULT_SAVE_GAME_NAME);
		}
	}
}
