using System;
using System.Text;

namespace System.Net.Cache
{
	// Token: 0x0200030B RID: 779
	internal class ResponseCacheControl
	{
		// Token: 0x06001BCB RID: 7115 RVA: 0x000842CC File Offset: 0x000824CC
		internal ResponseCacheControl()
		{
			this.MaxAge = (this.SMaxAge = -1);
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06001BCC RID: 7116 RVA: 0x000842F0 File Offset: 0x000824F0
		internal bool IsNotEmpty
		{
			get
			{
				return this.Public || this.Private || this.NoCache || this.NoStore || this.MustRevalidate || this.ProxyRevalidate || this.MaxAge != -1 || this.SMaxAge != -1;
			}
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x00084344 File Offset: 0x00082544
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Public)
			{
				stringBuilder.Append(" public");
			}
			if (this.Private)
			{
				stringBuilder.Append(" private");
				if (this.PrivateHeaders != null)
				{
					stringBuilder.Append('=');
					for (int i = 0; i < this.PrivateHeaders.Length - 1; i++)
					{
						stringBuilder.Append(this.PrivateHeaders[i]).Append(',');
					}
					stringBuilder.Append(this.PrivateHeaders[this.PrivateHeaders.Length - 1]);
				}
			}
			if (this.NoCache)
			{
				stringBuilder.Append(" no-cache");
				if (this.NoCacheHeaders != null)
				{
					stringBuilder.Append('=');
					for (int j = 0; j < this.NoCacheHeaders.Length - 1; j++)
					{
						stringBuilder.Append(this.NoCacheHeaders[j]).Append(',');
					}
					stringBuilder.Append(this.NoCacheHeaders[this.NoCacheHeaders.Length - 1]);
				}
			}
			if (this.NoStore)
			{
				stringBuilder.Append(" no-store");
			}
			if (this.MustRevalidate)
			{
				stringBuilder.Append(" must-revalidate");
			}
			if (this.ProxyRevalidate)
			{
				stringBuilder.Append(" proxy-revalidate");
			}
			if (this.MaxAge != -1)
			{
				stringBuilder.Append(" max-age=").Append(this.MaxAge);
			}
			if (this.SMaxAge != -1)
			{
				stringBuilder.Append(" s-maxage=").Append(this.SMaxAge);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001B26 RID: 6950
		internal bool Public;

		// Token: 0x04001B27 RID: 6951
		internal bool Private;

		// Token: 0x04001B28 RID: 6952
		internal string[] PrivateHeaders;

		// Token: 0x04001B29 RID: 6953
		internal bool NoCache;

		// Token: 0x04001B2A RID: 6954
		internal string[] NoCacheHeaders;

		// Token: 0x04001B2B RID: 6955
		internal bool NoStore;

		// Token: 0x04001B2C RID: 6956
		internal bool MustRevalidate;

		// Token: 0x04001B2D RID: 6957
		internal bool ProxyRevalidate;

		// Token: 0x04001B2E RID: 6958
		internal int MaxAge;

		// Token: 0x04001B2F RID: 6959
		internal int SMaxAge;
	}
}
