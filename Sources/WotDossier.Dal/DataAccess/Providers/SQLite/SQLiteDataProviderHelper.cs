using System.Data.Common;
using System.Data.SQLite;
using LiaSoft.Jeff.DataAccess;

namespace WotDossier.Dal.DataAccess.Providers.SQLite
{
    public class SQLiteDataProviderHelper : IJeffDataProviderHelper
    {
        public SQLiteDataProviderHelper()
        {
            
        }
        public DbStatement CreateStatement(JeffDbContext context, string commandText, DbTransaction trans)
        {
            return new SQLiteStatement(context, commandText, trans);
        }

        public DbConnectionStringBuilder MakeConnectionStringBuilder(JeffDbContext context)
        {
            var bld = new SQLiteConnectionStringBuilder
            {
                DataSource = context.Server,
                Version = 3,
                FailIfMissing = false
            };

            if (!context.IntegratedSecurity)
            {
                bld.Password = context.Password;
            }
            return bld;
        }

        public void ParseConnectionString(string connString, out string server, out string database, out bool integratedSecurity,
            out string userName, out string password)
        {
            var bld = new SQLiteConnectionStringBuilder(connString);
            database = string.Empty;
            server = bld.DataSource;
            password = bld.Password ?? string.Empty;
            integratedSecurity = string.IsNullOrEmpty(password);
            userName = string.Empty;
        }

        public bool HasUserName => false;
        public bool HasPassword => true;
        public bool HasIntegratedSecurity => false;
        public bool HasMultipleDatabases => false;
        public bool IsServerbased => false;
        public bool IsFilebased => true;
    }
}