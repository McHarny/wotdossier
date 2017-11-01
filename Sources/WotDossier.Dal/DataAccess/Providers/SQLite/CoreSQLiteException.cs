using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Text;
using Jeff.Core;
using LiaSoft.Jeff.DataAccess;

namespace WotDossier.Dal.DataAccess.Providers.SQLite
{
    public class CoreSQLiteException : CoreDbException
    {
        #region Поля
        private SQLiteErrorCode errorCode;
        #endregion

        #region Конструкторы
        public CoreSQLiteException(string message) : base(message) { }
        public CoreSQLiteException(SQLiteStatement st, string message) : base(st, message) { }

        public CoreSQLiteException(Exception innerException)
			: this(innerException.Message, innerException)
		{

        }

        public CoreSQLiteException(string message, Exception innerException)
			: base(message, innerException)
		{

        }

        public CoreSQLiteException(SQLiteStatement st, string message, Exception innerException)
			: base(st, message, innerException)
		{
        }

        public CoreSQLiteException(SQLiteStatement st, Exception innerException, string method)
			: base(st, innerException, method)
		{
        }
        #endregion

        public SQLiteErrorCode ResultCode
        {
            get
            {
                return errorCode;
            }
        }



        protected override void ParseInnerException(Exception innerException)
        {
            if (innerException is SQLiteException)
                errorCode = ((SQLiteException)innerException).ResultCode;
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
                    var param = (SQLiteParameter)par;
                    builder.Append(",\r\n");
                    vals.Append(",\r\n");
                    builder.Append("\t" + param.ParameterName + "\t\t");
                    vals.Append("\t" + param.ParameterName + "\t\t");
                    string tp;
                    bool isScale = false;
                    bool isPrec = false;
                    bool isQuote = false;

                    int prec = 0;
                    int scale = 0;

                    tp = param.DbType.DbTypeInfo(out isPrec, out isScale, out isQuote);

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
                                    if ((param.DbType == DbType.String) || (param.DbType == DbType.AnsiString))
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

        protected override string MakeAdditionalMessage()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("---------------------------------------------\r\n");
            builder.Append("SQLite ErrorCode :" + ":\t\t" + ResultCode + "\r\n");
            builder.Append(base.MakeAdditionalMessage());

            return builder.ToString();
        }
    }
}