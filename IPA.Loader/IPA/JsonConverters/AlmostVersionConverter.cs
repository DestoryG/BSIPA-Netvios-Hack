using System;
using IPA.Utilities;
using Newtonsoft.Json;

namespace IPA.JsonConverters
{
	// Token: 0x02000057 RID: 87
	internal class AlmostVersionConverter : JsonConverter<AlmostVersion>
	{
		// Token: 0x0600027A RID: 634 RVA: 0x0000CDBD File Offset: 0x0000AFBD
		public override AlmostVersion ReadJson(JsonReader reader, Type objectType, AlmostVersion existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (reader.Value != null)
			{
				return new AlmostVersion(reader.Value as string);
			}
			return null;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000CDD9 File Offset: 0x0000AFD9
		public override void WriteJson(JsonWriter writer, AlmostVersion value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			writer.WriteValue(value.ToString());
		}
	}
}
