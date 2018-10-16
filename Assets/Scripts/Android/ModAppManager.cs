using UnityEngine;

namespace Android
{
    /// <summary>
    /// This class calls the Android native library that handles app mod detection, installation and deinstallation.
    /// </summary>
    public class ModAppManager: AndroidPluginInteraction
    {
        public ModAppManager() : base("com.hsd.wth.androidunitybridge.ModAppManager") { }

        /// <summary>
        /// This method searches and installs new mods and removes uninstalled ones.
        /// </summary>
        public void RefreshMods()
        {
            Debug.Log("Refreshing Mods...");
            customClass.Call("FindModApps");
            customClass.Call("RemoveUninstalledMods");
            customClass.Call("GetMods");
        }
    }
}