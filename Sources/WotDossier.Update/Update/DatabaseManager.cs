using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using Common.Logging;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;

namespace WotDossier.Update.Update
{
    public class DatabaseManager
    {
        private const string SQL_SCRIPT_EXTENSION = ".sql";
        private const string SQL_SCRIPT_EXTENSION_MASK = "*.sql";
        private static readonly ILog Logger = LogManager.GetLogger<DatabaseManager>();

        public void Update()
        {
            List<IDbUpdate> updates = GetDbUpdates();

            long version = GetCurrentDbVersion();

            bool needVacuum = false;
            foreach (var dbUpdate in updates.OrderBy(x => x.Version))
            {
                if (dbUpdate.Version > version)
                {
                    SQLiteConnection connection = null;
                    SQLiteTransaction transaction = null;
                    try
                    {
                        if (!dbUpdate.NeedDatabase)
                        {
                            dbUpdate.Execute(connection, transaction);
                        }

                        connection = GetConnection();
                        Logger.Debug("Update. Source connection obtained");

                        transaction = BeginTransaction(connection);
                        Logger.Debug("Update. Dest transaction started");

                        if (dbUpdate.NeedDatabase)
                            dbUpdate.Execute(connection, transaction);

                        UpdateDbVersion(dbUpdate.Version, connection, transaction);
                        needVacuum = true;
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        RollbackTransaction(transaction);
                        Logger.Error("Exception", e);
                        throw;
                    }
                    finally
                    {
                        CloseConnection(connection);
                        Logger.Debug("Update. Source connection closed");
                    }
                }
            }

            

            if (needVacuum)
            {
                var commandText = @"VACUUM";
                var connection = GetConnection();
                var command = new SQLiteCommand(commandText, connection);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Logger.Error("Exception", e);
                }
            }

        }

        private void UpdateDbVersion(long max, SQLiteConnection connection, SQLiteTransaction transaction)
        {
            //Logger.Debug("BatchImportBcg. Source connection obtained");
            SQLiteCommand command = new SQLiteCommand(@"update DbVersion set SchemaVersion = @version", connection, transaction);
            command.CommandType = CommandType.Text;
            command.Parameters.Add("@version", DbType.String).Value = max.ToString();

            command.ExecuteNonQuery();
        }

        private void CloseConnection(SQLiteConnection connection)
        {
            if (connection != null)
            {
                connection.Close();
            }
        }

        private void RollbackTransaction(SQLiteTransaction transaction)
        {
            if (transaction != null)
            {
                transaction.Rollback();
            }
        }

        private SQLiteTransaction BeginTransaction(SQLiteConnection sourceConnection)
        {
            return sourceConnection.BeginTransaction();
        }

        private SQLiteConnection GetConnection()
        {
            var file = Path.Combine(Folder.DossierAppDataFolder, "dossier.s3db");
            SQLiteConnection connection = new SQLiteConnection($"Data Source={file};Version=3;");
            connection.Open();
            return connection;
        }

        private long GetCurrentDbVersion()
        {
            SQLiteConnection connection = null;

            try
            {
                connection = GetConnection();
                //Logger.Debug("BatchImportBcg. Source connection obtained");
                SQLiteCommand command = new SQLiteCommand(@"Select SchemaVersion from DbVersion limit 1", connection);
                command.CommandType = CommandType.Text;
                object result = command.ExecuteScalar();

                if (result == null)
                {
                    return 0;
                }
                else
                {
                    return int.Parse(result.ToString());
                }
            }
            catch (Exception e)
            {
                Logger.Error("Get Current DbVersion error", e);
                return int.MaxValue;
            }
            finally
            {
                CloseConnection(connection);
                //Logger.Debug("BatchImportBcg. Source connection closed");
            }
        }

        private List<IDbUpdate> GetDbUpdates()
        {
            var updateType = typeof(CodeUpdateBase);
            var types = updateType.Assembly.GetTypes().Where(type => updateType.IsAssignableFrom(type) && type != updateType);

            string currentDirectory = Folder.AssemblyDirectory();
            var updatesFolder = Path.Combine(currentDirectory, "Updates");
            
            IEnumerable<string> strings;
            List<IDbUpdate> updates;
            
            if (Directory.Exists(updatesFolder))
            {
                strings = Directory.GetFiles(updatesFolder, SQL_SCRIPT_EXTENSION_MASK);
                updates = strings.Select(x => (IDbUpdate)new SqlUpdate(x)).ToList();
            }
            else
            {
                Assembly entryAssembly = updateType.Assembly;
                strings = AssemblyExtensions.GetResourcesByMask(entryAssembly, SQL_SCRIPT_EXTENSION);
                updates = strings.Select(x => (IDbUpdate)new EmbeddedSqlUpdate(entryAssembly, x, SQL_SCRIPT_EXTENSION)).ToList();
            }

            updates.AddRange(types.Select(Activator.CreateInstance).Cast<IDbUpdate>());

            return updates.OrderBy(x => x.Version).ToList();
        }

        public void InitDatabase()
        {
            InitDbFile();
            Update();
        }

        private static void InitDbFile()
        {
            var path = Path.Combine(Folder.DossierAppDataFolder, "dossier.s3db");
            if (!File.Exists(path))
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();
                var resourceName = entryAssembly.GetName().Name + @".Data.init.s3db";
                byte[] embeddedResource = entryAssembly.GetEmbeddedResource(resourceName);
                using (FileStream fileStream = File.OpenWrite(path))
                {
                    fileStream.Write(embeddedResource, 0, embeddedResource.Length);
                    fileStream.Flush();
                }
            }
        }

        public void DeleteDatabase()
        {
            var path = Path.Combine(Folder.DossierAppDataFolder, "dossier.s3db");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
