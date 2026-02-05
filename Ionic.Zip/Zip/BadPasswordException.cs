using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x0200001F RID: 31
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000B")]
	[Serializable]
	public class BadPasswordException : ZipException
	{
		// Token: 0x060000FC RID: 252 RVA: 0x00005A9D File Offset: 0x00003C9D
		public BadPasswordException()
		{
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005AA5 File Offset: 0x00003CA5
		public BadPasswordException(string message)
			: base(message)
		{
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005AAE File Offset: 0x00003CAE
		public BadPasswordException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005AB8 File Offset: 0x00003CB8
		protected BadPasswordException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
