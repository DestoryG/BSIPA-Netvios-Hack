using System;
using System.Globalization;

namespace System.Security.Permissions
{
	// Token: 0x02000484 RID: 1156
	[Serializable]
	public sealed class StorePermission : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x06002AD6 RID: 10966 RVA: 0x000C3108 File Offset: 0x000C1308
		public StorePermission(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				this.m_flags = StorePermissionFlags.AllFlags;
				return;
			}
			if (state == PermissionState.None)
			{
				this.m_flags = StorePermissionFlags.NoFlags;
				return;
			}
			throw new ArgumentException(SR.GetString("Argument_InvalidPermissionState"));
		}

		// Token: 0x06002AD7 RID: 10967 RVA: 0x000C313A File Offset: 0x000C133A
		public StorePermission(StorePermissionFlags flag)
		{
			StorePermission.VerifyFlags(flag);
			this.m_flags = flag;
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x06002AD9 RID: 10969 RVA: 0x000C315E File Offset: 0x000C135E
		// (set) Token: 0x06002AD8 RID: 10968 RVA: 0x000C314F File Offset: 0x000C134F
		public StorePermissionFlags Flags
		{
			get
			{
				return this.m_flags;
			}
			set
			{
				StorePermission.VerifyFlags(value);
				this.m_flags = value;
			}
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x000C3166 File Offset: 0x000C1366
		public bool IsUnrestricted()
		{
			return this.m_flags == StorePermissionFlags.AllFlags;
		}

		// Token: 0x06002ADB RID: 10971 RVA: 0x000C3178 File Offset: 0x000C1378
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			IPermission permission;
			try
			{
				StorePermission storePermission = (StorePermission)target;
				StorePermissionFlags storePermissionFlags = this.m_flags | storePermission.m_flags;
				if (storePermissionFlags == StorePermissionFlags.NoFlags)
				{
					permission = null;
				}
				else
				{
					permission = new StorePermission(storePermissionFlags);
				}
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			return permission;
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x000C31F8 File Offset: 0x000C13F8
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this.m_flags == StorePermissionFlags.NoFlags;
			}
			bool flag;
			try
			{
				StorePermission storePermission = (StorePermission)target;
				StorePermissionFlags flags = this.m_flags;
				StorePermissionFlags flags2 = storePermission.m_flags;
				flag = (flags & flags2) == flags;
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			return flag;
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x000C3274 File Offset: 0x000C1474
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			IPermission permission;
			try
			{
				StorePermission storePermission = (StorePermission)target;
				StorePermissionFlags storePermissionFlags = storePermission.m_flags & this.m_flags;
				if (storePermissionFlags == StorePermissionFlags.NoFlags)
				{
					permission = null;
				}
				else
				{
					permission = new StorePermission(storePermissionFlags);
				}
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Argument_WrongType"), new object[] { base.GetType().FullName }));
			}
			return permission;
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x000C32EC File Offset: 0x000C14EC
		public override IPermission Copy()
		{
			return new StorePermission(this.m_flags);
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x000C32FC File Offset: 0x000C14FC
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

		// Token: 0x06002AE0 RID: 10976 RVA: 0x000C339C File Offset: 0x000C159C
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
				this.m_flags = StorePermissionFlags.AllFlags;
				return;
			}
			this.m_flags = StorePermissionFlags.NoFlags;
			string text3 = securityElement.Attribute("Flags");
			if (text3 != null)
			{
				StorePermissionFlags storePermissionFlags = (StorePermissionFlags)Enum.Parse(typeof(StorePermissionFlags), text3);
				StorePermission.VerifyFlags(storePermissionFlags);
				this.m_flags = storePermissionFlags;
			}
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x000C3452 File Offset: 0x000C1652
		internal static void VerifyFlags(StorePermissionFlags flags)
		{
			if ((flags & ~(StorePermissionFlags.CreateStore | StorePermissionFlags.DeleteStore | StorePermissionFlags.EnumerateStores | StorePermissionFlags.OpenStore | StorePermissionFlags.AddToStore | StorePermissionFlags.RemoveFromStore | StorePermissionFlags.EnumerateCertificates)) != StorePermissionFlags.NoFlags)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("Arg_EnumIllegalVal"), new object[] { (int)flags }));
			}
		}

		// Token: 0x04002651 RID: 9809
		private StorePermissionFlags m_flags;
	}
}
