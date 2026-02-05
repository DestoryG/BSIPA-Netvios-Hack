using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000132 RID: 306
	public interface ISymbolWriter : IDisposable
	{
		// Token: 0x06000B59 RID: 2905
		ISymbolReaderProvider GetReaderProvider();

		// Token: 0x06000B5A RID: 2906
		ImageDebugHeader GetDebugHeader();

		// Token: 0x06000B5B RID: 2907
		void Write(MethodDebugInformation info);
	}
}
