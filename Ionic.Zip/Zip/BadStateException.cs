using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x02000023 RID: 35
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00007")]
	[Serializable]
	public class BadStateException : ZipException
	{
		// Token: 0x0600010A RID: 266 RVA: 0x00005B1D File Offset: 0x00003D1D
		public BadStateException()
		{
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00005B25 File Offset: 0x00003D25
		public BadStateException(string message)
			: base(message)
		{
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00005B2E File Offset: 0x00003D2E
		public BadStateException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00005B38 File Offset: 0x00003D38
		protected BadStateException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
