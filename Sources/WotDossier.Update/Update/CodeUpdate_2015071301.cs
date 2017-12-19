using System;
using System.Data.SQLite;
using System.IO;
using Common.Logging;
using WotDossier.Dal;
using WotDossier.Domain;

namespace WotDossier.Update.Update
{
    /// <summary>
    /// Delete replays cache
    /// </summary>
    public class CodeUpdate_2015071301 : CodeUpdateBase
    {
        private static readonly ILog _log = LogManager.GetLogger<CodeUpdate_2015071301>();

        public override long Version => 2015071301;

        public override void Execute(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction)
        {
            var path = Path.Combine(Folder.DossierLocalAppDataFolder, "replays.cache");
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception e)
                {
                    _log.Error("Can't delete replays cache", e);
                }
            }
        }
    }
}