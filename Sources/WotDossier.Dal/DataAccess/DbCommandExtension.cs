using System;
using System.Data;
using System.Data.Common;
using System.Text;
using WotDossier.Common.Extensions;
using WotDossier.Dal.DataAccess;

namespace LiaSoft.Jeff.DataAccess
{
    public static class DbCommandExtension
    {
        private static byte get_Precision(DbParameter param)
        {
            return ((IDbDataParameter)param).Precision;
        }

        private static byte get_Scale(DbParameter param)
        {
            return ((IDbDataParameter)param).Scale;
        }

        public static string MakeCommandString(this IDbCommand cmd)
        {
            {
                var builder = new StringBuilder();
                StringBuilder vals = new StringBuilder();
                if (cmd.Parameters.Count > 0)
                {
                    builder.Append("DECLARE\r\n\t@__RetVal\tint");
                    vals.Append("SELECT\r\n\t@__RetVal\t= NULL");
                    foreach (DbParameter param in cmd.Parameters)
                    {
                        builder.Append(",\r\n");
                        vals.Append(",\r\n");
                        builder.Append("\t" + param.ParameterName + "\t\t");
                        vals.Append("\t" + param.ParameterName + "\t\t");

                        bool isScale = false;
                        bool isPrec = false;
                        bool quote = false;
                        int precision = get_Precision(param), scale = get_Scale(param);
                        var tp = param.DbType.DbTypeInfo(out isPrec, out isScale, out quote);

                        builder.Append(tp);
                        if (isPrec)
                        {
                            if (quote)
                            {
                                if (precision == 0)
                                {
                                    if (param.Size > 0 && param.Size < Int32.MaxValue)
                                        builder.Append("(" + param.Size);
                                    else
                                    {
                                        if ((param.DbType == DbType.AnsiString || param.DbType == DbType.String))
                                            builder.Append("(max");
                                        else
                                            builder.Append("(30"); // стандартный default для sql2005, пусть будет явным образом
                                    }
                                }
                            }
                            else
                            {
                                builder.Append("(" + precision);
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
                            if (quote) vals.Append("'");

                            var str = string.Empty;
                            if (param.DbType == DbType.Boolean)
                            {
                                str = param.Value.To(0).ToString();
                            }
                            else if (param.DbType == DbType.Date || param.DbType == DbType.DateTime || param.DbType == DbType.Time ||
                                    param.DbType == DbType.DateTime2)
                            {
                                str = param.Value.To<DateTime>().ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else
                                str = param.Value.ToString();

                            vals.Append(str);
                            if (quote) vals.Append("'");
                        }
                    }
                    builder.Append("\r\n" + vals);
                }
                builder.Append("\r\n");
                if (cmd.CommandType == CommandType.StoredProcedure)
                    builder.Append("EXEC @__RetVal = ");
                builder.Append(cmd.CommandText + "\r\n");
                if (cmd.CommandType == CommandType.StoredProcedure)
                    builder.Append("\r\nSELECT @__RetVal");
                builder.Append("\r\nGO\r\n");

                return builder.ToString();
            }
        }
    }
}