using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using LiaSoft.Jeff.DataAccess;

namespace WotDossier.Dal.DataAccess
{
	/// <summary>
	/// Представление DataRow как IDataRecord
	/// </summary>
	public class XRowRecord : IDataRecord
	{
		/// <summary>Конструктор на базе XElement</summary>
		/// <param name="row">Исходные данные в виде XElement</param>
		public XRowRecord(XElement row)
		{
			Row = row;
		}

		/// <summary>Возвращает объект типа DBRow</summary>
		/// <param name="xDoc">Исходные данные в виде XDocument</param>
		public XRowRecord(XDocument xDoc)
		{
			Row = xDoc.Root;
		}

		/// <summary>Для желающих работать с DataRow</summary>
		public XElement Row { get; private set; }


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
				return Row.Elements().Skip(i).First().Name.LocalName;
			}
			catch (Exception exception)
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
		/// <exception cref="NotImplementedException"></exception>
		/// <returns>
		/// The <see cref="T:System.Type"></see> information corresponding to the type of <see cref="T:System.Object"></see> that would be returned from <see cref="M:System.Data.IDataRecord.GetValue(System.Int32)"></see>.
		/// </returns>
		/// <param name="i">The index of the field to find. </param>
		public Type GetFieldType(int i)
		{
			throw new NotImplementedException("DataTypes not supported");
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
				return this[i].Value;
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
				throw new ArgumentNullException("values");
			}
			var cnt = Row.Elements().Count();
			Array.Copy(Row.Elements().Select(v => v.Value).ToArray(), values, (cnt > values.Length) ? values.Length : cnt);
			return (cnt <= values.Length) ? cnt : values.Length;
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
			var i = 0;
			foreach (var elem in Row.Elements())
			{
				if (string.Compare(elem.Name.LocalName, name, StringComparison.InvariantCultureIgnoreCase) == 0) return i;
				i++;
			}
			return -1;
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
				return Convert.ToBoolean(this[i].Value);
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
				return Convert.ToByte(this[i].Value);
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
				throw new ArgumentOutOfRangeException("fieldOffset");
			}

			if ((bufferoffset < 0) || ((bufferoffset > 0) && (bufferoffset >= buffer.Length)))
			{
				throw new ArgumentOutOfRangeException("bufferoffset");
			}


			byte[] buffer2;
			try
			{
				buffer2 = Encoding.UTF32.GetBytes(this[i].Value);
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
					throw new ArgumentOutOfRangeException("length");
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
				return Convert.ToChar(this[i].Value);
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
				throw new ArgumentOutOfRangeException("fieldoffset");
			}

			if ((bufferoffset < 0) || ((bufferoffset > 0) && (bufferoffset >= buffer.Length)))
			{
				throw new ArgumentOutOfRangeException("bufferoffset");
			}


			char[] buffer2;
			try
			{
				buffer2 = this[i].Value.ToCharArray();
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
					throw new ArgumentOutOfRangeException("length");
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
				return new Guid(this[i].Value);
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
				return Convert.ToInt16(this[i].Value);
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
				return Convert.ToInt32(this[i].Value);
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
				return Convert.ToInt64(this[i].Value);
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
				return Convert.ToSingle(this[i].Value);
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
				return Convert.ToDouble(this[i].Value);
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
				return this[i].Value;
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
				return Convert.ToDecimal(this[i].Value);
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
				return Convert.ToDateTime(this[i].Value);
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
				return (i < 0) || Row.Elements().Count() < (i + 1);
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
		public int FieldCount
		{
			get { return Row.Elements().Count(); }
		}

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
					return this[i].Value;
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
					return this[name].Value;
				}
				catch (IndexOutOfRangeException exception)
				{
					throw new NoDataFieldException(name, exception);
				}
			}
		}
		#endregion

		#region Helper Methods
		private XElement this[int index]
		{
			get
			{
				var elem = Row.Elements().Skip(index).FirstOrDefault();
				if (elem != null) return elem;
				throw new IndexOutOfRangeException("Index = " + index);
			}
		}

		private XElement this[string name]
		{
			get
			{
				var elem = Row.Elements(name).FirstOrDefault();
				if (elem != null) return elem;
				throw new IndexOutOfRangeException("Name = " + name);
			}
		}
		#endregion
	}
}