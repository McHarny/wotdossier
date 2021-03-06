﻿using System.Data;
using System.Data.SQLite;
using WotDossier.Dal;
using WotDossier.Domain;

namespace WotDossier.Update.Update
{
    /// <summary>
    /// Delete double tank statistic rows
    /// </summary>
    public class CodeUpdate_2014070802 : CodeUpdateBase
    {
        public override long Version => 2014070802;
        public override void Execute(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction)
        {
            AppSettings appSettings = AppSettings.Instance;
            if (appSettings.PlayerId > 0)
            {
                const string commandText = "Update Player set Server = @server where PlayerId = @playerId";
                SQLiteCommand command = new SQLiteCommand(commandText, sqlCeConnection, transaction);
                command.Parameters.Add("@server", DbType.String).Value = appSettings.Server;
                command.Parameters.Add("@playerId", DbType.Int32).Value = appSettings.PlayerId;
                command.ExecuteNonQuery();
            }
        }
    }
}