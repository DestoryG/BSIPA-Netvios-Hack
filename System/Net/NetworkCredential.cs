using System;
using System.Security;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x02000155 RID: 341
	[global::__DynamicallyInvokable]
	public class NetworkCredential : ICredentials, ICredentialsByHost
	{
		// Token: 0x06000BF2 RID: 3058 RVA: 0x00040DFF File Offset: 0x0003EFFF
		[global::__DynamicallyInvokable]
		public NetworkCredential()
			: this(string.Empty, string.Empty, string.Empty)
		{
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00040E16 File Offset: 0x0003F016
		[global::__DynamicallyInvokable]
		public NetworkCredential(string userName, string password)
			: this(userName, password, string.Empty)
		{
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x00040E25 File Offset: 0x0003F025
		public NetworkCredential(string userName, SecureString password)
			: this(userName, password, string.Empty)
		{
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x00040E34 File Offset: 0x0003F034
		[global::__DynamicallyInvokable]
		public NetworkCredential(string userName, string password, string domain)
		{
			this.UserName = userName;
			this.Password = password;
			this.Domain = domain;
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x00040E51 File Offset: 0x0003F051
		public NetworkCredential(string userName, SecureString password, string domain)
		{
			this.UserName = userName;
			this.SecurePassword = password;
			this.Domain = domain;
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x00040E70 File Offset: 0x0003F070
		private void InitializePart1()
		{
			if (NetworkCredential.m_environmentUserNamePermission == null)
			{
				object obj = NetworkCredential.lockingObject;
				lock (obj)
				{
					if (NetworkCredential.m_environmentUserNamePermission == null)
					{
						NetworkCredential.m_environmentDomainNamePermission = new EnvironmentPermission(EnvironmentPermissionAccess.Read, "USERDOMAIN");
						NetworkCredential.m_environmentUserNamePermission = new EnvironmentPermission(EnvironmentPermissionAccess.Read, "USERNAME");
					}
				}
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x00040EE0 File Offset: 0x0003F0E0
		// (set) Token: 0x06000BF9 RID: 3065 RVA: 0x00040EFA File Offset: 0x0003F0FA
		[global::__DynamicallyInvokable]
		public string UserName
		{
			[global::__DynamicallyInvokable]
			get
			{
				this.InitializePart1();
				NetworkCredential.m_environmentUserNamePermission.Demand();
				return this.InternalGetUserName();
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (value == null)
				{
					this.m_userName = string.Empty;
					return;
				}
				this.m_userName = value;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x00040F12 File Offset: 0x0003F112
		// (set) Token: 0x06000BFB RID: 3067 RVA: 0x00040F24 File Offset: 0x0003F124
		[global::__DynamicallyInvokable]
		public string Password
		{
			[global::__DynamicallyInvokable]
			get
			{
				ExceptionHelper.UnmanagedPermission.Demand();
				return this.InternalGetPassword();
			}
			[global::__DynamicallyInvokable]
			set
			{
				this.m_password = UnsafeNclNativeMethods.SecureStringHelper.CreateSecureString(value);
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000BFC RID: 3068 RVA: 0x00040F32 File Offset: 0x0003F132
		// (set) Token: 0x06000BFD RID: 3069 RVA: 0x00040F49 File Offset: 0x0003F149
		public SecureString SecurePassword
		{
			get
			{
				ExceptionHelper.UnmanagedPermission.Demand();
				return this.InternalGetSecurePassword().Copy();
			}
			set
			{
				if (value == null)
				{
					this.m_password = new SecureString();
					return;
				}
				this.m_password = value.Copy();
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000BFE RID: 3070 RVA: 0x00040F66 File Offset: 0x0003F166
		// (set) Token: 0x06000BFF RID: 3071 RVA: 0x00040F80 File Offset: 0x0003F180
		[global::__DynamicallyInvokable]
		public string Domain
		{
			[global::__DynamicallyInvokable]
			get
			{
				this.InitializePart1();
				NetworkCredential.m_environmentDomainNamePermission.Demand();
				return this.InternalGetDomain();
			}
			[global::__DynamicallyInvokable]
			set
			{
				if (value == null)
				{
					this.m_domain = string.Empty;
					return;
				}
				this.m_domain = value;
			}
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x00040F98 File Offset: 0x0003F198
		internal string InternalGetUserName()
		{
			return this.m_userName;
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00040FA0 File Offset: 0x0003F1A0
		internal string InternalGetPassword()
		{
			return UnsafeNclNativeMethods.SecureStringHelper.CreateString(this.m_password);
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x00040FBA File Offset: 0x0003F1BA
		internal SecureString InternalGetSecurePassword()
		{
			return this.m_password;
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x00040FC2 File Offset: 0x0003F1C2
		internal string InternalGetDomain()
		{
			return this.m_domain;
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00040FCC File Offset: 0x0003F1CC
		internal string InternalGetDomainUserName()
		{
			string text = this.InternalGetDomain();
			if (text.Length != 0)
			{
				text += "\\";
			}
			return text + this.InternalGetUserName();
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x00041002 File Offset: 0x0003F202
		[global::__DynamicallyInvokable]
		public NetworkCredential GetCredential(Uri uri, string authType)
		{
			return this;
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x00041005 File Offset: 0x0003F205
		[global::__DynamicallyInvokable]
		public NetworkCredential GetCredential(string host, int port, string authenticationType)
		{
			return this;
		}

		// Token: 0x04001135 RID: 4405
		private static volatile EnvironmentPermission m_environmentUserNamePermission;

		// Token: 0x04001136 RID: 4406
		private static volatile EnvironmentPermission m_environmentDomainNamePermission;

		// Token: 0x04001137 RID: 4407
		private static readonly object lockingObject = new object();

		// Token: 0x04001138 RID: 4408
		private string m_domain;

		// Token: 0x04001139 RID: 4409
		private string m_userName;

		// Token: 0x0400113A RID: 4410
		private SecureString m_password;
	}
}
