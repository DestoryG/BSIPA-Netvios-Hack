using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020001D0 RID: 464
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal struct AuthIdentity
	{
		// Token: 0x0600126A RID: 4714 RVA: 0x00062648 File Offset: 0x00060848
		internal AuthIdentity(string userName, string password, string domain)
		{
			this.UserName = userName;
			this.UserNameLength = ((userName == null) ? 0 : userName.Length);
			this.Password = password;
			this.PasswordLength = ((password == null) ? 0 : password.Length);
			this.Domain = domain;
			this.DomainLength = ((domain == null) ? 0 : domain.Length);
			this.Flags = 2;
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x000626A7 File Offset: 0x000608A7
		public override string ToString()
		{
			return ValidationHelper.ToString(this.Domain) + "\\" + ValidationHelper.ToString(this.UserName);
		}

		// Token: 0x040014C3 RID: 5315
		internal string UserName;

		// Token: 0x040014C4 RID: 5316
		internal int UserNameLength;

		// Token: 0x040014C5 RID: 5317
		internal string Domain;

		// Token: 0x040014C6 RID: 5318
		internal int DomainLength;

		// Token: 0x040014C7 RID: 5319
		internal string Password;

		// Token: 0x040014C8 RID: 5320
		internal int PasswordLength;

		// Token: 0x040014C9 RID: 5321
		internal int Flags;
	}
}
