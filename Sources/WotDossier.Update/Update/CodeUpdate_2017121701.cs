using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using WotDossier.Common.Extensions;
using WotDossier.Domain;

namespace WotDossier.Update.Update
{
    /// <summary>
    /// Delete double tank statistic rows
    /// </summary>
    public class CodeUpdate_2017121701 : CodeUpdateBase
    {
        public override long Version => 2017121701;

        public override bool NeedDatabase => false;

        public override void Execute(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction)
        {
            var curDir = Folder.AssemblyDirectory();

            if (File.Exists(Path.Combine(curDir, @"app.settings")))
            {
                if(File.Exists(Path.Combine(Folder.DossierAppDataFolder, "app.settings")))
                    File.Delete(Path.Combine(Folder.DossierAppDataFolder, "app.settings"));
                File.Move(Path.Combine(curDir, @"app.settings"), Path.Combine(Folder.DossierAppDataFolder, "app.settings"));
            }
            if (Directory.Exists(Path.Combine(curDir, @"Logs")))
            {
                if(Directory.Exists(Path.Combine(Folder.DossierLocalAppDataFolder, "Logs")))
                    Directory.Delete(Path.Combine(Folder.DossierLocalAppDataFolder, "Logs"), true);
                DirectoryExtensions.Copy(Path.Combine(curDir, @"Logs"), Path.Combine(Folder.DossierLocalAppDataFolder, "Logs"));
                Directory.Delete(Path.Combine(curDir, @"Logs"), true);
            }

            if (Directory.Exists(Path.Combine(curDir, @"backup")))
            {
                if (Directory.Exists(Path.Combine(Folder.DossierLocalAppDataFolder, "backup")))
                    Directory.Delete(Path.Combine(Folder.DossierLocalAppDataFolder, "backup"), true);
                //Directory.Move(Path.Combine(curDir, @"backup"), Path.Combine(Folder.DossierAppDataFolder, "backup"));
                DirectoryExtensions.Copy(Path.Combine(curDir, @"backup"), Path.Combine(Folder.DossierLocalAppDataFolder, "backup"));
                Directory.Delete(Path.Combine(curDir, @"backup"), true);
            }

            if (Directory.Exists(Path.Combine(curDir, @"IconsCache")))
            {
                if (Directory.Exists(Path.Combine(Folder.DossierLocalAppDataFolder, "IconsCache")))
                    Directory.Delete(Path.Combine(Folder.DossierLocalAppDataFolder, "IconsCache"), true);
                DirectoryExtensions.Copy(Path.Combine(curDir, @"IconsCache"), Path.Combine(Folder.DossierLocalAppDataFolder, "IconsCache"));
                Directory.Delete(Path.Combine(curDir, @"IconsCache"), true);
            }


            if (File.Exists(Path.Combine(curDir, @"Data\Medals.xml")))
            {
                File.Delete(Path.Combine(curDir, @"Data\Medals.xml"));
            }

            if (File.Exists(Path.Combine(curDir, @"Data\ReplaysCatalog.xml")))
            {
                if (File.Exists(Path.Combine(Folder.DossierAppDataFolder, "ReplaysCatalog.xml")))
                    File.Delete(Path.Combine(Folder.DossierAppDataFolder, "ReplaysCatalog.xml"));
                File.Move(Path.Combine(curDir, @"Data\ReplaysCatalog.xml"), Path.Combine(Folder.DossierAppDataFolder, "ReplaysCatalog.xml"));
            }

            if (File.Exists(Path.Combine(curDir, @"Data\dossier.s3db")))
            {
                if (File.Exists(Path.Combine(Folder.DossierAppDataFolder, "dossier.s3db")))
                    File.Delete(Path.Combine(Folder.DossierAppDataFolder, "dossier.s3db"));

                File.Move(Path.Combine(curDir, @"Data\dossier.s3db"), Path.Combine(Folder.DossierAppDataFolder, "dossier.s3db"));
            }
            if (Directory.Exists(Path.Combine(curDir, @"Data")))
                Directory.Delete(Path.Combine(curDir, @"Data"), true);

            if (Directory.Exists(Path.Combine(curDir, @"Config")))
                Directory.Delete(Path.Combine(curDir, @"Config"), true);
        }
    }
}