using System;

namespace WotDossier.Dal.DataAccess
{
    /// <summary>
    /// Исключение, используемое при доступе к полям DataRow, или похожим на него классов - если указанное поле отсутствует
    /// Возможна генерация за пределами Core
    /// </summary>
    public abstract class DataFieldException : Exception
    {
        /// <summary>Индекс поля</summary>
        public int FieldIndex { get; private set; }

        /// <summary>Имя поля</summary>
        public string FieldName { get; private set; }

        /// <summary>
        /// Было ли для доступа к полю использовано имя поля
        /// </summary>
        public bool IsFieldAccessedByName { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="index">Индекс поля данных</param>
        /// <param name="message">Строка сообщения</param>
		/// <param name="inner">Нижележащее исключение</param>
		protected DataFieldException(int index, string message, Exception inner)
			: base(string.Format(message, index), inner)
		{
			FieldIndex = index;
			FieldName = "";
		}

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя поля данных</param>
		/// <param name="inner">Нижележащее исключение</param>
		protected DataFieldException(string name, string message, Exception inner)
			: base(string.Format(message, name), inner)
        {
            FieldName = name;
            IsFieldAccessedByName = true;
            FieldIndex = -1;
        }
    }

    /// <summary>
    /// Исключение, используемое при null-значении поля в DataRow
    /// Возможна генерация за пределами Core
    /// </summary>
    public class NullDataFieldException : DataFieldException
    {
        /// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="index">Индекс поля данных</param>
		/// <param name="inner">Нижележащее исключение</param>
		public NullDataFieldException(int index, Exception inner = null) : base(index, "Field {0} is null", inner) { }

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Имя поля данных</param>
		/// <param name="inner">Нижележащее исключение</param>
		public NullDataFieldException(string name, Exception inner = null) : base(name, "Field {0} is null", inner) { }
    }

    /// <summary>
    /// Исключение, используемое при null-значении поля в DataRow
    /// Возможна генерация за пределами Core
    /// </summary>
    public class NoDataFieldException : DataFieldException
    {
        /// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="index">Индекс поля данных</param>
		/// <param name="inner">Нижележащее исключение</param>
		public NoDataFieldException(int index, Exception inner = null) : base(index, "Field {0} doesnot exist", inner) { }

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Имя поля данных</param>
		/// <param name="inner">Нижележащее исключение</param>
		public NoDataFieldException(string name, Exception inner = null) : base(name, "Field {0} doesnot exist", inner) { }
    }
}
