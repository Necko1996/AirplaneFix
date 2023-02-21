using HarmonyLib;
using UnityEngine;
using ColossalFramework;

namespace AirplaneFix.Harmony.Patches
{
    [HarmonyPatch(typeof(PassengerPlaneAI), "ArriveAtDestination")]
    public static class PassengerPlaneAIArriveAtDestinationPatch
    {
        public static bool Prefix(ushort vehicleID, ref Vehicle vehicleData, ref bool __result)
        {
            if (SaveGameSettings.GetSettings().PassengerPlaneLoadingSetting)
            {
                if ((vehicleData.m_flags & Vehicle.Flags.GoingBack) != (Vehicle.Flags)0)
                {
                    return true;
                }
                if ((vehicleData.m_flags & Vehicle.Flags.WaitingLoading) == (Vehicle.Flags)0)
                {
                    return true;
                }

                vehicleData.m_waitCounter = (byte)Mathf.Min((int)(vehicleData.m_waitCounter + 1), 255);

                if (vehicleData.m_waitCounter >= SaveGameSettings.GetSettings().PassengerPlaneLoadingTimeSetting)
                {
                    Singleton<VehicleManager>.instance.ReleaseVehicle(vehicleID);
                    __result = true;
                    return false;
                }

                __result = false;

                return false;
            }

            return true;
        }
    }
}
