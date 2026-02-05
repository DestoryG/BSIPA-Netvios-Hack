using System;

namespace System.IO
{
	// Token: 0x02000406 RID: 1030
	public struct WaitForChangedResult
	{
		// Token: 0x060026A2 RID: 9890 RVA: 0x000B1DAF File Offset: 0x000AFFAF
		internal WaitForChangedResult(WatcherChangeTypes changeType, string name, bool timedOut)
		{
			this = new WaitForChangedResult(changeType, name, null, timedOut);
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x000B1DBB File Offset: 0x000AFFBB
		internal WaitForChangedResult(WatcherChangeTypes changeType, string name, string oldName, bool timedOut)
		{
			this.changeType = changeType;
			this.name = name;
			this.oldName = oldName;
			this.timedOut = timedOut;
		}

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x060026A4 RID: 9892 RVA: 0x000B1DDA File Offset: 0x000AFFDA
		// (set) Token: 0x060026A5 RID: 9893 RVA: 0x000B1DE2 File Offset: 0x000AFFE2
		public WatcherChangeTypes ChangeType
		{
			get
			{
				return this.changeType;
			}
			set
			{
				this.changeType = value;
			}
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x060026A6 RID: 9894 RVA: 0x000B1DEB File Offset: 0x000AFFEB
		// (set) Token: 0x060026A7 RID: 9895 RVA: 0x000B1DF3 File Offset: 0x000AFFF3
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x060026A8 RID: 9896 RVA: 0x000B1DFC File Offset: 0x000AFFFC
		// (set) Token: 0x060026A9 RID: 9897 RVA: 0x000B1E04 File Offset: 0x000B0004
		public string OldName
		{
			get
			{
				return this.oldName;
			}
			set
			{
				this.oldName = value;
			}
		}

		// Token: 0x1700098C RID: 2444
		// (get) Token: 0x060026AA RID: 9898 RVA: 0x000B1E0D File Offset: 0x000B000D
		// (set) Token: 0x060026AB RID: 9899 RVA: 0x000B1E15 File Offset: 0x000B0015
		public bool TimedOut
		{
			get
			{
				return this.timedOut;
			}
			set
			{
				this.timedOut = value;
			}
		}

		// Token: 0x040020DD RID: 8413
		private WatcherChangeTypes changeType;

		// Token: 0x040020DE RID: 8414
		private string name;

		// Token: 0x040020DF RID: 8415
		private string oldName;

		// Token: 0x040020E0 RID: 8416
		private bool timedOut;

		// Token: 0x040020E1 RID: 8417
		internal static readonly WaitForChangedResult TimedOutResult = new WaitForChangedResult((WatcherChangeTypes)0, null, true);
	}
}
