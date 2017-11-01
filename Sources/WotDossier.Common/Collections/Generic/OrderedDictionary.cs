using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using WotDossier.Common.Extensions;

namespace WotDossier.Common.Collections.Generic
{
	/// <summary>Represents a collection of key/value pairs that are ordered based on the key/index.</summary>
    [Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false), DebuggerTypeProxy(typeof(System_DictionaryDebugView<,>))]
	public class OrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IOrderedDictionary, IComparable,
		IEquatable<OrderedDictionary<TKey, TValue>>, IEquatable<IOrderedDictionary>

#if !SILVERLIGHT
, IXmlSerializable, ISerializable, IDeserializationCallback
#endif
	{
		protected Dictionary<TKey, TValue> _dictionary;
		protected List<TKey> _keys;
		protected List<TValue> _values;

		[NonSerialized]
		private ReadOnlyCollection<TKey> keyList;

		[NonSerialized]
		private ReadOnlyCollection<TValue> valueList;

#if !SILVERLIGHT
		private readonly SerializationInfo _siInfo;
#endif

		[NonSerialized]
		private object _syncRoot;

		/// <summary>Индекс значения, служащего представлением объекта в виде строки</summary>
		[XmlElement("ToStringIndex")]
		public int ToStringIndex { get; set; }

		#region Конструкторы
		// Cannot easily support ctor that takes IEqualityComparer, since List doesn't have an easy 
		// way to use the IEqualityComparer.
		public OrderedDictionary()
			: this(0, null)
		{
		}

		public OrderedDictionary(int capacity)
			: this(capacity, null)
		{
		}

		public OrderedDictionary(IEqualityComparer<TKey> comparer)
			: this(0, comparer)
		{
		}

		public OrderedDictionary(int capacity, IEqualityComparer<TKey> comparer)
		{
			_dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
			_keys = new List<TKey>(capacity);
			_values = new List<TValue>(capacity);
			ToStringIndex = -1;
		}

#if !SILVERLIGHT
#if !FW35
		[TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
#endif
		protected OrderedDictionary(SerializationInfo info, StreamingContext context)
		{
			this._siInfo = info;
		}
#endif

		#endregion


		public int Count
		{
			get
			{
				return _dictionary.Count;
			}
		}

		public ICollection<TKey> Keys
		{
			get
			{
				return GetKeyList();
			}
		}

		public TValue this[TKey key]
		{
			get
			{
				return _dictionary[key];
			}
			set
			{
				var index = IndexOf(key);

				if ((index > Count) || (index < 0))
				{
					throw new ArgumentOutOfRangeException("index", String.Format("Индекс ({0}) выходит за границы коллекции", index));
				}

				_values[index] = value;
				_dictionary[key] = value;

				//RemoveFromLists(key);

				//_dictionary[key] = value;
				//_keys.Add(key);
				//_values.Add(value);
			}
		}

		public TKey GetKey(int index)
		{
			if ((index > Count) || (index < 0))
			{
				throw new ArgumentOutOfRangeException("index", String.Format("Индекс ({0}) выходит за границы коллекции", index));
			}
			return _keys[index];
		}

		/// <summary>Gets or sets the value at the specified index.</summary>
		/// <returns>The value of the item at the specified index. </returns>
		/// <param name="index">The zero-based index of the value to get or set.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero.-or-index is equal to or greater than <see cref="P:VirtuaObjects.Framework.Core.Collections.OrderedDictionary`2.Count"></see>.</exception>
		/// <exception cref="T:System.NotSupportedException">The property is being set and the <see cref="T:VirtuaObjects.Collections.OrderedDictionary`2"></see> collection is read-only.</exception>
		public KeyValuePair<TKey, TValue> this[int index]
		{
			get
			{
				if ((index > Count) || (index < 0))
				{
					throw new ArgumentOutOfRangeException("index", String.Format("Индекс ({0}) выходит за границы коллекции", index));
				}
				return new KeyValuePair<TKey, TValue>(_keys[index], _values[index]);
			}
			set
			{
				if ((index > Count) || (index < 0))
				{
					throw new ArgumentOutOfRangeException("index", String.Format("Индекс ({0}) выходит за границы коллекции", index));
				}
				_keys[index] = value.Key;
				_values[index] = value.Value;
				_dictionary[value.Key] = value.Value;
				//RemoveAt(index);
				//Insert(index, value.Key, value.Value);
			}
		}

		public ICollection<TValue> Values
		{
			get
			{
				return GetValueList();
			}
		}

		public void Add(TKey key, TValue value)
		{
			// Dictionary.Add() will throw if it already contains key
			_dictionary.Add(key, value);
			_keys.Add(key);
			_values.Add(value);
		}

		/// <summary>Inserts a new entry into the <see cref="T:VirtuaObjects.Collections.OrderedDictionary`2"></see> collection with the specified key and value at the specified index.</summary>
		/// <param name="value">The value of the entry to add. The value can be null.</param>
		/// <param name="key">The key of the entry to add.</param>
		/// <param name="index">The zero-based index at which the element should be inserted.</param>
		public void Insert(int index, TKey key, TValue value)
		{
			if ((index > Count) || (index < 0))
			{
				throw new ArgumentOutOfRangeException("index", String.Format("Индекс ({0}) выходит за границы коллекции", index));
			}
			_dictionary.Add(key, value);
			_keys.Insert(index, key);
			_values.Insert(index, value);
		}

		public void Clear()
		{
			_dictionary.Clear();
			_keys.Clear();
			_values.Clear();
		}

		public bool ContainsKey(TKey key)
		{
			return _dictionary.ContainsKey(key);
		}

		public bool ContainsValue(TValue value)
		{
			return _dictionary.ContainsValue(value);
		}

		public int IndexOf(TKey key)
		{
			return _keys.IndexOf(key);
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{

			for (var i = 0; i < Count; i++)
			{
				yield return new KeyValuePair<TKey, TValue>(_keys[i], _values[i]);
			}
			//// Must use foreach instead of a for loop, since we want the underlying List enumerator to
			//// throw an exception if the list is modified during enumeration. 
			//foreach (var key in _keys)
			//{
			//    yield return new KeyValuePair<TKey, TValue>(key, _values[i]);
			//    i++;
			//}
		}

		private void RemoveFromLists(TKey key)
		{
			var index = _keys.IndexOf(key);
			if (index != -1)
			{
				_keys.RemoveAt(index);
				_values.RemoveAt(index);
			}
		}

		public bool Remove(TKey key)
		{
			RemoveFromLists(key);
			return _dictionary.Remove(key);
		}

		public void RemoveAt(int index)
		{
			if ((index > Count) || (index < 0))
			{
				throw new ArgumentOutOfRangeException("index", String.Format("Индекс ({0}) выходит за границы коллекции", index));
			}
			var key = _keys[index];
			Remove(key);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return _dictionary.TryGetValue(key, out value);
		}

		#region ICollection<KeyValuePair<TKey,TValue>> Members
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get
			{
				return ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).IsReadOnly;
			}
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			Add(item.Key, item.Value);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Contains(item);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			((ICollection)this).CopyTo(array, arrayIndex);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			bool removed = ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Remove(item);

			// Only remove from lists if it was removed from the dictionary, since the dictionary may contain
			// the key but not the value. 
			if (removed)
			{
				RemoveFromLists(item.Key);
			}

			return removed;
		}
		#endregion

		#region IEnumerable Members
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion

		#region IOrderedDictionary Members

		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			return new OrderedDictionaryEnumerator(this);
		}

		void IOrderedDictionary.Insert(int index, object key, object value)
		{
			VerifyKey(key);
			VerifyValueType(value);
			Insert(index, (TKey)key, (TValue)value);
		}

		object IOrderedDictionary.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				if (!(value is KeyValuePair<TKey, TValue>))
					throw new ArgumentException(String.Format("The value \"{0}\" isn't of type \"{1}\" and can't be used in this generic collection.", value, typeof(KeyValuePair<TKey, TValue>)));
				this[index] = (KeyValuePair<TKey, TValue>)value;
			}
		}

		#endregion

		#region IDictionary Members
		void IDictionary.Add(object key, object value)
		{
			VerifyKey(key);
			VerifyValueType(value);
			Add((TKey)key, (TValue)value);
		}

		bool IDictionary.Contains(object key)
		{
			return (IsCompatibleKey(key) && ContainsKey((TKey)key));
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new OrderedDictionaryEnumerator(this);
		}

		bool IDictionary.IsFixedSize
		{
			get { return false; }
		}

		bool IDictionary.IsReadOnly
		{
			get { return false; }
		}

		ICollection IDictionary.Keys
		{
			get { return GetKeyList(); }
		}

		void IDictionary.Remove(object key)
		{
			if (IsCompatibleKey(key))
				Remove((TKey)key);
		}

		ICollection IDictionary.Values
		{
			get
			{
				return GetValueList();
			}
		}

		object IDictionary.this[object key]
		{
			get
			{
				if (IsCompatibleKey(key))
				{
					return this[(TKey)key];
				}
				return null;
			}
			set
			{
				VerifyKey(key);
				VerifyValueType(value);
				this[(TKey)key] = (TValue)value;
			}
		}

		#endregion

		#region ICollection Members
		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.ICollection"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing. </param>
		/// <param name="index">The zero-based index in array at which copying begins. </param>
		/// <exception cref="T:System.ArgumentNullException">array is null. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero. </exception>
		/// <exception cref="T:System.ArgumentException">array is multidimensional.-or- index is equal to or greater than the length of array.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"></see> is greater than the available space from index to the end of the destination array. </exception>
		/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.ICollection"></see> cannot be cast automatically to the type of the destination array. </exception><filterpriority>2</filterpriority>
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException("Only single dimensional arrays are supported for the requested action.");
			}
			if (array.GetLowerBound(0) != 0)
			{
				throw new ArgumentException("The lower bound of target array must be zero.");
			}
			if ((index < 0) || (index > array.Length))
			{
				throw new ArgumentOutOfRangeException("index", String.Format("Индекс ({0}) выходит за границы коллекции", index));
			}
			if ((array.Length - index) < Count)
			{
				throw new ArgumentException("Destination array is not long enough to copy all the items in the collection. Check array index and length.");
			}

			var pairArray = array as KeyValuePair<TKey, TValue>[];
			if (pairArray != null)
			{
				for (int i = 0; i < Count; i++)
				{
					pairArray[i + index] = new KeyValuePair<TKey, TValue>(_keys[i], _values[i]);
				}
			}
			else
			{
				var objArray = array as object[];
				if (objArray == null)
				{
					throw new ArgumentException("Некорректный тип массива назначения");
				}
				try
				{
					for (int i = 0; i < Count; i++)
					{
						objArray[i + index] = new KeyValuePair<TKey, TValue>(_keys[i], _values[i]);
					}
				}
				catch (ArrayTypeMismatchException)
				{
					throw new ArgumentException("Некорректный тип массива назначения");
				}
			}
		}

		/// <summary>
		/// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.
		/// </summary>
		/// <returns>
		/// An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		object ICollection.SyncRoot
		{
			get
			{
				if (_syncRoot == null)
				{
					Interlocked.CompareExchange(ref _syncRoot, new object(), null);
				}
				return _syncRoot;
			}
		}

		/// <summary>
		/// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe).
		/// </summary>
		/// <returns>
		/// true if access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe); otherwise, false.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		#endregion

		#region Получение списка ключей и значений
		private ReadOnlyCollection<TKey> GetKeyList()
		{
			if (keyList == null)
			{
				keyList = new ReadOnlyCollection<TKey>(_keys);
			}
			return keyList;
		}

		private ReadOnlyCollection<TValue> GetValueList()
		{
			if (valueList == null)
			{
				valueList = new ReadOnlyCollection<TValue>(_values);
			}
			return valueList;
		}
		#endregion

		#region Вспомогательные проверочные методы
		private static bool IsCompatibleKey(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return (key is TKey);
		}

		private static void VerifyKey(object key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (!(key is TKey))
			{
				throw new ArgumentException(String.Format("The value \"{0}\" isn't of type \"{1}\" and can't be used in this generic collection.", key, typeof(TKey)));
			}
		}

		private static void VerifyValueType(object value)
		{
			if (!(value is TValue) && ((value != null) || typeof(TValue).IsValueType))
			{
				throw new ArgumentException(String.Format("The value \"{0}\" isn't of type \"{1}\" and can't be used in this generic collection.", value, typeof(TValue)));
			}
		}
		#endregion

		#region OrderedDictionaryEnumerator
		[Serializable, StructLayout(LayoutKind.Sequential)]
		private struct OrderedDictionaryEnumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator
		{
			private readonly OrderedDictionary<TKey, TValue> _list;
			private TKey key;
			private TValue value;
			private int index;

			internal OrderedDictionaryEnumerator(OrderedDictionary<TKey, TValue> list)
			{
				_list = list;
				index = -1;
				key = default(TKey);
				value = default(TValue);
			}

			public void Dispose()
			{
				index = 0;
				key = default(TKey);
				value = default(TValue);
			}

			object IDictionaryEnumerator.Key
			{
				get
				{
					if (index < 0 || index >= _list.Count)
						throw new InvalidOperationException("Enumerator is positioned before the first element or after the last element of the collection.");
					return key;
				}
			}
			public bool MoveNext()
			{
				if (index < _list.Count)
				{
					key = _list._keys[index];
					value = _list._values[index];
					index++;
					return true;
				}
				index = _list.Count;
				key = default(TKey);
				value = default(TValue);
				return false;
			}

			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					if (index < 0 || index >= _list.Count)
						throw new InvalidOperationException("Enumerator is positioned before the first element or after the last element of the collection.");


					return new DictionaryEntry(key, value);
				}
			}
			public KeyValuePair<TKey, TValue> Current
			{
				get
				{
					return new KeyValuePair<TKey, TValue>(key, value);
				}
			}
			object IEnumerator.Current
			{
				get
				{
					if (index < 0 || index >= _list.Count)
						throw new InvalidOperationException("Enumerator is positioned before the first element or after the last element of the collection.");

					return new KeyValuePair<TKey, TValue>(key, value);
				}
			}

			object IDictionaryEnumerator.Value
			{
				get
				{

					if (index < 0 || index >= _list.Count)
						throw new InvalidOperationException("Enumerator is positioned before the first element or after the last element of the collection.");
					return value;
				}
			}

			void IEnumerator.Reset()
			{
				index = -1;
				key = default(TKey);
				value = default(TValue);
			}
		}
		#endregion

		#region IDeserializationCallback Members
#if !SILVERLIGHT
		void IDeserializationCallback.OnDeserialization(object sender)
		{
			OnDeserialization(sender);
		}
#endif
		#endregion

		#region IXmlSerializable Members
#if !SILVERLIGHT
		System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
		{
			var doc = new XmlDocument();
			doc.LoadXml(reader.ReadOuterXml());
			try
			{
				XmlNode nd = doc.FirstChild;
				ToStringIndex = Convert.ToInt32(nd.Attributes["ToStringIndex"].Value);
				XmlNode ItemsNode = nd.ChildNodes[0];
				foreach (XmlNode item in ItemsNode.ChildNodes)
				{
					object key = item.ChildNodes[0].InnerText.To(Type.GetType(item.ChildNodes[0].Attributes["Type"].Value));
					object value = null;
					if (item.ChildNodes[1].InnerText != "")
						value = item.ChildNodes[1].InnerText.To(Type.GetType(item.ChildNodes[1].Attributes["Type"].Value));
					VerifyKey(key);
					VerifyValueType(value);
					Add((TKey)key, (TValue)value);
				}
			}
			catch (Exception e)
			{
				throw new SerializationException("There was an error deserializing the OrderedDictionary.  The ArrayList does not contain DictionaryEntries.", e);
			}
		}

		void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
		{
			writer.WriteAttributeString("ToStringIndex", ToStringIndex.ToString());
			writer.WriteStartElement("Items");
			for (int i = 0; i < _keys.Count; i++)
			{
				writer.WriteStartElement("Item");
				var key = _keys[i];
				var value = _values[i];
				writer.WriteStartElement("Key");
				writer.WriteAttributeString("Type", key.GetType().FullName);
				writer.WriteValue(key.ToString());
				writer.WriteEndElement();

				writer.WriteStartElement("Value");
				if (value == null)
				{
					writer.WriteAttributeString("Type", typeof(TValue).FullName);
					writer.WriteValue("");
				}
				else
				{
					writer.WriteAttributeString("Type", value.GetType().FullName);
					writer.WriteValue(value.ToString());
				}
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}
#endif
		#endregion

		#region ISerializable Members
#if !SILVERLIGHT
		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable"></see> interface and returns the data needed to serialize the <see cref="T:VirtuaObjects.Collections.OrderedDictionary`2"></see> collection.</summary>
		/// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext"></see> object containing the source and destination of the serialized stream associated with the <see cref="T:VirtuaObjects.Collections.OrderedDictionary`2"></see>.</param>
		/// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> object containing the information required to serialize the <see cref="T:VirtuaObjects.Collections.OrderedDictionary`2"></see> collection.</param>
		/// <exception cref="T:System.ArgumentNullException">info is null.</exception>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			var k = new TKey[Count];
			_keys.CopyTo(k);
			info.AddValue("Keys", k);

			var v = new TValue[Count];
			_values.CopyTo(v);
			info.AddValue("Values", v);
			info.AddValue("ToStringValue", ToStringIndex);
		}

		/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable"></see> interface and is called back by the deserialization event when deserialization is complete.</summary>
		/// <param name="sender">The source of the deserialization event.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> object associated with the current <see cref="T:VirtuaObjects.Collections.OrderedDictionary`2"></see> collection is invalid.</exception>
		protected virtual void OnDeserialization(object sender)
		{
			if (_siInfo == null)
			{
				throw new SerializationException("OnDeserialization method was called while the object was not being deserialized.");
			}
			ToStringIndex = _siInfo.GetInt32("ToStringValue");
			var keyArray = (TKey[])_siInfo.GetValue("Keys", typeof(TKey[]));
			var valArray = (TValue[])_siInfo.GetValue("Values", typeof(TValue[]));
			try
			{
				for (int i = 0; i < keyArray.Length; i++)
				{
					Add(keyArray[i], valArray[i]);
				}
			}
			catch (Exception)
			{

				throw new SerializationException("There was an error deserializing the OrderedDictionary.  The ArrayList does not contain DictionaryEntries");
			}
		}
#endif
		#endregion

		#region IComparable Members
		int IComparable.CompareTo(object obj)
		{
			if (obj == null) return 1;
			var VCObj = obj as IOrderedDictionary;
			if (VCObj == null)
				throw new ArgumentException("Collections is not comparable", "obj");
			return CompareTo(VCObj);
		}

		public int CompareTo(IOrderedDictionary obj)
		{
			if (obj == null) return 1;
			if (Count == obj.Count)
			{
				for (int i = 0; i < Count; i++)
				{
					var cmp = this[i] as IComparable;
					if (cmp == null)
					{
						if (obj[i] == null) continue;
						return -1;
					}
					int Res = cmp.CompareTo(obj[i]);

					if (Res != 0)
					{
						return Res;
					}
				}

				return 0;
			}
			return Count.CompareTo(obj.Count);
		}
		#endregion

		#region Представление объекта в виде строки
		/// <summary>Представление объекта в виде строки</summary>
		/// <returns>Значения всех полей в одну строку</returns>
		public override string ToString()
		{
			if (ToStringIndex >= 0 && ToStringIndex < Count)
			{
				// Если определено значение ToString()
				return this[ToStringIndex].NotNull();
			}
			// Строим строку
			var bld = new StringBuilder();
			for (var i = 0; i < _keys.Count; i++)
			{
				if (bld.Length > 0)
					bld.Append("; ");

				bld.Append(_keys[i].ToString());
				bld.Append(" = ");
				bld.Append(_values[i].NotNull());
			}

			return bld.ToString();
		}

		#endregion

		#region Переопределение понятия равенства
		/// <summary>Два экземпляра OrderedDictionary считаются равными, если соответственно равны
		/// все хранящиеся в них значения (сопоставление значений происходит по целочисленному индексу)
		/// </summary>
		/// <param name="x">Объект 1</param>
		/// <param name="y">Объект 2</param>
		/// <returns>True, если коллекции равны</returns>
		public static bool operator ==(OrderedDictionary<TKey, TValue> x, OrderedDictionary<TKey, TValue> y)
		{
			return Equals(x, y);
		}

		/// <summary>Два экземпляра OrderedDictionary считаются не равными, если не равна
		/// хотя бы одна пара хранящиеся в них значений (сопоставление значений происходит
		/// по целочисленному индексу)</summary>
		/// <param name="x">Объект 1</param>
		/// <param name="y">Объект 2</param>
		/// <returns>True, если коллекции не равны</returns>
		public static bool operator !=(OrderedDictionary<TKey, TValue> x, OrderedDictionary<TKey, TValue> y)
		{
			return !Equals(x, y);
		}

		/// <summary>Переопределение стандартного метода</summary>
		/// <returns>True, коллекции равны</returns>
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof(IOrderedDictionary)) return false;
			return Equals((IOrderedDictionary)obj);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the other parameter; otherwise, false.
		/// </returns>
		/// <param name="obj">An object to compare with this object.</param>
		public bool Equals(OrderedDictionary<TKey, TValue> obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return Equals((IOrderedDictionary)obj);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the other parameter; otherwise, false.
		/// </returns>
		/// <param name="obj">An object to compare with this object.</param>
		public bool Equals(IOrderedDictionary obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.Count != Count)
			{
				return false;
			}

			for (var i = 0; i < Count; i++)
			{
				if (!Equals(this[i], obj[i]))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>Чтобы не было warning-а</summary>
		/// <returns>Хэш-код объекта</returns>
		public override int GetHashCode()
		{
			// Строим строку
			var bld = new StringBuilder();
			for (var i = 0; i < _keys.Count; i++)
			{
				if (bld.Length > 0)
					bld.Append("; ");

				bld.Append(_values[i].NotNull());
			}

			return bld.ToString().GetHashCode();
		}

		#endregion
	}
}
