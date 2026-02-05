using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x0200010C RID: 268
	public sealed class EmbeddedPortablePdbReader : ISymbolReader, IDisposable
	{
		// Token: 0x06000A99 RID: 2713 RVA: 0x00023190 File Offset: 0x00021390
		internal EmbeddedPortablePdbReader(PortablePdbReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException();
			}
			this.reader = reader;
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x000231A8 File Offset: 0x000213A8
		public ISymbolWriterProvider GetWriterProvider()
		{
			return new EmbeddedPortablePdbWriterProvider();
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x000231AF File Offset: 0x000213AF
		public bool ProcessDebugHeader(ImageDebugHeader header)
		{
			return this.reader.ProcessDebugHeader(header);
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x000231BD File Offset: 0x000213BD
		public MethodDebugInformation Read(MethodDefinition method)
		{
			return this.reader.Read(method);
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x000231CB File Offset: 0x000213CB
		public void Dispose()
		{
			this.reader.Dispose();
		}

		// Token: 0x0400067A RID: 1658
		private readonly PortablePdbReader reader;
	}
}
