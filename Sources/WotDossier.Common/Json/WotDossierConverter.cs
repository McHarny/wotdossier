using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WotDossier.Common.Json
{
    public class WotDossierConverter : JsonConverter
    {
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(string[]) || objectType == typeof(List<byte>) || objectType == typeof(byte[]);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value != null && value is byte[])
        {
            writer.WriteRawValue(JsonConvert.SerializeObject(((byte[])value).ToList(), Formatting.None));
        }
        else
            writer.WriteRawValue(JsonConvert.SerializeObject(value, Formatting.None));
    }
    }
}
