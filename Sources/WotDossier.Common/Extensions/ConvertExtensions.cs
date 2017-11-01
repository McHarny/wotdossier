using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WotDossier.Common.Reflection;

namespace WotDossier.Common.Extensions
{
	public static class ConvertExtensions
	{
		public static T To<T>(this object value, T defaultValue = default(T))
		{
			return (T)To(value, typeof(T), defaultValue);
		}

		public static object To(this object value, Type targetType, object defaultValue = null)
		{
			if (value == null || value == DBNull.Value)
			{
				return defaultValue;
			}

			var sourceType = value.GetType();

			if (sourceType.IsSubclassOf(targetType) || targetType.IsAssignableFrom(sourceType) || targetType.IsInstanceOfType(value))
				return value;

			if (targetType.IsNullable())
			{
				targetType = targetType.UnderlyingTypeOf();
			}
			try
			{
				if (targetType.IsEnum && !sourceType.IsPrimitive)
				{
					try
					{
						return Enum.Parse(targetType, value.ToString(), true);
					}
					catch { }
				}

				if (targetType == typeof(Guid))
				{
					return new Guid(value.ToString());
				}
				else if (targetType == typeof(string))
				{
					return value.ToString();
				}
				if (targetType == typeof(bool) && (sourceType.IsPrimitive || sourceType == typeof(string)))
				{
					long vl;
					if (Int64.TryParse(value.ToString(), out vl))
						return vl != 0;
					Decimal dv;
					if (Decimal.TryParse(value.ToString(), out dv))
						return dv != 0;
				}
				return Convert.ChangeType(value, (targetType.IsEnum) ? Enum.GetUnderlyingType(targetType) : targetType, null);
			}
			catch
			{
				return defaultValue;
			}
		}
	}
}
