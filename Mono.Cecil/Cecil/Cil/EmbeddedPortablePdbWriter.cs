using System;
using System.IO;
using System.IO.Compression;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000110 RID: 272
	public sealed class EmbeddedPortablePdbWriter : ISymbolWriter, IDisposable
	{
		// Token: 0x06000AB0 RID: 2736 RVA: 0x000235D8 File Offset: 0x000217D8
		internal EmbeddedPortablePdbWriter(Stream stream, PortablePdbWriter writer)
		{
			this.stream = stream;
			this.writer = writer;
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x000235EE File Offset: 0x000217EE
		public ISymbolReaderProvider GetReaderProvider()
		{
			return new EmbeddedPortablePdbReaderProvider();
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x000235F8 File Offset: 0x000217F8
		public ImageDebugHeader GetDebugHeader()
		{
			this.writer.Dispose();
			ImageDebugDirectory imageDebugDirectory = new ImageDebugDirectory
			{
				Type = ImageDebugType.EmbeddedPortablePdb,
				MajorVersion = 256,
				MinorVersion = 256
			};
			MemoryStream memoryStream = new MemoryStream();
			BinaryStreamWriter binaryStreamWriter = new BinaryStreamWriter(memoryStream);
			binaryStreamWriter.WriteByte(77);
			binaryStreamWriter.WriteByte(80);
			binaryStreamWriter.WriteByte(68);
			binaryStreamWriter.WriteByte(66);
			binaryStreamWriter.WriteInt32((int)this.stream.Length);
			this.stream.Position = 0L;
			using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress, true))
			{
				this.stream.CopyTo(deflateStream);
			}
			imageDebugDirectory.SizeOfData = (int)memoryStream.Length;
			return new ImageDebugHeader(new ImageDebugHeaderEntry[]
			{
				this.writer.GetDebugHeader().Entries[0],
				new ImageDebugHeaderEntry(imageDebugDirectory, memoryStream.ToArray())
			});
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x000236F4 File Offset: 0x000218F4
		public void Write(MethodDebugInformation info)
		{
			this.writer.Write(info);
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00002A0D File Offset: 0x00000C0D
		public void Dispose()
		{
		}

		// Token: 0x0400067F RID: 1663
		private readonly Stream stream;

		// Token: 0x04000680 RID: 1664
		private readonly PortablePdbWriter writer;
	}
}
