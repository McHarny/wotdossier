using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WotDossier.Common.Extensions
{
	public static class ValueExtensions
	{
		public static bool IsNull(this object value)
		{
			return value == null || value == DBNull.Value;
		}

		/// <summary>
		/// Подмена null-значения альтернативой
		/// </summary>
		/// <param name="val">Проверяемое значение</param>
		/// <param name="alt">Альтернатива</param>
		/// <returns>val == null ? alt : val</returns>
		public static string NotNull(this object val, string alt = "")
		{
			return val == null || val == DBNull.Value ? alt : val.ToString();
		}

		/// <summary>
		/// Подмена null-значения альтернативой
		/// </summary>
		/// <param name="val">Проверяемое значение</param>
		/// <param name="alt">Альтернатива</param>
		/// <returns>val == null ? alt : val</returns>
		public static T NotNull<T>(this object val, T alt = default(T))
		{
			return (val == null || val == DBNull.Value) ? alt : (T)val;
		}

		public static bool IsNullOrEmpty(this object value)
		{
			if (value == null || value == DBNull.Value)
				return true;

			if (value is string)
				return (string)value == String.Empty;

			if (value is Guid)
				return (Guid)value == Guid.Empty;

			return false;
		}
	}
}
