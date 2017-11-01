using System;
using System.Data;
using System.Xml.Linq;
using WotDossier.Common.Reflection;

namespace WotDossier.Dal.DataAccess
{
    public static class DbTypeExtensions
    {
        public static SqlDbType ToSqlType(this DbType tp)
        {
            switch (tp)
            {
                case DbType.AnsiString:
                    return SqlDbType.VarChar;
                case DbType.Binary:
                    return SqlDbType.VarBinary;
                case DbType.Byte:
                    return SqlDbType.TinyInt;
                case DbType.Boolean:
                    return SqlDbType.Bit;
                case DbType.Currency:
                    return SqlDbType.Money;
                case DbType.Date:
                    return SqlDbType.Date;
                case DbType.DateTime:
                    return SqlDbType.DateTime;
                case DbType.Decimal:
                    return SqlDbType.Decimal;
                case DbType.Double:
                    return SqlDbType.Float;
                case DbType.Guid:
                    return SqlDbType.UniqueIdentifier;
                case DbType.Int16:
                    return SqlDbType.SmallInt;
                case DbType.Int32:
                    return SqlDbType.Int;
                case DbType.Int64:
                    return SqlDbType.BigInt;
                case DbType.Object:
                    return SqlDbType.Variant;
                case DbType.Single:
                    return SqlDbType.Real;
                case DbType.String:
                    return SqlDbType.NVarChar;
                case DbType.Time:
                    return SqlDbType.DateTime;
                case DbType.AnsiStringFixedLength:
                    return SqlDbType.Char;
                case DbType.StringFixedLength:
                    return SqlDbType.NChar;
                case DbType.Xml:
                    return SqlDbType.Xml;
                case DbType.DateTime2:
                    return SqlDbType.DateTime2;
                case DbType.DateTimeOffset:
                    return SqlDbType.DateTimeOffset;
                default:
                    throw new ArgumentOutOfRangeException("tp");
            }
        }

        public static DbType ToDbType(this SqlDbType tp)
        {
            switch (tp)
            {
                case SqlDbType.BigInt:
                    return DbType.Int64;
                case SqlDbType.Binary:
                    return DbType.Binary;
                case SqlDbType.Bit:
                    return DbType.Boolean;
                case SqlDbType.Char:
                    return DbType.AnsiStringFixedLength;
                case SqlDbType.DateTime:
                    return DbType.DateTime;
                case SqlDbType.Decimal:
                    return DbType.Decimal;
                case SqlDbType.Float:
                    return DbType.Single;
                case SqlDbType.Image:
                    return DbType.Binary;
                case SqlDbType.Int:
                    return DbType.Int32;
                case SqlDbType.Money:
                    return DbType.Currency;
                case SqlDbType.NChar:
                    return DbType.StringFixedLength;
                case SqlDbType.NText:
                    return DbType.String;
                case SqlDbType.NVarChar:
                    return DbType.String;
                case SqlDbType.Real:
                    return DbType.Double;
                case SqlDbType.UniqueIdentifier:
                    return DbType.Guid;
                case SqlDbType.SmallDateTime:
                    return DbType.DateTime;
                case SqlDbType.SmallInt:
                    return DbType.Int16;
                case SqlDbType.SmallMoney:
                    return DbType.Currency;
                case SqlDbType.Text:
                    return DbType.AnsiString;
                case SqlDbType.Timestamp:
                    return DbType.Binary;
                case SqlDbType.TinyInt:
                    return DbType.Byte;
                case SqlDbType.VarBinary:
                    return DbType.Binary;
                case SqlDbType.VarChar:
                    return DbType.AnsiString;
                case SqlDbType.Variant:
                    return DbType.Object;
                case SqlDbType.Xml:
                    return DbType.Xml;
                case SqlDbType.Udt:
                    return DbType.Object;
                case SqlDbType.Structured:
                    return DbType.Object;
                case SqlDbType.Date:
                    return DbType.Date;
                case SqlDbType.Time:
                    return DbType.Time;
                case SqlDbType.DateTime2:
                    return DbType.DateTime2;
                case SqlDbType.DateTimeOffset:
                    return DbType.DateTimeOffset;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tp), tp, null);
            }
        }

        public static Type GetNetType(this DbType dbType)
        {
            switch (dbType)
            {
                case DbType.AnsiString:
                    return typeof(String);
                case DbType.Binary:
                    return typeof(Byte[]);
                case DbType.Byte:
                    return typeof(Byte);
                case DbType.Boolean:
                    return typeof(Boolean);
                case DbType.Currency:
                    return typeof(Decimal);
                case DbType.Date:
                case DbType.DateTime:
                    return typeof(DateTime);
                case DbType.Decimal:
                    return typeof(Decimal);
                case DbType.Double:
                    return typeof(Double);
                case DbType.Guid:
                    return typeof(Guid);
                case DbType.Int16:
                    return typeof(Int16);
                case DbType.Int32:
                    return typeof(Int32);
                case DbType.Int64:
                    return typeof(Int64);
                case DbType.Object:
                    return typeof(Object);
                case DbType.SByte:
                    return typeof(SByte);
                case DbType.Single:
                    return typeof(Single);
                case DbType.String:
                    return typeof(String);
                case DbType.Time:
                    return typeof(DateTime);
                case DbType.UInt16:
                    return typeof(UInt16);
                case DbType.UInt32:
                    return typeof(UInt32);
                case DbType.UInt64:
                    return typeof(UInt64);
                case DbType.VarNumeric:
                    return typeof(Double);
                case DbType.AnsiStringFixedLength:
                    return typeof(String);
                case DbType.StringFixedLength:
                    return typeof(String);
                case DbType.Xml:
                    return typeof(String);
                case DbType.DateTime2:
                    return typeof(DateTime);
                case DbType.DateTimeOffset:
                    return typeof(TimeSpan);
                default:
                    throw new ArgumentOutOfRangeException("dbType");
            }
        }

        /// <summary>Соответствие между <see cref="Type"/> и <see cref="DbType"/></summary>
        /// <param name="type">Тип, для которого ищется соответствие</param>
        /// <returns>Соответствующий парaметру <see cref="DbType"/></returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>ArgumentOutOfRangeException</c>.</exception>
        public static DbType GetDbType(this Type type)
        {
            if (type.IsNullable())
            {
                type = type.UnderlyingTypeOf();
            }
            if (type == typeof(string))
                return DbType.String;
            if (type == typeof(Guid))
                return DbType.Guid;

            if (type == typeof(Int32))
                return DbType.Int32;
            if (type == typeof(Int64))
                return DbType.Int64;
            if (type == typeof(Int16))
                return DbType.Int16;
            if (type == typeof(Byte))
                return DbType.Byte;
            if (type == typeof(SByte))
                return DbType.SByte;

            if (type == typeof(UInt32))
                return DbType.UInt32;
            if (type == typeof(UInt64))
                return DbType.UInt64;
            if (type == typeof(UInt16))
                return DbType.UInt16;

            if (type == typeof(DateTime))
                return DbType.DateTime;

            if (type == typeof(TimeSpan))
                return DbType.DateTime;

            if (type == typeof(Single))
                return DbType.Single;
            if (type == typeof(Double))
                return DbType.Double;
            if (type == typeof(Decimal))
                return DbType.Decimal;

            if (type == typeof(Byte[]))
                return DbType.Binary;

            if (type == typeof(Boolean))
                return DbType.Boolean;

            if (type == typeof(XNode) || type.IsSubclassOf(typeof(XNode)))
                return DbType.Xml;

            throw new ArgumentOutOfRangeException("type");
        }

        public static string SqlDbTypeInfo(this SqlDbType parm, out bool isPrec, out bool isScale, out bool isQuote)
        {
            string tp = String.Empty;
            isScale = false;
            isPrec = false;
            isQuote = false;

            switch (parm)
            {
                case SqlDbType.BigInt:
                    tp = "bigint";
                    break;
                case SqlDbType.Bit:
                    tp = "bit";
                    break;
                case SqlDbType.Int:
                    tp = "int";
                    break;
                case SqlDbType.SmallDateTime:
                    tp = "smalldatetime";
                    isQuote = true;
                    break;
                case SqlDbType.Date:
                    tp = "date";
                    isQuote = true;
                    break;
                case SqlDbType.DateTime2:
                    tp = "datetime2";
                    isQuote = true;
                    break;
                case SqlDbType.DateTimeOffset:
                    tp = "datetimeoffset";
                    isQuote = true;
                    break;
                case SqlDbType.Time:
                    tp = "time";
                    isQuote = true;
                    break;
                case SqlDbType.SmallInt:
                    tp = "smallint";
                    break;
                case SqlDbType.DateTime:
                    tp = "datetime";
                    isQuote = true;
                    break;
                case SqlDbType.Timestamp:
                    tp = "timestamp";
                    isQuote = true;
                    break;
                case SqlDbType.TinyInt:
                    tp = "tinyint";
                    break;

                case SqlDbType.Char:
                    tp = "char";
                    isPrec = true;
                    isQuote = true;
                    break;
                case SqlDbType.NChar:
                    tp = "nchar";
                    isPrec = true;
                    isQuote = true;
                    break;
                case SqlDbType.NVarChar:
                    tp = "nvarchar";
                    isPrec = true;
                    isQuote = true;
                    break;
                case SqlDbType.VarChar:
                    tp = "varchar";
                    isPrec = true;
                    isQuote = true;
                    break;
                case SqlDbType.Decimal:
                    tp = "decimal";
                    isPrec = true;
                    isScale = true;
                    break;
                case SqlDbType.Float:
                    tp = "float";
                    isPrec = true;
                    break;
                case SqlDbType.Money:
                    tp = "money";
                    break;
                case SqlDbType.Real:
                    tp = "real";
                    break;
                case SqlDbType.SmallMoney:
                    tp = "smallmoney";
                    break;

                case SqlDbType.Image:
                    tp = "image";
                    break;
                case SqlDbType.NText:
                    tp = "ntext";
                    break;
                case SqlDbType.Text:
                    tp = "text";
                    break;
                case SqlDbType.UniqueIdentifier:
                    tp = "uniqueidentifier";
                    isQuote = true;
                    break;
                case SqlDbType.Binary:
                    tp = "binary";
                    break;
                case SqlDbType.VarBinary:
                    tp = "varbinary";
                    break;
                case SqlDbType.Variant:
                    tp = "uniqueidentifier";
                    isQuote = true;
                    break;
                case SqlDbType.Xml:
                    tp = "xml";
                    isQuote = true;
                    break;
                case SqlDbType.Udt:
                    tp = "udt";
                    break;
                default:
                    tp = "unknown";
                    break;
            }
            return tp;
        }

        public static string DbTypeInfo(this DbType parm, out bool isPrec, out bool isScale, out bool isQuote)
        {
            string tp = String.Empty;
            isScale = false;
            isPrec = false;
            isQuote = false;

            switch (parm)
            {
                case DbType.Int64:
                    tp = "bigint";
                    break;
                case DbType.Boolean:
                    tp = "bit";
                    break;
                case DbType.Int32:
                    tp = "int";
                    break;
                case DbType.Date:
                    tp = "date";
                    isQuote = true;
                    break;
                case DbType.DateTime2:
                    tp = "datetime2";
                    isQuote = true;
                    break;
                case DbType.DateTimeOffset:
                    tp = "datetimeoffset";
                    isQuote = true;
                    break;
                case DbType.Time:
                    tp = "time";
                    isQuote = true;
                    break;
                case DbType.Int16:
                    tp = "smallint";
                    break;
                case DbType.DateTime:
                    tp = "datetime";
                    isQuote = true;
                    break;
                case DbType.Byte:
                    tp = "tinyint";
                    break;

                case DbType.AnsiStringFixedLength:
                    tp = "char";
                    isPrec = true;
                    isQuote = true;
                    break;
                case DbType.StringFixedLength:
                    tp = "nchar";
                    isPrec = true;
                    isQuote = true;
                    break;
                case DbType.String:
                    tp = "nvarchar";
                    isPrec = true;
                    isQuote = true;
                    break;
                case DbType.AnsiString:
                    tp = "varchar";
                    isPrec = true;
                    isQuote = true;
                    break;
                case DbType.Decimal:
                    tp = "decimal";
                    isPrec = true;
                    isScale = true;
                    break;
                case DbType.Currency:
                    tp = "money";
                    break;
                case DbType.Double:
                    tp = "real";
                    isPrec = true;
                    isScale = true;
                    break;
                case DbType.Single:
                    tp = "float";
                    isPrec = true;
                    isScale = true;
                    break;

                case DbType.Guid:
                    tp = "uniqueidentifier";
                    isQuote = true;
                    break;
                case DbType.Binary:
                    tp = "binary";
                    break;
                case DbType.Object:
                    tp = "uniqueidentifier";
                    isQuote = true;
                    break;
                case DbType.Xml:
                    tp = "xml";
                    isQuote = true;
                    break;
                default:
                    tp = "unknown";
                    break;
            }
            return tp;
        }
    }
}