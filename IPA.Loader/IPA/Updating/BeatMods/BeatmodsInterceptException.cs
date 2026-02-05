using System;
using System.Runtime.Serialization;

namespace IPA.Updating.BeatMods
{
	// Token: 0x02000010 RID: 16
	[Serializable]
	internal class BeatmodsInterceptException : Exception
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00002889 File Offset: 0x00000A89
		public BeatmodsInterceptException()
		{
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002891 File Offset: 0x00000A91
		public BeatmodsInterceptException(string message)
			: base(message)
		{
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000289A File Offset: 0x00000A9A
		public BeatmodsInterceptException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000028A4 File Offset: 0x00000AA4
		protected BeatmodsInterceptException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
