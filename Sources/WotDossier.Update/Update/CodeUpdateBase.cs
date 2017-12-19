using System.Data.SQLite;

namespace WotDossier.Update.Update
{
    public abstract class CodeUpdateBase : IDbUpdate
    {
        public virtual bool NeedDatabase => true;

        public abstract long Version { get; }

        public abstract void Execute(SQLiteConnection sqlCeConnection, SQLiteTransaction transaction);
    }
}