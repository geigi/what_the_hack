package com.hsd.wth.androidunitybridge;

import android.app.Activity;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.pm.ProviderInfo;
import android.content.res.AssetManager;
import android.content.res.Resources;
import android.util.Log;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.util.ArrayList;
import java.util.List;

/**
 * This class searches and installs Mods for installed Addon-Apps that provide Mods for the base game.
 * Install means that the Mod data is copied from the apps assets folder to the external storage.
 * After extensive research this seems to be the only possible solution with Unity and the mod system.
 */
public final class ModAppManager {
    List<ProviderInfo> validProviders;
    Activity myActivity;
    String TAG = "de.hsd.wth";
    String EXT_STORAGE;

    /**
     * Search for compatible apps and save them in a list.
     */
    public void FindModApps() {
        Log.d(TAG, "Start Mod App search...");
        validProviders = new ArrayList<>();

        for (PackageInfo pack : myActivity.getPackageManager().getInstalledPackages(PackageManager.GET_PROVIDERS)) {
            ProviderInfo[] providers = pack.providers;
            if (providers != null) {
                for (ProviderInfo provider : providers) {
                    if (provider.name.contains("WTHModProvider")) {
                        Log.d("de.hsd.wth", "Found Mod App: " + provider.name);
                        validProviders.add(provider);
                    }
                }
            }
        }
    }

    /**
     * Searches the mod folder for mods that belong to an addon app.
     * This is done by testing whether a version.txt exists to verify that the mod was not installed manually.
     * If the file exists the list of providers is searched for the mod folder.
     */
    public void RemoveUninstalledMods() {
        Log.d(TAG, "Scanning for uninstalled mod apps...");
        String modPath = EXT_STORAGE + "/Mods/";
        File modDir = new File(modPath);
        File[] modDirs = modDir.listFiles();

        for (File dir: modDirs) {
            Log.d(TAG, "Found installed mod: " + dir.getAbsolutePath());
            if (dir.isDirectory()) {
                File versionFile = new File(dir, "version.txt");
                Log.d(TAG, "Testing for version file: " + versionFile.getAbsolutePath());
                if (versionFile.exists()) {
                    Log.d(TAG, "Testing installed apps for name: " + dir.getName());
                    if (!containsId(dir.getName())) {
                        RemoveMod(dir);
                    }
                }
            }
        }
    }

    /**
     * For each valid addon app test if its installed, if the same version is installed and install it on demand.
     */
    public void GetMods() {
        for (ProviderInfo provider: validProviders) {
            try {
                String modPath = EXT_STORAGE + "/Mods/" + provider.packageName;
                File packageModDir = new File(modPath);

                if (packageModDir.isDirectory()) {
                    // Mod was installed before. Test if mod needs to be updated.
                    Log.d(TAG, "Existing Mod found: " + provider.packageName);

                    File version = new File(packageModDir, "version.txt");
                    if (version.exists()) {
                        Version installedVersion = ReadVersion(version);
                        Version appVersion = new Version(myActivity.getPackageManager().getPackageInfo(provider.applicationInfo.packageName, 0).versionName);

                        if (installedVersion.compareTo(appVersion) != 0) {
                            Log.d(TAG, "Mod is not same version, reinstall: " + provider.packageName);
                            // Installed version is older or newer. Clean install.
                            RemoveMod(packageModDir);
                            InstallMod(provider, packageModDir);
                        }
                    }
                    else {
                        Log.d(TAG, "Mod has no version.txt, reinstall: " + provider.packageName);
                        RemoveMod(packageModDir);
                        InstallMod(provider, packageModDir);
                    }
                }
                else {
                    Log.d(TAG, "New Mod found: " + provider.packageName);
                    InstallMod(provider, packageModDir);
                }

                Resources res = myActivity.getPackageManager().getResourcesForApplication(provider.applicationInfo);
                AssetManager a = res.getAssets();

                PackageInfo info = myActivity.getPackageManager().getPackageInfo(provider.applicationInfo.packageName, 0);

                Log.d(TAG, String.valueOf(a.list("/").length));
            } catch (PackageManager.NameNotFoundException e) {
                Log.d(TAG, "Failed to install mod: " + provider.packageName);
                e.printStackTrace();
            } catch (IOException e) {
                Log.d(TAG, "Failed to install mod: " + provider.packageName);
                e.printStackTrace();
            }
        }
    }

    /**
     * Called From C# to get the Activity Instance
     */
    public void receiveActivityInstance(Activity tempActivity) {
        myActivity = tempActivity;
        EXT_STORAGE = myActivity.getExternalFilesDir(null).getAbsolutePath();
    }

    /**
     * Install all mods from a given provider to external storage.
     * Partly from https://stackoverflow.com/questions/4447477/how-to-copy-files-from-assets-folder-to-sdcard
     * @param info ProviderInfo of app providing the mod(s).
     * @param destination Destination directory.
     * @return
     */
    private Boolean InstallMod(ProviderInfo info, File destination) {
        AssetManager assetManager = null;
        try {
            assetManager = myActivity.getPackageManager().getResourcesForApplication(info.applicationInfo).getAssets();
        } catch (PackageManager.NameNotFoundException e) {
            e.printStackTrace();
        }

        copyFileOrDir("", EXT_STORAGE + "/Mods/" + info.packageName + "/", assetManager);

        try {
            String version = myActivity.getPackageManager().getPackageInfo(info.applicationInfo.packageName, 0).versionName;
            FileWriter writer = new FileWriter(new File(destination, "version.txt"));
            writer.append(version);
            writer.flush();
            writer.close();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (PackageManager.NameNotFoundException e) {
            e.printStackTrace();
        }

        return true;
    }

    /**
     * Remove a mod completely by recursive dir removal.
     * @param dir
     * @return
     */
    private Boolean RemoveMod(File dir) {
        Log.d(TAG, "Removing mod: " + dir.getAbsolutePath());
        deleteRecursive(dir);
        return true;
    }

    /**
     * Read a version string from a text file.
     * @param file File which contains version.
     * @return
     */
    private Version ReadVersion(File file) {
        //Read text from file
        StringBuilder text = new StringBuilder();

        try {
            BufferedReader br = new BufferedReader(new FileReader(file));
            String line;

            line = br.readLine();
            br.close();

            return new Version(line);
        }
        catch (IOException e) {
            //You'll need to add proper error handling here
            return null;
        }
    }

    /**
     * Copy a directory or file from assets to external storage. Creates folders that are not existent.
     * Partly from https://stackoverflow.com/questions/4447477/how-to-copy-files-from-assets-folder-to-sdcard
     * @param path Path to copy
     * @param destination Destination folder
     * @param assetManager AssetManager which contains the files to copy.
     */
    private void copyFileOrDir(String path, String destination, AssetManager assetManager) {
        String assets[] = null;
        try {
            Log.i(TAG, "copyFileOrDir() "+path);
            assets = assetManager.list(path);
            if (assets.length == 0) {
                copyFile(path, destination, assetManager);
            } else {
                String fullPath = destination;
                Log.i(TAG, "path=" + fullPath);
                File dir = new File(fullPath);
                if (!dir.exists() && !path.startsWith("images") && !path.startsWith("sounds") && !path.startsWith("webkit"))
                    if (!dir.mkdirs())
                        Log.i(TAG, "could not create dir "+fullPath);
                for (int i = 0; i < assets.length; ++i) {
                    String p;
                    if (path.equals(""))
                        p = "";
                    else
                        p = path + "/";

                    if (!path.startsWith("images") && !path.startsWith("sounds") && !path.startsWith("webkit"))
                        copyFileOrDir( p + assets[i], destination, assetManager);
                }
            }
        } catch (IOException ex) {
            Log.e(TAG, "I/O Exception", ex);
        }
    }

    /**
     * Copy a file from assets to external storage.
     * @param filename File to copy
     * @param destination Destination dir
     * @param assetManager AssetManager containing the asset
     * @throws IOException
     */
    private void copyFile(String filename, String destination, AssetManager assetManager) throws IOException {
        InputStream in = null;
        OutputStream out = null;
        String newFileName = null;
        try {
            Log.i(TAG, "copyFile() "+filename);
            in = assetManager.open(filename);
            newFileName = destination + filename;
            File newFile = new File(newFileName);
            File dir = newFile.getParentFile();

            if (!dir.exists())
                if(!dir.mkdirs())
                    Log.i(TAG, "could not create dir " + dir.getAbsolutePath());

            out = new FileOutputStream(newFileName);

            byte[] buffer = new byte[1024];
            int read;
            while ((read = in.read(buffer)) != -1) {
                out.write(buffer, 0, read);
            }
            in.close();
            in = null;
            out.flush();
            out.close();
            out = null;
        } catch (Exception e) {
            Log.e(TAG, "Exception in copyFile() of "+newFileName);
            Log.e(TAG, "Exception in copyFile() "+e.toString());
        }
    }

    /**
     * Delete a complete directory.
     * From https://stackoverflow.com/questions/4943629/how-to-delete-a-whole-folder-and-content
     * @param fileOrDirectory
     */
    private void deleteRecursive(File fileOrDirectory) {
        if (fileOrDirectory.isDirectory())
            for (File child : fileOrDirectory.listFiles())
                deleteRecursive(child);

        fileOrDirectory.delete();
    }

    private boolean containsId(String name) {
        for (ProviderInfo provider: validProviders) {
            if (provider.packageName.equals(name)) {
                return true;
            }
        }
        return false;
    }
}
