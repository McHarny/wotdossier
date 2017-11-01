namespace WotDossier.Dal.DataAccess.Providers.SQLite
{
    public static class SQLiteDbTypeExtensions
    {
        //public static string SQLiteDbTypeInfo(this DbParameter param, ref bool isPrec, ref int Prec, ref bool isScale,
        //    ref int Scale, ref bool isQuote)
        //{
        //    var parm = (SQLiteParameter) param;
        //    string tp = String.Empty;
        //    isScale = false;
        //    isPrec = false;
        //    isQuote = false;

        //    switch (parm.DbType)
        //    {
        //        case DbType.Int64:
        //            tp = "bigint";
        //            break;
        //        case DbType.Boolean:
        //            tp = "bit";
        //            break;
        //        case DbType.Int32:
        //            tp = "int";
        //            break;
        //        case DbType.Date:
        //            tp = "date";
        //            isQuote = true;
        //            break;
        //        case DbType.DateTime2:
        //            tp = "datetime2";
        //            isQuote = true;
        //            break;
        //        case DbType.DateTimeOffset:
        //            tp = "datetimeoffset";
        //            isQuote = true;
        //            break;
        //        case DbType.Time:
        //            tp = "time";
        //            isQuote = true;
        //            break;
        //        case DbType.Int16:
        //            tp = "smallint";
        //            break;
        //        case DbType.DateTime:
        //            tp = "datetime";
        //            isQuote = true;
        //            break;
        //        case DbType.Byte:
        //            tp = "tinyint";
        //            break;

        //        case DbType.AnsiStringFixedLength:
        //            tp = "char";
        //            isPrec = true;
        //            isQuote = true;
        //            break;
        //        case DbType.StringFixedLength:
        //            tp = "nchar";
        //            isPrec = true;
        //            isQuote = true;
        //            break;
        //        case DbType.String:
        //            tp = "nvarchar";
        //            isPrec = true;
        //            isQuote = true;
        //            break;
        //        case DbType.AnsiString:
        //            tp = "varchar";
        //            isPrec = true;
        //            isQuote = true;
        //            break;
        //        case DbType.Decimal:
        //            tp = "decimal";
        //            isPrec = true;
        //            isScale = true;
        //            break;
        //        case DbType.Currency:
        //            tp = "money";
        //            break;
        //        case DbType.Double:
        //            tp = "real";
        //            isPrec = true;
        //            isScale = true;
        //            break;
        //        case DbType.Single:
        //            tp = "float";
        //            isPrec = true;
        //            isScale = true;
        //            break;

        //        case DbType.Guid:
        //            tp = "uniqueidentifier";
        //            isQuote = true;
        //            break;
        //        case DbType.Binary:
        //            tp = "binary";
        //            break;
        //        case DbType.Object:
        //            tp = "uniqueidentifier";
        //            isQuote = true;
        //            break;
        //        case DbType.Xml:
        //            tp = "xml";
        //            isQuote = true;
        //            break;
        //        default:
        //            tp = "unknown";
        //            break;
        //    }
        //    return tp;
        //}
    }
}