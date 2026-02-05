using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace System.Security.AccessControl
{
	// Token: 0x0200048F RID: 1167
	[ComVisible(false)]
	public sealed class SemaphoreSecurity : NativeObjectSecurity
	{
		// Token: 0x06002B32 RID: 11058 RVA: 0x000C48AD File Offset: 0x000C2AAD
		public SemaphoreSecurity()
			: base(true, ResourceType.KernelObject)
		{
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x000C48B7 File Offset: 0x000C2AB7
		public SemaphoreSecurity(string name, AccessControlSections includeSections)
			: base(true, ResourceType.KernelObject, name, includeSections, new NativeObjectSecurity.ExceptionFromErrorCode(SemaphoreSecurity._HandleErrorCode), null)
		{
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x000C48D0 File Offset: 0x000C2AD0
		internal SemaphoreSecurity(SafeWaitHandle handle, AccessControlSections includeSections)
			: base(true, ResourceType.KernelObject, handle, includeSections, new NativeObjectSecurity.ExceptionFromErrorCode(SemaphoreSecurity._HandleErrorCode), null)
		{
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x000C48EC File Offset: 0x000C2AEC
		private static Exception _HandleErrorCode(int errorCode, string name, SafeHandle handle, object context)
		{
			Exception ex = null;
			if (errorCode == 2 || errorCode == 6 || errorCode == 123)
			{
				if (name != null && name.Length != 0)
				{
					ex = new WaitHandleCannotBeOpenedException(SR.GetString("WaitHandleCannotBeOpenedException_InvalidHandle", new object[] { name }));
				}
				else
				{
					ex = new WaitHandleCannotBeOpenedException();
				}
			}
			return ex;
		}

		// Token: 0x06002B36 RID: 11062 RVA: 0x000C4936 File Offset: 0x000C2B36
		public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		{
			return new SemaphoreAccessRule(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, type);
		}

		// Token: 0x06002B37 RID: 11063 RVA: 0x000C4946 File Offset: 0x000C2B46
		public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		{
			return new SemaphoreAuditRule(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, flags);
		}

		// Token: 0x06002B38 RID: 11064 RVA: 0x000C4958 File Offset: 0x000C2B58
		internal AccessControlSections GetAccessControlSectionsFromChanges()
		{
			AccessControlSections accessControlSections = AccessControlSections.None;
			if (base.AccessRulesModified)
			{
				accessControlSections = AccessControlSections.Access;
			}
			if (base.AuditRulesModified)
			{
				accessControlSections |= AccessControlSections.Audit;
			}
			if (base.OwnerModified)
			{
				accessControlSections |= AccessControlSections.Owner;
			}
			if (base.GroupModified)
			{
				accessControlSections |= AccessControlSections.Group;
			}
			return accessControlSections;
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x000C4998 File Offset: 0x000C2B98
		internal void Persist(SafeWaitHandle handle)
		{
			base.WriteLock();
			try
			{
				AccessControlSections accessControlSectionsFromChanges = this.GetAccessControlSectionsFromChanges();
				if (accessControlSectionsFromChanges != AccessControlSections.None)
				{
					base.Persist(handle, accessControlSectionsFromChanges);
					base.OwnerModified = (base.GroupModified = (base.AuditRulesModified = (base.AccessRulesModified = false)));
				}
			}
			finally
			{
				base.WriteUnlock();
			}
		}

		// Token: 0x06002B3A RID: 11066 RVA: 0x000C49FC File Offset: 0x000C2BFC
		public void AddAccessRule(SemaphoreAccessRule rule)
		{
			base.AddAccessRule(rule);
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x000C4A05 File Offset: 0x000C2C05
		public void SetAccessRule(SemaphoreAccessRule rule)
		{
			base.SetAccessRule(rule);
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x000C4A0E File Offset: 0x000C2C0E
		public void ResetAccessRule(SemaphoreAccessRule rule)
		{
			base.ResetAccessRule(rule);
		}

		// Token: 0x06002B3D RID: 11069 RVA: 0x000C4A17 File Offset: 0x000C2C17
		public bool RemoveAccessRule(SemaphoreAccessRule rule)
		{
			return base.RemoveAccessRule(rule);
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x000C4A20 File Offset: 0x000C2C20
		public void RemoveAccessRuleAll(SemaphoreAccessRule rule)
		{
			base.RemoveAccessRuleAll(rule);
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x000C4A29 File Offset: 0x000C2C29
		public void RemoveAccessRuleSpecific(SemaphoreAccessRule rule)
		{
			base.RemoveAccessRuleSpecific(rule);
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x000C4A32 File Offset: 0x000C2C32
		public void AddAuditRule(SemaphoreAuditRule rule)
		{
			base.AddAuditRule(rule);
		}

		// Token: 0x06002B41 RID: 11073 RVA: 0x000C4A3B File Offset: 0x000C2C3B
		public void SetAuditRule(SemaphoreAuditRule rule)
		{
			base.SetAuditRule(rule);
		}

		// Token: 0x06002B42 RID: 11074 RVA: 0x000C4A44 File Offset: 0x000C2C44
		public bool RemoveAuditRule(SemaphoreAuditRule rule)
		{
			return base.RemoveAuditRule(rule);
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x000C4A4D File Offset: 0x000C2C4D
		public void RemoveAuditRuleAll(SemaphoreAuditRule rule)
		{
			base.RemoveAuditRuleAll(rule);
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x000C4A56 File Offset: 0x000C2C56
		public void RemoveAuditRuleSpecific(SemaphoreAuditRule rule)
		{
			base.RemoveAuditRuleSpecific(rule);
		}

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06002B45 RID: 11077 RVA: 0x000C4A5F File Offset: 0x000C2C5F
		public override Type AccessRightType
		{
			get
			{
				return typeof(SemaphoreRights);
			}
		}

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x06002B46 RID: 11078 RVA: 0x000C4A6B File Offset: 0x000C2C6B
		public override Type AccessRuleType
		{
			get
			{
				return typeof(SemaphoreAccessRule);
			}
		}

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x06002B47 RID: 11079 RVA: 0x000C4A77 File Offset: 0x000C2C77
		public override Type AuditRuleType
		{
			get
			{
				return typeof(SemaphoreAuditRule);
			}
		}
	}
}
