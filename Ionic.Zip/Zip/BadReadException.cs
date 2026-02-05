using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x02000020 RID: 32
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000A")]
	[Serializable]
	public class BadReadException : ZipException
	{
		// Token: 0x06000100 RID: 256 RVA: 0x00005AC2 File Offset: 0x00003CC2
		public BadReadException()
		{
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005ACA File Offset: 0x00003CCA
		public BadReadException(string message)
			: base(message)
		{
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005AD3 File Offset: 0x00003CD3
		public BadReadException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00005ADD File Offset: 0x00003CDD
		protected BadReadException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
