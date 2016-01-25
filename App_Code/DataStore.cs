using System;
using System.IO;
using System.Web.Hosting;

public class DataStore
{
    public static bool SaveSettings<T>(T setting) where T :class
    {
        try
        {
            string settingName = setting.GetType().Name;
            if(settingName == "StorageSettings"){
                File.WriteAllText(HostingEnvironment.MapPath(Constants.DATA_STORE_STORAGE_SETTINGS), Convert.ChangeType(setting, typeof(StorageSettings)).ToJSON());
                return true;
            }
            else if (settingName == "GeneralSettings")
            {
                File.WriteAllText(HostingEnvironment.MapPath(Constants.DATA_STORE_GEN_SETTINGS), Convert.ChangeType(setting, typeof(GeneralSettings)).ToJSON());
                return true;
            }
        }
        catch (System.Exception)
        {
            return false;
        }
        return false;
    }

    public static T ReadSettings<T>(string settingName) where T :class
    {
        T _settings = null;
        try
        {
            if(settingName == "GeneralSettings")
            {
                string generalSettingsString = File.ReadAllText(HostingEnvironment.MapPath(Constants.DATA_STORE_GEN_SETTINGS));
                _settings = generalSettingsString.ToObject<T>();
            }
            else if(settingName == "StorageSettings")
            {
                string storageSettingsString = File.ReadAllText(HostingEnvironment.MapPath(Constants.DATA_STORE_STORAGE_SETTINGS));
                _settings = storageSettingsString.ToObject<T>();
            }
        }
        catch (Exception)
        {
            return _settings;
        }
        return _settings;
    }
}