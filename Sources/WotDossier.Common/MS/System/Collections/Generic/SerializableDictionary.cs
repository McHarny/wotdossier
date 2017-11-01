using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace System.Collections.Generic
{
    [Serializable, DebuggerTypeProxy(typeof (System_DictionaryDebugView<,>)), DebuggerDisplay("Count = {Count}"),
     ComVisible(false)]

    //[XmlRoot("Dictionary")]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Generic.SerializableDictionary`2"/> class that is empty, has the default initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        public SerializableDictionary() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Generic.SerializableDictionary`2"/> class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.SerializableDictionary`2"/> can contain.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public SerializableDictionary(int capacity) : base(capacity) {  }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Generic.SerializableDictionary`2"/> class that is empty, has the default initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"/>.
        /// </summary>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> implementation to use when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1"/> for the type of the key.</param>
        public SerializableDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Generic.SerializableDictionary`2"/> class that is empty, has the specified initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"/>.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.SerializableDictionary`2"/> can contain.</param><param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> implementation to use when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1"/> for the type of the key.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public SerializableDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Generic.SerializableDictionary`2"/> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2"/> and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="dictionary">The <see cref="T:System.Collections.Generic.IDictionary`2"/> whose elements are copied to the new <see cref="T:System.Collections.Generic.Dictionary`2"/>.</param><exception cref="T:System.ArgumentNullException"><paramref name="dictionary"/> is null.</exception><exception cref="T:System.ArgumentException"><paramref name="dictionary"/> contains one or more duplicate keys.</exception>
        public SerializableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Generic.SerializableDictionary`2"/> class that contains elements copied from the specified <see cref="T:System.Collections.Generic.IDictionary`2"/> and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"/>.
        /// </summary>
        /// <param name="dictionary">The <see cref="T:System.Collections.Generic.IDictionary`2"/> whose elements are copied to the new <see cref="T:System.Collections.Generic.Dictionary`2"/>.</param><param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> implementation to use when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1"/> for the type of the key.</param><exception cref="T:System.ArgumentNullException"><paramref name="dictionary"/> is null.</exception><exception cref="T:System.ArgumentException"><paramref name="dictionary"/> contains one or more duplicate keys.</exception>
        public SerializableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer): base(dictionary, comparer) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Generic.SerializableDictionary`2"/> class with serialized data.
        /// </summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo"/> object containing the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2"/>.</param><param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext"/> structure containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2"/>.</param>
        protected SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            XDocument doc = null;
            using (XmlReader subtreeReader = reader.ReadSubtree())
            {
                doc = XDocument.Load(subtreeReader);
            }
            if (doc != null)
            {
                XmlSerializer serializer = new XmlSerializer(typeof (SerializableKeyValuePair));
                foreach (XElement item in doc.Descendants(XName.Get("Item")))
                {
                    using (XmlReader itemReader = item.CreateReader())
                    {
                        var kvp = serializer.Deserialize(itemReader) as SerializableKeyValuePair;
                        this.Add(kvp.Key, kvp.Value);
                    }
                }
            }
            if(doc.Root != null && !doc.Root.IsEmpty)
                reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof (SerializableKeyValuePair));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            foreach (TKey key in this.Keys)
            {
                TValue value = this[key];
                var kvp = new SerializableKeyValuePair(key, value);
                serializer.Serialize(writer, kvp, ns);
            }
        }

        #endregion

        [XmlRoot("Item")]
        public class SerializableKeyValuePair
        {
            [XmlAttribute("Key")] public TKey Key;

            [XmlAttribute("Value")] public TValue Value;

            /// <summary>
            /// Default constructor
            /// </summary>
            public SerializableKeyValuePair()
            {
            }

            public SerializableKeyValuePair(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }
    }
}