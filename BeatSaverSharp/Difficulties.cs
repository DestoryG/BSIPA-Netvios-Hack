using System;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
	// Token: 0x0200000C RID: 12
	public struct Difficulties
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003618 File Offset: 0x00001818
		// (set) Token: 0x06000080 RID: 128 RVA: 0x00003620 File Offset: 0x00001820
		[JsonProperty("easy")]
		public bool Easy { readonly get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003629 File Offset: 0x00001829
		// (set) Token: 0x06000082 RID: 130 RVA: 0x00003631 File Offset: 0x00001831
		[JsonProperty("normal")]
		public bool Normal { readonly get; private set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000083 RID: 131 RVA: 0x0000363A File Offset: 0x0000183A
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00003642 File Offset: 0x00001842
		[JsonProperty("hard")]
		public bool Hard { readonly get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000085 RID: 133 RVA: 0x0000364B File Offset: 0x0000184B
		// (set) Token: 0x06000086 RID: 134 RVA: 0x00003653 File Offset: 0x00001853
		[JsonProperty("expert")]
		public bool Expert { readonly get; private set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000087 RID: 135 RVA: 0x0000365C File Offset: 0x0000185C
		// (set) Token: 0x06000088 RID: 136 RVA: 0x00003664 File Offset: 0x00001864
		[JsonProperty("expertPlus")]
		public bool ExpertPlus { readonly get; private set; }
	}
}
