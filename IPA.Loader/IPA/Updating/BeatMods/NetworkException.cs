using System;
using System.Runtime.Serialization;

namespace IPA.Updating.BeatMods
{
	// Token: 0x0200000F RID: 15
	[Serializable]
	internal class NetworkException : Exception
	{
		// Token: 0x0600001F RID: 31 RVA: 0x00002864 File Offset: 0x00000A64
		public NetworkException()
		{
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000286C File Offset: 0x00000A6C
		public NetworkException(string message)
			: base(message)
		{
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002875 File Offset: 0x00000A75
		public NetworkException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000287F File Offset: 0x00000A7F
		protected NetworkException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
