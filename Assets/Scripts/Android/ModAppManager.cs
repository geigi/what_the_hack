namespace Android
{
    public class ModAppManager: AndroidPluginInteraction
    {
        public ModAppManager() : base("com.hsd.wth.androidunitybridge.ModAppManager") { }

        public void CopyMods()
        {
            customClass.Call("FindModApps");
            customClass.Call("RemoveUninstalledMods");
            customClass.Call("GetMods");
        }
    }
}