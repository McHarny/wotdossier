using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WotDossier.Domain.Replay
{
	public class PersonalAchievementsConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return true;
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			object result;
			try
			{
				result = serializer.Deserialize(reader, typeof(List<int>));
				return result;

			}
			catch (Exception e)
			{
				// ignored
			}

			try
			{
				var list = new List<int>();
				var arr = (object[])serializer.Deserialize(reader, typeof(object[]));
				foreach (var o in arr)
				{
					var mm = Dictionaries.Instance.Medals.Values.FirstOrDefault(m => string.Equals(m.Icon, arr[0].ToString() + arr[1].ToString(), StringComparison.CurrentCultureIgnoreCase));
					if (mm != null)
						list.Add(mm.Id);
					else
					{
						mm = Dictionaries.Instance.Medals.Values.FirstOrDefault(m => string.Equals(m.Icon, arr[0].ToString(), StringComparison.CurrentCultureIgnoreCase));
						if (mm != null)
							list.Add(mm.Id);
					}
				}
				return list;
			}
			catch (Exception ex)
			{
				// ignored
			}
			result = serializer.Deserialize(reader, typeof(Tuple<string, int>[]));
			
			return result;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			serializer.Serialize(writer, typeof(List<int>));
		}
	}
}
