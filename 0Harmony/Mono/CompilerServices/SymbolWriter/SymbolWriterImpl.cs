using System;
using System.Collections;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using System.Text;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200021B RID: 539
	internal class SymbolWriterImpl : ISymbolWriter
	{
		// Token: 0x06001018 RID: 4120 RVA: 0x0003726C File Offset: 0x0003546C
		public SymbolWriterImpl(Guid guid)
		{
			this.guid = guid;
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x00037291 File Offset: 0x00035491
		public void Close()
		{
			this.msw.WriteSymbolFile(this.guid);
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x000372A4 File Offset: 0x000354A4
		public void CloseMethod()
		{
			if (this.methodOpened)
			{
				this.methodOpened = false;
				this.nextLocalIndex = 0;
				this.msw.CloseMethod();
			}
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x000372C7 File Offset: 0x000354C7
		public void CloseNamespace()
		{
			this.namespaceStack.Pop();
			this.msw.CloseNamespace();
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x000372E0 File Offset: 0x000354E0
		public void CloseScope(int endOffset)
		{
			this.msw.CloseScope(endOffset);
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x000372F0 File Offset: 0x000354F0
		public ISymbolDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
		{
			SymbolDocumentWriterImpl symbolDocumentWriterImpl = (SymbolDocumentWriterImpl)this.documents[url];
			if (symbolDocumentWriterImpl == null)
			{
				SourceFileEntry sourceFileEntry = this.msw.DefineDocument(url);
				symbolDocumentWriterImpl = new SymbolDocumentWriterImpl(this.msw.DefineCompilationUnit(sourceFileEntry));
				this.documents[url] = symbolDocumentWriterImpl;
			}
			return symbolDocumentWriterImpl;
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x00010C51 File Offset: 0x0000EE51
		public void DefineField(SymbolToken parent, string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3)
		{
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x00010C51 File Offset: 0x0000EE51
		public void DefineGlobalVariable(string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3)
		{
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x00037340 File Offset: 0x00035540
		public void DefineLocalVariable(string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3, int startOffset, int endOffset)
		{
			MonoSymbolWriter monoSymbolWriter = this.msw;
			int num = this.nextLocalIndex;
			this.nextLocalIndex = num + 1;
			monoSymbolWriter.DefineLocalVariable(num, name);
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x00010C51 File Offset: 0x0000EE51
		public void DefineParameter(string name, ParameterAttributes attributes, int sequence, SymAddressKind addrKind, int addr1, int addr2, int addr3)
		{
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0003736C File Offset: 0x0003556C
		public void DefineSequencePoints(ISymbolDocumentWriter document, int[] offsets, int[] lines, int[] columns, int[] endLines, int[] endColumns)
		{
			SymbolDocumentWriterImpl symbolDocumentWriterImpl = (SymbolDocumentWriterImpl)document;
			SourceFileEntry sourceFileEntry = ((symbolDocumentWriterImpl != null) ? symbolDocumentWriterImpl.Entry.SourceFile : null);
			for (int i = 0; i < offsets.Length; i++)
			{
				if (i <= 0 || offsets[i] != offsets[i - 1] || lines[i] != lines[i - 1] || columns[i] != columns[i - 1])
				{
					this.msw.MarkSequencePoint(offsets[i], sourceFileEntry, lines[i], columns[i], false);
				}
			}
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x000373DB File Offset: 0x000355DB
		public void Initialize(IntPtr emitter, string filename, bool fFullBuild)
		{
			this.msw = new MonoSymbolWriter(filename);
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x000373E9 File Offset: 0x000355E9
		public void OpenMethod(SymbolToken method)
		{
			this.currentToken = method.GetToken();
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x000373F8 File Offset: 0x000355F8
		public void OpenNamespace(string name)
		{
			NamespaceInfo namespaceInfo = new NamespaceInfo();
			namespaceInfo.NamespaceID = -1;
			namespaceInfo.Name = name;
			this.namespaceStack.Push(namespaceInfo);
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x00037425 File Offset: 0x00035625
		public int OpenScope(int startOffset)
		{
			return this.msw.OpenScope(startOffset);
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x00037434 File Offset: 0x00035634
		public void SetMethodSourceRange(ISymbolDocumentWriter startDoc, int startLine, int startColumn, ISymbolDocumentWriter endDoc, int endLine, int endColumn)
		{
			int currentNamespace = this.GetCurrentNamespace(startDoc);
			SourceMethodImpl sourceMethodImpl = new SourceMethodImpl(this.methodName, this.currentToken, currentNamespace);
			this.msw.OpenMethod(((ICompileUnit)startDoc).Entry, currentNamespace, sourceMethodImpl);
			this.methodOpened = true;
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x00010C51 File Offset: 0x0000EE51
		public void SetScopeRange(int scopeID, int startOffset, int endOffset)
		{
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0003747C File Offset: 0x0003567C
		public void SetSymAttribute(SymbolToken parent, string name, byte[] data)
		{
			if (name == "__name")
			{
				this.methodName = Encoding.UTF8.GetString(data);
			}
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x00010C51 File Offset: 0x0000EE51
		public void SetUnderlyingWriter(IntPtr underlyingWriter)
		{
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x00010C51 File Offset: 0x0000EE51
		public void SetUserEntryPoint(SymbolToken entryMethod)
		{
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x0003749C File Offset: 0x0003569C
		public void UsingNamespace(string fullName)
		{
			if (this.namespaceStack.Count == 0)
			{
				this.OpenNamespace("");
			}
			NamespaceInfo namespaceInfo = (NamespaceInfo)this.namespaceStack.Peek();
			if (namespaceInfo.NamespaceID != -1)
			{
				NamespaceInfo namespaceInfo2 = namespaceInfo;
				this.CloseNamespace();
				this.OpenNamespace(namespaceInfo2.Name);
				namespaceInfo = (NamespaceInfo)this.namespaceStack.Peek();
				namespaceInfo.UsingClauses = namespaceInfo2.UsingClauses;
			}
			namespaceInfo.UsingClauses.Add(fullName);
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x0003751C File Offset: 0x0003571C
		private int GetCurrentNamespace(ISymbolDocumentWriter doc)
		{
			if (this.namespaceStack.Count == 0)
			{
				this.OpenNamespace("");
			}
			NamespaceInfo namespaceInfo = (NamespaceInfo)this.namespaceStack.Peek();
			if (namespaceInfo.NamespaceID == -1)
			{
				string[] array = (string[])namespaceInfo.UsingClauses.ToArray(typeof(string));
				int num = 0;
				if (this.namespaceStack.Count > 1)
				{
					this.namespaceStack.Pop();
					num = ((NamespaceInfo)this.namespaceStack.Peek()).NamespaceID;
					this.namespaceStack.Push(namespaceInfo);
				}
				namespaceInfo.NamespaceID = this.msw.DefineNamespace(namespaceInfo.Name, ((ICompileUnit)doc).Entry, array, num);
			}
			return namespaceInfo.NamespaceID;
		}

		// Token: 0x040009F6 RID: 2550
		private MonoSymbolWriter msw;

		// Token: 0x040009F7 RID: 2551
		private int nextLocalIndex;

		// Token: 0x040009F8 RID: 2552
		private int currentToken;

		// Token: 0x040009F9 RID: 2553
		private string methodName;

		// Token: 0x040009FA RID: 2554
		private Stack namespaceStack = new Stack();

		// Token: 0x040009FB RID: 2555
		private bool methodOpened;

		// Token: 0x040009FC RID: 2556
		private Hashtable documents = new Hashtable();

		// Token: 0x040009FD RID: 2557
		private Guid guid;
	}
}
