using System;
using System.Security.Permissions;
using System.Threading;

namespace System.ComponentModel
{
	// Token: 0x02000513 RID: 1299
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public sealed class AsyncOperation
	{
		// Token: 0x06003127 RID: 12583 RVA: 0x000DEB04 File Offset: 0x000DCD04
		private AsyncOperation(object userSuppliedState, SynchronizationContext syncContext)
		{
			this.userSuppliedState = userSuppliedState;
			this.syncContext = syncContext;
			this.alreadyCompleted = false;
			this.syncContext.OperationStarted();
		}

		// Token: 0x06003128 RID: 12584 RVA: 0x000DEB2C File Offset: 0x000DCD2C
		~AsyncOperation()
		{
			if (!this.alreadyCompleted && this.syncContext != null)
			{
				this.syncContext.OperationCompleted();
			}
		}

		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x06003129 RID: 12585 RVA: 0x000DEB70 File Offset: 0x000DCD70
		[global::__DynamicallyInvokable]
		public object UserSuppliedState
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.userSuppliedState;
			}
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x0600312A RID: 12586 RVA: 0x000DEB78 File Offset: 0x000DCD78
		[global::__DynamicallyInvokable]
		public SynchronizationContext SynchronizationContext
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this.syncContext;
			}
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x000DEB80 File Offset: 0x000DCD80
		[global::__DynamicallyInvokable]
		public void Post(SendOrPostCallback d, object arg)
		{
			this.VerifyNotCompleted();
			this.VerifyDelegateNotNull(d);
			this.syncContext.Post(d, arg);
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x000DEB9C File Offset: 0x000DCD9C
		[global::__DynamicallyInvokable]
		public void PostOperationCompleted(SendOrPostCallback d, object arg)
		{
			this.Post(d, arg);
			this.OperationCompletedCore();
		}

		// Token: 0x0600312D RID: 12589 RVA: 0x000DEBAC File Offset: 0x000DCDAC
		[global::__DynamicallyInvokable]
		public void OperationCompleted()
		{
			this.VerifyNotCompleted();
			this.OperationCompletedCore();
		}

		// Token: 0x0600312E RID: 12590 RVA: 0x000DEBBC File Offset: 0x000DCDBC
		private void OperationCompletedCore()
		{
			try
			{
				this.syncContext.OperationCompleted();
			}
			finally
			{
				this.alreadyCompleted = true;
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x0600312F RID: 12591 RVA: 0x000DEBF4 File Offset: 0x000DCDF4
		private void VerifyNotCompleted()
		{
			if (this.alreadyCompleted)
			{
				throw new InvalidOperationException(SR.GetString("Async_OperationAlreadyCompleted"));
			}
		}

		// Token: 0x06003130 RID: 12592 RVA: 0x000DEC0E File Offset: 0x000DCE0E
		private void VerifyDelegateNotNull(SendOrPostCallback d)
		{
			if (d == null)
			{
				throw new ArgumentNullException(SR.GetString("Async_NullDelegate"), "d");
			}
		}

		// Token: 0x06003131 RID: 12593 RVA: 0x000DEC28 File Offset: 0x000DCE28
		internal static AsyncOperation CreateOperation(object userSuppliedState, SynchronizationContext syncContext)
		{
			return new AsyncOperation(userSuppliedState, syncContext);
		}

		// Token: 0x04002901 RID: 10497
		private SynchronizationContext syncContext;

		// Token: 0x04002902 RID: 10498
		private object userSuppliedState;

		// Token: 0x04002903 RID: 10499
		private bool alreadyCompleted;
	}
}
