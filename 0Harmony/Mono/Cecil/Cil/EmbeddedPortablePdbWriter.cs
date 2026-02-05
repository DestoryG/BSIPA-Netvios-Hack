using System;
using System.IO;
using System.IO.Compression;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001D4 RID: 468
	internal sealed class EmbeddedPortablePdbWriter : ISymbolWriter, IDisposable
	{
		// Token: 0x06000E97 RID: 3735 RVA: 0x000327E4 File Offset: 0x000309E4
		internal EmbeddedPortablePdbWriter(Stream stream, PortablePdbWriter writer)
		{
			this.stream = stream;
			this.writer = writer;
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x000327FA File Offset: 0x000309FA
		public ISymbolReaderProvider GetReaderProvider()
		{
			return new EmbeddedPortablePdbReaderProvider();
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00032804 File Offset: 0x00030A04
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

		// Token: 0x06000E9A RID: 3738 RVA: 0x00032900 File Offset: 0x00030B00
		public void Write(MethodDebugInformation info)
		{
			this.writer.Write(info);
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00010C51 File Offset: 0x0000EE51
		public void Dispose()
		{
		}

		// Token: 0x040008DE RID: 2270
		private readonly Stream stream;

		// Token: 0x040008DF RID: 2271
		private readonly PortablePdbWriter writer;
	}
}
