using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppPause : MonoBehaviour { 

	// Use this for initialization
	void Start () {
		
	}

#if UNITY_ANDROID
public AndroidNotificationManager notificationManager;
	void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) {
			notificationManager.queueNotification(15, "Test Message");
		}
		else {
			notificationManager.cancelNotifications();
		}
    }
#endif
}
