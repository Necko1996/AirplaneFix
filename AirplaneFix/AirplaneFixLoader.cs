using ICities;
using AirplaneFix.Harmony;

namespace AirplaneFix
{
    public class AirplaneFixLoader : LoadingExtensionBase
    {
        private static bool s_loaded = false;

        public override void OnLevelLoaded(LoadMode mode)
        {
            if (AirplaneFixMain.IsEnabled && ActiveInMode(mode))
            {
                s_loaded = true;

                // Patch game using Harmony
                if (HarmonyHelper.ApplyHarmonyPatches())
                {

                }
                else
                {
                    s_loaded = false;
                    return;
                }
            }
        }

        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();

            // Remove patches first so object aren't called after being destroyed
            HarmonyHelper.RemoveHarmonyPathes();

            s_loaded = false;
        }

        public static bool IsLoaded() 
        { 
            return s_loaded; 
        }

        private static bool ActiveInMode(LoadMode mode)
        {
            switch (mode)
            {
                case LoadMode.NewGame:
                case LoadMode.NewGameFromScenario:
                case LoadMode.LoadGame:
                    return true;

                default:
                    return false;
            }
        }
    }
}