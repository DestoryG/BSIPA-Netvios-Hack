using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001F0 RID: 496
	internal interface ISymbolReader : IDisposable
	{
		// Token: 0x06000F37 RID: 3895
		ISymbolWriterProvider GetWriterProvider();

		// Token: 0x06000F38 RID: 3896
		bool ProcessDebugHeader(ImageDebugHeader header);

		// Token: 0x06000F39 RID: 3897
		MethodDebugInformation Read(MethodDefinition method);
	}
}
