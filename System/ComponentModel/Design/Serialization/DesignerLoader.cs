using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000606 RID: 1542
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class DesignerLoader
	{
		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x0600389F RID: 14495 RVA: 0x000F1A36 File Offset: 0x000EFC36
		public virtual bool Loading
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060038A0 RID: 14496
		public abstract void BeginLoad(IDesignerLoaderHost host);

		// Token: 0x060038A1 RID: 14497
		public abstract void Dispose();

		// Token: 0x060038A2 RID: 14498 RVA: 0x000F1A39 File Offset: 0x000EFC39
		public virtual void Flush()
		{
		}
	}
}
