using System;
using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000511 RID: 1297
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class AsyncCompletedEventArgs : EventArgs
	{
		// Token: 0x0600311D RID: 12573 RVA: 0x000DEA8F File Offset: 0x000DCC8F
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public AsyncCompletedEventArgs()
		{
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x000DEA97 File Offset: 0x000DCC97
		[global::__DynamicallyInvokable]
		public AsyncCompletedEventArgs(Exception error, bool cancelled, object userState)
		{
			this.error = error;
			this.cancelled = cancelled;
			this.userState = userState;
		}

		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x0600311F RID: 12575 RVA: 0x000DEAB4 File Offset: 0x000DCCB4
		[SRDescription("Async_AsyncEventArgs_Cancelled")]
		[global::__DynamicallyInvokable]
		public bool Cancelled
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.cancelled;
			}
		}

		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x06003120 RID: 12576 RVA: 0x000DEABC File Offset: 0x000DCCBC
		[SRDescription("Async_AsyncEventArgs_Error")]
		[global::__DynamicallyInvokable]
		public Exception Error
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.error;
			}
		}

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x06003121 RID: 12577 RVA: 0x000DEAC4 File Offset: 0x000DCCC4
		[SRDescription("Async_AsyncEventArgs_UserState")]
		[global::__DynamicallyInvokable]
		public object UserState
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.userState;
			}
		}

		// Token: 0x06003122 RID: 12578 RVA: 0x000DEACC File Offset: 0x000DCCCC
		[global::__DynamicallyInvokable]
		protected void RaiseExceptionIfNecessary()
		{
			if (this.Error != null)
			{
				throw new TargetInvocationException(SR.GetString("Async_ExceptionOccurred"), this.Error);
			}
			if (this.Cancelled)
			{
				throw new InvalidOperationException(SR.GetString("Async_OperationCancelled"));
			}
		}

		// Token: 0x040028FE RID: 10494
		private readonly Exception error;

		// Token: 0x040028FF RID: 10495
		private readonly bool cancelled;

		// Token: 0x04002900 RID: 10496
		private readonly object userState;
	}
}
