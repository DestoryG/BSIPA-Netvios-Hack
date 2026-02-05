using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020005A8 RID: 1448
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class RunWorkerCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06003608 RID: 13832 RVA: 0x000EC23C File Offset: 0x000EA43C
		[global::__DynamicallyInvokable]
		public RunWorkerCompletedEventArgs(object result, Exception error, bool cancelled)
			: base(error, cancelled, null)
		{
			this.result = result;
		}

		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x06003609 RID: 13833 RVA: 0x000EC24E File Offset: 0x000EA44E
		[global::__DynamicallyInvokable]
		public object Result
		{
			[global::__DynamicallyInvokable]
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.result;
			}
		}

		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x0600360A RID: 13834 RVA: 0x000EC25C File Offset: 0x000EA45C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[global::__DynamicallyInvokable]
		public new object UserState
		{
			[global::__DynamicallyInvokable]
			get
			{
				return base.UserState;
			}
		}

		// Token: 0x04002A8B RID: 10891
		private object result;
	}
}
