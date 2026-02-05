using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000596 RID: 1430
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class ProgressChangedEventArgs : EventArgs
	{
		// Token: 0x0600351A RID: 13594 RVA: 0x000E785C File Offset: 0x000E5A5C
		[global::__DynamicallyInvokable]
		public ProgressChangedEventArgs(int progressPercentage, object userState)
		{
			this.progressPercentage = progressPercentage;
			this.userState = userState;
		}

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x0600351B RID: 13595 RVA: 0x000E7872 File Offset: 0x000E5A72
		[SRDescription("Async_ProgressChangedEventArgs_ProgressPercentage")]
		[global::__DynamicallyInvokable]
		public int ProgressPercentage
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.progressPercentage;
			}
		}

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x0600351C RID: 13596 RVA: 0x000E787A File Offset: 0x000E5A7A
		[SRDescription("Async_ProgressChangedEventArgs_UserState")]
		[global::__DynamicallyInvokable]
		public object UserState
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.userState;
			}
		}

		// Token: 0x04002A31 RID: 10801
		private readonly int progressPercentage;

		// Token: 0x04002A32 RID: 10802
		private readonly object userState;
	}
}
