using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020005A5 RID: 1445
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class RefreshEventArgs : EventArgs
	{
		// Token: 0x060035FA RID: 13818 RVA: 0x000EC185 File Offset: 0x000EA385
		public RefreshEventArgs(object componentChanged)
		{
			this.componentChanged = componentChanged;
			this.typeChanged = componentChanged.GetType();
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x000EC1A0 File Offset: 0x000EA3A0
		public RefreshEventArgs(Type typeChanged)
		{
			this.typeChanged = typeChanged;
		}

		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x060035FC RID: 13820 RVA: 0x000EC1AF File Offset: 0x000EA3AF
		public object ComponentChanged
		{
			get
			{
				return this.componentChanged;
			}
		}

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x060035FD RID: 13821 RVA: 0x000EC1B7 File Offset: 0x000EA3B7
		public Type TypeChanged
		{
			get
			{
				return this.typeChanged;
			}
		}

		// Token: 0x04002A85 RID: 10885
		private object componentChanged;

		// Token: 0x04002A86 RID: 10886
		private Type typeChanged;
	}
}
