using System;
using Newtonsoft.Json;
using SemVer;

namespace IPA.JsonConverters
{
	// Token: 0x0200005A RID: 90
	internal class SemverVersionConverter : JsonConverter<global::SemVer.Version>
	{
		// Token: 0x06000283 RID: 643 RVA: 0x0000CE9F File Offset: 0x0000B09F
		public override global::SemVer.Version ReadJson(JsonReader reader, Type objectType, global::SemVer.Version existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (reader.Value != null)
			{
				return new global::SemVer.Version(reader.Value as string, true);
			}
			return null;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000CEBC File Offset: 0x0000B0BC
		public override void WriteJson(JsonWriter writer, global::SemVer.Version value, JsonSerializer serializer)
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
