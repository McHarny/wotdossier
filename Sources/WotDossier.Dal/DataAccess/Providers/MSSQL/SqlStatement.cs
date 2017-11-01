using System;
using System.Data;
using System.Data.Common;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Linq;
using Jeff.Core;
using LiaSoft.Jeff.DataAccess;

namespace WotDossier.Dal.DataAccess.Providers.MSSQL
{
	public class SqlStatement : DbStatement
	{
        #region Конструкторы
        /// <summary>
        /// Конструирование объекта <see cref="T:VirtuaObjects.Framework.Core.DAL.SqlStatement"></see> по тексту TSQL-команды и транзакции
        /// </summary>
        /// <param name="ctx">Экземпляр контекста БД</param>
        /// <param name="cmdText">Текст SQL-запроса</param>
        /// <param name="trans">Транзакция</param>
        /// <exception cref="FrameworkException"><c>CoreException</c>.</exception>
        /// <exception cref="CoreDALException"><c>CoreDALException</c>.</exception>
        internal SqlStatement(JeffDbContext ctx, string cmdText = "", DbTransaction trans = null)
			: base(ctx, cmdText, trans)
		{
			try
			{
				((SqlConnection)Command.Connection).InfoMessage += Connection_InfoMessage;
			}
			catch (CoreDbException)
			{
				throw;
			}
			catch (Exception e)
			{
				throw CreateException("Can't create sql command {0}", e, cmdText);
			}
		}

		/// <summary>
		/// Конструирование объекта <see cref="T:VirtuaObjects.Framework.Core.DAL.SqlStatement"></see> по экземпляру SQL-команды
		/// </summary>
		/// <param name="cmd">Экземпляр объекта SQL-команды</param>
		private SqlStatement(DbCommand cmd) : base(cmd) { }
        #endregion

        #region Получение экземпляров объектов работы с БД: Connection, Command и т.д.
        protected override DbConnection CreateConnection(JeffDbContext ctx)
		{
			var cn = new SqlConnection(ctx.ConnectionString);
			cn.Open();

		    ctx.ConnectionCallback?.Invoke(cn);

		    return cn;
		}

		protected override DbDataAdapter CreateDataAdapter()
		{
			return new SqlDataAdapter((SqlCommand)Command);
		}
		#endregion

		#region Получение нужного исключения
		private void Connection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
		{
			foreach (SqlError se in e.Errors)
			{
				if (se.Class > 10) // в соответствии с документацией (например, по SqlException) эти значения severity соотв. ошибкам
				{
					CoreDbException sqe = new CoreSqlException(this, e);
					throw sqe;
				}
				//TraceGlobals.SQLTracer.TraceInformation("Connection Info Message Class: {0}, Number: {1}, LineNumber: {2}, State: {3}, Procedure: {4}, Server: {5}, Source: {6}, Message: {7}", se.Class, se.Number, se.LineNumber, se.State, se.Procedure, se.Server, se.Source, se.Message);
			}
		}

		protected override CoreDbException CreateException(string errorText, Exception inner, params object[] parms)
		{
			return new CoreSqlException(this, string.Format(errorText, parms), inner);
		}

		protected override CoreDbException CreateException(Exception inner, string method)
		{
			return new CoreSqlException(this, inner, method);
		}
		#endregion

		#region ICloneable Members
		/// <summary>Создать объект <see cref="T:System.Data.SqlClient.SqlCommand"></see>, который будет копией текущего экземпляра.</summary>
		/// <returns>Новый экземпляр <see cref="T:System.Data.SqlClient.SqlCommand"></see>, который является копией текущего экземпляра.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		/// <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*" />
		/// </PermissionSet>
		public override DbStatement Clone()
		{
			return new SqlStatement(((SqlCommand)Command).Clone());
		}
		#endregion

		#region Имплементация DbCommand
		/// <summary>Получить новый экземпляр параметра <see cref="T:System.Data.Common.DbParameter"></see></summary>
		/// <returns>Объект <see cref="T:System.Data.SqlClient.SqlParameter"></see></returns>
		/// <filterpriority>2</filterpriority>
		protected override DbParameter CreateDbParameter()
		{
			return ((SqlCommand)Command).CreateParameter();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (Command != null && OwnConnection)
				{
					((SqlConnection)Command.Connection).InfoMessage -= Connection_InfoMessage;
				}
			}
			base.Dispose(disposing);
		}
		#endregion

		#region Имплементация методов и св-в SqlCommand
		/// <summary>Sends the <see cref="P:System.Data.SqlClient.SqlCommand.CommandText"></see> to the <see cref="P:System.Data.SqlClient.SqlCommand.Connection"></see> and builds an <see cref="T:System.Xml.XmlReader"></see> object.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlReader"></see> object.</returns>
		/// <exception cref="T:System.Data.SqlClient.SqlException">An exception occurred while executing the command against a locked row. This exception is not generated when you are using Microsoft .NET Framework version 1.0. </exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		/// <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// <IPermission class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence, ControlPolicy, ControlAppDomain" />
		/// <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// <IPermission class="System.Data.SqlClient.SqlClientPermission, System.Data, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override XmlReader ExecuteXmlReader()
		{
			try
			{
				XmlReader xr = ((SqlCommand)Command).ExecuteXmlReader();
				return xr;
			}
			catch (Exception e)
			{
				throw CreateException(e, "ExecuteXmlReader");
			}
		}

		/// <summary>Gets or sets a value that specifies the <see cref="T:System.Data.Sql.SqlNotificationRequest"></see> object bound to this command.</summary>
		/// <returns>When set to null (default), no notification should be requested.</returns>
		/// <filterpriority>2</filterpriority>
		public SqlNotificationRequest Notification
		{
			get { return ((SqlCommand)Command).Notification; }
			set { ((SqlCommand)Command).Notification = value; }
		}

		/// <summary>Gets or sets a value indicating whether the application should automatically receive query notifications from a common <see cref="T:System.Data.SqlClient.SqlDependency"></see> object.</summary>
		/// <returns>true if the application should automatically receive query notifications; otherwise false. The default value is true.</returns>
		/// <filterpriority>2</filterpriority>
		public bool NotificationAutoEnlist
		{
			get { return ((SqlCommand)Command).NotificationAutoEnlist; }
			set { ((SqlCommand)Command).NotificationAutoEnlist = value; }
		}
		#endregion

		public override DbStatement CreateChild(string cmdSql)
		{
			var cmd = ((SqlCommand)Command).Clone();
			cmd.CommandText = cmdSql;
			return new SqlStatement(cmd);
		}

		#region Установка параметров
		/// <summary>
		/// Добавление параметра команды
		/// </summary>
		/// <param name="name">Название параметра</param>
		/// <param name="sqlDbType">Тип параметра</param>
		/// <param name="value">Значение параметра</param>
		/// <param name="direction">В каком направлении параметр передается</param>
		/// <param name="isNullable">Параметр может принимать NULL значения</param>
		/// <returns>Новый параметр команды</returns>
		public override DbParameter SetParameter(string name, DbType sqlDbType, object value, ParameterDirection direction = ParameterDirection.Input, bool isNullable = false)
		{
			SqlParameter param = ((SqlCommand)Command).Parameters.Add(name, sqlDbType.ToSqlType());
			param.Direction = direction;
			param.IsNullable = isNullable;
			if(value != null)
			{
				var vl = value;
				if(value is XNode)
					vl = value.ToString();

			    if (value is IDbParameterValue)
			        vl = ((IDbParameterValue) value).GetParameterValue();

				param.Value = vl;
			}
			else if (isNullable)
				param.Value = DBNull.Value;

			return param;
		}
		#endregion

		//#region Получение строки на выполнение

  //      protected override string GetParamTypeInfo(DbParameter param, ref bool isPrec, ref int Prec, ref bool isScale, ref int Scale, ref bool isQuote)
  //      {
  //          var parm = (SqlParameter) param;
  //          Prec = parm.Precision;
  //          Scale = parm.Scale;
  //          return parm.SqlDbType.SqlDbTypeInfo(ref isPrec, ref isScale, ref isQuote);
  //      }
		//#endregion
	}
}
