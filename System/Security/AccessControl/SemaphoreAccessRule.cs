using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x0200048D RID: 1165
	[ComVisible(false)]
	public sealed class SemaphoreAccessRule : AccessRule
	{
		// Token: 0x06002B2B RID: 11051 RVA: 0x000C484C File Offset: 0x000C2A4C
		public SemaphoreAccessRule(IdentityReference identity, SemaphoreRights eventRights, AccessControlType type)
			: this(identity, (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x000C485A File Offset: 0x000C2A5A
		public SemaphoreAccessRule(string identity, SemaphoreRights eventRights, AccessControlType type)
			: this(new NTAccount(identity), (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, type)
		{
		}

		// Token: 0x06002B2D RID: 11053 RVA: 0x000C486D File Offset: 0x000C2A6D
		internal SemaphoreAccessRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
			: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, type)
		{
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06002B2E RID: 11054 RVA: 0x000C487E File Offset: 0x000C2A7E
		public SemaphoreRights SemaphoreRights
		{
			get
			{
				return (SemaphoreRights)base.AccessMask;
			}
		}
	}
}
