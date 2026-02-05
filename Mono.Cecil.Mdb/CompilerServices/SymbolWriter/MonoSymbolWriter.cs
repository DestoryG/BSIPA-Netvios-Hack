using System;
using System.Collections.Generic;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000016 RID: 22
	public class MonoSymbolWriter
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00004E8C File Offset: 0x0000308C
		public MonoSymbolWriter(string filename)
		{
			this.methods = new List<SourceMethodBuilder>();
			this.sources = new List<SourceFileEntry>();
			this.comp_units = new List<CompileUnitEntry>();
			this.file = new MonoSymbolFile();
			this.filename = filename + ".mdb";
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00004EE7 File Offset: 0x000030E7
		public MonoSymbolFile SymbolFile
		{
			get
			{
				return this.file;
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004EEF File Offset: 0x000030EF
		public void CloseNamespace()
		{
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004EF1 File Offset: 0x000030F1
		public void DefineLocalVariable(int index, string name)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.AddLocal(index, name);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004F09 File Offset: 0x00003109
		public void DefineCapturedLocal(int scope_id, string name, string captured_name)
		{
			this.file.DefineCapturedVariable(scope_id, name, captured_name, CapturedVariable.CapturedKind.Local);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004F1A File Offset: 0x0000311A
		public void DefineCapturedParameter(int scope_id, string name, string captured_name)
		{
			this.file.DefineCapturedVariable(scope_id, name, captured_name, CapturedVariable.CapturedKind.Parameter);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004F2B File Offset: 0x0000312B
		public void DefineCapturedThis(int scope_id, string captured_name)
		{
			this.file.DefineCapturedVariable(scope_id, "this", captured_name, CapturedVariable.CapturedKind.This);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004F40 File Offset: 0x00003140
		public void DefineCapturedScope(int scope_id, int id, string captured_name)
		{
			this.file.DefineCapturedScope(scope_id, id, captured_name);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004F50 File Offset: 0x00003150
		public void DefineScopeVariable(int scope, int index)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.AddScopeVariable(scope, index);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004F68 File Offset: 0x00003168
		public void MarkSequencePoint(int offset, SourceFileEntry file, int line, int column, bool is_hidden)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.MarkSequencePoint(offset, file, line, column, is_hidden);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004F88 File Offset: 0x00003188
		public SourceMethodBuilder OpenMethod(ICompileUnit file, int ns_id, IMethodDef method)
		{
			SourceMethodBuilder sourceMethodBuilder = new SourceMethodBuilder(file, ns_id, method);
			this.current_method_stack.Push(this.current_method);
			this.current_method = sourceMethodBuilder;
			this.methods.Add(this.current_method);
			return sourceMethodBuilder;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004FC8 File Offset: 0x000031C8
		public void CloseMethod()
		{
			this.current_method = this.current_method_stack.Pop();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004FDC File Offset: 0x000031DC
		public SourceFileEntry DefineDocument(string url)
		{
			SourceFileEntry sourceFileEntry = new SourceFileEntry(this.file, url);
			this.sources.Add(sourceFileEntry);
			return sourceFileEntry;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00005004 File Offset: 0x00003204
		public SourceFileEntry DefineDocument(string url, byte[] guid, byte[] checksum)
		{
			SourceFileEntry sourceFileEntry = new SourceFileEntry(this.file, url, guid, checksum);
			this.sources.Add(sourceFileEntry);
			return sourceFileEntry;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00005030 File Offset: 0x00003230
		public CompileUnitEntry DefineCompilationUnit(SourceFileEntry source)
		{
			CompileUnitEntry compileUnitEntry = new CompileUnitEntry(this.file, source);
			this.comp_units.Add(compileUnitEntry);
			return compileUnitEntry;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00005057 File Offset: 0x00003257
		public int DefineNamespace(string name, CompileUnitEntry unit, string[] using_clauses, int parent)
		{
			if (unit == null || using_clauses == null)
			{
				throw new NullReferenceException();
			}
			return unit.DefineNamespace(name, using_clauses, parent);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000506F File Offset: 0x0000326F
		public int OpenScope(int start_offset)
		{
			if (this.current_method == null)
			{
				return 0;
			}
			this.current_method.StartBlock(CodeBlockEntry.Type.Lexical, start_offset);
			return 0;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005089 File Offset: 0x00003289
		public void CloseScope(int end_offset)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.EndBlock(end_offset);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000050A0 File Offset: 0x000032A0
		public void OpenCompilerGeneratedBlock(int start_offset)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.StartBlock(CodeBlockEntry.Type.CompilerGenerated, start_offset);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005089 File Offset: 0x00003289
		public void CloseCompilerGeneratedBlock(int end_offset)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.EndBlock(end_offset);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000050B8 File Offset: 0x000032B8
		public void StartIteratorBody(int start_offset)
		{
			this.current_method.StartBlock(CodeBlockEntry.Type.IteratorBody, start_offset);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000050C7 File Offset: 0x000032C7
		public void EndIteratorBody(int end_offset)
		{
			this.current_method.EndBlock(end_offset);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000050D5 File Offset: 0x000032D5
		public void StartIteratorDispatcher(int start_offset)
		{
			this.current_method.StartBlock(CodeBlockEntry.Type.IteratorDispatcher, start_offset);
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000050C7 File Offset: 0x000032C7
		public void EndIteratorDispatcher(int end_offset)
		{
			this.current_method.EndBlock(end_offset);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000050E4 File Offset: 0x000032E4
		public void DefineAnonymousScope(int id)
		{
			this.file.DefineAnonymousScope(id);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000050F4 File Offset: 0x000032F4
		public void WriteSymbolFile(Guid guid)
		{
			foreach (SourceMethodBuilder sourceMethodBuilder in this.methods)
			{
				sourceMethodBuilder.DefineMethod(this.file);
			}
			try
			{
				File.Delete(this.filename);
			}
			catch
			{
			}
			using (FileStream fileStream = new FileStream(this.filename, FileMode.Create, FileAccess.Write))
			{
				this.file.CreateSymbolFile(guid, fileStream);
			}
		}

		// Token: 0x04000081 RID: 129
		private List<SourceMethodBuilder> methods;

		// Token: 0x04000082 RID: 130
		private List<SourceFileEntry> sources;

		// Token: 0x04000083 RID: 131
		private List<CompileUnitEntry> comp_units;

		// Token: 0x04000084 RID: 132
		protected readonly MonoSymbolFile file;

		// Token: 0x04000085 RID: 133
		private string filename;

		// Token: 0x04000086 RID: 134
		private SourceMethodBuilder current_method;

		// Token: 0x04000087 RID: 135
		private Stack<SourceMethodBuilder> current_method_stack = new Stack<SourceMethodBuilder>();
	}
}
