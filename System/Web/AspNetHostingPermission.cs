using System;
using System.Security;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x0200006A RID: 106
	[Serializable]
	public sealed class AspNetHostingPermission : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x06000466 RID: 1126 RVA: 0x0001EB2A File Offset: 0x0001CD2A
		internal static void VerifyAspNetHostingPermissionLevel(AspNetHostingPermissionLevel level, string arg)
		{
			if (level <= AspNetHostingPermissionLevel.Low)
			{
				if (level == AspNetHostingPermissionLevel.None || level == AspNetHostingPermissionLevel.Minimal || level == AspNetHostingPermissionLevel.Low)
				{
					return;
				}
			}
			else if (level == AspNetHostingPermissionLevel.Medium || level == AspNetHostingPermissionLevel.High || level == AspNetHostingPermissionLevel.Unrestricted)
			{
				return;
			}
			throw new ArgumentException(arg);
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0001EB6C File Offset: 0x0001CD6C
		public AspNetHostingPermission(PermissionState state)
		{
			if (state == PermissionState.None)
			{
				this._level = AspNetHostingPermissionLevel.None;
				return;
			}
			if (state == PermissionState.Unrestricted)
			{
				this._level = AspNetHostingPermissionLevel.Unrestricted;
				return;
			}
			throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
			{
				state.ToString(),
				"state"
			}));
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0001EBC8 File Offset: 0x0001CDC8
		public AspNetHostingPermission(AspNetHostingPermissionLevel level)
		{
			AspNetHostingPermission.VerifyAspNetHostingPermissionLevel(level, "level");
			this._level = level;
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x0001EBE2 File Offset: 0x0001CDE2
		// (set) Token: 0x0600046A RID: 1130 RVA: 0x0001EBEA File Offset: 0x0001CDEA
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

		// Token: 0x0600046B RID: 1131 RVA: 0x0001EBFE File Offset: 0x0001CDFE
		public bool IsUnrestricted()
		{
			return this._level == AspNetHostingPermissionLevel.Unrestricted;
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0001EC0D File Offset: 0x0001CE0D
		public override IPermission Copy()
		{
			return new AspNetHostingPermission(this._level);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0001EC1C File Offset: 0x0001CE1C
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			if (target.GetType() != typeof(AspNetHostingPermission))
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					(target == null) ? "null" : target.ToString(),
					"target"
				}));
			}
			AspNetHostingPermission aspNetHostingPermission = (AspNetHostingPermission)target;
			if (this.Level >= aspNetHostingPermission.Level)
			{
				return new AspNetHostingPermission(this.Level);
			}
			return new AspNetHostingPermission(aspNetHostingPermission.Level);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0001ECA8 File Offset: 0x0001CEA8
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			if (target.GetType() != typeof(AspNetHostingPermission))
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					(target == null) ? "null" : target.ToString(),
					"target"
				}));
			}
			AspNetHostingPermission aspNetHostingPermission = (AspNetHostingPermission)target;
			if (this.Level <= aspNetHostingPermission.Level)
			{
				return new AspNetHostingPermission(this.Level);
			}
			return new AspNetHostingPermission(aspNetHostingPermission.Level);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0001ED30 File Offset: 0x0001CF30
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this._level == AspNetHostingPermissionLevel.None;
			}
			if (target.GetType() != typeof(AspNetHostingPermission))
			{
				throw new ArgumentException(SR.GetString("InvalidArgument", new object[]
				{
					(target == null) ? "null" : target.ToString(),
					"target"
				}));
			}
			AspNetHostingPermission aspNetHostingPermission = (AspNetHostingPermission)target;
			return this.Level <= aspNetHostingPermission.Level;
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0001EDAC File Offset: 0x0001CFAC
		public override void FromXml(SecurityElement securityElement)
		{
			if (securityElement == null)
			{
				throw new ArgumentNullException(SR.GetString("AspNetHostingPermissionBadXml", new object[] { "securityElement" }));
			}
			if (!securityElement.Tag.Equals("IPermission"))
			{
				throw new ArgumentException(SR.GetString("AspNetHostingPermissionBadXml", new object[] { "securityElement" }));
			}
			string text = securityElement.Attribute("class");
			if (text == null)
			{
				throw new ArgumentException(SR.GetString("AspNetHostingPermissionBadXml", new object[] { "securityElement" }));
			}
			if (text.IndexOf(base.GetType().FullName, StringComparison.Ordinal) < 0)
			{
				throw new ArgumentException(SR.GetString("AspNetHostingPermissionBadXml", new object[] { "securityElement" }));
			}
			string text2 = securityElement.Attribute("version");
			if (string.Compare(text2, "1", StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new ArgumentException(SR.GetString("AspNetHostingPermissionBadXml", new object[] { "version" }));
			}
			string text3 = securityElement.Attribute("Level");
			if (text3 == null)
			{
				this._level = AspNetHostingPermissionLevel.None;
				return;
			}
			this._level = (AspNetHostingPermissionLevel)Enum.Parse(typeof(AspNetHostingPermissionLevel), text3);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0001EED8 File Offset: 0x0001D0D8
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", base.GetType().FullName + ", " + base.GetType().Module.Assembly.FullName.Replace('"', '\''));
			securityElement.AddAttribute("version", "1");
			securityElement.AddAttribute("Level", Enum.GetName(typeof(AspNetHostingPermissionLevel), this._level));
			if (this.IsUnrestricted())
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			return securityElement;
		}

		// Token: 0x04000BCF RID: 3023
		private AspNetHostingPermissionLevel _level;
	}
}
