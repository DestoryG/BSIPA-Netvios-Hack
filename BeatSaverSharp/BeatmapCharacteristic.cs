using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
	// Token: 0x0200000D RID: 13
	public struct BeatmapCharacteristic
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000089 RID: 137 RVA: 0x0000366D File Offset: 0x0000186D
		// (set) Token: 0x0600008A RID: 138 RVA: 0x00003675 File Offset: 0x00001875
		[JsonProperty("name")]
		public string Name { readonly get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600008B RID: 139 RVA: 0x0000367E File Offset: 0x0000187E
		// (set) Token: 0x0600008C RID: 140 RVA: 0x00003686 File Offset: 0x00001886
		[JsonProperty("difficulties")]
		public ReadOnlyDictionary<string, BeatmapCharacteristicDifficulty?> Difficulties { readonly get; private set; }
	}
}
