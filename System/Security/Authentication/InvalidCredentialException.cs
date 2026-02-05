using System;
using System.Runtime.Serialization;

namespace System.Security.Authentication
{
	// Token: 0x0200043B RID: 1083
	[Serializable]
	public class InvalidCredentialException : AuthenticationException
	{
		// Token: 0x0600287A RID: 10362 RVA: 0x000B9DFB File Offset: 0x000B7FFB
		public InvalidCredentialException()
		{
		}

		// Token: 0x0600287B RID: 10363 RVA: 0x000B9E03 File Offset: 0x000B8003
		protected InvalidCredentialException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x0600287C RID: 10364 RVA: 0x000B9E0D File Offset: 0x000B800D
		public InvalidCredentialException(string message)
			: base(message)
		{
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x000B9E16 File Offset: 0x000B8016
		public InvalidCredentialException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
