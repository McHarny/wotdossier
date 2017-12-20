using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using WotDossier.Domain.Settings;

namespace WotDossier.Domain
{
    /// <summary>
    /// Application settings
    /// </summary>
    public class AppSettings : AppSettingsBase
    {
        private static AppSettings instance = null;
        private static readonly object _syncObject = new object();

        public static  AppSettings Instance
        {
            get
            {
                lock (_syncObject)
                {
                    if (instance != null) return instance;
                    instance = 
                }
            }
        }

        /// <summary>
        /// Reads this instance.
        /// </summary>
        /// <returns></returns>
        private static AppSettings Read()
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

        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        /// <value>
        /// The name of the player.
        /// </value>
        public string PlayerName { get; set; }

        /// <summary>
        /// Gets or sets the player id.
        /// </summary>
        /// <value>
        /// The player id.
        /// </value>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the replays upload server path.
        /// </summary>
        /// <value>
        /// The replays upload server path.
        /// </value>
        public string ReplaysUploadServerPath { get; set; } = "http://wotreplays.ru/site/upload";

        /// <summary>
        /// Gets or sets a value indicating whether [check for updates].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [check for updates]; otherwise, <c>false</c>.
        /// </value>
        public bool CheckForUpdates { get; set; } = true;

        /// <summary>
        /// Gets or sets the new version check last date.
        /// </summary>
        /// <value>
        /// The new version check last date.
        /// </value>
        public DateTime NewVersionCheckLastDate { get; set; }

        /// <summary>
        /// Gets or sets the tank filter settings.
        /// </summary>
        /// <value>
        /// The tank filter settings.
        /// </value>
        public TankFilterSettings TankFilterSettings { get; set; } = new TankFilterSettings();

        /// <summary>
        /// Gets or sets the period settings.
        /// </summary>
        /// <value>
        /// The period settings.
        /// </value>
        public PeriodSettings PeriodSettings { get; set; } = new PeriodSettings();

        /// <summary>
        /// Gets or sets a value indicating whether [automatic load statistic].
        /// </summary>
        /// <value>
        /// <c>true</c> if [automatic load statistic]; otherwise, <c>false</c>.
        /// </value>
        public bool AutoLoadStatistic { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show extended replays data].
        /// </summary>
        /// <value>
        /// <c>true</c> if [show extended replays data]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowExtendedReplaysData { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to try to find tank analog in tanks ratings.
        /// </summary>
        /// <value>
        /// <c>true</c> if try to find tank analog; otherwise, <c>false</c>.
        /// </value>
        public bool TryFindTankAnalog { get; set; } = true;

        public bool UseIncompleteReplaysResultsForCharts { get; set; } = false;

        private List<ReplayPlayer> _replayPlayers;
        public List<ReplayPlayer> ReplayPlayers
        {
            get
            {
                if (_replayPlayers == null)
                {
                    _replayPlayers = new List<ReplayPlayer>();
                }
                return _replayPlayers ;
            }
            set { _replayPlayers = value; }
        }

        public string ExternalDataVersion { get; set; }

        public String ColumnInfo { get; set; }
        public string DossierCachePath { get; set; }

        public string WotFolderPath { get; set; }

        public int WindowState { get; set; }


    }
}
