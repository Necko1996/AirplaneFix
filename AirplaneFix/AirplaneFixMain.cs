using AirplaneFix.Settings;
using ICities;

namespace AirplaneFix
{
    public class AirplaneFixMain : IUserMod
    {
        private static string Version = "v1.0.0";

        public static string ModName => "AirplaneFix " + Version;
        public static string Title => "AirplaneFix " + " " + Version;

        public static bool IsEnabled = false;

        public static bool Debug = false;

        public string Name
        {
            get { return ModName; }
        }

        public string Description
        {
            get { return "Make planes despawn at outside connection and lowers the time of load"; }
        }

        public void OnEnabled()
        {
            IsEnabled = true;
        }

        public void OnDisabled()
        {
            IsEnabled = false;
        }

        public void OnSettingsUI(UIHelper helper)
        {
            SettingsUI.OnSettingsUI(helper);
        }
    }
}