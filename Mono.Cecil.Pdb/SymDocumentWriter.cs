using System;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200000F RID: 15
	internal class SymDocumentWriter
	{
		// Token: 0x06000133 RID: 307 RVA: 0x00003937 File Offset: 0x00001B37
		public SymDocumentWriter(ISymUnmanagedDocumentWriter unmanagedDocumentWriter)
		{
			this.m_unmanagedDocumentWriter = unmanagedDocumentWriter;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00003946 File Offset: 0x00001B46
		public ISymUnmanagedDocumentWriter GetUnmanaged()
		{
			return this.m_unmanagedDocumentWriter;
		}

		// Token: 0x04000019 RID: 25
		private readonly ISymUnmanagedDocumentWriter m_unmanagedDocumentWriter;
	}
}
