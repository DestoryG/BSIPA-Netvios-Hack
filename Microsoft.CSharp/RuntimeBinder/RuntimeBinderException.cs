using System;
using System.Runtime.Serialization;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x0200001F RID: 31
	[Serializable]
	public class RuntimeBinderException : Exception
	{
		// Token: 0x06000114 RID: 276 RVA: 0x000063CA File Offset: 0x000045CA
		public RuntimeBinderException()
		{
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000063D2 File Offset: 0x000045D2
		public RuntimeBinderException(string message)
			: base(message)
		{
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000063DB File Offset: 0x000045DB
		public RuntimeBinderException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000063E5 File Offset: 0x000045E5
		protected RuntimeBinderException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			throw new PlatformNotSupportedException();
		}
	}
}
