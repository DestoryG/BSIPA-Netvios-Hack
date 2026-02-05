using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005CC RID: 1484
	[ComVisible(true)]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class ComponentChangedEventArgs : EventArgs
	{
		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x0600375E RID: 14174 RVA: 0x000F02F6 File Offset: 0x000EE4F6
		public object Component
		{
			get
			{
				return this.component;
			}
		}

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x0600375F RID: 14175 RVA: 0x000F02FE File Offset: 0x000EE4FE
		public MemberDescriptor Member
		{
			get
			{
				return this.member;
			}
		}

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x06003760 RID: 14176 RVA: 0x000F0306 File Offset: 0x000EE506
		public object NewValue
		{
			get
			{
				return this.newValue;
			}
		}

		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x06003761 RID: 14177 RVA: 0x000F030E File Offset: 0x000EE50E
		public object OldValue
		{
			get
			{
				return this.oldValue;
			}
		}

		// Token: 0x06003762 RID: 14178 RVA: 0x000F0316 File Offset: 0x000EE516
		public ComponentChangedEventArgs(object component, MemberDescriptor member, object oldValue, object newValue)
		{
			this.component = component;
			this.member = member;
			this.oldValue = oldValue;
			this.newValue = newValue;
		}

		// Token: 0x04002AE3 RID: 10979
		private object component;

		// Token: 0x04002AE4 RID: 10980
		private MemberDescriptor member;

		// Token: 0x04002AE5 RID: 10981
		private object oldValue;

		// Token: 0x04002AE6 RID: 10982
		private object newValue;
	}
}
