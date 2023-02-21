using System;
using System.Collections.Generic;
using AirplaneFix.Log;
using Harmony2 = HarmonyLib.Harmony;
using AirplaneFix.Harmony.Patches;

namespace AirplaneFix.Harmony
{
    public static class Patcher
    {
        public const string HarmonyId = "Necko.AirplaneFix";

        private static bool s_patched = false;
        private static int s_iHarmonyPatches = 0;

        public static int GetHarmonyPatchCount()
        {
            return s_iHarmonyPatches;
        }

        public static void PatchAll()
        {
            if (!s_patched)
            {
                UnityEngine.Debug.Log(AirplaneFixMain.ModName + ": Patching...");

                s_patched = true;

                var harmony = new Harmony2(HarmonyId);

                List<Type> patchList = new List<Type>();

                patchList.Add(typeof(CargoPlaneAIGetCargoPlaneLoadingTimePatch));
                patchList.Add(typeof(CargoPlaneAIArriveAtDestinationPatch));

                patchList.Add(typeof(PassengerPlaneAIAddFakePassengersPatch));
                patchList.Add(typeof(PassengerPlaneAIDummyLoadPassengersPatch));
                patchList.Add(typeof(PassengerPlaneAICanLeavePatch));
                patchList.Add(typeof(PassengerPlaneAIArriveAtDestinationPatch));


                s_iHarmonyPatches = patchList.Count;

                string sMessage = "Patching the following functions:\r\n";

                foreach (var patchType in patchList)
                {
                    sMessage += patchType.ToString() + "\r\n";
                    harmony.CreateClassProcessor(patchType).Patch();
                }

                Debug.Log(sMessage);
            }
        }

        public static void UnpatchAll()
        {
            if (s_patched)
            {
                var harmony = new Harmony2(HarmonyId);

                harmony.UnpatchAll(HarmonyId);
                s_patched = false;

                UnityEngine.Debug.Log(AirplaneFixMain.ModName + ": Unpatching...");
            }
        }

        public static int GetPatchCount()
        {
            var harmony = new Harmony2(HarmonyId);
            var methods = harmony.GetPatchedMethods();

            int i = 0;
            foreach (var method in methods)
            {
                var info = Harmony2.GetPatchInfo(method);

                if (info.Owners?.Contains(harmony.Id) == true)
                {
                    i++;
                }
            }

            return i;
        }
    }
}
