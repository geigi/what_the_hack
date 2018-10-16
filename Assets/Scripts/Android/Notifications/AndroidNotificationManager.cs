//#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using Android;
using UnityEngine;

public class AndroidNotificationManager : AndroidPluginInteraction {
	public AndroidNotificationManager(): base("com.hsd.wth.androidunitybridge.NotificationProviderStarter") {}

	public void Start() {
		//Now, start service
		startService();

		notificationProvider = customClass.Get<AndroidJavaObject>("mService");
		notificationProvider.Call("createNotificationChannel");
	}

	public void queueNotification(int delay, string message) {
		notificationProvider.Call("queueNotification", delay, message);
	}

	public void cancelNotifications() {
		notificationProvider.Call("cancelAlarms");
	}

	void startService() {
		customClass.Call("StartNotificationService");
	}
}
//#endif