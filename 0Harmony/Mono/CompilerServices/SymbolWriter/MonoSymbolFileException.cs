using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000200 RID: 512
	internal class MonoSymbolFileException : Exception
	{
		// Token: 0x06000F5B RID: 3931 RVA: 0x00009F70 File Offset: 0x00008170
		public MonoSymbolFileException()
		{
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x00033D5C File Offset: 0x00031F5C
		public MonoSymbolFileException(string message, params object[] args)
			: base(string.Format(message, args))
		{
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x00009F81 File Offset: 0x00008181
		public MonoSymbolFileException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
