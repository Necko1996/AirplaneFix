using ColossalFramework.Plugins;
using AirplaneFix.Log;
using System.Reflection;

namespace AirplaneFix.Harmony
{
    public class HarmonyHelper
    {
        public static bool IsHarmonyRunning()
        {
            // We look for either Harmony 2.2-0 steam ID or CitiesHarmony assembly name
            const string sPLUGIN_ID = "2040656402";
            return IsPluginRunningNotCached(sPLUGIN_ID, "CitiesHarmony");
        }

        public static bool IsPluginRunningNotCached(string sPluginId, string sAssemblyName)
        {
            bool bRunning = false;
            foreach (PluginManager.PluginInfo oPlugin in PluginManager.instance.GetPluginsInfo())
            {
                if (oPlugin.isEnabled)
                {
                    if (!string.IsNullOrEmpty(sPluginId))
                    {
                        if (oPlugin.name == sPluginId)
                        {
                            bRunning = true;
                            break;
                        };
                    }

                    if (!string.IsNullOrEmpty(sAssemblyName))
                    {
                        foreach (Assembly assembly in oPlugin.GetAssemblies())
                        {
                            if (assembly.GetName().Name.Contains(sAssemblyName))
                            {
                                bRunning = true;
                                break;
                            }
                        }
                    }
                }
            }

            return bRunning;
        }

        public static bool ApplyHarmonyPatches()
        {
            // Harmony
            if (IsHarmonyRunning())
            {
                Patcher.PatchAll();
            }
            else
            {
                string sMessage = "Mod Dependency Error:\r\n";
                sMessage += "\r\n";
                sMessage += "Harmony not found.\r\n";
                sMessage += "\r\n";
                sMessage += "Mod disabled until dependencies resolved, please subscribe to Harmony.";

                Prompt.ErrorFormat(AirplaneFixMain.ModName, sMessage);

                return false;
            }

            if (!IsHarmonyValid())
            {
                RemoveHarmonyPathes();

                string strMessage = "Harmony patching failed...\r\n";

                Prompt.ErrorFormat(AirplaneFixMain.ModName, strMessage);

                return false;
            }

            return true;
        }

        public static bool IsHarmonyValid()
        {
            if (IsHarmonyRunning())
            {
                int iHarmonyPatches = Patcher.GetPatchCount();

                Debug.Log("Harmony patches: " + iHarmonyPatches);

                return iHarmonyPatches == Patcher.GetHarmonyPatchCount();
            }

            return false;
        }

        public static void RemoveHarmonyPathes()
        {
            if (AirplaneFixLoader.IsLoaded() && IsHarmonyRunning())
            {
                Patcher.UnpatchAll();
            }
        }
    }
}
