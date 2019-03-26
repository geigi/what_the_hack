using SaveGame;
using UE.StateMachine;
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
		public GameObject TutorialPrefab;
		public State TutorialState;
		
		private void Start()
		{
			if (SettingsManager.GetTutorialState() && (GameSettings.NewGame) || (!GameSettings.NewGame && SaveGameSystem.Instance.GetCurrentSaveGame().TutorialStage > 0))
			{
				Instantiate(TutorialPrefab);
                TutorialState.Enter();
			}
			else
			{
				GameTime.GameTime.Instance.StartGame();
			}
		}

		void OnApplicationPause(bool isPaused)
		{
			if (isPaused)
				ContentHub.Instance.SaveGameSystem.SaveGame(SaveGameSystem.DEFAULT_SAVE_GAME_NAME);
		}
	}
}
