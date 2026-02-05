using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000572 RID: 1394
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[Serializable]
	public class InvalidAsynchronousStateException : ArgumentException
	{
		// Token: 0x060033CE RID: 13262 RVA: 0x000E3E98 File Offset: 0x000E2098
		public InvalidAsynchronousStateException()
			: this(null)
		{
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x000E3EA1 File Offset: 0x000E20A1
		public InvalidAsynchronousStateException(string message)
			: base(message)
		{
		}

		// Token: 0x060033D0 RID: 13264 RVA: 0x000E3EAA File Offset: 0x000E20AA
		public InvalidAsynchronousStateException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x000E3EB4 File Offset: 0x000E20B4
		protected InvalidAsynchronousStateException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
