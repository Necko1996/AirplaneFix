using HarmonyLib;

namespace AirplaneFix.Harmony.Patches
{
    [HarmonyPatch(typeof(PassengerPlaneAI), "DummyLoadPassengers")]
    public static class PassengerPlaneAIDummyLoadPassengersPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(ushort vehicleID, ref Vehicle data, ushort currentStop)
        {
            if (SaveGameSettings.GetSettings().PassengerPlaneNoDummyPassengers)
            {
                return false;
            }

            return true;
        }
    }
}
