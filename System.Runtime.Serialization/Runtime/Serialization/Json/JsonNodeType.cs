using System;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x0200010E RID: 270
	internal enum JsonNodeType
	{
		// Token: 0x04000827 RID: 2087
		None,
		// Token: 0x04000828 RID: 2088
		Object,
		// Token: 0x04000829 RID: 2089
		Element,
		// Token: 0x0400082A RID: 2090
		EndElement,
		// Token: 0x0400082B RID: 2091
		QuotedText,
		// Token: 0x0400082C RID: 2092
		StandaloneText,
		// Token: 0x0400082D RID: 2093
		Collection
	}
}
