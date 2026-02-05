using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002EC RID: 748
	public class PingOptions
	{
		// Token: 0x06001A4D RID: 6733 RVA: 0x0007FAEA File Offset: 0x0007DCEA
		internal PingOptions(IPOptions options)
		{
			this.ttl = (int)options.ttl;
			this.dontFragment = (options.flags & 2) > 0;
		}

		// Token: 0x06001A4E RID: 6734 RVA: 0x0007FB1E File Offset: 0x0007DD1E
		public PingOptions(int ttl, bool dontFragment)
		{
			if (ttl <= 0)
			{
				throw new ArgumentOutOfRangeException("ttl");
			}
			this.ttl = ttl;
			this.dontFragment = dontFragment;
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x0007FB4E File Offset: 0x0007DD4E
		public PingOptions()
		{
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001A50 RID: 6736 RVA: 0x0007FB61 File Offset: 0x0007DD61
		// (set) Token: 0x06001A51 RID: 6737 RVA: 0x0007FB69 File Offset: 0x0007DD69
		public int Ttl
		{
			get
			{
				return this.ttl;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ttl = value;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001A52 RID: 6738 RVA: 0x0007FB81 File Offset: 0x0007DD81
		// (set) Token: 0x06001A53 RID: 6739 RVA: 0x0007FB89 File Offset: 0x0007DD89
		public bool DontFragment
		{
			get
			{
				return this.dontFragment;
			}
			set
			{
				this.dontFragment = value;
			}
		}

		// Token: 0x04001A7D RID: 6781
		private const int DontFragmentFlag = 2;

		// Token: 0x04001A7E RID: 6782
		private int ttl = 128;

		// Token: 0x04001A7F RID: 6783
		private bool dontFragment;
	}
}
