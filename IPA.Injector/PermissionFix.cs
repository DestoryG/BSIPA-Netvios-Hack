using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using IPA.Logging;

namespace IPA.Injector
{
	// Token: 0x02000006 RID: 6
	internal static class PermissionFix
	{
		// Token: 0x06000018 RID: 24 RVA: 0x00002BC8 File Offset: 0x00000DC8
		public static Task FixPermissions(DirectoryInfo root)
		{
			if (!root.Exists)
			{
				return new Task(delegate
				{
				});
			}
			return Task.Factory.StartNew(delegate
			{
				try
				{
					DirectorySecurity acl = root.GetAccessControl();
					AuthorizationRuleCollection rules = acl.GetAccessRules(true, true, typeof(SecurityIdentifier));
					FileSystemRights requestedRights = FileSystemRights.Modify;
					InheritanceFlags requestedInheritance = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
					PropagationFlags requestedPropagation = PropagationFlags.InheritOnly;
					bool hasRule = false;
					for (int i = 0; i < rules.Count; i++)
					{
						FileSystemAccessRule fsrule = rules[i] as FileSystemAccessRule;
						if (fsrule != null && fsrule.AccessControlType == AccessControlType.Allow && fsrule.InheritanceFlags.HasFlag(requestedInheritance) && fsrule.PropagationFlags == requestedPropagation && fsrule.FileSystemRights.HasFlag(requestedRights))
						{
							hasRule = true;
							break;
						}
					}
					if (!hasRule)
					{
						acl.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), requestedRights, requestedInheritance, requestedPropagation, AccessControlType.Allow));
						root.SetAccessControl(acl);
					}
				}
				catch (Exception e)
				{
					Logger.log.Warn("Error configuring permissions in the game install dir");
					Logger.log.Warn(e);
				}
			});
		}
	}
}
