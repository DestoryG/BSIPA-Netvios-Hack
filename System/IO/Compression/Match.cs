using System;

namespace System.IO.Compression
{
	// Token: 0x02000435 RID: 1077
	internal class Match
	{
		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06002854 RID: 10324 RVA: 0x000B986E File Offset: 0x000B7A6E
		// (set) Token: 0x06002855 RID: 10325 RVA: 0x000B9876 File Offset: 0x000B7A76
		internal MatchState State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x06002856 RID: 10326 RVA: 0x000B987F File Offset: 0x000B7A7F
		// (set) Token: 0x06002857 RID: 10327 RVA: 0x000B9887 File Offset: 0x000B7A87
		internal int Position
		{
			get
			{
				return this.pos;
			}
			set
			{
				this.pos = value;
			}
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06002858 RID: 10328 RVA: 0x000B9890 File Offset: 0x000B7A90
		// (set) Token: 0x06002859 RID: 10329 RVA: 0x000B9898 File Offset: 0x000B7A98
		internal int Length
		{
			get
			{
				return this.len;
			}
			set
			{
				this.len = value;
			}
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x0600285A RID: 10330 RVA: 0x000B98A1 File Offset: 0x000B7AA1
		// (set) Token: 0x0600285B RID: 10331 RVA: 0x000B98A9 File Offset: 0x000B7AA9
		internal byte Symbol
		{
			get
			{
				return this.symbol;
			}
			set
			{
				this.symbol = value;
			}
		}

		// Token: 0x04002223 RID: 8739
		private MatchState state;

		// Token: 0x04002224 RID: 8740
		private int pos;

		// Token: 0x04002225 RID: 8741
		private int len;

		// Token: 0x04002226 RID: 8742
		private byte symbol;
	}
}
