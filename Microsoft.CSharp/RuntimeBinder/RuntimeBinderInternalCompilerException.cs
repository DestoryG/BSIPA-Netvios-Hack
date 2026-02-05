using System;
using System.Runtime.Serialization;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x02000021 RID: 33
	[Serializable]
	public class RuntimeBinderInternalCompilerException : Exception
	{
		// Token: 0x06000122 RID: 290 RVA: 0x00006AFD File Offset: 0x00004CFD
		public RuntimeBinderInternalCompilerException()
		{
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00006B05 File Offset: 0x00004D05
		public RuntimeBinderInternalCompilerException(string message)
			: base(message)
		{
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00006B0E File Offset: 0x00004D0E
		public RuntimeBinderInternalCompilerException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006B18 File Offset: 0x00004D18
		protected RuntimeBinderInternalCompilerException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			throw new PlatformNotSupportedException();
		}
	}
}
