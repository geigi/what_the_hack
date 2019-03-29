using System;
using System.Collections.Generic;
using System.Linq;
using Android.Notifications;
using Assets.Scripts.NotificationSystem;
using Employees;
using Missions;
using NotificationSamples;
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
		public GameNotificationsManager NotificationsManager;
		
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

			if (SettingsManager.GetGameTime() == SettingsManager.GameTimeMode.Realtime)
			{
				cancelNotifications();
			}
			
			var channel = new GameNotificationChannel("1", "Default Game Channel", "Generic notifications");
			NotificationsManager.Initialize(channel);
		}

		private static void cancelNotifications()
		{
#if UNITY_ANDROID
#endif
		}

		void OnApplicationPause(bool isPaused)
		{
			if (isPaused)
			{
				ContentHub.Instance.SaveGameSystem.SaveGame(SaveGameSystem.DEFAULT_SAVE_GAME_NAME);
				
				if (SettingsManager.GetGameTime() == SettingsManager.GameTimeMode.Realtime)
					CreateRealtimeModeNotifications();
			}
			else
			{
				if (SettingsManager.GetGameTime() == SettingsManager.GameTimeMode.Realtime)
					cancelNotifications();
			}
		}

		/// <summary>
		/// Create notifications for the host system in realtime game mode.
		/// </summary>
		private void CreateRealtimeModeNotifications()
		{
			List<HostNotification> notifications = MissionManager.Instance.GenerateHostNotifications();
			int remainingTicksPayday = GameTime.GameTime.Instance.RemainingTicksTillPayday();
			notifications.Append(new HostNotification("Payday arrives soon!", "",
				(int) Math.Max(remainingTicksPayday * GameTime.GameTime.Instance.RealtimeMinutesPerTick, 60)));
			if (EmployeeManager.Instance.AllEmployeesIdle())
			{
				notifications.Append(new HostNotification("Your employees don't have anything to do.", "",
					120));
			}

			foreach (var notification in notifications)
			{
				IGameNotification n = NotificationsManager.CreateNotification();
				n.Title = notification.Title;
				n.Body = notification.Body;
				DateTime date = DateTime.Now;
				n.DeliveryTime = date.Add(new TimeSpan(0, notification.Delay, 0));
				NotificationsManager.ScheduleNotification(n);
			}
		}
	}
}
