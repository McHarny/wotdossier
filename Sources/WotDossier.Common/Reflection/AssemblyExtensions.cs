using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace WotDossier.Common.Reflection
{
	public static class AssemblyExtensions
	{
		public static Type GetTypeByName(this Assembly assembly, [LocalizationRequired(false)] string typeName)
		{
			return assembly.GetTypes().FirstOrDefault(t => t.Name.ToUpper() == typeName.ToUpper() || t.FullName.ToUpper() == typeName.ToUpper());
		}
	}
}
