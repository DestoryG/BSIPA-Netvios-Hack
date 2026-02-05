using System;

namespace System.Net
{
	// Token: 0x020000D7 RID: 215
	internal struct HeaderVariantInfo
	{
		// Token: 0x0600074B RID: 1867 RVA: 0x000282A6 File Offset: 0x000264A6
		internal HeaderVariantInfo(string name, CookieVariant variant)
		{
			this.m_name = name;
			this.m_variant = variant;
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x000282B6 File Offset: 0x000264B6
		internal string Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x000282BE File Offset: 0x000264BE
		internal CookieVariant Variant
		{
			get
			{
				return this.m_variant;
			}
		}

		// Token: 0x04000D0D RID: 3341
		private string m_name;

		// Token: 0x04000D0E RID: 3342
		private CookieVariant m_variant;
	}
}
