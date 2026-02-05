using System;
using System.Runtime.Serialization;

namespace System.Security.Authentication
{
	// Token: 0x0200043A RID: 1082
	[Serializable]
	public class AuthenticationException : SystemException
	{
		// Token: 0x06002876 RID: 10358 RVA: 0x000B9DD6 File Offset: 0x000B7FD6
		public AuthenticationException()
		{
		}

		// Token: 0x06002877 RID: 10359 RVA: 0x000B9DDE File Offset: 0x000B7FDE
		protected AuthenticationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x06002878 RID: 10360 RVA: 0x000B9DE8 File Offset: 0x000B7FE8
		public AuthenticationException(string message)
			: base(message)
		{
		}

		// Token: 0x06002879 RID: 10361 RVA: 0x000B9DF1 File Offset: 0x000B7FF1
		public AuthenticationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
