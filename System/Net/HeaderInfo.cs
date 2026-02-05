using System;

namespace System.Net
{
	// Token: 0x020001B4 RID: 436
	internal class HeaderInfo
	{
		// Token: 0x06001135 RID: 4405 RVA: 0x0005D80A File Offset: 0x0005BA0A
		internal HeaderInfo(string name, bool requestRestricted, bool responseRestricted, bool multi, HeaderParser p)
		{
			this.HeaderName = name;
			this.IsRequestRestricted = requestRestricted;
			this.IsResponseRestricted = responseRestricted;
			this.Parser = p;
			this.AllowMultiValues = multi;
		}

		// Token: 0x0400140A RID: 5130
		internal readonly bool IsRequestRestricted;

		// Token: 0x0400140B RID: 5131
		internal readonly bool IsResponseRestricted;

		// Token: 0x0400140C RID: 5132
		internal readonly HeaderParser Parser;

		// Token: 0x0400140D RID: 5133
		internal readonly string HeaderName;

		// Token: 0x0400140E RID: 5134
		internal readonly bool AllowMultiValues;
	}
}
