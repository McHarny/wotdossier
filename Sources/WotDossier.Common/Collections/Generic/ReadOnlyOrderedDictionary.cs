using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using WotDossier.Common.Extensions;

namespace WotDossier.Common.Collections.Generic
{
    /// <summary>Represents a collection of key/value pairs that are ordered based on the key/index.</summary>
    [Serializable, DebuggerDisplay("Count = {Count}"), ComVisible(false), DebuggerTypeProxy(typeof(System_DictionaryDebugView<,>))]
    public class ReadOnlyOrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IOrderedDictionary, IComparable,
        //ISerializable, IXmlSerializable, IDeserializationCallback,
        IEquatable<ReadOnlyOrderedDictionary<TKey, TValue>>, IEquatable<IOrderedDictionary>
    {
        readonly OrderedDictionary<TKey, TValue> _dictionary;
        //private readonly SerializationInfo _siInfo;

        #region Конструкторы
        public ReadOnlyOrderedDictionary(OrderedDictionary<TKey, TValue> dict)
            : this(dict, false)
        {
        }

        // Cannot easily support ctor that takes IEqualityComparer, since List doesn't have an easy 
        // way to use the IEqualityComparer.
        public ReadOnlyOrderedDictionary(OrderedDictionary<TKey, TValue> dict, bool copy)
        {
            if (dict == null)
            {
                throw new ArgumentNullException("dict");
            }
            if (copy)
            {
                _dictionary = new OrderedDictionary<TKey, TValue>(dict.Count);
                foreach (var pair in dict)
                {
                    _dictionary.Add(pair.Key, pair.Value);
                    _dictionary.ToStringIndex = dict.ToStringIndex;
                }
            }
            else
                _dictionary = dict;
        }
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
                return _dictionary.Keys;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return _dictionary[key];
            }
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
                return _dictionary[index];
            }
        }

        /// <summary>Получить ключ по индексу</summary>
        /// <returns>The value of the item at the specified index. </returns>
        /// <param name="index">The zero-based index of the value to get or set.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero.-or-index is equal to or greater than <see cref="P:VirtuaObjects.Framework.Core.Collections.OrderedDictionary`2.Count"></see>.</exception>
        /// <exception cref="T:System.NotSupportedException">The property is being set and the <see cref="T:VirtuaObjects.Collections.OrderedDictionary`2"></see> collection is read-only.</exception>
        public TKey GetKey(int index)
        {
            return _dictionary[index].Key;
        }

        public ICollection<TValue> Values
        {
            get
            {
                return _dictionary.Values;
            }
        }

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return _dictionary.ContainsValue(value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        #region IDictionary<TKey, TValue> members
        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get
            {
                return _dictionary[key];
            }
            set
            {
                throw new NotSupportedException("Коллекция OrderedDictionary является только для чтения и не может быть модифицирована");
            }
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw new NotSupportedException("Коллекция OrderedDictionary является только для чтения и не может быть модифицирована");
        }

        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            throw new NotSupportedException("Коллекция OrderedDictionary является только для чтения и не может быть модифицирована");
        }
        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members
        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            throw new NotSupportedException("Коллекция OrderedDictionary является только для чтения и не может быть модифицирована");
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get
            {
                return true;
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException("Коллекция OrderedDictionary является только для чтения и не может быть модифицирована");
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
            throw new NotSupportedException("Коллекция OrderedDictionary является только для чтения и не может быть модифицирована");
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
            return ((IOrderedDictionary)_dictionary).GetEnumerator();
        }

        void IOrderedDictionary.Insert(int index, object key, object value)
        {
            throw new NotSupportedException("Коллекция OrderedDictionary является только для чтения и не может быть модифицирована");
        }

        object IOrderedDictionary.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw new NotSupportedException("Коллекция OrderedDictionary является только для чтения и не может быть модифицирована");
            }
        }

        void IOrderedDictionary.RemoveAt(int index)
        {
            throw new NotSupportedException("Коллекция OrderedDictionary является только для чтения и не может быть модифицирована");
        }
        #endregion

        #region IDictionary Members
        void IDictionary.Add(object key, object value)
        {
            throw new NotSupportedException("Коллекция OrderedDictionary является только для чтения и не может быть модифицирована");
        }

        bool IDictionary.Contains(object key)
        {
            return ((IDictionary)_dictionary).Contains(key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)_dictionary).GetEnumerator();
        }

        bool IDictionary.IsFixedSize
        {
            get { return true; }
        }

        bool IDictionary.IsReadOnly
        {
            get { return true; }
        }

        ICollection IDictionary.Keys
        {
            get { return ((IDictionary)_dictionary).Keys; }
        }

        void IDictionary.Remove(object key)
        {
            throw new NotSupportedException("Коллекция OrderedDictionary является только для чтения и не может быть модифицирована");
        }

        ICollection IDictionary.Values
        {
            get { return ((IDictionary)_dictionary).Values; }
        }

        object IDictionary.this[object key]
        {
            get
            {
                return ((IDictionary)_dictionary)[key];
            }
            set
            {
                throw new NotSupportedException("Коллекция OrderedDictionary является только для чтения и не может быть модифицирована");
            }
        }

        void IDictionary.Clear()
        {
            throw new NotSupportedException("Коллекция OrderedDictionary является только для чтения и не может быть модифицирована");
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
            ((ICollection)_dictionary).CopyTo(array, index);
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
                return ((ICollection)_dictionary).SyncRoot;
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
            get { return ((ICollection)_dictionary).IsSynchronized; }
        }

        #endregion

        #region IDeserializationCallback Members
        //void IDeserializationCallback.OnDeserialization(object sender)
        //{
        //    OnDeserialization(sender);
        //}
        #endregion

        #region IXmlSerializable Members

        //System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        //{
        //    return null;
        //}

        //void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
        //{
        //    var doc = new XmlDocument();
        //    doc.LoadXml(reader.ReadOuterXml());
        //    try
        //    {
        //        XmlNode nd = doc.FirstChild;
        //        m_ToStringValue = Convert.ToInt32(nd.Attributes["ToStringIndex"].Value);
        //        XmlNode ItemsNode = nd.ChildNodes[0];
        //        foreach (XmlNode item in ItemsNode.ChildNodes)
        //        {
        //            object key = Convert.ChangeType(item.ChildNodes[0].InnerText, Type.GetType(item.ChildNodes[0].Attributes["Type"].Value));
        //            object value = null;
        //            if (item.ChildNodes[1].InnerText != "")
        //                value = Convert.ChangeType(item.ChildNodes[1].InnerText, Type.GetType(item.ChildNodes[1].Attributes["Type"].Value));
        //            VerifyKey(key);
        //            VerifyValueType(value);
        //            Add((TKey)key, (TValue)value);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new SerializationException("There was an error deserializing the OrderedDictionary.  The ArrayList does not contain DictionaryEntries.", e);
        //    }
        //}

        //void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
        //{
        //    writer.WriteAttributeString("ToStringIndex", m_ToStringValue.ToString());
        //    writer.WriteStartElement("Items");
        //    for (int i = 0; i < _keys.Count; i++)
        //    {
        //        writer.WriteStartElement("Item");
        //        var key = _keys[i];
        //        var value = _values[i];
        //        writer.WriteStartElement("Key");
        //        writer.WriteAttributeString("Type", key.GetType().FullName);
        //        writer.WriteValue(key.ToString());
        //        writer.WriteEndElement();

        //        writer.WriteStartElement("Value");
        //        if (value == null)
        //        {
        //            writer.WriteAttributeString("Type", typeof(TValue).FullName);
        //            writer.WriteValue("");
        //        }
        //        else
        //        {
        //            writer.WriteAttributeString("Type", value.GetType().FullName);
        //            writer.WriteValue(value.ToString());
        //        }
        //        writer.WriteEndElement();
        //        writer.WriteEndElement();
        //    }
        //    writer.WriteEndElement();
        //}

        #endregion

        #region ISerializable Members
        ///// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable"></see> interface and returns the data needed to serialize the <see cref="T:VirtuaObjects.Framework.Core.Collections.OrderedDictionary`2"></see> collection.</summary>
        ///// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext"></see> object containing the source and destination of the serialized stream associated with the <see cref="T:VirtuaObjects.Framework.Core.Collections.OrderedDictionary`2"></see>.</param>
        ///// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> object containing the information required to serialize the <see cref="T:VirtuaObjects.Framework.Core.Collections.OrderedDictionary`2"></see> collection.</param>
        ///// <exception cref="T:System.ArgumentNullException">info is null.</exception>
        //[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        //void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    if (info == null)
        //    {
        //        throw new ArgumentNullException("info");
        //    }
        //    ((ISerializable)_dictionary).GetObjectData(info, context);
        //}

        ///// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable"></see> interface and is called back by the deserialization event when deserialization is complete.</summary>
        ///// <param name="sender">The source of the deserialization event.</param>
        ///// <exception cref="T:System.Runtime.Serialization.SerializationException">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> object associated with the current <see cref="T:VirtuaObjects.Framework.Core.Collections.OrderedDictionary`2"></see> collection is invalid.</exception>
        //protected virtual void OnDeserialization(object sender)
        //{
        //    if (_siInfo == null)
        //    {
        //        throw new SerializationException("OnDeserialization method was called while the object was not being deserialized.");
        //    }
        //    _dictionary.ToStringIndex = _siInfo.GetInt32("ToStringValue");
        //    var keyArray = (TKey[])_siInfo.GetValue("Keys", typeof(TKey[]));
        //    var valArray = (TValue[])_siInfo.GetValue("Values", typeof(TValue[]));
        //    try
        //    {
        //        for (int i = 0; i < keyArray.Length; i++)
        //        {
        //            Add(keyArray[i], valArray[i]);
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw new SerializationException("There was an error deserializing the OrderedDictionary.  The ArrayList does not contain DictionaryEntries");
        //    }
        //}
        #endregion

        #region IComparable Members
        int IComparable.CompareTo(object obj)
        {
            return ((IComparable)_dictionary).CompareTo(obj);
        }

        #endregion

        #region Переопределение понятия равенства
        /// <summary>Два экземпляра OrderedDictionary считаются равными, если соответственно равны
        /// все хранящиеся в них значения (сопоставление значений происходит по целочисленному индексу)
        /// </summary>
        /// <param name="x">Объект 1</param>
        /// <param name="y">Объект 2</param>
        /// <returns>True, если коллекции равны</returns>
        public static bool operator ==(ReadOnlyOrderedDictionary<TKey, TValue> x, ReadOnlyOrderedDictionary<TKey, TValue> y)
        {
            return Equals(x, y);
        }

        /// <summary>Два экземпляра OrderedDictionary считаются не равными, если не равна
        /// хотя бы одна пара хранящиеся в них значений (сопоставление значений происходит
        /// по целочисленному индексу)</summary>
        /// <param name="x">Объект 1</param>
        /// <param name="y">Объект 2</param>
        /// <returns>True, если коллекции не равны</returns>
        public static bool operator !=(ReadOnlyOrderedDictionary<TKey, TValue> x, ReadOnlyOrderedDictionary<TKey, TValue> y)
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
        public bool Equals(ReadOnlyOrderedDictionary<TKey, TValue> obj)
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
            return _dictionary.Equals(obj);
        }

        /// <summary>Чтобы не было warning-а</summary>
        /// <returns>Хэш-код объекта</returns>
        public override int GetHashCode()
        {
            // Строим строку
            var bld = new StringBuilder();
            for (var i = 0; i < _dictionary.Count; i++)
            {
                if (bld.Length > 0)
                    bld.Append("; ");

                bld.Append(_dictionary[i].NotNull());
            }
            bld.Append("RO");
            return bld.ToString().GetHashCode();
        }

        #endregion

    }

}

