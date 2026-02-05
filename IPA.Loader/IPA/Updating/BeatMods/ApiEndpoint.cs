using System;
using System.Linq;
using IPA.JsonConverters;
using IPA.Utilities;
using Newtonsoft.Json;
using SemVer;

namespace IPA.Updating.BeatMods
{
	// Token: 0x0200000D RID: 13
	internal class ApiEndpoint
	{
		// Token: 0x04000008 RID: 8
		public const string BeatModBase = "https://beatmods.com";

		// Token: 0x04000009 RID: 9
		public const string ApiBase = "https://beatmods.com/api/v1/mod";

		// Token: 0x0400000A RID: 10
		public const string GetModInfoEndpoint = "?name={0}&version={1}";

		// Token: 0x0400000B RID: 11
		public const string GetModsByName = "?name={0}";

		// Token: 0x0200009C RID: 156
		private class HexArrayConverter : JsonConverter
		{
			// Token: 0x060003F0 RID: 1008 RVA: 0x00013D87 File Offset: 0x00011F87
			public override bool CanConvert(Type objectType)
			{
				return objectType == typeof(byte[]);
			}

			// Token: 0x060003F1 RID: 1009 RVA: 0x00013D9C File Offset: 0x00011F9C
			public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
			{
				if (reader.TokenType == JsonToken.Null)
				{
					return null;
				}
				if (reader.TokenType == JsonToken.String)
				{
					try
					{
						return Utils.StringToByteArray((string)reader.Value);
					}
					catch (Exception ex)
					{
						throw new Exception(string.Format("Error parsing version string: {0}", reader.Value), ex);
					}
				}
				throw new Exception(string.Format("Unexpected token or value when parsing hex string. Token: {0}, Value: {1}", reader.TokenType, reader.Value));
			}

			// Token: 0x060003F2 RID: 1010 RVA: 0x00013E1C File Offset: 0x0001201C
			public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
			{
				if (value == null)
				{
					writer.WriteNull();
					return;
				}
				if (!(value is byte[]))
				{
					throw new JsonSerializationException("Expected byte[] object value");
				}
				writer.WriteValue(Utils.ByteArrayToString((byte[])value));
			}
		}

		// Token: 0x0200009D RID: 157
		private class ModMultiformatJsonConverter : JsonConverter<ApiEndpoint.Mod>
		{
			// Token: 0x060003F4 RID: 1012 RVA: 0x00013E54 File Offset: 0x00012054
			public override ApiEndpoint.Mod ReadJson(JsonReader reader, Type objectType, ApiEndpoint.Mod existingValue, bool hasExistingValue, JsonSerializer serializer)
			{
				if (reader.TokenType == JsonToken.String)
				{
					return new ApiEndpoint.Mod
					{
						Id = (reader.Value as string),
						IsIdReference = true
					};
				}
				if (reader.TokenType != JsonToken.StartObject)
				{
					return null;
				}
				return serializer.Deserialize<ApiEndpoint.Mod>(reader);
			}

			// Token: 0x060003F5 RID: 1013 RVA: 0x00013E91 File Offset: 0x00012091
			public override void WriteJson(JsonWriter writer, ApiEndpoint.Mod value, JsonSerializer serializer)
			{
				serializer.Serialize(writer, value);
			}
		}

		// Token: 0x0200009E RID: 158
		[Serializable]
		public class Mod
		{
			// Token: 0x060003F7 RID: 1015 RVA: 0x00013EA4 File Offset: 0x000120A4
			public override string ToString()
			{
				string text = "{{\"{0}\"v{1} by {2} files for {3}}}";
				object[] array = new object[4];
				array[0] = this.Name;
				array[1] = this.Version;
				array[2] = this.Author;
				array[3] = string.Join(", ", this.Downloads.Select((ApiEndpoint.Mod.DownloadsObject d) => d.Type));
				return string.Format(text, array);
			}

			/// <summary>
			/// Will be a useless string of characters. Do not use.
			/// </summary>
			// Token: 0x04000134 RID: 308
			[JsonProperty("_id")]
			public string Id;

			// Token: 0x04000135 RID: 309
			[JsonIgnore]
			public bool IsIdReference;

			// Token: 0x04000136 RID: 310
			[JsonProperty("required")]
			public bool Required;

			// Token: 0x04000137 RID: 311
			[JsonProperty("name")]
			public string Name;

			// Token: 0x04000138 RID: 312
			[JsonProperty("version")]
			[JsonConverter(typeof(SemverVersionConverter))]
			public global::SemVer.Version Version;

			// Token: 0x04000139 RID: 313
			[JsonProperty("gameVersion")]
			[JsonConverter(typeof(IPA.JsonConverters.AlmostVersionConverter))]
			public AlmostVersion GameVersion;

			// Token: 0x0400013A RID: 314
			[JsonProperty("author")]
			public ApiEndpoint.Mod.AuthorType Author;

			// Token: 0x0400013B RID: 315
			[JsonProperty("status")]
			public string Status;

			// Token: 0x0400013C RID: 316
			public const string ApprovedStatus = "approved";

			// Token: 0x0400013D RID: 317
			[JsonProperty("description")]
			public string Description;

			// Token: 0x0400013E RID: 318
			[JsonProperty("category")]
			public string Category;

			// Token: 0x0400013F RID: 319
			[JsonProperty("link")]
			public Uri Link;

			// Token: 0x04000140 RID: 320
			[JsonProperty("downloads")]
			public ApiEndpoint.Mod.DownloadsObject[] Downloads;

			// Token: 0x04000141 RID: 321
			[JsonProperty("dependencies", ItemConverterType = typeof(ApiEndpoint.ModMultiformatJsonConverter))]
			public ApiEndpoint.Mod[] Dependencies;

			// Token: 0x0200015A RID: 346
			[Serializable]
			public class AuthorType
			{
				// Token: 0x060006AC RID: 1708 RVA: 0x0001882A File Offset: 0x00016A2A
				public override string ToString()
				{
					return this.Name;
				}

				// Token: 0x0400045C RID: 1116
				[JsonProperty("username")]
				public string Name;

				// Token: 0x0400045D RID: 1117
				[JsonProperty("_id")]
				public string Id;
			}

			// Token: 0x0200015B RID: 347
			[Serializable]
			public class DownloadsObject
			{
				// Token: 0x0400045E RID: 1118
				public const string TypeUniversal = "universal";

				// Token: 0x0400045F RID: 1119
				public const string TypeSteam = "steam";

				// Token: 0x04000460 RID: 1120
				public const string TypeOculus = "oculus";

				// Token: 0x04000461 RID: 1121
				[JsonProperty("type")]
				public string Type;

				// Token: 0x04000462 RID: 1122
				[JsonProperty("url")]
				public string Path;

				/// <summary>
				/// Hashes stored are MD5
				/// </summary>
				// Token: 0x04000463 RID: 1123
				[JsonProperty("hashMd5")]
				public ApiEndpoint.Mod.DownloadsObject.HashObject[] Hashes;

				// Token: 0x02000163 RID: 355
				[Serializable]
				public class HashObject
				{
					// Token: 0x04000473 RID: 1139
					[JsonProperty("hash")]
					[JsonConverter(typeof(ApiEndpoint.HexArrayConverter))]
					public byte[] Hash;

					// Token: 0x04000474 RID: 1140
					[JsonProperty("file")]
					public string File;
				}
			}
		}
	}
}
