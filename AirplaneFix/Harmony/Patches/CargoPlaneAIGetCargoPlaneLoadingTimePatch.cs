using HarmonyLib;

namespace AirplaneFix.Harmony.Patches
{
    [HarmonyPatch(typeof(CargoPlaneAI), "GetCargoPlaneLoadingTime")]
    public static class CargoPlaneAIGetCargoPlaneLoadingTimePatch
    {
        [HarmonyPrefix]
        public static bool Prefix(ushort building, ref int __result)
        {
            if (SaveGameSettings.GetSettings().CargoPlaneLoadingSetting)
            {
                __result = SaveGameSettings.GetSettings().CargoPlaneLoadingTimeSetting;

                // Returning false, we skip vanilla function
                return false;
            }

            return true;
        }
    }
}
