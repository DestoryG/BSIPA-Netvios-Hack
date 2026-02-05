using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ionic.Zip
{
	// Token: 0x02000021 RID: 33
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00009")]
	[Serializable]
	public class BadCrcException : ZipException
	{
		// Token: 0x06000104 RID: 260 RVA: 0x00005AE7 File Offset: 0x00003CE7
		public BadCrcException()
		{
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005AEF File Offset: 0x00003CEF
		public BadCrcException(string message)
			: base(message)
		{
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005AF8 File Offset: 0x00003CF8
		protected BadCrcException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
