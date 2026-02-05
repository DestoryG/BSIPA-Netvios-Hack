using System;
using System.Globalization;

namespace System.Security.Permissions
{
	// Token: 0x02000488 RID: 1160
	[Serializable]
	public sealed class TypeDescriptorPermission : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x06002AF4 RID: 10996 RVA: 0x000C3600 File Offset: 0x000C1800
		public TypeDescriptorPermission(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				this.SetUnrestricted(true);
				return;
			}
			if (state == PermissionState.None)
			{
				this.SetUnrestricted(false);
				return;
			}
			throw new ArgumentException(SR.GetString("Argument_InvalidPermissionState"));
		}

		// Token: 0x06002AF5 RID: 10997 RVA: 0x000C362E File Offset: 0x000C182E
		public TypeDescriptorPermission(TypeDescriptorPermissionFlags flag)
		{
			this.VerifyAccess(flag);
			this.SetUnrestricted(false);
			this.m_flags = flag;
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x000C364B File Offset: 0x000C184B
		private void SetUnrestricted(bool unrestricted)
		{
			if (unrestricted)
			{
				this.m_flags = TypeDescriptorPermissionFlags.RestrictedRegistrationAccess;
				return;
			}
			this.Reset();
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x000C365E File Offset: 0x000C185E
		private void Reset()
		{
			this.m_flags = TypeDescriptorPermissionFlags.NoFlags;
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06002AF9 RID: 11001 RVA: 0x000C3677 File Offset: 0x000C1877
		// (set) Token: 0x06002AF8 RID: 11000 RVA: 0x000C3667 File Offset: 0x000C1867
		public TypeDescriptorPermissionFlags Flags
		{
			get
			{
				return this.m_flags;
			}
			set
			{
				this.VerifyAccess(value);
				this.m_flags = value;
			}
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x000C367F File Offset: 0x000C187F
		public bool IsUnrestricted()
		{
			return this.m_flags == TypeDescriptorPermissionFlags.RestrictedRegistrationAccess;
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x000C368C File Offset: 0x000C188C
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			IPermission permission;
			try
			{
				TypeDescriptorPermission typeDescriptorPermission = (TypeDescriptorPermission)target;
				TypeDescriptorPermissionFlags typeDescriptorPermissionFlags = this.m_flags | typeDescriptorPermission.m_flags;
				if (typeDescriptorPermissionFlags == TypeDescriptorPermissionFlags.NoFlags)
				{
					permission = null;
				}
				else
				{
					permission = new TypeDescriptorPermission(typeDescriptorPermissionFlags);
				}
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			return permission;
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x000C370C File Offset: 0x000C190C
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this.m_flags == TypeDescriptorPermissionFlags.NoFlags;
			}
			bool flag;
			try
			{
				TypeDescriptorPermission typeDescriptorPermission = (TypeDescriptorPermission)target;
				TypeDescriptorPermissionFlags flags = this.m_flags;
				TypeDescriptorPermissionFlags flags2 = typeDescriptorPermission.m_flags;
				flag = (flags & flags2) == flags;
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			return flag;
		}

		// Token: 0x06002AFD RID: 11005 RVA: 0x000C3788 File Offset: 0x000C1988
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			IPermission permission;
			try
			{
				TypeDescriptorPermission typeDescriptorPermission = (TypeDescriptorPermission)target;
				TypeDescriptorPermissionFlags typeDescriptorPermissionFlags = typeDescriptorPermission.m_flags & this.m_flags;
				if (typeDescriptorPermissionFlags == TypeDescriptorPermissionFlags.NoFlags)
				{
					permission = null;
				}
				else
				{
					permission = new TypeDescriptorPermission(typeDescriptorPermissionFlags);
				}
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			return permission;
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x000C3800 File Offset: 0x000C1A00
		public override IPermission Copy()
		{
			return new TypeDescriptorPermission(this.m_flags);
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x000C380D File Offset: 0x000C1A0D
		private void VerifyAccess(TypeDescriptorPermissionFlags type)
		{
			if ((type & ~TypeDescriptorPermissionFlags.RestrictedRegistrationAccess) != TypeDescriptorPermissionFlags.NoFlags)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { (int)type }));
			}
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x000C3840 File Offset: 0x000C1A40
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			securityElement.AddAttribute("class", base.GetType().FullName + ", " + base.GetType().Module.Assembly.FullName.Replace('"', '\''));
			securityElement.AddAttribute("version", "1");
			if (!this.IsUnrestricted())
			{
				securityElement.AddAttribute("Flags", this.m_flags.ToString());
			}
			else
			{
				securityElement.AddAttribute("Unrestricted", "true");
			}
			return securityElement;
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x000C38E0 File Offset: 0x000C1AE0
		public override void FromXml(SecurityElement securityElement)
		{
			if (securityElement == null)
			{
				throw new ArgumentNullException("securityElement");
			}
			string text = securityElement.Attribute("class");
			if (text == null || text.IndexOf(base.GetType().FullName, StringComparison.Ordinal) == -1)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidClassAttribute"), "securityElement");
			}
			string text2 = securityElement.Attribute("Unrestricted");
			if (text2 != null && string.Compare(text2, "true", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.m_flags = TypeDescriptorPermissionFlags.RestrictedRegistrationAccess;
				return;
			}
			this.m_flags = TypeDescriptorPermissionFlags.NoFlags;
			string text3 = securityElement.Attribute("Flags");
			if (text3 != null)
			{
				TypeDescriptorPermissionFlags typeDescriptorPermissionFlags = (TypeDescriptorPermissionFlags)Enum.Parse(typeof(TypeDescriptorPermissionFlags), text3);
				TypeDescriptorPermission.VerifyFlags(typeDescriptorPermissionFlags);
				this.m_flags = typeDescriptorPermissionFlags;
			}
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x000C3992 File Offset: 0x000C1B92
		internal static void VerifyFlags(TypeDescriptorPermissionFlags flags)
		{
			if ((flags & ~TypeDescriptorPermissionFlags.RestrictedRegistrationAccess) != TypeDescriptorPermissionFlags.NoFlags)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { (int)flags }));
			}
		}

		// Token: 0x04002660 RID: 9824
		private TypeDescriptorPermissionFlags m_flags;
	}
}
