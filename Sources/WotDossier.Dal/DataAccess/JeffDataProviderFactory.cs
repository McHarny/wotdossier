using System;
using System.Data.Common;
using System.Reflection;
using LiaSoft.Jeff.DataAccess;
using WotDossier.Common.Reflection;
using WotDossier.Dal.DataAccess.Providers.MSSQL;
using WotDossier.Dal.DataAccess.Providers.SQLite;

namespace WotDossier.Dal.DataAccess
{
    public abstract class JeffDataProviderFactory
    {
        public static IJeffDataProviderHelper CrateHelper(ProviderType providerType)
        {
            switch (providerType)
            {
                case ProviderType.SQLServer:
                    //return CreateProvider(InternalConstants.SRAssemblyCommon, "LiaSoft.Jeff.DataAccess.SqlDataProviderHelper");
	                return new SqlDataProviderHelper();
				case ProviderType.SQLite:
                    //return CreateProvider(InternalConstants.SRAssemblyCache, "LiaSoft.Jeff.DataAccess.SQLiteDataProviderHelper");
					return new SQLiteDataProviderHelper();
                default:
                    throw new ArgumentOutOfRangeException(nameof(providerType), providerType, null);
            }
        }

        private static IJeffDataProviderHelper CreateProvider(string assemblyName, string typeName)
        {
            var assembly = Assembly.Load(assemblyName);
            var type = assembly.GetTypeByName(typeName);
            return Activator.CreateInstance(type) as IJeffDataProviderHelper;
        }

        public abstract DbStatement CreateStatement(JeffDbContext context, string commandText, DbTransaction trans);
    }

    

}