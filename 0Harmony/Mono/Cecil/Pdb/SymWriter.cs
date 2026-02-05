using System;
using System.Runtime.InteropServices;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200023B RID: 571
	internal class SymWriter
	{
		// Token: 0x060011A6 RID: 4518
		[DllImport("ole32.dll")]
		private static extern int CoCreateInstance([In] ref Guid rclsid, [MarshalAs(UnmanagedType.IUnknown)] [In] object pUnkOuter, [In] uint dwClsContext, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		// Token: 0x060011A7 RID: 4519 RVA: 0x000398E4 File Offset: 0x00037AE4
		public SymWriter()
		{
			object obj;
			SymWriter.CoCreateInstance(ref SymWriter.s_CorSymWriter_SxS_ClassID, null, 1U, ref SymWriter.s_symUnmangedWriterIID, out obj);
			this.writer = (ISymUnmanagedWriter2)obj;
			this.documents = new Collection<ISymUnmanagedDocumentWriter>();
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x00039924 File Offset: 0x00037B24
		public byte[] GetDebugInfo(out ImageDebugDirectory idd)
		{
			int num;
			this.writer.GetDebugInfo(out idd, 0, out num, null);
			byte[] array = new byte[num];
			this.writer.GetDebugInfo(out idd, num, out num, array);
			return array;
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x0003995C File Offset: 0x00037B5C
		public void DefineLocalVariable2(string name, VariableAttributes attributes, int sigToken, int addr1, int addr2, int addr3, int startOffset, int endOffset)
		{
			this.writer.DefineLocalVariable2(name, (int)attributes, sigToken, 1, addr1, addr2, addr3, startOffset, endOffset);
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x00039982 File Offset: 0x00037B82
		public void DefineConstant2(string name, object value, int sigToken)
		{
			if (value == null)
			{
				this.writer.DefineConstant2(name, 0, sigToken);
				return;
			}
			this.writer.DefineConstant2(name, value, sigToken);
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x000399AC File Offset: 0x00037BAC
		public void Close()
		{
			this.writer.Close();
			Marshal.ReleaseComObject(this.writer);
			foreach (ISymUnmanagedDocumentWriter symUnmanagedDocumentWriter in this.documents)
			{
				Marshal.ReleaseComObject(symUnmanagedDocumentWriter);
			}
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x00039A14 File Offset: 0x00037C14
		public void CloseMethod()
		{
			this.writer.CloseMethod();
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x00039A21 File Offset: 0x00037C21
		public void CloseNamespace()
		{
			this.writer.CloseNamespace();
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x00039A2E File Offset: 0x00037C2E
		public void CloseScope(int endOffset)
		{
			this.writer.CloseScope(endOffset);
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x00039A3C File Offset: 0x00037C3C
		public SymDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
		{
			ISymUnmanagedDocumentWriter symUnmanagedDocumentWriter;
			this.writer.DefineDocument(url, ref language, ref languageVendor, ref documentType, out symUnmanagedDocumentWriter);
			this.documents.Add(symUnmanagedDocumentWriter);
			return new SymDocumentWriter(symUnmanagedDocumentWriter);
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x00039A6F File Offset: 0x00037C6F
		public void DefineSequencePoints(SymDocumentWriter document, int[] offsets, int[] lines, int[] columns, int[] endLines, int[] endColumns)
		{
			this.writer.DefineSequencePoints(document.Writer, offsets.Length, offsets, lines, columns, endLines, endColumns);
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x00039A8D File Offset: 0x00037C8D
		public void Initialize(object emitter, string filename, bool fFullBuild)
		{
			this.writer.Initialize(emitter, filename, null, fFullBuild);
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00039A9E File Offset: 0x00037C9E
		public void SetUserEntryPoint(int methodToken)
		{
			this.writer.SetUserEntryPoint(methodToken);
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x00039AAC File Offset: 0x00037CAC
		public void OpenMethod(int methodToken)
		{
			this.writer.OpenMethod(methodToken);
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x00039ABA File Offset: 0x00037CBA
		public void OpenNamespace(string name)
		{
			this.writer.OpenNamespace(name);
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00039AC8 File Offset: 0x00037CC8
		public int OpenScope(int startOffset)
		{
			int num;
			this.writer.OpenScope(startOffset, out num);
			return num;
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x00039AE4 File Offset: 0x00037CE4
		public void UsingNamespace(string fullName)
		{
			this.writer.UsingNamespace(fullName);
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00039AF4 File Offset: 0x00037CF4
		public void DefineCustomMetadata(string name, byte[] metadata)
		{
			GCHandle gchandle = GCHandle.Alloc(metadata, GCHandleType.Pinned);
			this.writer.SetSymAttribute(0U, name, (uint)metadata.Length, gchandle.AddrOfPinnedObject());
			gchandle.Free();
		}

		// Token: 0x04000A36 RID: 2614
		private static Guid s_symUnmangedWriterIID = new Guid("0b97726e-9e6d-4f05-9a26-424022093caa");

		// Token: 0x04000A37 RID: 2615
		private static Guid s_CorSymWriter_SxS_ClassID = new Guid("108296c1-281e-11d3-bd22-0000f80849bd");

		// Token: 0x04000A38 RID: 2616
		private readonly ISymUnmanagedWriter2 writer;

		// Token: 0x04000A39 RID: 2617
		private readonly Collection<ISymUnmanagedDocumentWriter> documents;
	}
}
