using System.Data.Common;
using WotDossier.Dal.DataAccess;

namespace LiaSoft.Jeff.DataAccess
{
    public interface IJeffDataProviderHelper
    {
        DbStatement CreateStatement(JeffDbContext context, string commandText, DbTransaction trans);

        DbConnectionStringBuilder MakeConnectionStringBuilder(JeffDbContext context);

        void ParseConnectionString(string connString, out string server, out string database, out bool integratedSecurity,
            out string userName, out string password);

        bool HasUserName { get; }
        bool HasPassword { get; }
        bool HasIntegratedSecurity { get; }
        bool HasMultipleDatabases { get; }
        bool IsServerbased { get; }
        bool IsFilebased { get; }
    }
}