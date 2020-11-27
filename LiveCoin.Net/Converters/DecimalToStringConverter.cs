using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;

namespace LiveCoin.Net.Converters
{
    class DecimalToStringConverter : JsonConverter
    {
        public override bool CanConvert(Type type)
        {
            return type == typeof(decimal) || type == typeof(decimal?);
        }

        public override object? ReadJson(JsonReader reader, Type type, object existingInstance, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return null;
            var t = decimal.Parse(reader.Value.ToString(), NumberStyles.Float);
            return t;
        }

        public override void WriteJson(JsonWriter writer, object instance, JsonSerializer serializer)
        {
            var number = (decimal)instance;
            writer.WriteValue(number.ToString(CultureInfo.InvariantCulture));
        }
    }
}