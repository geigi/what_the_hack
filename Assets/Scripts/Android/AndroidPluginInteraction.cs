using UnityEngine;

namespace Android
{
    public abstract class AndroidPluginInteraction
    {
        internal AndroidJavaClass unityClass;
        internal AndroidJavaObject unityActivity;
        internal AndroidJavaObject customClass;
        internal AndroidJavaObject notificationProvider;
        private bool requiresContext = false;

        public AndroidPluginInteraction(string packageName, bool requiresContext = false) {
            //Replace with your full package name
            sendActivityReference(packageName);
            this.requiresContext = requiresContext;
        }

        internal void sendActivityReference(string packageName) {
            unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            customClass = new AndroidJavaObject(packageName);
            if (requiresContext)
                customClass = new AndroidJavaObject(packageName, unityActivity);
            else
                customClass = new AndroidJavaObject(packageName);
            customClass.Call("receiveActivityInstance", unityActivity);
            Debug.Log(unityClass);
            Debug.Log(unityActivity);
            Debug.Log(customClass);
        }
    }
}