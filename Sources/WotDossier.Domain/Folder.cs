using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace WotDossier.Domain
{
    public static class Folder
    {
        public static string GetDossierCacheFolder()
        {
            var appSettings = AppSettings.Instance;
            if (string.IsNullOrEmpty(appSettings.DossierCachePath))
            {
                return GetDefaultDossierCacheFolder();
            }
            return appSettings.DossierCachePath;
        }

        public static string GetDefaultDossierCacheFolder()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string dossierCacheFolder = appDataPath + @"\Wargaming.net\WorldOfTanks\dossier_cache";
            return dossierCacheFolder;
        }

        public static string DossierLocalAppDataFolder
        {
            get
            {
                var dossierCacheFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WotDossier");
                if (Debugger.IsAttached)
                    dossierCacheFolder =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            "WotDossier-Debug");
                if (!Directory.Exists(dossierCacheFolder))
                {
                    try
                    {
                        Directory.CreateDirectory(dossierCacheFolder);
                    }
                    catch (Exception e)
                    {
                    }
                }
                return dossierCacheFolder;
            }
        }

        public static string DossierAppDataFolder
        {
            get
            {
                var dossierCacheFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WotDossier");
                if (Debugger.IsAttached)
                    dossierCacheFolder =
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                            "WotDossier-Debug");
                if (!Directory.Exists(dossierCacheFolder))
                {
                    try
                    {
                        Directory.CreateDirectory(dossierCacheFolder);
                    }
                    catch (Exception e)
                    {
                    }
                }
                return dossierCacheFolder;
            }
        }

        public static string AssemblyDirectory()
        {
            var uri = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
            return Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));
        }
    }
}
