using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001F7 RID: 503
	internal interface ISymbolWriter : IDisposable
	{
		// Token: 0x06000F49 RID: 3913
		ISymbolReaderProvider GetReaderProvider();

		// Token: 0x06000F4A RID: 3914
		ImageDebugHeader GetDebugHeader();

		// Token: 0x06000F4B RID: 3915
		void Write(MethodDebugInformation info);
	}
}
