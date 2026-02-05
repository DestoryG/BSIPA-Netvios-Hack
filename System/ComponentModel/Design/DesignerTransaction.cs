using System;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005D5 RID: 1493
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class DesignerTransaction : IDisposable
	{
		// Token: 0x06003784 RID: 14212 RVA: 0x000F0511 File Offset: 0x000EE711
		protected DesignerTransaction()
			: this("")
		{
		}

		// Token: 0x06003785 RID: 14213 RVA: 0x000F051E File Offset: 0x000EE71E
		protected DesignerTransaction(string description)
		{
			this.desc = description;
		}

		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x06003786 RID: 14214 RVA: 0x000F052D File Offset: 0x000EE72D
		public bool Canceled
		{
			get
			{
				return this.canceled;
			}
		}

		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x06003787 RID: 14215 RVA: 0x000F0535 File Offset: 0x000EE735
		public bool Committed
		{
			get
			{
				return this.committed;
			}
		}

		// Token: 0x17000D5D RID: 3421
		// (get) Token: 0x06003788 RID: 14216 RVA: 0x000F053D File Offset: 0x000EE73D
		public string Description
		{
			get
			{
				return this.desc;
			}
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x000F0545 File Offset: 0x000EE745
		public void Cancel()
		{
			if (!this.canceled && !this.committed)
			{
				this.canceled = true;
				GC.SuppressFinalize(this);
				this.suppressedFinalization = true;
				this.OnCancel();
			}
		}

		// Token: 0x0600378A RID: 14218 RVA: 0x000F0571 File Offset: 0x000EE771
		public void Commit()
		{
			if (!this.committed && !this.canceled)
			{
				this.committed = true;
				GC.SuppressFinalize(this);
				this.suppressedFinalization = true;
				this.OnCommit();
			}
		}

		// Token: 0x0600378B RID: 14219
		protected abstract void OnCancel();

		// Token: 0x0600378C RID: 14220
		protected abstract void OnCommit();

		// Token: 0x0600378D RID: 14221 RVA: 0x000F05A0 File Offset: 0x000EE7A0
		~DesignerTransaction()
		{
			this.Dispose(false);
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x000F05D0 File Offset: 0x000EE7D0
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			if (!this.suppressedFinalization)
			{
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x0600378F RID: 14223 RVA: 0x000F05E7 File Offset: 0x000EE7E7
		protected virtual void Dispose(bool disposing)
		{
			this.Cancel();
		}

		// Token: 0x04002AEE RID: 10990
		private bool committed;

		// Token: 0x04002AEF RID: 10991
		private bool canceled;

		// Token: 0x04002AF0 RID: 10992
		private bool suppressedFinalization;

		// Token: 0x04002AF1 RID: 10993
		private string desc;
	}
}
