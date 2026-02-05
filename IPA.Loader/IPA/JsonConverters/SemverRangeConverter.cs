using System;
using Newtonsoft.Json;
using SemVer;

namespace IPA.JsonConverters
{
	// Token: 0x02000059 RID: 89
	internal class SemverRangeConverter : JsonConverter<Range>
	{
		// Token: 0x06000280 RID: 640 RVA: 0x0000CE76 File Offset: 0x0000B076
		public override Range ReadJson(JsonReader reader, Type objectType, Range existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return new Range(reader.Value as string, false);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000CE89 File Offset: 0x0000B089
		public override void WriteJson(JsonWriter writer, Range value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}
	}
}
