using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005CA RID: 1482
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public class CheckoutException : ExternalException
	{
		// Token: 0x06003752 RID: 14162 RVA: 0x000F01D6 File Offset: 0x000EE3D6
		public CheckoutException()
		{
		}

		// Token: 0x06003753 RID: 14163 RVA: 0x000F01DE File Offset: 0x000EE3DE
		public CheckoutException(string message)
			: base(message)
		{
		}

		// Token: 0x06003754 RID: 14164 RVA: 0x000F01E7 File Offset: 0x000EE3E7
		public CheckoutException(string message, int errorCode)
			: base(message, errorCode)
		{
		}

		// Token: 0x06003755 RID: 14165 RVA: 0x000F01F1 File Offset: 0x000EE3F1
		protected CheckoutException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x06003756 RID: 14166 RVA: 0x000F01FB File Offset: 0x000EE3FB
		public CheckoutException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x04002AE0 RID: 10976
		public static readonly CheckoutException Canceled = new CheckoutException(SR.GetString("CHECKOUTCanceled"), -2147467260);
	}
}
