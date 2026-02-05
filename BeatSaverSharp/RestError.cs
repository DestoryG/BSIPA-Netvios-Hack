using System;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
	// Token: 0x02000011 RID: 17
	public struct RestError
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000BC RID: 188 RVA: 0x0000393B File Offset: 0x00001B3B
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00003943 File Offset: 0x00001B43
		[JsonProperty("code")]
		public int Code { readonly get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000BE RID: 190 RVA: 0x0000394C File Offset: 0x00001B4C
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00003954 File Offset: 0x00001B54
		[JsonProperty("identifier")]
		public string Identifier { readonly get; private set; }
	}
}
