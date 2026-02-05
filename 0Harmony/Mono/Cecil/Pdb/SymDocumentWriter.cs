using System;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200023A RID: 570
	internal class SymDocumentWriter
	{
		// Token: 0x1700037A RID: 890
		// (get) Token: 0x060011A2 RID: 4514 RVA: 0x000398A7 File Offset: 0x00037AA7
		public ISymUnmanagedDocumentWriter Writer
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x000398AF File Offset: 0x00037AAF
		public SymDocumentWriter(ISymUnmanagedDocumentWriter writer)
		{
			this.writer = writer;
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x000398BE File Offset: 0x00037ABE
		public void SetSource(byte[] source)
		{
			this.writer.SetSource((uint)source.Length, source);
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x000398CF File Offset: 0x00037ACF
		public void SetCheckSum(Guid hashAlgo, byte[] checkSum)
		{
			this.writer.SetCheckSum(hashAlgo, (uint)checkSum.Length, checkSum);
		}

		// Token: 0x04000A35 RID: 2613
		private readonly ISymUnmanagedDocumentWriter writer;
	}
}
