using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Xml;
using Jeff.Core;
using LiaSoft.Jeff.DataAccess;
using WotDossier.Common.Extensions;
using WotDossier.Common.Reflection;

namespace WotDossier.Dal.DataAccess
{
	/// <summary>
	///		Базовый класс, предоставляющий удобный интерфейс для выполения TSQL-команд
	/// </summary>
	///<example>
	/// <code>
	/// using(DALStatement delParam = 
	///			new DALStatement("DELETE Cfg_Params WHERE ParamID = @ParamID")
	///	) {
	///		delParam.SetIntParameter("@ParamID", aParamID);
	///		delParam.ExecuteNonQuery();
	///	}
	///	// Конструкция using гарантирует вызов Dispose() и освобождение ресурсов.
	///	// Возможно, впрочем, использование try - finally вместо using.
	///	</code>
	///	</example>
	public abstract class DbStatement : DbCommand, ICloneable, IEnumerable<IDataRecord>, IEnumerator<IDataRecord>
	{
		/// <summary>Тайм-аут по умолчанию для выполнения TSQL-операции (в секундах)</summary>
        //public const int DefaultCommandTimeout = 36000;

	    public static int DefaultCommandTimeout { get; set; }

        /// <summary>
        /// Коннект к БД собственный т.е. его надо самим закрыть!
        /// </summary>
        protected bool OwnConnection;

        /// <summary>Объект SqlDataReader, используемый для отображения данных</summary>
        private DbDataReader rdr;

        #region Конструкторы

	    static DbStatement()
	    {
	        DefaultCommandTimeout = 36000;

	    }

        ///// <summary>
        ///// Конструирование объекта <see cref="T:VirtuaObjects.Framework.Core.DAL.DALStatement"></see> по тексту TSQL-команды и транзакции
        ///// </summary>
        ///// <param name="ctx">Экземпляр контекста БД</param>
        ///// <param name="cmdText">Текст SQL-запроса</param>
        ///// <param name="trans">Транзакция</param>
        //protected DbStatement(string connString, string cmdText = "", DbTransaction trans = null)
        //{
        //    try
        //    {
        //        // получение соединения с СУБД
        //        DbConnection conn;
        //        if (trans == null)
        //        {
        //            conn = CreateConnection(connString);
        //            OwnConnection = true;
        //        }
        //        else
        //            conn = trans.Connection;
        //        Command = conn.CreateCommand();
        //        Command.CommandText = cmdText;
        //        Command.CommandTimeout = DefaultCommandTimeout;
        //        Command.Transaction = trans;
        //    }
        //    catch (CoreDbException)
        //    {
        //        throw;
        //    }
        //    catch (Exception e)
        //    {
        //        throw CreateException(DataAccessStrings.ErrCantCreateSQLCommand, e, cmdText);
        //    }
        //}

        /// <summary>
        /// Конструирование объекта <see cref="T:VirtuaObjects.Framework.Core.DAL.DALStatement"></see> по тексту TSQL-команды и транзакции
        /// </summary>
        /// <param name="ctx">Экземпляр контекста БД</param>
        /// <param name="cmdText">Текст SQL-запроса</param>
        /// <param name="trans">Транзакция</param>
        protected DbStatement(JeffDbContext ctx, string cmdText = "", DbTransaction trans = null)
		{
			try
			{
				// получение соединения с СУБД
				DbConnection conn;
				if (trans == null)
				{
					conn = CreateConnection(ctx);
					OwnConnection = true;
				}
				else
					conn = trans.Connection;
				Command = conn.CreateCommand();
				Command.CommandText = cmdText;
				Command.CommandTimeout = DefaultCommandTimeout;
				Command.Transaction = trans;
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
        /// Конструирование объекта <see cref="T:VirtuaObjects.Framework.Core.DAL.DALStatement"></see> по экземпляру SQL-команды
        /// </summary>
        /// <param name="cmd">Экземпляр объекта SQL-команды</param>
        protected DbStatement(DbCommand cmd)
        {
            if(cmd == null)
                throw new ArgumentNullException("cmd", "Ошибка создания DALStatement!!!!");
            Command = cmd;
        }
        #endregion

        #region Получение экземпляров объектов работы с БД: Connection, Command и т.д.
        protected abstract DbConnection CreateConnection(JeffDbContext ctx);

        protected abstract DbDataAdapter CreateDataAdapter();
        #endregion

        #region Вспомогательный свойства
        public string Server { get; private set; }
        public string Database { get; private set; }

        /// <summary>Объект DbCommand, используемый для выполнения запроса</summary>
        protected DbCommand Command { get; set; }
        #endregion

        #region Имплементация DbCommand
        /// <summary>Попытка прервать выполнение <see cref="T:VirtuaObjects.Framework.Core.DAL.DALStatement"></see>.</summary>
        public override void Cancel()
        {
            try
            {
                Command.Cancel();
                //TraceGlobals.DALTracer.TraceInformation("Cancel SQL command", this);
            }
            catch(Exception e)
            {
                throw CreateException(e, "Cancel");
            }
        }

        /// <summary>Sends the <see cref="P:System.Data.SqlClient.SqlCommand.CommandText"></see> to the <see cref="P:System.Data.SqlClient.SqlCommand.Connection"></see>, and builds a <see cref="T:System.Data.SqlClient.SqlDataReader"></see> using one of the <see cref="T:System.Data.CommandBehavior"></see> values.</summary>
        /// <returns>A <see cref="T:System.Data.SqlClient.SqlDataReader"></see> object.</returns>
        /// <param name="behavior">One of the <see cref="T:System.Data.CommandBehavior"></see> values. </param>
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            try
            {
                var rd = Command.ExecuteReader(behavior);
                //TraceGlobals.DALTracer.TraceInformation("ExecuteReader", this);
                return rd;
            }
            catch(Exception e)
            {
                throw CreateException(e, "ExecuteReader");
            }
        }

        /// <summary>Executes a Transact-SQL statement against the connection and returns the number of rows affected.</summary>
        /// <returns>The number of rows affected.</returns>
        /// <exception cref="T:System.Data.SqlClient.SqlException">An exception occurred while executing the command against a locked row. This exception is not generated when you are using Microsoft .NET Framework version 1.0. </exception>
        public override int ExecuteNonQuery()
        {
            try
            {
                int i = Command.ExecuteNonQuery();
                //TraceGlobals.DALTracer.TraceInformation("ExecuteNonQuery", this);
                return i;
            }
            catch(Exception e)
            {
                throw CreateException(e, "ExecuteNonQuery");
            }
        }

        /// <summary>Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.</summary>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        /// <exception cref="T:System.Data.SqlClient.SqlException">An exception occurred while executing the command against a locked row. This exception is not generated when you are using Microsoft .NET Framework version 1.0. </exception>
        public override object ExecuteScalar()
        {
            try
            {
                object o = Command.ExecuteScalar();
                //TraceGlobals.DALTracer.TraceInformation("ExecuteScalar", this);
                return o;
            }
            catch(Exception e)
            {
                throw CreateException(e, "ExecuteScalar");
            }
        }

        /// <summary>Executes the stored procedure, and returns the first column of the first row in the result set returned by the stored procedure. Additional columns or rows are ignored.</summary>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        /// <exception cref="T:System.Data.SqlClient.SqlException">An exception occurred while executing the command against a locked row. This exception is not generated when you are using Microsoft .NET Framework version 1.0. </exception>
        public object ExecuteSPScalar()
        {
            Command.CommandType = CommandType.StoredProcedure;
            return ExecuteScalar();
        }

        /// <summary>Executes stored proceduare, and returns the first record of the first row in the result set returned by the query. Additional rows are ignored.</summary>
        /// <returns>The DbRecord instance encapsulates the first row in the result set, or a empty DALRecord if the result set is empty.</returns>
        /// <exception cref="T:System.Data.SqlClient.SqlException">An exception occurred while executing the command against a locked row. This exception is not generated when you are using Microsoft .NET Framework version 1.0. </exception>
        public DbRecord ExecuteSPRecord()
        {
            return ExecuteRecord(true);
        }

        /// <summary>Executes the query, and returns the first record of the first row in the result set returned by the query. Additional rows are ignored.</summary>
        /// <returns>The DbRecord instance encapsulates the first row in the result set, or a empty DALRecord if the result set is empty.</returns>
        /// <exception cref="T:System.Data.SqlClient.SqlException">An exception occurred while executing the command against a locked row. This exception is not generated when you are using Microsoft .NET Framework version 1.0. </exception>
        public DbRecord ExecuteRecord()
        {
            return ExecuteRecord(false);
        }

        private DbRecord ExecuteRecord(bool isProc)
        {
            try
            {
                if(isProc || !Command.CommandText.Contains(" "))
                    Command.CommandType = CommandType.StoredProcedure;
                
                using (var rd = Command.ExecuteReader())
                {
                    if (!rd.HasRows)
                        return null;
                    if (!rd.Read())
                        return null;

                    //TraceGlobals.DALTracer.TraceInformation("ExecuteRecord", this);
                    return new DbRecord(rd);
                }
            }
            catch (Exception e)
            {
                throw CreateException(e, "ExecuteRecord");
            }
        }

        /// <summary>Creates a prepared version of the command on an instance of SQL Server.</summary>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Data.SqlClient.SqlCommand.Connection"></see> is not set.-or- The <see cref="P:System.Data.SqlClient.SqlCommand.Connection"></see> is not <see cref="M:System.Data.SqlClient.SqlConnection.Open"></see>. </exception>
        public override void Prepare()
        {
            try
            {
                Command.Prepare();
                //TraceGlobals.DALTracer.TraceInformation("Prepare", this);
            }
            catch(Exception e)
            {
                throw CreateException(e, "Prepare");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(rdr != null)
                {
                    rdr.Close();
                    rdr = null;
                }
                if(Command != null)
                {
                    if(OwnConnection)
                    {
                        if(Command.Connection.State == ConnectionState.Open)
                            Command.Connection.Close();
                        Command.Connection.Dispose();
                    }
                    Command.Dispose();
                    Command = null;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>Заполнение DataTаble</summary>
        /// <param name="Table">Таблица, которую заполняем</param>
        public void FillSchema(DataTable Table)
        {
            if(Command != null && Table != null)
            {
                try
                {
                    DbDataAdapter adapter = CreateDataAdapter();
                    adapter.FillSchema(Table, SchemaType.Mapped);
                    //TraceGlobals.DALTracer.TraceInformation("FillSchema", this);
                }
                catch(Exception ex)
                {
                    throw CreateException(ex, "FillSchema (" + Table.TableName + ")");
                }
            }
        }

        /// <summary>
        ///		Получение информации о возвращаемом DataSet
        /// </summary>
        /// <remarks>Формат возвращаемого DataSet подробно описан в доке на SqlDataReader.GetSchemaTable()</remarks>
        /// <returns>См. опсание SqlDataReader.GetSchemaTable()</returns>
        public DataTable GetSchemaTable()
        {
            try
            {
                using(DbDataReader reader = Command.ExecuteReader(CommandBehavior.KeyInfo))
                {
                    DataTable dt = reader.GetSchemaTable();
                    //TraceGlobals.DALTracer.TraceInformation("GetSchemaTable", this);
                    return dt;
                }
            }
            catch(Exception ex)
            {
                throw CreateException(ex, "GetSchemaTable");
            }
        }

        /// <summary>Gets or sets the Transact-SQL statement or stored procedure to execute at the data source.</summary>
        /// <returns>The Transact-SQL statement or stored procedure to execute. The default is an empty string.</returns>
        public override string CommandText
        {
            get { return Command.CommandText; }
            set { Command.CommandText = value; }
        }

        /// <summary>Gets or sets the wait time before terminating the attempt to execute a command and generating an error.</summary>
        /// <returns>The time in seconds to wait for the command to execute. The default is 600 seconds.</returns>
        public override int CommandTimeout
        {
            get { return Command.CommandTimeout; }
            set { Command.CommandTimeout = value; }
        }

        /// <summary>Gets or sets a value indicating how the <see cref="P:System.Data.SqlClient.SqlCommand.CommandText"></see> property is to be interpreted.</summary>
        /// <returns>One of the <see cref="T:System.Data.CommandType"></see> values. The default is Text.</returns>
        /// <exception cref="T:System.ArgumentException">The value was not a valid <see cref="T:System.Data.CommandType"></see>. </exception>
        public override CommandType CommandType
        {
            get { return Command.CommandType; }
            set { Command.CommandType = value; }
        }

        protected override DbConnection DbConnection
        {
            get { return Command.Connection; }
            set { Command.Connection = value; }
        }

        protected override DbParameterCollection DbParameterCollection
        {
            get { return Command.Parameters; }
        }

        protected override DbTransaction DbTransaction
        {
            get { return Command.Transaction; }
            set { Command.Transaction = value; }
        }

        /// <summary>Gets or sets a value indicating whether the command object should be visible in a Windows Form Designer control.</summary>
        /// <returns>A value indicating whether the command object should be visible in a control. The default is true.</returns>
        [DesignOnly(true), Browsable(false), DefaultValue(true), EditorBrowsable(EditorBrowsableState.Never)]
        public override bool DesignTimeVisible
        {
            get { return Command.DesignTimeVisible; }
            set { Command.DesignTimeVisible = value; }
        }

        /// <summary>Gets or sets how command results are applied to the <see cref="T:System.Data.DataRow"></see> when used by the Update method of the <see cref="T:System.Data.Common.DbDataAdapter"></see>.</summary>
        /// <returns>One of the <see cref="T:System.Data.UpdateRowSource"></see> values.</returns>
        public override UpdateRowSource UpdatedRowSource
        {
            get { return Command.UpdatedRowSource; }
            set { Command.UpdatedRowSource = value; }
        }
        #endregion

        #region ICloneable Members
        /// <summary>Создать объект <see cref="T:System.Data.SqlClient.SqlCommand"></see>, который будет копией текущего экземпляра.</summary>
        /// <returns>Новый экземпляр <see cref="T:System.Data.SqlClient.SqlCommand"></see>, который является копией текущего экземпляра.</returns>
        public abstract DbStatement Clone();

        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion

        #region Имплементация методов и св-в SqlCommand
        /// <summary>Sends the <see cref="P:System.Data.SqlClient.SqlCommand.CommandText"></see> to the <see cref="P:System.Data.SqlClient.SqlCommand.Connection"></see> and builds an <see cref="T:System.Xml.XmlReader"></see> object.</summary>
        /// <returns>An <see cref="T:System.Xml.XmlReader"></see> object.</returns>
        /// <exception cref="T:System.Data.SqlClient.SqlException">An exception occurred while executing the command against a locked row. This exception is not generated when you are using Microsoft .NET Framework version 1.0. </exception>
        public abstract XmlReader ExecuteXmlReader();

        /// <summary>Resets the <see cref="P:System.Data.SqlClient.SqlCommand.CommandTimeout"></see> property to its default value.</summary>
        public void ResetCommandTimeout()
        {
            if(DefaultCommandTimeout != Command.CommandTimeout)
            {
                Command.CommandTimeout = DefaultCommandTimeout;
            }
        }
        #endregion

        #region Собственные методы облегчающие работу с DbCommand
        /// <summary>
        /// Исполнение хранимой процедуры для получения ее кода возврата
        /// </summary>
        /// <returns>Код возврата хранимой процедуры или 0</returns>
        public int ExecuteSP()
        {
            Command.CommandType = CommandType.StoredProcedure;
            SetParameter("@return_value", (Int32?)null, ParameterDirection.ReturnValue);
            ExecuteNonQuery();
            return GetOutParam("@return_value", 0);
        }

        /// <summary>
        /// Исполнение хранимой процедуры возврящающей DataReader
        /// </summary>
        /// <returns>DataReader</returns>
        public DbDataReader ExecuteSPReader()
        {
            Command.CommandType = CommandType.StoredProcedure;
            return Command.ExecuteReader();
        }

        public abstract DbStatement CreateChild(string cmdSql);

        /// <summary>Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.</summary>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        /// <exception cref="T:System.Data.SqlClient.SqlException">An exception occurred while executing the command against a locked row. This exception is not generated when you are using Microsoft .NET Framework version 1.0. </exception>
        public T ExecuteScalar<T>(T defaultValue = default(T))
        {
            try
            {
                object o = Command.ExecuteScalar();
                //TraceGlobals.DALTracer.TraceInformation("ExecuteScalar<T>", this);
                return (o == null || o == DBNull.Value) ? defaultValue : (T)o;
            }
            catch(Exception e)
            {
                throw CreateException(e, "ExecuteScalar");
            }
        }

        /// <summary>Executes the stored procedure, and returns the first column of the first row in the result set returned by the stored procedure. Additional columns or rows are ignored.</summary>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        /// <exception cref="T:System.Data.SqlClient.SqlException">An exception occurred while executing the command against a locked row. This exception is not generated when you are using Microsoft .NET Framework version 1.0. </exception>
        public T ExecuteSPScalar<T>()
        {
            Command.CommandType = CommandType.StoredProcedure;
            return ExecuteScalar<T>();
        }
        #endregion

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
        public abstract DbParameter SetParameter(string name, DbType sqlDbType, object value, ParameterDirection direction = ParameterDirection.Input, bool isNullable = false);

        /// <summary>
		/// Установка значения INPUT-параметра
		/// </summary>
		/// <param name="name">Название параметра</param>
		/// <param name="value">Значение параметра</param>
		/// <param name="direction">В каком направлении параметр передается</param>
		/// <returns>Вновь созданный параметр</returns>
		public DbParameter SetParameter<T>(string name, T value, ParameterDirection direction = ParameterDirection.Input)
		{
            return SetParameter(name, typeof(T).GetDbType(), value, direction, typeof(T).IsNullable());
		}

		/// <summary>
		/// Установка значения INPUT-параметра
		/// </summary>
		/// <param name="name">Название параметра</param>
		/// <param name="value">Значение параметра</param>
		/// <param name="direction">В каком направлении параметр передается</param>
		/// <returns>Вновь созданный параметр</returns>
		public DbStatement WithParameter<T>(string name, T value, ParameterDirection direction = ParameterDirection.Input)
		{
			SetParameter(name, value, direction);
			return this;
		}
		#endregion

        #region Получение значений Out-параметров
		///// <summary>
		///// Получение значения Out параметра
		///// </summary>
		///// <param name="name">Название параметра</param>
		///// <returns>Значение параметра</returns>
		//public object GetOutParam(string name)
		//{
		//	return Command.Parameters[name].Value;
		//}

		///// <summary>
		///// Получение значения Out параметра
		///// </summary>
		///// <param name="name">Название параметра</param>
		///// <param name="nullValue">Значение, заменяющее NULL</param>
		///// <returns>Значение параметра</returns>
		//public object GetOutParam(string name, object nullValue)
		//{
		//	if (Convert.IsDBNull(Command.Parameters[name].Value))
		//	{
		//		return nullValue;
		//	}
		//	return Command.Parameters[name].Value;
		//}

        /// <summary>
        /// Получение значения Out параметра
        /// </summary>
        /// <param name="name">Название параметра</param>
        /// <param name="nullValue">Значение, заменяющее NULL</param>
        /// <returns>Значение параметра</returns>
        public T GetOutParam<T>(string name, T nullValue = default(T))
        {
            return ConvertExtensions.To(Command.Parameters[name].Value, nullValue);
        }

		///// <summary>
		///// Получение значения целочисленного Out параметра
		///// </summary>
		///// <param name="name">Название параметра</param>
		///// <param name="nullValue">Значение, заменяющее NULL</param>
		///// <returns>Значение параметра</returns>
		//public Int32 GetOutParam(string name, Int32 nullValue)
		//{
		//	if(Convert.IsDBNull(Command.Parameters[name].Value))
		//	{
		//		return nullValue;
		//	}
		//	return Convert.ToInt32(Command.Parameters[name].Value);
		//}

        #endregion

        #region Реализация упрошенного доступа к результирующему дата-сету
        /// <summary>Метод определен для соответствия IEnumerable</summary>
        /// <returns>this</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

		/// <summary>Метод определен для соответствия IEnumerable</summary>
		/// <returns>this</returns>
		IEnumerator<IDataRecord> IEnumerable<IDataRecord>.GetEnumerator()
		{
			return this;
		}

        /// <summary>
        /// Перемещение к следующей записи ResultSet
        /// </summary>
        /// <returns>true, если переход на следующую запись удался. False - если 
        /// достигнут конец ResultSet</returns>
        public bool MoveNext()
        {
            if(rdr != null)
            {
                // не проверять на HasRows! В результате могут быть пропущены некоторые SQL-ошибки!
                try
                {
                    return rdr.Read();
                }
                catch(Exception ex)
                {
                    throw CreateException(ex, "MoveNext");
                }
            }
            return false;
        }

        /// <summary>
        /// Получить инфу о наличии записей в RecordSet'е
        /// </summary>
        /// <returns>True, если имеются записи. False - если RecordSet пустой</returns>
        public bool HasRows
        {
            get { return (rdr != null) ? rdr.HasRows : false; }
        }

        /// <summary>
        /// Операция смысла не имеет, но метод следует определить для соответствия IEnumerator
        /// </summary>
        void IEnumerator.Reset()
        {
            throw new InvalidOperationException();
        }

        /// <summary>Переропределение метода IEnumerator</summary>
        object IEnumerator.Current
        {
            get { return rdr; }
        }

        /// <summary>Текущая запись</summary>
		IDataRecord IEnumerator<IDataRecord>.Current
        {
            get { return rdr; }
        }

        /// <summary>
        /// Выполнение SELECT-оператора или процедуры, возвращающего ResultSet
        /// </summary>
        /// <remarks>Перед первым чтением данных необходим вызов MoveNext()</remarks>
        public void Open()
        {
            if (!Command.CommandText.Contains(" ")) 
                Command.CommandType = CommandType.StoredProcedure;
            
            rdr = ExecuteReader();
            
            if(rdr == null)
            {
                string res = Command.CommandType == CommandType.StoredProcedure ? "Can't execute Stored procedure '{0}'": "Can't execute sql statement '{0}'";
                throw CreateException(res, null, Command.CommandText);
            }
        }

		/// <summary>
		/// Выполнение SELECT-оператора или процедуры, возвращающего ResultSet в виду удобном для последующих вызовов
		/// </summary>
		public DbStatement OpenReader()
		{
			Open();
			return this;
		}
        #endregion

        #region Получение нужного исключения
		protected abstract CoreDbException CreateException(string errorText, Exception inner, params object[] parms);
		protected abstract CoreDbException CreateException(Exception inner, string method);
        #endregion

  //      #region Получение строки на выполнение
  //      protected abstract string GetParamTypeInfo(DbParameter param, ref bool isPrec, ref int Prec, ref bool isScale, ref int Scale, ref bool isQuote);
        
		//public string MakeCommandString()
  //      {
  //          var builder = new StringBuilder();
  //          StringBuilder vals = new StringBuilder();
  //          if (Parameters.Count > 0)
  //          {
  //              builder.Append("DECLARE\r\n\t@__RetVal\tint");
  //              vals.Append("SELECT\r\n\t@__RetVal\t= NULL");
  //              foreach (DbParameter param in Parameters)
  //              {
  //                  builder.Append(",\r\n");
  //                  vals.Append(",\r\n");
  //                  builder.Append("\t" + param.ParameterName + "\t\t");
  //                  vals.Append("\t" + param.ParameterName + "\t\t");
                    
  //                  bool isScale = false;
  //                  bool isPrec = false;
  //                  bool quote = false;
  //                  int Prec = 0, Scale = 0;
  //                  var tp = GetParamTypeInfo(param, ref isPrec, ref Prec, ref isScale, ref Scale, ref quote);
                    
  //                  builder.Append(tp);
  //                  if (isPrec)
  //                  {
  //                      if (quote)
  //                      {
  //                          if (Prec == 0)
  //                          {
  //                              if (param.Size > 0 && param.Size < Int32.MaxValue)
  //                                  builder.Append("(" + param.Size);
  //                              else
  //                              {
  //                                  if ((param.DbType == DbType.AnsiString || param.DbType == DbType.String))
  //                                      builder.Append("(max");
  //                                  else
  //                                      builder.Append("(30"); // стандартный default для sql2005, пусть будет явным образом
  //                              }
  //                          }
  //                      }
  //                      else
  //                      {
  //                          builder.Append("(" + Prec);
  //                          if (isScale)
  //                              builder.Append(", " + Scale);
  //                      }
  //                      builder.Append(")");
  //                  }
  //                  vals.Append(" = ");
  //                  if ((param.Value == null) || (param.Value == DBNull.Value))
  //                      vals.Append("NULL");
  //                  else
  //                  {
  //                      if (quote) vals.Append("'");

	 //                   var str = string.Empty;
		//				if (param.DbType == DbType.Boolean)
		//				{
		//					str = param.Value.To(0).ToString();
		//				}
		//				else if(param.DbType == DbType.Date || param.DbType == DbType.DateTime || param.DbType == DbType.Time ||
		//				        param.DbType == DbType.DateTime2)
		//				{
		//					str = param.Value.To<DateTime>().ToString("yyyy-MM-dd HH:mm:ss");
		//				}
		//				else
		//					str = param.Value.ToString();

  //                      vals.Append(str);
  //                      if (quote) vals.Append("'");
  //                  }
  //              }
  //              builder.Append("\r\n" + vals);
  //          }
  //          builder.Append("\r\n");
  //          if (CommandType == CommandType.StoredProcedure)
  //              builder.Append("EXEC @__RetVal = ");
  //          builder.Append(CommandText + "\r\n");
  //          if (CommandType == CommandType.StoredProcedure)
  //              builder.Append("\r\nSELECT @__RetVal");
  //          builder.Append("\r\nGO\r\n");

  //          return builder.ToString();
  //      }
  //      #endregion
	}
}
