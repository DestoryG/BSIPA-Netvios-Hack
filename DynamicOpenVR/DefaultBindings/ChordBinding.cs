using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DynamicOpenVR.DefaultBindings
{
	// Token: 0x020000E2 RID: 226
	internal class ChordBinding
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600022E RID: 558 RVA: 0x00006756 File Offset: 0x00004956
		// (set) Token: 0x0600022F RID: 559 RVA: 0x0000675E File Offset: 0x0000495E
		[JsonProperty(PropertyName = "output")]
		public string output { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000230 RID: 560 RVA: 0x00006767 File Offset: 0x00004967
		// (set) Token: 0x06000231 RID: 561 RVA: 0x0000676F File Offset: 0x0000496F
		[JsonProperty(PropertyName = "inputs")]
		public List<List<string>> inputs { get; set; }
	}
}
