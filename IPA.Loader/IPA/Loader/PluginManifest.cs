using System;
using System.Collections.Generic;
using IPA.JsonConverters;
using IPA.Utilities;
using Newtonsoft.Json;
using SemVer;

namespace IPA.Loader
{
	// Token: 0x02000048 RID: 72
	internal class PluginManifest
	{
		// Token: 0x040000B8 RID: 184
		[JsonProperty("name", Required = Required.Always)]
		public string Name;

		// Token: 0x040000B9 RID: 185
		[JsonProperty("id", Required = Required.AllowNull)]
		public string Id;

		// Token: 0x040000BA RID: 186
		[JsonProperty("description", Required = Required.Always)]
		[JsonConverter(typeof(MultilineStringConverter))]
		public string Description;

		// Token: 0x040000BB RID: 187
		[JsonProperty("version", Required = Required.Always)]
		[JsonConverter(typeof(SemverVersionConverter))]
		public global::SemVer.Version Version;

		// Token: 0x040000BC RID: 188
		[JsonProperty("gameVersion", Required = Required.Always)]
		[JsonConverter(typeof(IPA.JsonConverters.AlmostVersionConverter))]
		public AlmostVersion GameVersion;

		// Token: 0x040000BD RID: 189
		[JsonProperty("author", Required = Required.Always)]
		public string Author;

		// Token: 0x040000BE RID: 190
		[JsonProperty("dependsOn", Required = Required.DisallowNull, ItemConverterType = typeof(SemverRangeConverter))]
		public Dictionary<string, Range> Dependencies = new Dictionary<string, Range>();

		// Token: 0x040000BF RID: 191
		[JsonProperty("conflictsWith", Required = Required.DisallowNull, ItemConverterType = typeof(SemverRangeConverter))]
		public Dictionary<string, Range> Conflicts = new Dictionary<string, Range>();

		// Token: 0x040000C0 RID: 192
		[JsonProperty("features", Required = Required.DisallowNull)]
		public string[] Features = Array.Empty<string>();

		// Token: 0x040000C1 RID: 193
		[JsonProperty("loadBefore", Required = Required.DisallowNull)]
		public string[] LoadBefore = Array.Empty<string>();

		// Token: 0x040000C2 RID: 194
		[JsonProperty("loadAfter", Required = Required.DisallowNull)]
		public string[] LoadAfter = Array.Empty<string>();

		// Token: 0x040000C3 RID: 195
		[JsonProperty("icon", Required = Required.DisallowNull)]
		public string IconPath;

		// Token: 0x040000C4 RID: 196
		[JsonProperty("files", Required = Required.DisallowNull)]
		public string[] Files = Array.Empty<string>();

		// Token: 0x040000C5 RID: 197
		[JsonProperty("links", Required = Required.DisallowNull)]
		public PluginManifest.LinksObject Links;

		// Token: 0x040000C6 RID: 198
		[JsonProperty("misc", Required = Required.DisallowNull)]
		public PluginManifest.MiscObject Misc;

		// Token: 0x02000100 RID: 256
		[Serializable]
		public class LinksObject
		{
			// Token: 0x04000397 RID: 919
			[JsonProperty("project-home", Required = Required.DisallowNull)]
			public Uri ProjectHome;

			// Token: 0x04000398 RID: 920
			[JsonProperty("project-source", Required = Required.DisallowNull)]
			public Uri ProjectSource;

			// Token: 0x04000399 RID: 921
			[JsonProperty("donate", Required = Required.DisallowNull)]
			public Uri Donate;
		}

		// Token: 0x02000101 RID: 257
		[Serializable]
		public class MiscObject
		{
			// Token: 0x0400039A RID: 922
			[JsonProperty("plugin-hint", Required = Required.DisallowNull)]
			public string PluginMainHint;
		}
	}
}
