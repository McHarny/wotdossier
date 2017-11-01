using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Xml;
using System.Xml.Linq;
using Jeff.Core;
using WotDossier.Dal.DataAccess;
using WotDossier.Dal.DataAccess.Providers.SQLite;

namespace LiaSoft.Jeff.DataAccess
{
    public class SQLiteStatement : DbStatement
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
        internal SQLiteStatement(JeffDbContext ctx, string cmdText = "", DbTransaction trans = null)
			: base(ctx, cmdText, trans)
		{
        }

        /// <summary>
        /// Конструирование объекта <see cref="T:VirtuaObjects.Framework.Core.DAL.SqlStatement"></see> по экземпляру SQL-команды
        /// </summary>
        /// <param name="cmd">Экземпляр объекта SQL-команды</param>
        private SQLiteStatement(DbCommand cmd) : base(cmd) { }
        #endregion

        #region Получение экземпляров объектов работы с БД: Connection, Command и т.д.
        protected override DbConnection CreateConnection(JeffDbContext ctx)
        {
            var cn = new SQLiteConnection(ctx.ConnectionString);
            cn.Open();

            if (ctx.ConnectionCallback != null)
                ctx.ConnectionCallback(cn);

            return cn;
        }

        protected override DbDataAdapter CreateDataAdapter()
        {
            return new SQLiteDataAdapter((SQLiteCommand)Command);
        }
        #endregion

        #region Получение нужного исключения
        protected override CoreDbException CreateException(string errorText, Exception inner, params object[] parms)
        {
            return new CoreSQLiteException(this, string.Format(errorText, parms), inner);
        }

        protected override CoreDbException CreateException(Exception inner, string method)
        {
            return new CoreSQLiteException(this, inner, method);
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
            return new SQLiteStatement((SQLiteCommand)((SQLiteCommand)Command).Clone());
        }
        #endregion

        #region Имплементация DbCommand
        /// <summary>Получить новый экземпляр параметра <see cref="T:System.Data.Common.DbParameter"></see></summary>
        /// <returns>Объект <see cref="T:System.Data.SqlClient.SqlParameter"></see></returns>
        /// <filterpriority>2</filterpriority>
        protected override DbParameter CreateDbParameter()
        {
            return ((SQLiteCommand)Command).CreateParameter();
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
            throw new NotImplementedException("TODO");
        }
        #endregion

        public override DbStatement CreateChild(string cmdSql)
        {
            var cmd = (SQLiteCommand)((SQLiteCommand)Command).Clone();
            cmd.CommandText = cmdSql;
            return new SQLiteStatement(cmd);
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
            var param = ((SQLiteCommand)Command).Parameters.Add(name, sqlDbType);
            param.Direction = direction;
            param.IsNullable = isNullable;
            if (value != null)
            {
                var vl = value;
                if (value is XNode)
                    vl = value.ToString();

                if (value is IDbParameterValue)
                    vl = ((IDbParameterValue)value).GetParameterValue();

                param.Value = vl;
            }
            else if (isNullable)
                param.Value = DBNull.Value;

            return param;
        }
        #endregion

        //#region Получение строки на выполнение
        //protected override string GetParamTypeInfo(DbParameter param, ref bool isPrec, ref int Prec, ref bool isScale, ref int Scale, ref bool isQuote)
        //{
        //    return param.DbType.DbTypeInfo(ref isPrec, ref isScale, ref isQuote);
        //}
        //#endregion
    }
}