using System;

namespace System.Net
{
	// Token: 0x020000C6 RID: 198
	public class Authorization
	{
		// Token: 0x0600069B RID: 1691 RVA: 0x00025281 File Offset: 0x00023481
		public Authorization(string token)
		{
			this.m_Message = ValidationHelper.MakeStringNull(token);
			this.m_Complete = true;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0002529C File Offset: 0x0002349C
		public Authorization(string token, bool finished)
		{
			this.m_Message = ValidationHelper.MakeStringNull(token);
			this.m_Complete = finished;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x000252B7 File Offset: 0x000234B7
		public Authorization(string token, bool finished, string connectionGroupId)
			: this(token, finished, connectionGroupId, false)
		{
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x000252C3 File Offset: 0x000234C3
		internal Authorization(string token, bool finished, string connectionGroupId, bool mutualAuth)
		{
			this.m_Message = ValidationHelper.MakeStringNull(token);
			this.m_ConnectionGroupId = ValidationHelper.MakeStringNull(connectionGroupId);
			this.m_Complete = finished;
			this.m_MutualAuth = mutualAuth;
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x000252F2 File Offset: 0x000234F2
		public string Message
		{
			get
			{
				return this.m_Message;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x000252FA File Offset: 0x000234FA
		public string ConnectionGroupId
		{
			get
			{
				return this.m_ConnectionGroupId;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x00025302 File Offset: 0x00023502
		public bool Complete
		{
			get
			{
				return this.m_Complete;
			}
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0002530A File Offset: 0x0002350A
		internal void SetComplete(bool complete)
		{
			this.m_Complete = complete;
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x00025313 File Offset: 0x00023513
		// (set) Token: 0x060006A4 RID: 1700 RVA: 0x0002531C File Offset: 0x0002351C
		public string[] ProtectionRealm
		{
			get
			{
				return this.m_ProtectionRealm;
			}
			set
			{
				string[] array = ValidationHelper.MakeEmptyArrayNull(value);
				this.m_ProtectionRealm = array;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x00025337 File Offset: 0x00023537
		// (set) Token: 0x060006A6 RID: 1702 RVA: 0x00025349 File Offset: 0x00023549
		public bool MutuallyAuthenticated
		{
			get
			{
				return this.Complete && this.m_MutualAuth;
			}
			set
			{
				this.m_MutualAuth = value;
			}
		}

		// Token: 0x04000C87 RID: 3207
		private string m_Message;

		// Token: 0x04000C88 RID: 3208
		private bool m_Complete;

		// Token: 0x04000C89 RID: 3209
		private string[] m_ProtectionRealm;

		// Token: 0x04000C8A RID: 3210
		private string m_ConnectionGroupId;

		// Token: 0x04000C8B RID: 3211
		private bool m_MutualAuth;
	}
}
