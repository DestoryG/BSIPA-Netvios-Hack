using System;
using System.Security.Permissions;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000612 RID: 1554
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class ResolveNameEventArgs : EventArgs
	{
		// Token: 0x060038DF RID: 14559 RVA: 0x000F203E File Offset: 0x000F023E
		public ResolveNameEventArgs(string name)
		{
			this.name = name;
			this.value = null;
		}

		// Token: 0x17000D9A RID: 3482
		// (get) Token: 0x060038E0 RID: 14560 RVA: 0x000F2054 File Offset: 0x000F0254
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x060038E1 RID: 14561 RVA: 0x000F205C File Offset: 0x000F025C
		// (set) Token: 0x060038E2 RID: 14562 RVA: 0x000F2064 File Offset: 0x000F0264
		public object Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x04002B74 RID: 11124
		private string name;

		// Token: 0x04002B75 RID: 11125
		private object value;
	}
}
