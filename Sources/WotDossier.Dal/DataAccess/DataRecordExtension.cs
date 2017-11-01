using System;
using System.Data;
using WotDossier.Common.Extensions;
using WotDossier.Common.Reflection;

namespace WotDossier.Dal.DataAccess
{
	/// <summary>
	/// Класс расширяющий возможности интервейса IDataRecord
	/// </summary>
	public static class DataRecordExtension
	{
		/// <summary>Определение порядкового номера поля по наименованию</summary>
		/// <param name="rec">Запись</param>
		/// <param name="fieldName">Имя поля</param>
		/// <returns>Порядковый номер поля (начиная с 0)</returns>
		private static int GetOrdinalCore(this IDataRecord rec, string fieldName)
		{
			try
			{
				return rec.GetOrdinal(fieldName);
			}
			catch (Exception e)
			{
				throw new NoDataFieldException(fieldName, e);
			}
		}

		/// <summary>Определение что запись содержит колонку с наименованием <paramref name="fieldName"/></summary>
		/// <param name="rec">Запись</param>
		/// <param name="fieldName">Имя поля</param>
		/// <returns>True, если такая колонка есть в записи, иначе False</returns>
		public static bool ContainsField(this IDataRecord rec, string fieldName)
		{
			for (var i = 0; i < rec.FieldCount; i++)
			{
				if (String.Compare(rec.GetName(i), fieldName, StringComparison.OrdinalIgnoreCase) == 0) return true;
			}
			return false;
		}

		#region Значение поля Generic'ом
		/// <summary>Поле записи, приведенное к типу T</summary>
		/// <param name="rec">Запись</param>
		/// <param name="fieldOrdinal">Номер поля</param>
		/// <returns>Значение заданного поля</returns>
		/// <remarks>В случае NULL-значения в БД произойдет Exception</remarks>
		public static T Get<T>(this IDataRecord rec, int fieldOrdinal)
		{
            if (rec.IsDBNull(fieldOrdinal) && !typeof(T).IsNullable() && !typeof(T).IsClass)
			{
				throw new NullDataFieldException(fieldOrdinal);
			}

			return rec.GetValue(fieldOrdinal).To<T>();
		}

		/// <summary>Поле записи, приведенное к типу T</summary>
		/// <param name="rec">Запись</param>
		/// <param name="fieldName">Имя поля</param>
		/// <returns>Значение названного поля</returns>
		/// <remarks>В случае NULL-значения в БД произойдет Exception</remarks>
		public static T Get<T>(this IDataRecord rec, String fieldName)
		{
			var fieldPosition = rec.GetOrdinalCore(fieldName);

            if (rec.IsDBNull(fieldPosition) && !typeof(T).IsNullable() && !typeof(T).IsClass)
			{
				throw new NullDataFieldException(fieldName);
			}
			return rec.GetValue(fieldPosition).To<T>();
			//return (T)Convert.ChangeType(rec.GetValue(FieldPosition), typeof(T));
		}

		/// <summary>Поле записи, конвертированное в Int32</summary>
		/// <param name="rec">Запись</param>
		/// <param name="fieldName">Имя поля</param>
		/// <param name="nullValue">Значение, заменяющее NULL</param>
		/// <returns>Значение названного поля, либо указанная альтернатива</returns>
		public static T Get<T>(this IDataRecord rec, String fieldName, T nullValue)
		{
			int fieldPosition = rec.GetOrdinalCore(fieldName);

			return rec.IsDBNull(fieldPosition) ? nullValue : rec.GetValue(fieldPosition).To<T>();
		}
		#endregion


	    public static bool IsDBNull(this IDataRecord rec, string fieldName)
	    {
	        return rec.IsDBNull(GetOrdinalCore(rec, fieldName));
	    }

	}
}