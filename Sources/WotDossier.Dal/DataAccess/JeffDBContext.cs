using System;
using System.Data;
using System.Data.Common;
using LiaSoft.Jeff.DataAccess;

namespace WotDossier.Dal.DataAccess
{
	public class JeffDbContext : IEquatable<JeffDbContext>
	{
	    private IJeffDataProviderHelper helper;

		private object lockObject = new object();

        public JeffDbContext(ProviderType providerType, string server, string database, bool integratedSecurity, string userName, string password)
		{
		    ProviderType = providerType;
            helper = JeffDataProviderFactory.CrateHelper(providerType);

            if (helper.HasMultipleDatabases && string.IsNullOrEmpty(database))
                throw new ArgumentNullException(nameof(database));

            if (string.IsNullOrEmpty(server))
                throw new ArgumentNullException(nameof(server));

            if (!integratedSecurity && helper.HasUserName && string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));

            Database = database;
            Server = server;
            IntegratedSecurity = integratedSecurity;
            if (!IntegratedSecurity)
            {
                UserName = userName ?? string.Empty;
                Password = password ?? string.Empty;
            }
            else
            {
                UserName = string.Empty;
                Password = string.Empty;
            }
        }

        public JeffDbContext(string server, string database, bool integratedSecurity, string userName, string password) :
            this(ProviderType.SQLServer, server, database, integratedSecurity, userName, password)
        {
            
        }

        public JeffDbContext(ProviderType providerType, string server, string database) : this(providerType, server, database, true, string.Empty, string.Empty)
        {

        }
        public JeffDbContext(string server, string database) : this(ProviderType.SQLServer, server, database, true, string.Empty, string.Empty)
        {
            
        }

		public JeffDbContext(string connString) : this(ProviderType.SQLServer, connString)
        {
           
        }

        public JeffDbContext(ProviderType providerType, string connString)
        {
            if (string.IsNullOrEmpty(connString))
                throw new ArgumentNullException(nameof(connString));

            ProviderType = providerType;
            helper = JeffDataProviderFactory.CrateHelper(providerType);

            string server;
            string database;
            bool integratedSecurity;
            string userName;
            string password;

            helper.ParseConnectionString(connString, out server, out database, out integratedSecurity, out userName, out password);

            Server = server;
            Database = database;
            IntegratedSecurity = integratedSecurity;
            UserName = userName;
            Password = password;
        }

        public string Database { get; }
        public string Server { get; }
        public bool IntegratedSecurity { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }

		#region Equals && GetHashCode
		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the other parameter; otherwise, false.
		/// </returns>
		/// <param name="obj">An object to compare with this object.</param>
		public bool Equals(JeffDbContext obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return
                obj.ProviderType.Equals(ProviderType) &&
                String.Equals(obj.Database, Database, StringComparison.InvariantCultureIgnoreCase) &&
				String.Equals(obj.Server, Server, StringComparison.InvariantCultureIgnoreCase) &&
				obj.IntegratedSecurity.Equals(IntegratedSecurity) &&
				String.Equals(obj.UserName, UserName, StringComparison.InvariantCultureIgnoreCase) &&
				Equals(obj.Password, Password);
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
		/// </returns>
		/// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>. </param><filterpriority>2</filterpriority>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof(JeffDbContext)) return false;
			return Equals((JeffDbContext)obj);
		}

		///<summary>
		/// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"></see>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			unchecked
			{
				int result = ProviderType.GetHashCode();
                result = (result * 397) ^ Database.ToUpperInvariant().GetHashCode();
                result = (result * 397) ^ Server.ToUpperInvariant().GetHashCode();
				result = (result * 397) ^ IntegratedSecurity.GetHashCode();
				result = (result * 397) ^ UserName.ToUpperInvariant().GetHashCode();
				result = (result * 397) ^ Password.GetHashCode();
				return result;
			}
		}

		public static bool operator ==(JeffDbContext left, JeffDbContext right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(JeffDbContext left, JeffDbContext right)
		{
			return !Equals(left, right);
		}
		#endregion


		private string MakeConnectionString()
		{
		    var bld = helper.MakeConnectionStringBuilder(this);

			var additionalParamsCallback = AdditionalParamsCallback;
		    additionalParamsCallback?.Invoke(bld);

		    return bld.ConnectionString;
		}

		public void SetSecurity(string userName = null, string password = null)
		{
			lock(lockObject)
			{
				if(string.IsNullOrEmpty(userName))
				{
					IntegratedSecurity = true;
					UserName = null;
					Password = null;
				}
				else
				{
					IntegratedSecurity = false;
					UserName = userName;
					Password = password;
				}
			}

		}

		public string DisplayText
		{
			get { return string.Format("{0} - {1}", Server, Database); }
		}

        public ProviderType ProviderType { get; private set;}

		public override string ToString()
		{
			return ConnectionString;
		}

		public string ConnectionString => MakeConnectionString();

	    public Action<DbConnectionStringBuilder> AdditionalParamsCallback { get; set; }

		public Action<IDbConnection> ConnectionCallback { get; set; }

	    public Func<string> ContextSetterCommandCallback { get; set; }

	    public string ContextSetterCommand
	    {
	        get
	        {
	            if (ContextSetterCommandCallback != null)
	                return ContextSetterCommandCallback();
	            return string.Empty;
	        }
	    }

	    public DbStatement CreateStatement(string commandText = "", DbTransaction trans = null)
		{
			return helper.CreateStatement(this, commandText, trans);
		}

	    public DbStatement CreateCommand(string commandText = "", DbTransaction trans = null)
		{
			var st = CreateStatement(commandText, trans);
			st.CommandType = CommandType.StoredProcedure;
			return st;
		}

		public void Check()
		{
			var tmp = CreateStatement();
			tmp.Dispose();
		}
	}
}
