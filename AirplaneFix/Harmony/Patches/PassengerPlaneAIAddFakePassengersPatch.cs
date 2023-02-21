using HarmonyLib;

namespace AirplaneFix.Harmony.Patches
{
    [HarmonyPatch(typeof(PassengerPlaneAI), "AddFakePassengers")]
    public static class PassengerPlaneAIAddFakePassengersPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(ref Vehicle data)
        {
            if (SaveGameSettings.GetSettings().PassengerPlaneNoFakePassengers)
            {
                return false;
            }

            return true;
        }
    }
}
