using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x0200012B RID: 299
	public interface ISymbolReader : IDisposable
	{
		// Token: 0x06000B47 RID: 2887
		ISymbolWriterProvider GetWriterProvider();

		// Token: 0x06000B48 RID: 2888
		bool ProcessDebugHeader(ImageDebugHeader header);

		// Token: 0x06000B49 RID: 2889
		MethodDebugInformation Read(MethodDefinition method);
	}
}
