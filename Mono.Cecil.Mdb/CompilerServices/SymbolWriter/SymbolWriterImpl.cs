using System;
using System.Collections;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using System.Text;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000018 RID: 24
	public class SymbolWriterImpl : ISymbolWriter
	{
		// Token: 0x060000BB RID: 187 RVA: 0x0000553C File Offset: 0x0000373C
		public SymbolWriterImpl(Guid guid)
		{
			this.guid = guid;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00005561 File Offset: 0x00003761
		public void Close()
		{
			this.msw.WriteSymbolFile(this.guid);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00005574 File Offset: 0x00003774
		public void CloseMethod()
		{
			if (this.methodOpened)
			{
				this.methodOpened = false;
				this.nextLocalIndex = 0;
				this.msw.CloseMethod();
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00005597 File Offset: 0x00003797
		public void CloseNamespace()
		{
			this.namespaceStack.Pop();
			this.msw.CloseNamespace();
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000055B0 File Offset: 0x000037B0
		public void CloseScope(int endOffset)
		{
			this.msw.CloseScope(endOffset);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000055C0 File Offset: 0x000037C0
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

		// Token: 0x060000C1 RID: 193 RVA: 0x00004EEF File Offset: 0x000030EF
		public void DefineField(SymbolToken parent, string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3)
		{
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004EEF File Offset: 0x000030EF
		public void DefineGlobalVariable(string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3)
		{
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00005610 File Offset: 0x00003810
		public void DefineLocalVariable(string name, FieldAttributes attributes, byte[] signature, SymAddressKind addrKind, int addr1, int addr2, int addr3, int startOffset, int endOffset)
		{
			MonoSymbolWriter monoSymbolWriter = this.msw;
			int num = this.nextLocalIndex;
			this.nextLocalIndex = num + 1;
			monoSymbolWriter.DefineLocalVariable(num, name);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004EEF File Offset: 0x000030EF
		public void DefineParameter(string name, ParameterAttributes attributes, int sequence, SymAddressKind addrKind, int addr1, int addr2, int addr3)
		{
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000563C File Offset: 0x0000383C
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

		// Token: 0x060000C6 RID: 198 RVA: 0x000056AB File Offset: 0x000038AB
		public void Initialize(IntPtr emitter, string filename, bool fFullBuild)
		{
			this.msw = new MonoSymbolWriter(filename);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000056B9 File Offset: 0x000038B9
		public void OpenMethod(SymbolToken method)
		{
			this.currentToken = method.GetToken();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000056C8 File Offset: 0x000038C8
		public void OpenNamespace(string name)
		{
			NamespaceInfo namespaceInfo = new NamespaceInfo();
			namespaceInfo.NamespaceID = -1;
			namespaceInfo.Name = name;
			this.namespaceStack.Push(namespaceInfo);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x000056F5 File Offset: 0x000038F5
		public int OpenScope(int startOffset)
		{
			return this.msw.OpenScope(startOffset);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00005704 File Offset: 0x00003904
		public void SetMethodSourceRange(ISymbolDocumentWriter startDoc, int startLine, int startColumn, ISymbolDocumentWriter endDoc, int endLine, int endColumn)
		{
			int currentNamespace = this.GetCurrentNamespace(startDoc);
			SourceMethodImpl sourceMethodImpl = new SourceMethodImpl(this.methodName, this.currentToken, currentNamespace);
			this.msw.OpenMethod(((ICompileUnit)startDoc).Entry, currentNamespace, sourceMethodImpl);
			this.methodOpened = true;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004EEF File Offset: 0x000030EF
		public void SetScopeRange(int scopeID, int startOffset, int endOffset)
		{
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000574C File Offset: 0x0000394C
		public void SetSymAttribute(SymbolToken parent, string name, byte[] data)
		{
			if (name == "__name")
			{
				this.methodName = Encoding.UTF8.GetString(data);
			}
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004EEF File Offset: 0x000030EF
		public void SetUnderlyingWriter(IntPtr underlyingWriter)
		{
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004EEF File Offset: 0x000030EF
		public void SetUserEntryPoint(SymbolToken entryMethod)
		{
		}

		// Token: 0x060000CF RID: 207 RVA: 0x0000576C File Offset: 0x0000396C
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

		// Token: 0x060000D0 RID: 208 RVA: 0x000057EC File Offset: 0x000039EC
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

		// Token: 0x04000090 RID: 144
		private MonoSymbolWriter msw;

		// Token: 0x04000091 RID: 145
		private int nextLocalIndex;

		// Token: 0x04000092 RID: 146
		private int currentToken;

		// Token: 0x04000093 RID: 147
		private string methodName;

		// Token: 0x04000094 RID: 148
		private Stack namespaceStack = new Stack();

		// Token: 0x04000095 RID: 149
		private bool methodOpened;

		// Token: 0x04000096 RID: 150
		private Hashtable documents = new Hashtable();

		// Token: 0x04000097 RID: 151
		private Guid guid;
	}
}
