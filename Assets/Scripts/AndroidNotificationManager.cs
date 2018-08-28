﻿#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidNotificationManager : MonoBehaviour {

	AndroidJavaClass unityClass;
	AndroidJavaObject unityActivity;
	AndroidJavaObject customClass;
	AndroidJavaObject notificationProvider;

	void Start() {
		//Replace with your full package name
		sendActivityReference("com.hsd.wth.notificationprovider.NotificationProviderStarter");

		//Now, start service
		startService();

		notificationProvider = customClass.Get<AndroidJavaObject>("mService");
		notificationProvider.Call("createNotificationChannel");
		notificationProvider.Call("queueNotification", 5, "TestMessage");
	}

	void sendActivityReference(string packageName) {
		unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
		customClass = new AndroidJavaObject(packageName);
		customClass.Call("receiveActivityInstance", unityActivity);
	}

	void startService() {
		customClass.Call("StartNotificationService");
	}
}
#endif