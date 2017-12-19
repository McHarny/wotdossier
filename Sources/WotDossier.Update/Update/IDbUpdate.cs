using System.Data.SQLite;

namespace WotDossier.Update.Update
{
    public interface IDbUpdate
    {
        bool NeedDatabase { get; }
        long Version { get; }
        void Execute(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction);
    }
}