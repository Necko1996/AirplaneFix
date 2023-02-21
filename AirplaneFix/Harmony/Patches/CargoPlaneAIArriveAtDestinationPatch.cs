using ColossalFramework;
using HarmonyLib;
using UnityEngine;
using System;

namespace AirplaneFix.Harmony.Patches
{
    [HarmonyPatch(typeof(CargoPlaneAI), "ArriveAtDestination")]
    public static class CargoPlaneAIArriveAtDestinationPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(ushort vehicleID, ref Vehicle vehicleData, ref bool __result, CargoPlaneAI __instance)
        {
            if (SaveGameSettings.GetSettings().NoReturnOnImportExportSetting)
            {
                if ((vehicleData.m_flags & Vehicle.Flags.WaitingTarget) != (Vehicle.Flags)0)
                {
                    __result = false;
                }
                if ((vehicleData.m_flags & Vehicle.Flags.WaitingLoading) != (Vehicle.Flags)0)
                {
                    vehicleData.m_waitCounter = (byte)Mathf.Min((int)(vehicleData.m_waitCounter + 1), 255);
                    if ((int)vehicleData.m_waitCounter < GetCargoPlaneLoadingTime(vehicleData.m_targetBuilding))
                    {
                        __result = false;
                    }
                    if (vehicleData.m_targetBuilding != 0 && (Singleton<BuildingManager>.instance.m_buildings.m_buffer[(int)vehicleData.m_targetBuilding].m_flags & Building.Flags.IncomingOutgoing) == Building.Flags.None)
                    {
                        ushort num = CargoTruckAI.FindNextCargoParent(vehicleData.m_targetBuilding, __instance.m_info.m_class.m_service, __instance.m_info.m_class.m_subService);
                        if (num != 0)
                        {
                            ushort targetBuilding = Singleton<VehicleManager>.instance.m_vehicles.m_buffer[(int)num].m_targetBuilding;
                            if (targetBuilding != 0)
                            {
                                CargoTruckAI.SwitchCargoParent(num, vehicleID);
                                vehicleData.m_waitCounter = 0;
                                vehicleData.m_flags &= ~Vehicle.Flags.WaitingLoading;
                                __instance.SetTarget(vehicleID, ref vehicleData, targetBuilding);
                                __result = (vehicleData.m_flags & Vehicle.Flags.Spawned) == (Vehicle.Flags)0;
                            }
                        }
                    }

                    Singleton<VehicleManager>.instance.ReleaseVehicle(vehicleID);
                    __result = true;

                    return false;
                }
                else
                {
                    return true;
                }
            }

            return true;
        }

        public static int GetCargoPlaneLoadingTime(ushort building)
        {
            if (SaveGameSettings.GetSettings().CargoPlaneLoadingSetting)
            {
                return SaveGameSettings.GetSettings().CargoPlaneLoadingTimeSetting;
            }

            BuildingAI buildingAI = Singleton<BuildingManager>.instance.m_buildings.m_buffer[(int)building].Info.m_buildingAI;
            CargoStationAI cargoStationAI = buildingAI as CargoStationAI;

            if (cargoStationAI != null)
            {
                return cargoStationAI.m_cargoPlaneLoadingTime;
            }

            return 52;
        }
    }
}
