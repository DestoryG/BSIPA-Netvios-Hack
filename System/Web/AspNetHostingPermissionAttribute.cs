using System;
using System.Security;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x02000069 RID: 105
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class AspNetHostingPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06000462 RID: 1122 RVA: 0x0001EAE1 File Offset: 0x0001CCE1
		public AspNetHostingPermissionAttribute(SecurityAction action)
			: base(action)
		{
			this._level = AspNetHostingPermissionLevel.None;
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x0001EAF2 File Offset: 0x0001CCF2
		// (set) Token: 0x06000464 RID: 1124 RVA: 0x0001EAFA File Offset: 0x0001CCFA
		public AspNetHostingPermissionLevel Level
		{
			get
			{
				return this._level;
			}
			set
			{
				AspNetHostingPermission.VerifyAspNetHostingPermissionLevel(value, "Level");
				this._level = value;
			}
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0001EB0E File Offset: 0x0001CD0E
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new AspNetHostingPermission(PermissionState.Unrestricted);
			}
			return new AspNetHostingPermission(this._level);
		}

		// Token: 0x04000BCE RID: 3022
		private AspNetHostingPermissionLevel _level;
	}
}
