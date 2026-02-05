using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001D0 RID: 464
	internal sealed class EmbeddedPortablePdbReader : ISymbolReader, IDisposable
	{
		// Token: 0x06000E80 RID: 3712 RVA: 0x0003239C File Offset: 0x0003059C
		internal EmbeddedPortablePdbReader(PortablePdbReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException();
			}
			this.reader = reader;
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x000323B4 File Offset: 0x000305B4
		public ISymbolWriterProvider GetWriterProvider()
		{
			return new EmbeddedPortablePdbWriterProvider();
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x000323BB File Offset: 0x000305BB
		public bool ProcessDebugHeader(ImageDebugHeader header)
		{
			return this.reader.ProcessDebugHeader(header);
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x000323C9 File Offset: 0x000305C9
		public MethodDebugInformation Read(MethodDefinition method)
		{
			return this.reader.Read(method);
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x000323D7 File Offset: 0x000305D7
		public void Dispose()
		{
			this.reader.Dispose();
		}

		// Token: 0x040008D9 RID: 2265
		private readonly PortablePdbReader reader;
	}
}
