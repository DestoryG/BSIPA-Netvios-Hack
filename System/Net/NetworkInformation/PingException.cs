using System;
using System.Runtime.Serialization;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002EB RID: 747
	[Serializable]
	public class PingException : InvalidOperationException
	{
		// Token: 0x06001A49 RID: 6729 RVA: 0x0007FAC5 File Offset: 0x0007DCC5
		internal PingException()
		{
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x0007FACD File Offset: 0x0007DCCD
		protected PingException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x0007FAD7 File Offset: 0x0007DCD7
		public PingException(string message)
			: base(message)
		{
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x0007FAE0 File Offset: 0x0007DCE0
		public PingException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
