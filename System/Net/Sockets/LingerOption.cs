using System;

namespace System.Net.Sockets
{
	// Token: 0x0200036D RID: 877
	public class LingerOption
	{
		// Token: 0x06001FCC RID: 8140 RVA: 0x00094D68 File Offset: 0x00092F68
		public LingerOption(bool enable, int seconds)
		{
			this.Enabled = enable;
			this.LingerTime = seconds;
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06001FCD RID: 8141 RVA: 0x00094D7E File Offset: 0x00092F7E
		// (set) Token: 0x06001FCE RID: 8142 RVA: 0x00094D86 File Offset: 0x00092F86
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				this.enabled = value;
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06001FCF RID: 8143 RVA: 0x00094D8F File Offset: 0x00092F8F
		// (set) Token: 0x06001FD0 RID: 8144 RVA: 0x00094D97 File Offset: 0x00092F97
		public int LingerTime
		{
			get
			{
				return this.lingerTime;
			}
			set
			{
				this.lingerTime = value;
			}
		}

		// Token: 0x04001DD7 RID: 7639
		private bool enabled;

		// Token: 0x04001DD8 RID: 7640
		private int lingerTime;
	}
}
