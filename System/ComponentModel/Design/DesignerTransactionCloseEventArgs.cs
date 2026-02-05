using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005D6 RID: 1494
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class DesignerTransactionCloseEventArgs : EventArgs
	{
		// Token: 0x06003790 RID: 14224 RVA: 0x000F05EF File Offset: 0x000EE7EF
		[Obsolete("This constructor is obsolete. Use DesignerTransactionCloseEventArgs(bool, bool) instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public DesignerTransactionCloseEventArgs(bool commit)
			: this(commit, true)
		{
		}

		// Token: 0x06003791 RID: 14225 RVA: 0x000F05F9 File Offset: 0x000EE7F9
		public DesignerTransactionCloseEventArgs(bool commit, bool lastTransaction)
		{
			this.commit = commit;
			this.lastTransaction = lastTransaction;
		}

		// Token: 0x17000D5E RID: 3422
		// (get) Token: 0x06003792 RID: 14226 RVA: 0x000F060F File Offset: 0x000EE80F
		public bool TransactionCommitted
		{
			get
			{
				return this.commit;
			}
		}

		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x06003793 RID: 14227 RVA: 0x000F0617 File Offset: 0x000EE817
		public bool LastTransaction
		{
			get
			{
				return this.lastTransaction;
			}
		}

		// Token: 0x04002AF2 RID: 10994
		private bool commit;

		// Token: 0x04002AF3 RID: 10995
		private bool lastTransaction;
	}
}
