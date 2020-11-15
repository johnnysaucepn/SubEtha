using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using static Howatworks.SubEtha.Journal.Exploration.Scan;

namespace Howatworks.SubEtha.Parser
{
    public class ParentItemConverter : JsonConverter<ParentItem>
    {
        public override ParentItem ReadJson(JsonReader reader, Type objectType, ParentItem existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            var prop = jObject.Properties().First();
            //return new ParentItem { BodyType = prop.Name, BodyID = jObject.Value<int>() };
            //return new ParentItem { BodyType = prop.Name, BodyID = prop.Value.Value<int>() };

            //return new ParentItem();
            return new ParentItem { BodyType = prop.Name, BodyID = prop.Value.Value<int>() };
            //return new ParentItem { BodyType = reader.Path, BodyID = value.Value };
        }

        public override void WriteJson(JsonWriter writer, ParentItem value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(value.BodyType);
            writer.WriteValue(value.BodyID);
            writer.WriteEndObject();
        }
    }
}
