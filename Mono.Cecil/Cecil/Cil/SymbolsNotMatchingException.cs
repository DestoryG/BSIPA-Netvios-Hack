using System;
using System.Runtime.Serialization;

namespace Mono.Cecil.Cil
{
	// Token: 0x0200012E RID: 302
	[Serializable]
	public sealed class SymbolsNotMatchingException : InvalidOperationException
	{
		// Token: 0x06000B4E RID: 2894 RVA: 0x000244C1 File Offset: 0x000226C1
		public SymbolsNotMatchingException(string message)
			: base(message)
		{
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x000244CA File Offset: 0x000226CA
		private SymbolsNotMatchingException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
