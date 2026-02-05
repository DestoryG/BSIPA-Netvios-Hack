using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000215 RID: 533
	internal class NegotiationInfoClass
	{
		// Token: 0x060013BF RID: 5055 RVA: 0x000685D8 File Offset: 0x000667D8
		internal NegotiationInfoClass(SafeHandle safeHandle, int negotiationState)
		{
			if (safeHandle.IsInvalid)
			{
				return;
			}
			IntPtr intPtr = safeHandle.DangerousGetHandle();
			if (negotiationState == 0 || negotiationState == 1)
			{
				IntPtr intPtr2 = Marshal.ReadIntPtr(intPtr, SecurityPackageInfo.NameOffest);
				string text = null;
				if (intPtr2 != IntPtr.Zero)
				{
					text = Marshal.PtrToStringUni(intPtr2);
				}
				if (string.Compare(text, "Kerberos", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.AuthenticationPackage = "Kerberos";
					return;
				}
				if (string.Compare(text, "NTLM", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.AuthenticationPackage = "NTLM";
					return;
				}
				if (string.Compare(text, "WDigest", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.AuthenticationPackage = "WDigest";
					return;
				}
				this.AuthenticationPackage = text;
			}
		}

		// Token: 0x040015B2 RID: 5554
		internal const string NTLM = "NTLM";

		// Token: 0x040015B3 RID: 5555
		internal const string Kerberos = "Kerberos";

		// Token: 0x040015B4 RID: 5556
		internal const string WDigest = "WDigest";

		// Token: 0x040015B5 RID: 5557
		internal const string Negotiate = "Negotiate";

		// Token: 0x040015B6 RID: 5558
		internal string AuthenticationPackage;
	}
}
