using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace WotDossier.Common.Reflection
{
	partial class ReflectionExtensions
	{
		/// <exception cref="EntryPointNotFoundException"><c>EntryPointNotFoundException</c>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="instance" /> is <c>null</c>.</exception>
		public static T GetField<T>(this object instance, [LocalizationRequired(false)] string fieldName)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			Type type = null;
			if (instance is Type)
			{
				type = (Type)instance;
				instance = null;
			}

			return GetField<T>(instance, type, fieldName);
		}

		/// <exception cref="EntryPointNotFoundException"><c>EntryPointNotFoundException</c>.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="instance" /> is <c>null</c>.</exception>
		public static T GetField<T>(this object instance, Type type, [LocalizationRequired(false)] string fieldName)
		{
			if (instance == null && type == null)
				throw new ArgumentNullException("instance");

			bool isStatic = (instance == null);

			if (type == null)
				type = instance.GetType();


			var fi = type.GetField(fieldName, BindingFlags.GetField | BindingFlags.Public | ((isStatic) ? BindingFlags.Static : BindingFlags.Instance));
			if (fi == null)
			{
				fi = type.GetField(fieldName, BindingFlags.GetField | BindingFlags.NonPublic | ((isStatic) ? BindingFlags.Static : BindingFlags.Instance));
				if (fi == null)
					throw new EntryPointNotFoundException(string.Format("Field with name {0} Not fount in Type {1}", fieldName, type.Name));
			}
			return (T)fi.GetValue(instance);
		}

		public static void SetField(this object instance, [LocalizationRequired(false)] string fieldName, object value)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			Type type = null;
			if (instance is Type)
			{
				type = (Type)instance;
				instance = null;
			}

			SetField(instance, type, fieldName, value);
		}

		public static void SetField(this object instance, Type type, [LocalizationRequired(false)] string fieldName, object value)
		{

			if (instance == null && type == null)
				throw new ArgumentNullException("instance");

			bool isStatic = (instance == null);

			if (type == null)
				type = instance.GetType();

			var fi = type.GetField(fieldName, BindingFlags.SetField | BindingFlags.Public | ((isStatic) ? BindingFlags.Static : BindingFlags.Instance));
			if (fi == null)
			{
				fi = type.GetField(fieldName, BindingFlags.SetField | BindingFlags.NonPublic | ((isStatic) ? BindingFlags.Static : BindingFlags.Instance));
				if (fi == null)
					throw new EntryPointNotFoundException(string.Format("Field with name {0} Not fount in Type {1}", fieldName, type.Name));
			}
			fi.SetValue(instance, value);
		}


		#region Set Properties
		public static T GetProperty<T>(this object instance, [LocalizationRequired(false)] string propName)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			Type type = null;
			if (instance is Type)
			{
				type = (Type)instance;
				instance = null;
			}

			return GetProperty<T>(instance, type, propName);
		}

		public static T GetProperty<T>(this object instance, Type type, [LocalizationRequired(false)] string propName)
		{
			if (instance == null && type == null)
				throw new ArgumentNullException("instance");

			bool isStatic = (instance == null);

			if (type == null)
				type = instance.GetType();

			var fi = type.GetProperty(propName, BindingFlags.GetProperty | BindingFlags.Public | ((isStatic) ? BindingFlags.Static : BindingFlags.Instance));
			if (fi == null)
			{
				fi = type.GetProperty(propName, BindingFlags.GetProperty | BindingFlags.NonPublic | ((isStatic) ? BindingFlags.Static : BindingFlags.Instance));
				if (fi == null)
					throw new EntryPointNotFoundException(string.Format("Field with name {0} Not fount in Type {1}", propName, type.Name));
			}
			return (T)fi.GetValue(instance, null);
		}

		public static void SetProperty(this object instance, [LocalizationRequired(false)] string propName, object value)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			Type type = null;
			if (instance is Type)
			{
				type = (Type)instance;
				instance = null;
			}

			SetProperty(instance, type, propName, value);
		}

		public static void SetProperty(this object instance, Type type, [LocalizationRequired(false)] string propName, object value)
		{
			if (instance == null && type == null)
				throw new ArgumentNullException("instance");

			bool isStatic = (instance == null);

			if (type == null)
				type = instance.GetType();

			var fi = type.GetProperty(propName, BindingFlags.GetProperty | BindingFlags.Public | ((isStatic) ? BindingFlags.Static : BindingFlags.Instance));
			if (fi == null)
			{
				fi = type.GetProperty(propName, BindingFlags.GetProperty | BindingFlags.NonPublic | ((isStatic) ? BindingFlags.Static : BindingFlags.Instance));
				if (fi == null)
					throw new EntryPointNotFoundException(string.Format("Property with name {0} Not fount in Type {1}", propName, type.Name));
			}
			fi.SetValue(((isStatic) ? null : instance), value, null);
		}
		#endregion

		#region Invoke Methods
		public static T InvokeMethod<T>(this object instance, [LocalizationRequired(false)] string methodName, params object[] parms)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			Type type = null;
			if (instance is Type)
			{
				type = (Type)instance;
				instance = null;
			}

			return InvokeMethod<T>(instance, type, methodName, parms);

		}

		public static T InvokeMethod<T>(this object instance, Type type, [LocalizationRequired(false)] string methodName, params object[] parms)
		{
			if (instance == null && type == null)
				throw new ArgumentNullException("instance");

			bool isStatic = (instance == null);

			if (type == null)
				type = instance.GetType();

			var mi = type.GetMethod(methodName, BindingFlags.InvokeMethod | ((isStatic) ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.Public);

			if (mi == null)
			{
				mi = type.GetMethod(methodName, BindingFlags.InvokeMethod | ((isStatic) ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.NonPublic);
				if (mi == null)
					throw new EntryPointNotFoundException(string.Format("Method with name {0} Not fount in Type {1}. Use more specified methods", methodName, type.Name));
			}

			var result = mi.Invoke(instance, parms);

			return (mi.ReturnType == typeof(void)) ? default(T) : (T)result;
		}

		public static T InvokeMethodSpec<T>(this object instance, [LocalizationRequired(false)] string methodName, Type[] types, params object[] parms)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			bool isStatic = false;
			Type type;
			if (instance is Type)
			{
				type = (Type)instance;
				isStatic = true;
			}
			else
				type = instance.GetType();

			var mi = type.GetMethod(methodName, BindingFlags.InvokeMethod | ((isStatic) ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.Public, null, types, null);

			if (mi == null)
			{
				mi = type.GetMethod(methodName, BindingFlags.InvokeMethod | ((isStatic) ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.NonPublic, null, types, null);
				if (mi == null)
					throw new EntryPointNotFoundException(string.Format("Method with name {0} Not fount in Type {1}. Use more specified methods", methodName, type.Name));
			}

			var result = mi.Invoke(((isStatic) ? null : instance), parms);
			return (mi.ReturnType == typeof(void)) ? default(T) : (T)result;
		}

		public static T InvokeBaseMethodSpec<T>(this object instance, [LocalizationRequired(false)] string methodName, Type baseType, Type[] types, params object[] parms)
		{
			if(instance == null)
				throw new ArgumentNullException(nameof(instance));

			bool isStatic = false;
			Type type;
			if (instance is Type)
			{
				type = (Type)instance;
				isStatic = true;
			}
			else
				type = instance.GetType();

			var mi = baseType.GetMethod(methodName, BindingFlags.InvokeMethod | ((isStatic) ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.Public, null, types, null);

			if (mi == null)
			{
				mi = baseType.GetMethod(methodName, BindingFlags.InvokeMethod | ((isStatic) ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.NonPublic, null, types, null);
				if (mi == null)
					throw new EntryPointNotFoundException(
						$"Method with name {methodName} Not fount in Type {type.Name}. Use more specified methods");
			}

			var result = mi.Invoke(((isStatic) ? null : instance), parms);
			return (mi.ReturnType == typeof(void)) ? default(T) : (T)result;
		}
		#endregion

		#region Invoke Methods

		public static Delegate CreateDelegate(this object instance, Type delegateType, [LocalizationRequired(false)] string methodName)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");

			if (delegateType == null)
				throw new ArgumentNullException("delegateType");

			Type type = null;
			if (instance is Type)
			{
				type = (Type)instance;
				instance = null;
			}

			return CreateDelegate(instance, type, delegateType, methodName);
		}

		public static Delegate CreateDelegate(this object instance, Type type, Type delegateType, [LocalizationRequired(false)] string methodName)
		{
			if (instance == null && type == null)
				throw new ArgumentNullException("instance");

			if (delegateType == null)
				throw new ArgumentNullException("delegateType");

			bool isStatic = (instance == null);

			if (type == null)
				type = instance.GetType();

			var mi = type.GetMethod(methodName, BindingFlags.InvokeMethod | ((isStatic) ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.Public);

			if (mi == null)
			{
				mi = type.GetMethod(methodName, BindingFlags.InvokeMethod | ((isStatic) ? BindingFlags.Static : BindingFlags.Instance) | BindingFlags.NonPublic);
				if (mi == null)
					throw new EntryPointNotFoundException(string.Format("Method with name {0} Not fount in Type {1}. Use more specified methods", methodName, type.Name));
			}

			return (isStatic) ? Delegate.CreateDelegate(delegateType, mi, true) : Delegate.CreateDelegate(delegateType, instance, mi, true);
		}

		#endregion

		/// <summary>
		/// The is assignable from ex.
		/// </summary>
		/// <param name="type">
		/// The type.
		/// </param>
		/// <param name="typeToCheck">
		/// The type to check.
		/// </param>
		/// <returns>
		/// The is assignable from ex.
		/// </returns>
		/// <exception cref="System.ArgumentNullException">
		/// The <paramref name="type"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.ArgumentNullException">
		/// The <paramref name="typeToCheck"/> is <c>null</c>.
		/// </exception>
		[ContractAnnotation("type: null=>halt", true)]
		[ContractAnnotation("typeToCheck: null=>halt", true)]
		public static bool IsAssignableFromEx(this Type type, Type typeToCheck)
		{
			Contract.Requires(type != null);
			Contract.Requires(typeToCheck != null);

#if !SILVERLIGHT
			//return type.GetTypeInfo().IsAssignableFrom(typeToCheck.GetTypeInfo());
			return type.IsAssignableFrom(typeToCheck);
#else
            return type.IsAssignableFrom(typeToCheck);
#endif
		}

		public static bool IsNullable(this Type t)
		{
			if (!t.IsGenericType || !t.IsValueType)
			{
				return false;
			}
			return t.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		/// <summary>
		/// Для данного класса коллекция -> для данного базового интерфейса коллекция методов
		/// </summary>
		private static Dictionary<Type, Dictionary<Type, Dictionary<string, MethodInfo>>> baseInterfaceCache = new Dictionary<Type, Dictionary<Type, Dictionary<string, MethodInfo>>>();


		public static T InvokeBaseInterfaceMethod<T>(this object instance, Type interfaceType,
			[LocalizationRequired(false)] string methodName, params object[] parms)
		{
			var type = instance.GetType();
			Contract.Assume(type != null);
			Contract.Assume(type != typeof(object));
			return InvokeBaseInterfaceMethod<T>(instance, type, interfaceType, methodName, parms);
		}

		public static T InvokeBaseInterfaceMethod<T>(this object instance, Type type, Type interfaceType,
			[LocalizationRequired(false)] string methodName, params object[] parms)
		{
			Contract.Requires(instance != null);
			Contract.Requires(interfaceType != null);
			Contract.Requires(interfaceType.IsInterface, "ifaceType must be an interface type");
			Contract.Requires(type != null);
			Contract.Requires(type != typeof(object));


			Dictionary<Type, Dictionary<string, MethodInfo>> typeCache;
			if (!baseInterfaceCache.TryGetValue(type, out typeCache))
			{
				typeCache = new Dictionary<Type, Dictionary<string, MethodInfo>>();
				baseInterfaceCache.Add(type, typeCache);
			}

			Dictionary<string, MethodInfo> interfaceCache;
			if (!typeCache.TryGetValue(interfaceType, out interfaceCache))
			{
				interfaceCache = new Dictionary<string, MethodInfo>();
				typeCache.Add(interfaceType, interfaceCache);
			}

			MethodInfo mi;
			if (!interfaceCache.TryGetValue(methodName, out mi))
			{
				var methodFullName = interfaceType.FullName + "." + methodName;
				var baseClass = type.BaseType;
				while (baseClass != null && baseClass != typeof(object))
				{
					var ti = baseClass.GetTypeInfo();
					if (!ti.ImplementedInterfaces.Contains(interfaceType)) continue;
					mi = ti.DeclaredMethods.FirstOrDefault(m => m.Name == methodFullName);
					if (mi != null)
					{
						interfaceCache.Add(methodName, mi);
						break;
					}

					baseClass = baseClass.BaseType;
				}

			}

			if (mi == null)
				throw new EntryPointNotFoundException($"Method with name {methodName} Not fount in Type {type.Name}. Use more specified methods");

			var result = mi.Invoke(instance, parms);

			return (mi.ReturnType == typeof(void)) ? default(T) : (T)result;
		}

		public static Type UnderlyingTypeOf(this Type t)
		{
			return t.GetGenericArguments()[0];
		}
	}
}
