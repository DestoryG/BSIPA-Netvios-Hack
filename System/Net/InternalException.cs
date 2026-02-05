using System;
using System.Runtime.Serialization;

namespace System.Net
{
	// Token: 0x02000114 RID: 276
	internal class InternalException : SystemException
	{
		// Token: 0x06000B05 RID: 2821 RVA: 0x0003CC6F File Offset: 0x0003AE6F
		internal InternalException()
		{
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x0003CC77 File Offset: 0x0003AE77
		internal InternalException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}
	}
}
