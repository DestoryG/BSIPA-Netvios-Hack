using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000002 RID: 2
	public class MonoSymbolFileException : Exception
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public MonoSymbolFileException()
		{
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public MonoSymbolFileException(string message, params object[] args)
			: base(string.Format(message, args))
		{
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002067 File Offset: 0x00000267
		public MonoSymbolFileException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
