using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005D2 RID: 1490
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class ComponentRenameEventArgs : EventArgs
	{
		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x06003774 RID: 14196 RVA: 0x000F0378 File Offset: 0x000EE578
		public object Component
		{
			get
			{
				return this.component;
			}
		}

		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x06003775 RID: 14197 RVA: 0x000F0380 File Offset: 0x000EE580
		public virtual string OldName
		{
			get
			{
				return this.oldName;
			}
		}

		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x06003776 RID: 14198 RVA: 0x000F0388 File Offset: 0x000EE588
		public virtual string NewName
		{
			get
			{
				return this.newName;
			}
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x000F0390 File Offset: 0x000EE590
		public ComponentRenameEventArgs(object component, string oldName, string newName)
		{
			this.oldName = oldName;
			this.newName = newName;
			this.component = component;
		}

		// Token: 0x04002AEA RID: 10986
		private object component;

		// Token: 0x04002AEB RID: 10987
		private string oldName;

		// Token: 0x04002AEC RID: 10988
		private string newName;
	}
}
