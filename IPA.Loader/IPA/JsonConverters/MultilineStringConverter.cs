using System;
using Newtonsoft.Json;

namespace IPA.JsonConverters
{
	// Token: 0x02000058 RID: 88
	internal class MultilineStringConverter : JsonConverter<string>
	{
		// Token: 0x0600027D RID: 637 RVA: 0x0000CE00 File Offset: 0x0000B000
		public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.StartArray)
			{
				string[] list = serializer.Deserialize<string[]>(reader);
				return string.Join("\n", list);
			}
			return reader.Value as string;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000CE38 File Offset: 0x0000B038
		public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
		{
			string[] list = value.Split(new char[] { '\n' });
			if (list.Length == 1)
			{
				serializer.Serialize(writer, value);
				return;
			}
			serializer.Serialize(writer, list);
		}
	}
}
