using System;

namespace System.Net
{
	// Token: 0x0200020D RID: 525
	internal class SpnToken
	{
		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001397 RID: 5015 RVA: 0x00066FB8 File Offset: 0x000651B8
		// (set) Token: 0x06001398 RID: 5016 RVA: 0x00066FC0 File Offset: 0x000651C0
		internal bool IsTrusted
		{
			get
			{
				return this.isTrusted;
			}
			set
			{
				this.isTrusted = false;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06001399 RID: 5017 RVA: 0x00066FC9 File Offset: 0x000651C9
		internal string Spn
		{
			get
			{
				return this.spn;
			}
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x00066FD1 File Offset: 0x000651D1
		internal SpnToken(string spn)
			: this(spn, true)
		{
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x00066FDB File Offset: 0x000651DB
		internal SpnToken(string spn, bool trusted)
		{
			this.spn = spn;
			this.isTrusted = trusted;
		}

		// Token: 0x04001562 RID: 5474
		private readonly string spn;

		// Token: 0x04001563 RID: 5475
		private bool isTrusted;
	}
}
