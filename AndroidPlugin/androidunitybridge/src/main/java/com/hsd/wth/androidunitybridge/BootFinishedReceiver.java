package com.hsd.wth.androidunitybridge;

import android.app.Notification;
import android.app.NotificationManager;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.os.Build;

/**
 * This class handles a notification that will be displayed after a reboot.
 */
public class BootFinishedReceiver extends BroadcastReceiver {
    @Override
    public void onReceive(Context context, Intent intent) {
        if (intent.getAction().equals("android.intent.action.BOOT_COMPLETED")) {
            Notification.Builder builder = new Notification.Builder(context);
            builder.setContentTitle("Help your employees hacking");
            builder.setContentText("Check back into What the Hack, your employees might have done some progress.");
            builder.setSmallIcon(R.drawable.notification_icon);
            builder.setPriority(Notification.PRIORITY_MAX);

            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
                builder.setChannelId("0");
            }

            NotificationManager notificationManager = (NotificationManager)context.getSystemService(Context.NOTIFICATION_SERVICE);

            notificationManager.notify(0, builder.build());
        }
    }
}