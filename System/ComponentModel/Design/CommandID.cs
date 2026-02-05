using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005CB RID: 1483
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class CommandID
	{
		// Token: 0x06003758 RID: 14168 RVA: 0x000F0220 File Offset: 0x000EE420
		public CommandID(Guid menuGroup, int commandID)
		{
			this.menuGroup = menuGroup;
			this.commandID = commandID;
		}

		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x06003759 RID: 14169 RVA: 0x000F0236 File Offset: 0x000EE436
		public virtual int ID
		{
			get
			{
				return this.commandID;
			}
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x000F0240 File Offset: 0x000EE440
		public override bool Equals(object obj)
		{
			if (!(obj is CommandID))
			{
				return false;
			}
			CommandID commandID = (CommandID)obj;
			return commandID.menuGroup.Equals(this.menuGroup) && commandID.commandID == this.commandID;
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x000F0284 File Offset: 0x000EE484
		public override int GetHashCode()
		{
			return (this.menuGroup.GetHashCode() << 2) | this.commandID;
		}

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x0600375C RID: 14172 RVA: 0x000F02AE File Offset: 0x000EE4AE
		public virtual Guid Guid
		{
			get
			{
				return this.menuGroup;
			}
		}

		// Token: 0x0600375D RID: 14173 RVA: 0x000F02B8 File Offset: 0x000EE4B8
		public override string ToString()
		{
			return this.menuGroup.ToString() + " : " + this.commandID.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x04002AE1 RID: 10977
		private readonly Guid menuGroup;

		// Token: 0x04002AE2 RID: 10978
		private readonly int commandID;
	}
}
