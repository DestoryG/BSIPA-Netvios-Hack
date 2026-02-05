using System;
using System.Security.Permissions;

namespace Microsoft.Win32
{
	// Token: 0x02000012 RID: 18
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class PowerModeChangedEventArgs : EventArgs
	{
		// Token: 0x0600019C RID: 412 RVA: 0x0000D4A2 File Offset: 0x0000B6A2
		public PowerModeChangedEventArgs(PowerModes mode)
		{
			this.mode = mode;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600019D RID: 413 RVA: 0x0000D4B1 File Offset: 0x0000B6B1
		public PowerModes Mode
		{
			get
			{
				return this.mode;
			}
		}

		// Token: 0x040002F1 RID: 753
		private readonly PowerModes mode;
	}
}
