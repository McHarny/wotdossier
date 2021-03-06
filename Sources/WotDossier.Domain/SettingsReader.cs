﻿using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using WotDossier.Common;


namespace WotDossier.Domain
{
    public static class SettingsReader111
    {
        private static readonly object _syncObject = new object();

        //private static readonly string _filePath = AppConfigSettings.SettingsPath;

        /// <summary>
        /// Reads this instance.
        /// </summary>
        /// <returns></returns>
        public static AppSettings Get()
        {
            var filePath = AppConfigSettings.SettingsPath;

            if (File.Exists(filePath))
            {
	            lock (_syncObject)
	            {
		            var readToEnd = File.ReadAllText(filePath);
		            if (!string.IsNullOrEmpty(readToEnd))
			            return Deserialize<AppSettings>(readToEnd);
	            }
            }
            
            //create settings file if not exists
            AppSettings settingsDto = new AppSettings();
            settingsDto.DossierCachePath = Folder.GetDefaultDossierCacheFolder();
            Save(settingsDto);
            return settingsDto;
        }

        public static T Get<T>() where T : class, new()
        {
            var filePath = AppConfigSettings.SettingsPath;

            if (File.Exists(filePath))
            {
                FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (StreamReader reader = new StreamReader(stream))
                {
                    var readToEnd = reader.ReadToEnd();
                    return Deserialize<T>(readToEnd);
                }
            }

            //create settings file if not exists
            T settingsDto = new T();
            Save(settingsDto);
            return settingsDto;
        }

        /// <summary>
        /// Saves the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public static void Save(object settings)
        {
            var filePath = AppConfigSettings.SettingsPath;

            lock (_syncObject)
            {
				File.WriteAllText(filePath, Serialize(settings));
            }
        }

        private static string Serialize(object settings)
        {
            return JsonConvert.SerializeObject(settings, Formatting.Indented);
        }

        private static T Deserialize<T>(string serializedObject) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(serializedObject);
            }
            catch
            {
                return XmlSerializer.LoadObjectFromXml<T>(serializedObject);
            }
        }
    }
}
