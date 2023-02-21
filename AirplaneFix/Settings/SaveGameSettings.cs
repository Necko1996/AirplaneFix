using AirplaneFix.Log;
using AirplaneFix.Settings;

namespace AirplaneFix
{
    public class SaveGameSettings
    {
        const int iSAVE_GAME_SETTINGS_DATA_VERSION = 1;
        public static SaveGameSettings s_SaveGameSettings = new SaveGameSettings();

        // Settings

        //CargoPlane
        public bool CargoPlaneLoadingSetting = true;
        public int CargoPlaneLoadingTimeSetting = 16;
        public bool NoReturnOnImportExportSetting = true;

        //PassangerPlane
        public bool PassengerPlaneLoadingSetting = true;
        public int PassengerPlaneLoadingTimeSetting = 16;
        public bool PassengerPlaneNoFakePassengers = true;
        public bool PassengerPlaneNoDummyPassengers = true;


        public SaveGameSettings()
        {
        }

        public static SaveGameSettings GetSettings()
        {
            return s_SaveGameSettings;
        }

        public static void SetSettings(SaveGameSettings settings)
        {
            s_SaveGameSettings = settings;
        }

        public static void SaveData(FastList<byte> Data)
        {
            s_SaveGameSettings.SaveDataInternal(Data);
        }

        private void SaveDataInternal(FastList<byte> Data)
        {
            StorageData.WriteInt32(iSAVE_GAME_SETTINGS_DATA_VERSION, Data); 
            
            //CargoPlane
            StorageData.WriteBool(CargoPlaneLoadingSetting, Data);
            StorageData.WriteInt32(CargoPlaneLoadingTimeSetting, Data);

            StorageData.WriteBool(NoReturnOnImportExportSetting, Data);

            //PasangerPlane
            StorageData.WriteBool(PassengerPlaneLoadingSetting, Data);
            StorageData.WriteInt32(PassengerPlaneLoadingTimeSetting, Data);
            StorageData.WriteBool(PassengerPlaneNoFakePassengers, Data);
            StorageData.WriteBool(PassengerPlaneNoDummyPassengers, Data);
        }

        public static void LoadData(int iGlobalVersion, byte[] Data, ref int iIndex)
        {
            if (AirplaneFixMain.Debug)
            {
                Debug.Log("1Global: " + iGlobalVersion + " DataLength: " + Data.Length + " Index: " + iIndex);
            }

            if (Data != null && Data.Length > iIndex)
            {
                int iSaveGameSettingVersion = StorageData.ReadInt32(Data, ref iIndex);

                if (AirplaneFixMain.Debug)
                {
                    Debug.Log("2Global: " + iGlobalVersion + " SaveGameVersion: " + iSaveGameSettingVersion + " DataLength: " + Data.Length + " Index: " + iIndex);
                }

                if (s_SaveGameSettings != null)
                {
                    switch (iSaveGameSettingVersion)
                    {
                        case 1: s_SaveGameSettings.LoadDataVersion1(Data, ref iIndex); break;

                        default:
                            {
                                Debug.Log("New data version, unable to load!");
                                break;
                            }
                    }

                    if (AirplaneFixMain.Debug)
                    {
                        Debug.Log("Settings:\r\n" + s_SaveGameSettings.DebugSettings());
                    }

                    SettingsUI.UpdateAirplaneFixSettings(s_SaveGameSettings);
                }
            }
        }

        private void LoadDataVersion1(byte[] Data, ref int iIndex)
        {
            //CargoPlane
            CargoPlaneLoadingSetting = StorageData.ReadBool(Data, ref iIndex);
            CargoPlaneLoadingTimeSetting = StorageData.ReadInt32(Data, ref iIndex);

            NoReturnOnImportExportSetting = StorageData.ReadBool(Data, ref iIndex);

            //PassngerPlane
            PassengerPlaneLoadingSetting = StorageData.ReadBool(Data,ref iIndex);
            PassengerPlaneLoadingTimeSetting = StorageData.ReadInt32(Data,ref iIndex);

            PassengerPlaneNoFakePassengers = StorageData.ReadBool(Data, ref iIndex);
            PassengerPlaneNoDummyPassengers = StorageData.ReadBool(Data, ref iIndex);
        }

        public string DebugSettings()
        {
            string sMessage = "===== Save Game Settings =====\r\n";
            sMessage += "CargoPlaneLoadingSetting: " + CargoPlaneLoadingSetting + "\r\n";
            sMessage += "CargoPlaneLoadingTimeSetting: " + CargoPlaneLoadingTimeSetting + "\r\n";
            sMessage += "NoReturnOnImportExportSetting: " + NoReturnOnImportExportSetting + "\r\n";
            sMessage += "-------------------------------------" + "\r\n";
            sMessage += "PassengerPlaneLoadingSetting: " + PassengerPlaneLoadingSetting + "\r\n";
            sMessage += "PassengerPlaneLoadingTimeSetting: " + PassengerPlaneLoadingTimeSetting + "\r\n";
            sMessage += "PassengerPlaneNoFakePassengers: " + PassengerPlaneNoFakePassengers + "\r\n";
            sMessage += "PassengerPlaneNoDummyPassengers: " + PassengerPlaneNoDummyPassengers + "\r\n";

            return sMessage;
        }
    }
}