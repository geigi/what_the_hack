package com.hsd.wth.androidunitybridge;

import android.app.Notification;
import android.app.NotificationManager;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

public class AlarmReceiver extends BroadcastReceiver {

    public static String NOTIFICATION_ID = "notification-id";
    public static String NOTIFICATION = "notification";


    /**
     * Get's called when a notification alarm is fired.
     * Fires the notification.
     * @param context
     * @param intent
     */
    @Override
    public void onReceive(Context context, Intent intent) {
        NotificationManager notificationManager = (NotificationManager)context.getSystemService(Context.NOTIFICATION_SERVICE);

        Notification notification = intent.getParcelableExtra(NOTIFICATION);
        int id = intent.getIntExtra(NOTIFICATION_ID, 0);
        notificationManager.notify(id, notification);
    }
}
