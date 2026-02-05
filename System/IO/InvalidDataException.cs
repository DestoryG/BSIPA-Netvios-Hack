using System;
using System.Runtime.Serialization;

namespace System.IO
{
	// Token: 0x020003F9 RID: 1017
	[global::__DynamicallyInvokable]
	[Serializable]
	public sealed class InvalidDataException : SystemException
	{
		// Token: 0x0600264D RID: 9805 RVA: 0x000B0A3F File Offset: 0x000AEC3F
		[global::__DynamicallyInvokable]
		public InvalidDataException()
			: base(SR.GetString("GenericInvalidData"))
		{
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x000B0A51 File Offset: 0x000AEC51
		[global::__DynamicallyInvokable]
		public InvalidDataException(string message)
			: base(message)
		{
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x000B0A5A File Offset: 0x000AEC5A
		[global::__DynamicallyInvokable]
		public InvalidDataException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x000B0A64 File Offset: 0x000AEC64
		internal InvalidDataException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
