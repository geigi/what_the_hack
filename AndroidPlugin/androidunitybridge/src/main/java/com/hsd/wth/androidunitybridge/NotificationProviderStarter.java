package com.hsd.wth.androidunitybridge;

import android.app.Activity;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.os.IBinder;

public final class NotificationProviderStarter {
    Activity myActivity;
    public NotificationProvider mService;
    NotificationProvider.LocalBinder binder;

    // Called From C# to get the Activity Instance
    public void receiveActivityInstance(Activity tempActivity) {
        myActivity = tempActivity;
        Intent intent = new Intent(myActivity, NotificationProvider.class);
        myActivity.bindService(intent, mConnection, Context.BIND_AUTO_CREATE);
    }

    /**
     * Start the notification Android service.
     */
    public void StartNotificationService() {
        myActivity.startService(new Intent(myActivity, NotificationProvider.class));
    }

    /** Defines callbacks for service binding, passed to bindService() */
    private ServiceConnection mConnection = new ServiceConnection() {

        @Override
        public void onServiceConnected(ComponentName className,
                                       IBinder service) {
            // We've bound to LocalService, cast the IBinder and get LocalService instance
            binder = (NotificationProvider.LocalBinder) service;
            mService = binder.getService();
        }

        @Override
        public void onServiceDisconnected(ComponentName arg0) {
        }
    };
}