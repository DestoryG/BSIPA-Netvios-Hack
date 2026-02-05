using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005CE RID: 1486
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class ComponentChangingEventArgs : EventArgs
	{
		// Token: 0x17000D54 RID: 3412
		// (get) Token: 0x06003767 RID: 14183 RVA: 0x000F033B File Offset: 0x000EE53B
		public object Component
		{
			get
			{
				return this.component;
			}
		}

		// Token: 0x17000D55 RID: 3413
		// (get) Token: 0x06003768 RID: 14184 RVA: 0x000F0343 File Offset: 0x000EE543
		public MemberDescriptor Member
		{
			get
			{
				return this.member;
			}
		}

		// Token: 0x06003769 RID: 14185 RVA: 0x000F034B File Offset: 0x000EE54B
		public ComponentChangingEventArgs(object component, MemberDescriptor member)
		{
			this.component = component;
			this.member = member;
		}

		// Token: 0x04002AE7 RID: 10983
		private object component;

		// Token: 0x04002AE8 RID: 10984
		private MemberDescriptor member;
	}
}
