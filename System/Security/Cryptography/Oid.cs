using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography
{
	// Token: 0x0200045E RID: 1118
	public sealed class Oid
	{
		// Token: 0x0600297F RID: 10623 RVA: 0x000BC5F8 File Offset: 0x000BA7F8
		public Oid()
		{
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x000BC600 File Offset: 0x000BA800
		public Oid(string oid)
			: this(oid, OidGroup.All, true)
		{
		}

		// Token: 0x06002981 RID: 10625 RVA: 0x000BC60C File Offset: 0x000BA80C
		internal Oid(string oid, OidGroup group, bool lookupFriendlyName)
		{
			if (lookupFriendlyName)
			{
				string text = global::System.Security.Cryptography.X509Certificates.X509Utils.FindOidInfoWithFallback(2U, oid, group);
				if (text == null)
				{
					text = oid;
				}
				this.Value = text;
			}
			else
			{
				this.Value = oid;
			}
			this.m_group = group;
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x000BC647 File Offset: 0x000BA847
		public Oid(string value, string friendlyName)
		{
			this.m_value = value;
			this.m_friendlyName = friendlyName;
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x000BC65D File Offset: 0x000BA85D
		public Oid(Oid oid)
		{
			if (oid == null)
			{
				throw new ArgumentNullException("oid");
			}
			this.m_value = oid.m_value;
			this.m_friendlyName = oid.m_friendlyName;
			this.m_group = oid.m_group;
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x000BC697 File Offset: 0x000BA897
		private Oid(string value, string friendlyName, OidGroup group)
		{
			this.m_value = value;
			this.m_friendlyName = friendlyName;
			this.m_group = group;
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x000BC6B4 File Offset: 0x000BA8B4
		public static Oid FromFriendlyName(string friendlyName, OidGroup group)
		{
			if (friendlyName == null)
			{
				throw new ArgumentNullException("friendlyName");
			}
			string text = global::System.Security.Cryptography.X509Certificates.X509Utils.FindOidInfo(2U, friendlyName, group);
			if (text == null)
			{
				throw new CryptographicException(global::System.SR.GetString("Cryptography_Oid_InvalidValue"));
			}
			return new Oid(text, friendlyName, group);
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x000BC6F4 File Offset: 0x000BA8F4
		public static Oid FromOidValue(string oidValue, OidGroup group)
		{
			if (oidValue == null)
			{
				throw new ArgumentNullException("oidValue");
			}
			string text = global::System.Security.Cryptography.X509Certificates.X509Utils.FindOidInfo(1U, oidValue, group);
			if (text == null)
			{
				throw new CryptographicException(global::System.SR.GetString("Cryptography_Oid_InvalidValue"));
			}
			return new Oid(oidValue, text, group);
		}

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x06002987 RID: 10631 RVA: 0x000BC733 File Offset: 0x000BA933
		// (set) Token: 0x06002988 RID: 10632 RVA: 0x000BC73B File Offset: 0x000BA93B
		public string Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				this.m_value = value;
			}
		}

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x06002989 RID: 10633 RVA: 0x000BC744 File Offset: 0x000BA944
		// (set) Token: 0x0600298A RID: 10634 RVA: 0x000BC774 File Offset: 0x000BA974
		public string FriendlyName
		{
			get
			{
				if (this.m_friendlyName == null && this.m_value != null)
				{
					this.m_friendlyName = global::System.Security.Cryptography.X509Certificates.X509Utils.FindOidInfoWithFallback(1U, this.m_value, this.m_group);
				}
				return this.m_friendlyName;
			}
			set
			{
				this.m_friendlyName = value;
				if (this.m_friendlyName != null)
				{
					string text = global::System.Security.Cryptography.X509Certificates.X509Utils.FindOidInfoWithFallback(2U, this.m_friendlyName, this.m_group);
					if (text != null)
					{
						this.m_value = text;
					}
				}
			}
		}

		// Token: 0x0400258F RID: 9615
		private string m_value;

		// Token: 0x04002590 RID: 9616
		private string m_friendlyName;

		// Token: 0x04002591 RID: 9617
		private OidGroup m_group;
	}
}
