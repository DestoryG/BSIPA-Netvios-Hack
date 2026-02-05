using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x0200001E RID: 30
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00006")]
	[Serializable]
	public class ZipException : Exception
	{
		// Token: 0x060000F8 RID: 248 RVA: 0x00005A78 File Offset: 0x00003C78
		public ZipException()
		{
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00005A80 File Offset: 0x00003C80
		public ZipException(string message)
			: base(message)
		{
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00005A89 File Offset: 0x00003C89
		public ZipException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00005A93 File Offset: 0x00003C93
		protected ZipException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
