using System;
using System.Runtime.Serialization;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001F3 RID: 499
	[Serializable]
	internal sealed class SymbolsNotMatchingException : InvalidOperationException
	{
		// Token: 0x06000F3E RID: 3902 RVA: 0x00033879 File Offset: 0x00031A79
		public SymbolsNotMatchingException(string message)
			: base(message)
		{
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x00033882 File Offset: 0x00031A82
		private SymbolsNotMatchingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
