using System;

namespace System.Net.Cache
{
	// Token: 0x02000312 RID: 786
	public class RequestCachePolicy
	{
		// Token: 0x06001C0E RID: 7182 RVA: 0x00085B49 File Offset: 0x00083D49
		public RequestCachePolicy()
			: this(RequestCacheLevel.Default)
		{
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x00085B52 File Offset: 0x00083D52
		public RequestCachePolicy(RequestCacheLevel level)
		{
			if (level < RequestCacheLevel.Default || level > RequestCacheLevel.NoCacheNoStore)
			{
				throw new ArgumentOutOfRangeException("level");
			}
			this.m_Level = level;
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06001C10 RID: 7184 RVA: 0x00085B74 File Offset: 0x00083D74
		public RequestCacheLevel Level
		{
			get
			{
				return this.m_Level;
			}
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x00085B7C File Offset: 0x00083D7C
		public override string ToString()
		{
			return "Level:" + this.m_Level.ToString();
		}

		// Token: 0x04001B51 RID: 6993
		private RequestCacheLevel m_Level;
	}
}
