using System;
using System.Data;
using LiaSoft.Jeff.DataAccess;

namespace WotDossier.Dal.DataAccess
{
	/// <summary>
	/// Представление DataRow как IDataRecord
	/// </summary>
	public class DataRowRecord : IDataRecord
	{
		/// <summary>Возвращает объект типа DBRow</summary>
		/// <param name="row">Исходные данные в виде DataRow</param>
		public DataRowRecord(DataRow row)
		{
			Row = row;
		}

		/// <summary>Возвращает объект типа DBRow</summary>
		/// <param name="rowView">Исходные данные в виде DataRowView</param>
		public DataRowRecord(DataRowView rowView)
		{
			Row = rowView.Row;
		}

		/// <summary>Для желающих работать с DataRow</summary>
		public DataRow Row { get; }


		#region Implementation of IDataRecord
		/// <summary>
		/// Gets the name for the field to find.
		/// </summary>
		/// <returns>
		/// The name of the field or the empty string (""), if there is no value to return.
		/// </returns>
		/// <param name="i">The index of the field to find. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public string GetName(int i)
		{
			try
			{
				return Row.Table.Columns[i].ColumnName;
			}
			catch (IndexOutOfRangeException exception)
			{
				throw new NoDataFieldException(i, exception);
			}
		}

		/// <summary>
		/// Gets the data type information for the specified field.
		/// </summary>
		/// <returns>
		/// The data type information for the specified field.
		/// </returns>
		/// <param name="i">The index of the field to find. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public string GetDataTypeName(int i)
		{
			return GetFieldType(i).Name;
		}

		/// <summary>
		/// Gets the <see cref="T:System.Type"></see> information corresponding to the type of <see cref="T:System.Object"></see> that would be returned from <see cref="M:System.Data.IDataRecord.GetValue(System.Int32)"></see>.
		/// </summary>
		/// <returns>
		/// The <see cref="T:System.Type"></see> information corresponding to the type of <see cref="T:System.Object"></see> that would be returned from <see cref="M:System.Data.IDataRecord.GetValue(System.Int32)"></see>.
		/// </returns>
		/// <param name="i">The index of the field to find. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public Type GetFieldType(int i)
		{
			try
			{
				return Row.Table.Columns[i].DataType;
			}
			catch (IndexOutOfRangeException exception)
			{
				throw new NoDataFieldException(i, exception);
			}
		}

		/// <summary>
		/// Return the value of the specified field.
		/// </summary>
		/// <returns>
		/// The <see cref="T:System.Object"></see> which will contain the field value upon return.
		/// </returns>
		/// <param name="i">The index of the field to find. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public object GetValue(int i)
		{
			try
			{
				return Row[i];
			}
			catch (IndexOutOfRangeException exception)
			{
				throw new NoDataFieldException(i, exception);
			}
		}

		/// <summary>
		/// Gets all the attribute fields in the collection for the current record.
		/// </summary>
		/// <returns>
		/// The number of instances of <see cref="T:System.Object"></see> in the array.
		/// </returns>
		/// <param name="values">An array of <see cref="T:System.Object"></see> to copy the attribute fields into. </param><filterpriority>2</filterpriority>
		public int GetValues(object[] values)
		{
			if (values == null)
			{
				throw new ArgumentNullException(nameof(values));
			}
			Array.Copy(Row.ItemArray, values, (Row.ItemArray.Length > values.Length) ? values.Length : Row.ItemArray.Length);
			return (Row.ItemArray.Length <= values.Length) ? Row.ItemArray.Length : values.Length;
		}

		/// <summary>
		/// Return the index of the named field.
		/// </summary>
		/// <returns>
		/// The index of the named field.
		/// </returns>
		/// <param name="name">The name of the field to find. </param><filterpriority>2</filterpriority>
		public int GetOrdinal(string name)
		{
			if(!Row.Table.Columns.Contains(name))
				throw new NoDataFieldException(name);

			return Row.Table.Columns[name].Ordinal;

		}

		/// <summary>
		/// Gets the value of the specified column as a Boolean.
		/// </summary>
		/// <returns>
		/// The value of the column.
		/// </returns>
		/// <param name="i">The zero-based column ordinal. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public bool GetBoolean(int i)
		{
			try
			{
				return Convert.ToBoolean(Row[i]);
			}
			catch (IndexOutOfRangeException exception)
			{
				throw new NoDataFieldException(i, exception);
			}
		}

		/// <summary>
		/// Gets the 8-bit unsigned integer value of the specified column.
		/// </summary>
		/// <returns>
		/// The 8-bit unsigned integer value of the specified column.
		/// </returns>
		/// <param name="i">The zero-based column ordinal. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public byte GetByte(int i)
		{
			try
			{
				return Convert.ToByte(Row[i]);
			}
			catch (IndexOutOfRangeException exception)
			{
				throw new NoDataFieldException(i, exception);
			}
		}

		/// <summary>
		/// Reads a stream of bytes from the specified column offset into the buffer as an array, starting at the given buffer offset.
		/// </summary>
		/// <returns>
		/// The actual number of bytes read.
		/// </returns>
		/// <param name="buffer">The buffer into which to read the stream of bytes. </param>
		/// <param name="bufferoffset">The index for buffer to start the read operation. </param>
		/// <param name="fieldOffset">The index within the field from which to start the read operation. </param>
		/// <param name="i">The zero-based column ordinal. </param>
		/// <param name="length">The number of bytes to read. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			if (fieldOffset < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(fieldOffset));
			}

			if ((bufferoffset < 0) || ((bufferoffset > 0) && (bufferoffset >= buffer.Length)))
			{
				throw new ArgumentOutOfRangeException(nameof(bufferoffset));
			}


			byte[] buffer2;
			try
			{
				buffer2 = (byte[])Row[i];
			}
			catch (IndexOutOfRangeException exception)
			{
				throw new NoDataFieldException(i, exception);
			}

			if (buffer == null)
			{
				return buffer2.Length;
			}

			int num = Math.Min(buffer2.Length - (int)fieldOffset, length);
			if (num > 0)
			{
				Array.Copy(buffer2, fieldOffset, buffer, bufferoffset, num);
			}
			else
			{
				if (length < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(length));
				}
				num = 0;
			}
			return num;
		}

		/// <summary>
		/// Gets the character value of the specified column.
		/// </summary>
		/// <returns>
		/// The character value of the specified column.
		/// </returns>
		/// <param name="i">The zero-based column ordinal. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public char GetChar(int i)
		{
			try
			{
				return Convert.ToChar(Row[i]);
			}
			catch (IndexOutOfRangeException exception)
			{
				throw new NoDataFieldException(i, exception);
			}
		}

		/// <summary>
		/// Reads a stream of characters from the specified column offset into the buffer as an array, starting at the given buffer offset.
		/// </summary>
		/// <returns>
		/// The actual number of characters read.
		/// </returns>
		/// <param name="fieldoffset">The index within the row from which to start the read operation. </param>
		/// <param name="buffer">The buffer into which to read the stream of bytes. </param>
		/// <param name="bufferoffset">The index for buffer to start the read operation. </param>
		/// <param name="i">The zero-based column ordinal. </param>
		/// <param name="length">The number of bytes to read. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			if (fieldoffset < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(fieldoffset));
			}

			if ((bufferoffset < 0) || ((bufferoffset > 0) && (bufferoffset >= buffer.Length)))
			{
				throw new ArgumentOutOfRangeException(nameof(bufferoffset));
			}


			char[] buffer2;
			try
			{
				buffer2 = (char[])Row[i];
			}
			catch (IndexOutOfRangeException exception)
			{
				throw new NoDataFieldException(i, exception);
			}

			if (buffer == null)
			{
				return buffer2.Length;
			}

			int num = Math.Min(buffer2.Length - (int)fieldoffset, length);
			if (num > 0)
			{
				Array.Copy(buffer2, fieldoffset, buffer, bufferoffset, num);
			}
			else
			{
				if (length < 0)
				{
					throw new ArgumentOutOfRangeException(nameof(length));
				}
				num = 0;
			}
			return num;
		}

		/// <summary>
		/// Returns the GUID value of the specified field.
		/// </summary>
		/// <returns>
		/// The GUID value of the specified field.
		/// </returns>
		/// <param name="i">The index of the field to find. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public Guid GetGuid(int i)
		{
			try
			{
				return (Guid)Row[i];
			}
			catch (IndexOutOfRangeException exception)
			{
				throw new NoDataFieldException(i, exception);
			}
		}

		/// <summary>
		/// Gets the 16-bit signed integer value of the specified field.
		/// </summary>
		/// <returns>
		/// The 16-bit signed integer value of the specified field.
		/// </returns>
		/// <param name="i">The index of the field to find. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public short GetInt16(int i)
		{
			try
			{
				return Convert.ToInt16(Row[i]);
			}
			catch (IndexOutOfRangeException exception)
			{
				throw new NoDataFieldException(i, exception);
			}
		}

		/// <summary>
		/// Gets the 32-bit signed integer value of the specified field.
		/// </summary>
		/// <returns>
		/// The 32-bit signed integer value of the specified field.
		/// </returns>
		/// <param name="i">The index of the field to find. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public int GetInt32(int i)
		{
			try
			{
				return Convert.ToInt32(Row[i]);
			}
			catch (IndexOutOfRangeException exception)
			{
				throw new NoDataFieldException(i, exception);
			}
		}

		/// <summary>
		/// Gets the 64-bit signed integer value of the specified field.
		/// </summary>
		/// <returns>
		/// The 64-bit signed integer value of the specified field.
		/// </returns>
		/// <param name="i">The index of the field to find. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public long GetInt64(int i)
		{
			try
			{
				return Convert.ToInt64(Row[i]);
			}
			catch (IndexOutOfRangeException exception)
			{
				throw new NoDataFieldException(i, exception);
			}
		}

		/// <summary>
		/// Gets the single-precision floating point number of the specified field.
		/// </summary>
		/// <returns>
		/// The single-precision floating point number of the specified field.
		/// </returns>
		/// <param name="i">The index of the field to find. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public float GetFloat(int i)
		{
			try
			{
				return Convert.ToSingle(Row[i]);
			}
			catch (IndexOutOfRangeException exception)
			{
				throw new NoDataFieldException(i, exception);
			}
		}

		/// <summary>
		/// Gets the double-precision floating point number of the specified field.
		/// </summary>
		/// <returns>
		/// The double-precision floating point number of the specified field.
		/// </returns>
		/// <param name="i">The index of the field to find. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public double GetDouble(int i)
		{
			try
			{
				return Convert.ToDouble(Row[i]);
			}
			catch (IndexOutOfRangeException exception)
			{
				throw new NoDataFieldException(i, exception);
			}
		}

		/// <summary>
		/// Gets the string value of the specified field.
		/// </summary>
		/// <returns>
		/// The string value of the specified field.
		/// </returns>
		/// <param name="i">The index of the field to find. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public string GetString(int i)
		{
			try
			{
				return Convert.ToString(Row[i]);
			}
			catch (IndexOutOfRangeException exception)
			{
				throw new NoDataFieldException(i, exception);
			}
		}

		/// <summary>
		/// Gets the fixed-position numeric value of the specified field.
		/// </summary>
		/// <returns>
		/// The fixed-position numeric value of the specified field.
		/// </returns>
		/// <param name="i">The index of the field to find. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public decimal GetDecimal(int i)
		{
			try
			{
				return Convert.ToDecimal(Row[i]);
			}
			catch (IndexOutOfRangeException exception)
			{
				throw new NoDataFieldException(i, exception);
			}
		}

		/// <summary>
		/// Gets the date and time data value of the specified field.
		/// </summary>
		/// <returns>
		/// The date and time data value of the spcified field.
		/// </returns>
		/// <param name="i">The index of the field to find. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public DateTime GetDateTime(int i)
		{
			try
			{
				return Convert.ToDateTime(Row[i]);
			}
			catch (IndexOutOfRangeException exception)
			{
				throw new NoDataFieldException(i, exception);
			}
		}

		/// <summary>
		/// Gets an <see cref="T:System.Data.IDataReader"></see> to be used when the field points to more remote structured data.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Data.IDataReader"></see> to be used when the field points to more remote structured data.
		/// </returns>
		/// <param name="i">The index of the field to find. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public IDataReader GetData(int i)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Return whether the specified field is set to null.
		/// </summary>
		/// <returns>
		/// true if the specified field is set to null. Otherwise, false.
		/// </returns>
		/// <param name="i">The index of the field to find. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		public bool IsDBNull(int i)
		{
			try
			{
				return Row.IsNull(i);
			}
			catch (IndexOutOfRangeException exception)
			{
				throw new NoDataFieldException(i, exception);
			}
		}

		/// <summary>
		/// Gets the number of columns in the current row.
		/// </summary>
		/// <returns>
		/// When not positioned in a valid recordset, 0; otherwise the number of columns in the current record. The default is -1.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public int FieldCount => Row.Table.Columns.Count;

	    /// <summary>
		/// Gets the column located at the specified index.
		/// </summary>
		/// <returns>
		/// The column located at the specified index as an <see cref="T:System.Object"></see>.
		/// </returns>
		/// <param name="i">The index of the column to get. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception><filterpriority>2</filterpriority>
		object IDataRecord.this[int i]
		{
			get
			{
				try
				{
					return Row[i];
				}
				catch (IndexOutOfRangeException exception)
				{
					throw new NoDataFieldException(i, exception);
				}
			}
		}

		/// <summary>
		/// Gets the column with the specified name.
		/// </summary>
		/// <returns>
		/// The column with the specified name as an <see cref="T:System.Object"></see>.
		/// </returns>
		/// <param name="name">The name of the column to find. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">No column with the specified name was found. </exception><filterpriority>2</filterpriority>
		object IDataRecord.this[string name]
		{
			get
			{
				try
				{
					return Row[name];
				}
				catch (IndexOutOfRangeException exception)
				{
					throw new NoDataFieldException(name, exception);
				}
			}
		}
		#endregion
	}
}