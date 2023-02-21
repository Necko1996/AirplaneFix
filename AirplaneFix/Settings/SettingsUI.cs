using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace AirplaneFix.Settings
{
    public static class SettingsUI
    {
        private static UICheckBox? m_chkCargoPlaneLoadingSetting = null;
        private static SettingsSlider? m_sliderCargoPlaneLoadingTimeSetting = null;
        private static UICheckBox? m_chkNoReturnOnImportExportSetting = null;

        private static UICheckBox? m_chkPassengerPlaneLoadingSetting = null;
        private static SettingsSlider? m_sliderPassengerPlaneLoadingTimeSetting = null;
        private static UICheckBox? m_chkPassengerPlaneNoFakePassengersSetting = null;
        private static UICheckBox? m_chkPassengerPlaneNoDummyPassengersSetting = null;

        public static void OnSettingsUI(UIHelper helper)
        {
            if (AirplaneFixMain.Debug)
            {
                Log.Debug.Log("Start of OnSettingsUI");
            }
            
            UIComponent pnlMain = (UIComponent)helper.self;
            UILabel txtTitle = AddDescription(pnlMain, "title", pnlMain, 1.4f, AirplaneFixMain.Title);
            helper.AddSpace(4);

            //Get default values for SaveGame
            SaveGameSettings oSettings = SaveGameSettings.GetSettings();

            UIHelper cargoPlaneSettingsGroup = (UIHelper) helper.AddGroup("Cargo plane settings");
            UIPanel cargoPlaneSettingsPanel = cargoPlaneSettingsGroup.self as UIPanel;

            m_chkCargoPlaneLoadingSetting = (UICheckBox) cargoPlaneSettingsGroup.AddCheckbox("Enable reduced loading", oSettings.CargoPlaneLoadingSetting, onCargoPlaneLoadingSetting);
            cargoPlaneSettingsGroup.AddSpace(2);
            m_sliderCargoPlaneLoadingTimeSetting = SettingsSlider.Create(cargoPlaneSettingsGroup, LayoutDirection.Horizontal, "Loading time", 1.0f, 400, 200, 16f, 100f, 1f, (float) oSettings.CargoPlaneLoadingTimeSetting, onCargoPlaneLoadingTimeSetting);
            AddDescription(cargoPlaneSettingsPanel, "CargoPlaneLoadingTimeSetting", cargoPlaneSettingsPanel, 1.0f, "16s - All other big transport vehicles (Recommanded)"+"\r\n"+"52s - Default value for CargoPlanes"+"\r\n"+"100s - Default value for CargoPlanes in AirportDLC area");

            cargoPlaneSettingsGroup.AddSpace(4);

            m_chkNoReturnOnImportExportSetting = (UICheckBox) cargoPlaneSettingsGroup.AddCheckbox("Enable no return on Import/Export", oSettings.NoReturnOnImportExportSetting, onNoReturnOnImportExportSetting);


            UIHelper passengerPlaneSettingsGroup = (UIHelper) helper.AddGroup("Passenger plane settings");
            UIPanel passengerPlaneSettingsPanel = passengerPlaneSettingsGroup.self as UIPanel;

            m_chkPassengerPlaneLoadingSetting = (UICheckBox) passengerPlaneSettingsGroup.AddCheckbox("Enable reduced loading", oSettings.PassengerPlaneLoadingSetting, onPassengerPlaneLoadingSetting);
            passengerPlaneSettingsGroup.AddSpace(2);
            m_sliderPassengerPlaneLoadingTimeSetting = SettingsSlider.Create(passengerPlaneSettingsGroup, LayoutDirection.Horizontal, "Loading time", 1.0f, 400, 200, 16f, 200f, 1f, (float)oSettings.PassengerPlaneLoadingTimeSetting, onPassengerPlaneLoadingTimeSetting);
            AddDescription(passengerPlaneSettingsPanel, "PassengerPlaneLoadingTimeSetting", passengerPlaneSettingsPanel, 1.0f, "16s - All other big transport vehicles (Recommanded)" + "\r\n" + "200s - Default value for all PassengerPlanes");

            passengerPlaneSettingsGroup.AddSpace(4);

            m_chkPassengerPlaneNoFakePassengersSetting = (UICheckBox) passengerPlaneSettingsGroup.AddCheckbox("Enable no fake passengers", oSettings.PassengerPlaneNoFakePassengers, onPassengerPlaneNoFakePassengers);
            m_chkPassengerPlaneNoDummyPassengersSetting = (UICheckBox) passengerPlaneSettingsGroup.AddCheckbox("Enable no dummy passengers", oSettings.PassengerPlaneNoDummyPassengers, onPassengerPlaneNoDummyPassengers);

            if (AirplaneFixMain.Debug)
            {
                Log.Debug.Log("End of OnSettingsUI");
            }
        }

        private static readonly Color32 m_greyColor = new Color32(0xe6, 0xe6, 0xe6, 0xee);
        private static UILabel AddDescription(UIHelper parent, string name, float fontScale, string text)
        {
            return AddDescription(parent.self as UIPanel, name, parent.self as UIPanel, fontScale, text);
        }
        private static UILabel AddDescription(UIComponent panel, string name, UIComponent alignTo, float fontScale, string text)
        {
            UILabel desc = panel.AddUIComponent<UILabel>();

            desc.name = name;
            desc.width = panel.width;
            desc.wordWrap = true;
            desc.autoHeight = true;
            desc.textScale = fontScale;
            desc.textColor = m_greyColor;
            desc.text = text;
            desc.relativePosition = new Vector3(alignTo.relativePosition.x + 26f, alignTo.relativePosition.y + alignTo.height + 10);

            return desc;
        }

        public static void onCargoPlaneLoadingSetting(bool bChecked)
        {
            SaveGameSettings oSettings = SaveGameSettings.GetSettings();
            oSettings.CargoPlaneLoadingSetting = bChecked;

            m_sliderCargoPlaneLoadingTimeSetting.Enable(oSettings.CargoPlaneLoadingSetting);
        }

        public static void onCargoPlaneLoadingTimeSetting(float fValue)
        {
            SaveGameSettings oSettings = SaveGameSettings.GetSettings();
            oSettings.CargoPlaneLoadingTimeSetting = (int) fValue;
        }

        public static void onNoReturnOnImportExportSetting(bool bChecked)
        {
            SaveGameSettings oSettings = SaveGameSettings.GetSettings();
            oSettings.NoReturnOnImportExportSetting = bChecked;
        }

        public static void onPassengerPlaneLoadingSetting(bool bChecked)
        {
            SaveGameSettings oSettings = SaveGameSettings.GetSettings();
            oSettings.PassengerPlaneLoadingSetting = bChecked;

            m_sliderPassengerPlaneLoadingTimeSetting.Enable(oSettings.PassengerPlaneLoadingSetting);
        }

        public static void onPassengerPlaneLoadingTimeSetting(float fValue)
        {
            SaveGameSettings oSettings = SaveGameSettings.GetSettings();
            oSettings.PassengerPlaneLoadingTimeSetting = (int)fValue;
        }

        public static void onPassengerPlaneNoFakePassengers(bool bChecked)
        {
            SaveGameSettings oSettings = SaveGameSettings.GetSettings();
            oSettings.PassengerPlaneNoFakePassengers = bChecked;
        }

        public static void onPassengerPlaneNoDummyPassengers(bool bChecked)
        {
            SaveGameSettings oSettings = SaveGameSettings.GetSettings();
            oSettings.PassengerPlaneNoDummyPassengers = bChecked;
        }
       
        public static void UpdateAirplaneFixSettings(SaveGameSettings oSettings)
        {
            m_sliderCargoPlaneLoadingTimeSetting.Enable(oSettings.CargoPlaneLoadingSetting);
            m_sliderPassengerPlaneLoadingTimeSetting.Enable(oSettings.PassengerPlaneLoadingSetting);

            //CargoPlane
            m_chkCargoPlaneLoadingSetting.isChecked = oSettings.CargoPlaneLoadingSetting;
            m_sliderCargoPlaneLoadingTimeSetting.SetValue(oSettings.CargoPlaneLoadingTimeSetting);

            m_chkNoReturnOnImportExportSetting.isChecked = oSettings.NoReturnOnImportExportSetting;

            //PassangerPlane
            m_chkPassengerPlaneLoadingSetting.isChecked = oSettings.PassengerPlaneLoadingSetting;
            m_sliderPassengerPlaneLoadingTimeSetting.SetValue(oSettings.PassengerPlaneLoadingTimeSetting);

            m_chkPassengerPlaneNoFakePassengersSetting.isChecked = oSettings.PassengerPlaneNoFakePassengers;
            m_chkPassengerPlaneNoDummyPassengersSetting.isChecked = oSettings.PassengerPlaneNoDummyPassengers;
        }
    }
}
