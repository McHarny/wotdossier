using System.Data.Common;
using System.Data.SqlClient;
using LiaSoft.Jeff.DataAccess;

namespace WotDossier.Dal.DataAccess.Providers.MSSQL
{
    public class SqlDataProviderHelper : IJeffDataProviderHelper
    {
        public SqlDataProviderHelper()
        {

        }
        public DbStatement CreateStatement(JeffDbContext context, string commandText, DbTransaction trans)
        {
            return new SqlStatement(context, commandText, trans);
        }

        public DbConnectionStringBuilder MakeConnectionStringBuilder(JeffDbContext context)
        {
            var bld = new SqlConnectionStringBuilder
            {
                InitialCatalog = context.Database,
                DataSource = context.Server,
                IntegratedSecurity = context.IntegratedSecurity
            };

            if (!context.IntegratedSecurity)
            {
                bld.UserID = context.UserName;
                bld.Password = context.Password;
            }
            return bld;
        }

        public void ParseConnectionString(string connString, out string server, out string database, out bool integratedSecurity,
            out string userName, out string password)
        {
            var bld = new SqlConnectionStringBuilder(connString);
            database = bld.InitialCatalog;
            server = bld.DataSource;
            integratedSecurity = bld.IntegratedSecurity;
            if (!integratedSecurity)
            {
                userName = bld.UserID ?? string.Empty;
                password = bld.Password ?? string.Empty;
            }
            else
            {
                userName = string.Empty;
                password = string.Empty;
            }
        }

        public bool HasUserName => true;
        public bool HasPassword => true;
        public bool HasIntegratedSecurity => true;
        public bool HasMultipleDatabases => true;
        public bool IsServerbased => true;
        public bool IsFilebased => false;
    }
}