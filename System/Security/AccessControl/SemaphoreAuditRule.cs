using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	// Token: 0x0200048E RID: 1166
	[ComVisible(false)]
	public sealed class SemaphoreAuditRule : AuditRule
	{
		// Token: 0x06002B2F RID: 11055 RVA: 0x000C4886 File Offset: 0x000C2A86
		public SemaphoreAuditRule(IdentityReference identity, SemaphoreRights eventRights, AuditFlags flags)
			: this(identity, (int)eventRights, false, InheritanceFlags.None, PropagationFlags.None, flags)
		{
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x000C4894 File Offset: 0x000C2A94
		internal SemaphoreAuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
			: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags, flags)
		{
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x06002B31 RID: 11057 RVA: 0x000C48A5 File Offset: 0x000C2AA5
		public SemaphoreRights SemaphoreRights
		{
			get
			{
				return (SemaphoreRights)base.AccessMask;
			}
		}
	}
}
