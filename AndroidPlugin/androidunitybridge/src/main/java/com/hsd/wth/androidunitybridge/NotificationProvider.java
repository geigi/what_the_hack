package com.hsd.wth.androidunitybridge;

import android.app.AlarmManager;
import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.Service;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.Binder;
import android.os.Build;
import android.os.IBinder;

public class NotificationProvider extends Service {
    // Binder given to clients
    private final IBinder mBinder = new LocalBinder();

    /**
     * Class used for the client Binder.  Because we know this service always
     * runs in the same process as its clients, we don't need to deal with IPC.
     */
    public class LocalBinder extends Binder {
        NotificationProvider getService() {
            // Return this instance of LocalService so clients can call public methods
            return NotificationProvider.this;
        }
    }

    public NotificationProvider() {
    }

    /**
     * Creates the main notification channel if OS supports it.
     */
    public void createNotificationChannel() {
        // Create the NotificationChannel, but only on API 26+ because
        // the NotificationChannel class is new and not in the support library
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            CharSequence name = "General";
            String description = "Game progress notifications";
            int importance = NotificationManager.IMPORTANCE_DEFAULT;
            NotificationChannel channel = new NotificationChannel("0", name, importance);
            channel.setDescription(description);
            // Register the channel with the system; you can't change the importance
            // or other notification behaviors after this
            NotificationManager notificationManager = getSystemService(NotificationManager.class);
            notificationManager.createNotificationChannel(channel);
        }
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        return Service.START_STICKY;
    }

    @Override
    public IBinder onBind(Intent intent) {
        return mBinder;
    }

    /**
     * Queue a notification in Android.
     * This notification will display even if the app isn't running.
     * @param delay Delay when the notification should be fired in seconds.
     * @param message Message to display in the notification.
     */
    public void queueNotification(int delay, String message) {
        AlarmManager alarmManager = (AlarmManager) this.getSystemService(Context.ALARM_SERVICE);
        Intent notificationIntent = new Intent(this, AlarmReceiver.class);
        notificationIntent.putExtra(AlarmReceiver.NOTIFICATION_ID, 1);
        notificationIntent.putExtra(AlarmReceiver.NOTIFICATION, createNotification(message));
        PendingIntent alarmIntent = PendingIntent.getBroadcast(this, 0, notificationIntent, PendingIntent.FLAG_UPDATE_CURRENT);

        // Set the alarm
        alarmManager.set(AlarmManager.ELAPSED_REALTIME_WAKEUP,
                delay * 1000,
                alarmIntent);
    }

    /**
     * Cancel a given alarm.
     */
    private void cancelAlarms()
    {
        AlarmManager alarmManager = (AlarmManager) this.getSystemService(Context.ALARM_SERVICE);
        PendingIntent alarmIntent = PendingIntent.getBroadcast(this, 0, new Intent(this, AlarmReceiver.class), 0);
        alarmManager.cancel(alarmIntent);
    }

    /**
     * Enable alarms (notifications) and make them persistend across reboots.
     * @param context
     */
    public void enablePersistentAlarms(Context context) {
        // Restart alarm if device is rebooted
        // This overrides the manifest setting
        ComponentName receiver = new ComponentName(context, BootFinishedReceiver.class);
        PackageManager pm = context.getPackageManager();

        pm.setComponentEnabledSetting(receiver,
                PackageManager.COMPONENT_ENABLED_STATE_ENABLED,
                PackageManager.DONT_KILL_APP);
    }

    /**
     * Disable all notifications.
     * @param context
     */
    public void disablePersistentAlarms(Context context) {
        // Disable BootReceiver so that alarm won't start again if device is rebooted
        ComponentName receiver = new ComponentName(context, BootFinishedReceiver.class);
        PackageManager pm = context.getPackageManager();
        pm.setComponentEnabledSetting(receiver,
                PackageManager.COMPONENT_ENABLED_STATE_DISABLED,
                PackageManager.DONT_KILL_APP);
    }

    /**
     * Create a notification object.
     * @param content Message to display in the notification.
     * @return
     */
    private Notification createNotification(String content) {
        Notification.Builder builder = new Notification.Builder(this);
        builder.setContentTitle("Help your employees hacking");
        builder.setContentText(content);
        builder.setSmallIcon(R.drawable.notification_icon);
        builder.setPriority(Notification.PRIORITY_MAX);

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            builder.setChannelId("0");
        }
        return builder.build();
    }
}
