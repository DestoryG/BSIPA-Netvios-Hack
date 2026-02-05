using System;
using System.Runtime.InteropServices;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000010 RID: 16
	internal class SymWriter
	{
		// Token: 0x06000135 RID: 309
		[DllImport("ole32.dll")]
		private static extern int CoCreateInstance([In] ref Guid rclsid, [MarshalAs(UnmanagedType.IUnknown)] [In] object pUnkOuter, [In] uint dwClsContext, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		// Token: 0x06000136 RID: 310 RVA: 0x00003950 File Offset: 0x00001B50
		public SymWriter()
		{
			object obj;
			SymWriter.CoCreateInstance(ref SymWriter.s_CorSymWriter_SxS_ClassID, null, 1U, ref SymWriter.s_symUnmangedWriterIID, out obj);
			this.m_writer = (ISymUnmanagedWriter2)obj;
			this.documents = new Collection<ISymUnmanagedDocumentWriter>();
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00003990 File Offset: 0x00001B90
		public byte[] GetDebugInfo(out ImageDebugDirectory idd)
		{
			int num;
			this.m_writer.GetDebugInfo(out idd, 0, out num, null);
			byte[] array = new byte[num];
			this.m_writer.GetDebugInfo(out idd, num, out num, array);
			return array;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000039C8 File Offset: 0x00001BC8
		public void DefineLocalVariable2(string name, VariableAttributes attributes, int sigToken, int addr1, int addr2, int addr3, int startOffset, int endOffset)
		{
			this.m_writer.DefineLocalVariable2(name, (int)attributes, sigToken, 1, addr1, addr2, addr3, startOffset, endOffset);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000039EE File Offset: 0x00001BEE
		public void DefineConstant2(string name, object value, int sigToken)
		{
			if (value == null)
			{
				this.m_writer.DefineConstant2(name, 0, sigToken);
				return;
			}
			this.m_writer.DefineConstant2(name, value, sigToken);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00003A18 File Offset: 0x00001C18
		public void Close()
		{
			this.m_writer.Close();
			Marshal.ReleaseComObject(this.m_writer);
			foreach (ISymUnmanagedDocumentWriter symUnmanagedDocumentWriter in this.documents)
			{
				Marshal.ReleaseComObject(symUnmanagedDocumentWriter);
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00003A80 File Offset: 0x00001C80
		public void CloseMethod()
		{
			this.m_writer.CloseMethod();
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00003A8D File Offset: 0x00001C8D
		public void CloseNamespace()
		{
			this.m_writer.CloseNamespace();
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00003A9A File Offset: 0x00001C9A
		public void CloseScope(int endOffset)
		{
			this.m_writer.CloseScope(endOffset);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00003AA8 File Offset: 0x00001CA8
		public SymDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
		{
			ISymUnmanagedDocumentWriter symUnmanagedDocumentWriter;
			this.m_writer.DefineDocument(url, ref language, ref languageVendor, ref documentType, out symUnmanagedDocumentWriter);
			this.documents.Add(symUnmanagedDocumentWriter);
			return new SymDocumentWriter(symUnmanagedDocumentWriter);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00003ADB File Offset: 0x00001CDB
		public void DefineSequencePoints(SymDocumentWriter document, int[] offsets, int[] lines, int[] columns, int[] endLines, int[] endColumns)
		{
			this.m_writer.DefineSequencePoints(document.GetUnmanaged(), offsets.Length, offsets, lines, columns, endLines, endColumns);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00003AF9 File Offset: 0x00001CF9
		public void Initialize(object emitter, string filename, bool fFullBuild)
		{
			this.m_writer.Initialize(emitter, filename, null, fFullBuild);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00003B0A File Offset: 0x00001D0A
		public void SetUserEntryPoint(int methodToken)
		{
			this.m_writer.SetUserEntryPoint(methodToken);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00003B18 File Offset: 0x00001D18
		public void OpenMethod(int methodToken)
		{
			this.m_writer.OpenMethod(methodToken);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00003B26 File Offset: 0x00001D26
		public void OpenNamespace(string name)
		{
			this.m_writer.OpenNamespace(name);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00003B34 File Offset: 0x00001D34
		public int OpenScope(int startOffset)
		{
			int num;
			this.m_writer.OpenScope(startOffset, out num);
			return num;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00003B50 File Offset: 0x00001D50
		public void UsingNamespace(string fullName)
		{
			this.m_writer.UsingNamespace(fullName);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00003B60 File Offset: 0x00001D60
		public void DefineCustomMetadata(string name, byte[] metadata)
		{
			GCHandle gchandle = GCHandle.Alloc(metadata, GCHandleType.Pinned);
			this.m_writer.SetSymAttribute(0U, name, (uint)metadata.Length, gchandle.AddrOfPinnedObject());
			gchandle.Free();
		}

		// Token: 0x0400001A RID: 26
		private static Guid s_symUnmangedWriterIID = new Guid("0b97726e-9e6d-4f05-9a26-424022093caa");

		// Token: 0x0400001B RID: 27
		private static Guid s_CorSymWriter_SxS_ClassID = new Guid("108296c1-281e-11d3-bd22-0000f80849bd");

		// Token: 0x0400001C RID: 28
		private readonly ISymUnmanagedWriter2 m_writer;

		// Token: 0x0400001D RID: 29
		private readonly Collection<ISymUnmanagedDocumentWriter> documents;
	}
}
