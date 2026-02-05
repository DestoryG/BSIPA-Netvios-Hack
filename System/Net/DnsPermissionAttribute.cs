using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x020000E1 RID: 225
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class DnsPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060007BE RID: 1982 RVA: 0x0002B22F File Offset: 0x0002942F
		public DnsPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0002B238 File Offset: 0x00029438
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new DnsPermission(PermissionState.Unrestricted);
			}
			return new DnsPermission(PermissionState.None);
		}
	}
}
