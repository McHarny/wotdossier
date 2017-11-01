using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using WotDossier.Dal.DataAccess;
using WotDossier.Dal.DataAccess.Providers.MSSQL;

namespace Jeff.Core
{
	/// <summary>
	/// Класс исключения, которые генерит VirtuaObjects.Framework.Core (only Core!) для Sql-ошибок. 
	/// </summary>
	public abstract class CoreDbException : Exception
	{
		#region Поля
		/// <summary>Параметры SQL-оператора</summary>
		private readonly List<DbParameter> m_Params = new List<DbParameter>();

		/// <summary>Расширенная информация о команде</summary>
		private string m_AdditionalInfo = "";

		/// <summary>Имеется информация о SQLStatemet</summary>
		protected bool HasStatementInfo;
		#endregion

		#region Конструкторы
		protected CoreDbException(string message) : base(message) { }
		protected CoreDbException(DbStatement st, string message)
			: this(st, message, null)
		{
		}

		protected CoreDbException(Exception innerException)
			: this(innerException.Message, innerException)
		{
		}

		protected CoreDbException(string message, Exception innerException)
			: base(message, innerException)
		{
			ParseInnerException(innerException);
		}

		protected CoreDbException(DbStatement st, string message, Exception innerException)
			: this(message, innerException)
		{
			ParseStatement(st);
		}

		protected CoreDbException(DbStatement st, Exception innerException, string Method)
			: this(st, innerException.Message, innerException)
		{
			MethodText = Method;
		}
		#endregion

		protected abstract void ParseInnerException(Exception innerException);

		private void ParseStatement(DbStatement st)
		{
			ExecuteTime = DateTime.Now;

			try
			{
				CommandText = st.CommandText;
				CommandType = st.CommandType;

				Server = (st.Connection != null) ? st.Connection.DataSource : "";
				Database = (st.Connection != null) ? st.Connection.Database : "";

				// копируем массив параметров
				foreach (DbParameter param in st.Parameters)
				{
					m_Params.Add(param);
				}

				HasStatementInfo = true;
			}
			catch { }

		}

        /// <summary>
        /// Создать дополнительное описание ошибки, если таковое есть
        /// </summary>
        /// <returns>Дополнительное описание (строка, not null)</returns>
        protected virtual string MakeAdditionalMessage()
		{
			StringBuilder builder = new StringBuilder();

			builder.Append("---------------------------------------------\r\n");
			builder.Append("Code" + ":\t\t" + Number + "\r\n");
			builder.Append("Line" + ":\t\t" + LineNumber + "\r\n");
			builder.Append("Class" + ":\t\t" + Class + "\r\n");
			if (!string.IsNullOrEmpty(Procedure))
				builder.Append("Procedure" + ":\t" + Procedure + "\r\n");
			if (!string.IsNullOrEmpty(MethodText))
				builder.Append("Method" + ":\t" + MethodText + "\r\n");

			builder.Append(AdditionalInfo);

			return builder.ToString();
		}



		private string MakeAdditionalInfo()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append("---------------------------------------------\r\n");

			builder.Append("Server" + ":\t\t" + ((HasStatementInfo) ? Server : DatabaseServer) + "\r\n");
			builder.Append("Database" + ":\t\t" + Database + "\r\n");
			builder.Append("UserName" + ":\t" + Environment.UserDomainName + '\\' + Environment.UserName + "\r\n");
			builder.Append("---------------------------------------------\r\n");

			builder.Append("SQL Command" + ":\r\n");
			MakeSQLInfo(builder);
			builder.Append("SQL Error" + ":\r\n" + Message + "\r\n");

			return builder.ToString();
		}

		protected abstract void MakeSQLInfo(StringBuilder builder);

		#region Свойства
		/// <summary>Severity</summary>
		public virtual byte Class { get { return 0; } }

		/// <summary>
		/// Номер строки
		/// </summary>
		public virtual int LineNumber { get { return 0; } }

		/// <summary>Код типа ошибки</summary>
		public virtual int Number { get { return 0; } }

		/// <summary>
		/// Процедура, или SQL-код, вызвавшый ошибку
		/// </summary>
		public virtual string Procedure { get { return CommandText; } }

		/// <summary>Инстанс БД, на котором возникла ошибка</summary>
		public virtual string DatabaseServer { get { return ""; } }

		/// <summary>Код ошибки</summary>
		public virtual byte State { get { return 0; } }

		/// <summary>Дата-время исполнения SQL-оператора</summary>
		public DateTime ExecuteTime { get; private set; }

		/// <summary>Текст SQL-оператора</summary>
		public string CommandText { get; private set; }

		/// <summary>Описание выполняемой операции, если есть</summary>
		public string MethodText { get; private set; }

		/// <summary>Тип SQL-команды</summary>
		protected CommandType CommandType { get; private set; }

		/// <summary>Параметры SQL-оператора</summary>
		public List<DbParameter> Params { get { return m_Params; } }

		/// <summary>Сервер, на котором выполнялась операция</summary>
		public string Server { get; private set; }

		/// <summary>БД, на которой выполнялась операция</summary>
		public string Database { get; private set; }

		/// <summary>Расширенная информация о команде</summary>
		public string AdditionalInfo
		{
			get
			{
				lock (this)
				{
					if (m_AdditionalInfo == "")
						m_AdditionalInfo = MakeAdditionalInfo();
				}
				return m_AdditionalInfo;
			}
		}
		#endregion
	}

	public class CoreSqlException : CoreDbException
	{
		#region Поля
		protected List<System.Data.SqlClient.SqlError> mErrs = new List<System.Data.SqlClient.SqlError>();
		#endregion

		#region Конструкторы
		public CoreSqlException(string message) : base(message) { }
		public CoreSqlException(SqlStatement st, string message) : base(st, message) { }

		public CoreSqlException(Exception innerException)
			: this(innerException.Message, innerException)
		{

		}

		public CoreSqlException(string message, Exception innerException)
			: base(message, innerException)
		{

		}

		public CoreSqlException(SqlStatement st, string message, Exception innerException)
			: base(st, message, innerException)
		{
		}

		public CoreSqlException(SqlStatement st, System.Data.SqlClient.SqlInfoMessageEventArgs msg)
			: base(st, msg.Message)
		{
			Source = msg.Source;
			SetErrors(msg.Errors);
		}

		public CoreSqlException(SqlStatement st, Exception innerException, string method)
			: base(st, innerException, method)
		{
		}
		#endregion

		#region Свойства
		/// <summary>Полный перечень ошибок и сообщений</summary>
		public List<System.Data.SqlClient.SqlError> Errors { get { return mErrs; } }

		/// <summary>Severity</summary>
		public override byte Class { get { return (Errors.Count > 0) ? Errors[0].Class : base.Class; } }

		/// <summary>
		/// Номер строки
		/// </summary>
		public override int LineNumber { get { return (Errors.Count > 0) ? Errors[0].LineNumber : base.LineNumber; } }

		/// <summary>Код типа ошибки</summary>
		public override int Number { get { return (Errors.Count > 0) ? Errors[0].Number : base.Number; } }

		/// <summary>
		/// Процедура, или SQL-код, вызвавшый ошибку
		/// </summary>
		public override string Procedure { get { return (Errors.Count > 0) ? Errors[0].Procedure : base.Procedure; } }

		/// <summary>Инстанс БД, на котором возникла ошибка</summary>
		public override string DatabaseServer { get { return (Errors.Count > 0) ? Errors[0].Server : base.DatabaseServer; } }

		/// <summary>Код ошибки</summary>
		public override byte State { get { return (Errors.Count > 0) ? Errors[0].State : base.State; } }


		#endregion

		protected override void ParseInnerException(Exception innerException)
		{
			if (innerException is System.Data.SqlClient.SqlException)
				SetErrors(((System.Data.SqlClient.SqlException)innerException).Errors);
		}

		private void SetErrors(ICollection err)
		{
			int i = 0;
			foreach (System.Data.SqlClient.SqlError error in err)
			{
				Data.Add(i, error);
				mErrs.Add(error);
				i++;
			}
		}

		protected override void MakeSQLInfo(StringBuilder builder)
		{
			StringBuilder vals = new StringBuilder();
			if (Params.Count > 0)
			{
				builder.Append("DECLARE\r\n\t@__RetVal\tint");
				vals.Append("SELECT\r\n\t@__RetVal\t= NULL");
				foreach (DbParameter par in Params)
				{
					var param = (System.Data.SqlClient.SqlParameter)par;
					builder.Append(",\r\n");
					vals.Append(",\r\n");
					builder.Append("\t" + param.ParameterName + "\t\t");
					vals.Append("\t" + param.ParameterName + "\t\t");
					string tp;
					bool isScale = false;
					bool isPrec = false;
					bool isQuote = false;

				    var prec = param.Precision;
				    var scale = param.Scale;

                    tp = param.SqlDbType.SqlDbTypeInfo(out isPrec, out isScale, out isQuote);

					builder.Append(tp);
					if (isPrec)
					{
						if (isQuote)
						{
							if (prec == 0)
							{
								if (param.Size > 0)
									builder.Append("(" + param.Size);
								else
								{
									if ((param.SqlDbType == SqlDbType.VarChar) || (param.SqlDbType == SqlDbType.NVarChar))
										builder.Append("(max");
									else
										builder.Append("(30"); // стандартный default для sql2005, пусть будет явным образом
								}
							}
						}
						else
						{
							builder.Append("(" + prec);
							if (isScale)
								builder.Append(", " + scale);
						}
						builder.Append(")");
					}
					vals.Append(" = ");
					if ((param.Value == null) || (param.Value == DBNull.Value))
						vals.Append("NULL");
					else
					{
						if (isQuote) vals.Append("'");
						vals.Append(param.Value.ToString());
						if (isQuote) vals.Append("'");
					}
				}
				builder.Append("\r\n" + vals);
			}
			builder.Append("\r\n");
			if (HasStatementInfo)
			{
				if (CommandType == CommandType.StoredProcedure)
					builder.Append("EXEC @__RetVal = ");
				builder.Append(CommandText + "\r\n");
				if (CommandType == CommandType.StoredProcedure)
					builder.Append("\r\nSELECT @__RetVal");
				builder.Append("\r\nGO\r\n");
			}
			else
				builder.Append(Procedure + "\r\n");
			builder.Append("---------------------------------------------\r\n");
		}

	}
}