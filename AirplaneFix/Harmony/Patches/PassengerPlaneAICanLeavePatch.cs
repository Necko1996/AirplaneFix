using HarmonyLib;

namespace AirplaneFix.Harmony.Patches
{
    [HarmonyAfter(new string[] { "com.vectorial1024.cities.ptu.Patch_PassengerPlaneAI_AntiRogue" })]
    [HarmonyPatch(typeof(PassengerPlaneAI), "CanLeave")]
    public static class PassengerPlaneAICanLeavePatch
    {
        private static VehicleAI s_vehicleAI = new VehicleAI();

        [HarmonyPrefix]
        public static bool Prefix(ushort vehicleID, ref Vehicle vehicleData, ref bool __result)
        {
            if (SaveGameSettings.GetSettings().PassengerPlaneLoadingSetting)
            {
                __result = vehicleData.m_waitCounter >= SaveGameSettings.GetSettings().PassengerPlaneLoadingTimeSetting && s_vehicleAI.CanLeave(vehicleID, ref vehicleData);

                // Returning false, we skip vanilla function
                return false;
            }

            return true;
        }
    }
}
