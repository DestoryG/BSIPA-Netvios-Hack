using System;
using System.Collections.Generic;
using System.IO;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000219 RID: 537
	internal class MonoSymbolWriter
	{
		// Token: 0x06000FEE RID: 4078 RVA: 0x00036BC0 File Offset: 0x00034DC0
		public MonoSymbolWriter(string filename)
		{
			this.methods = new List<SourceMethodBuilder>();
			this.sources = new List<SourceFileEntry>();
			this.comp_units = new List<CompileUnitEntry>();
			this.file = new MonoSymbolFile();
			this.filename = filename + ".mdb";
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000FEF RID: 4079 RVA: 0x00036C1B File Offset: 0x00034E1B
		public MonoSymbolFile SymbolFile
		{
			get
			{
				return this.file;
			}
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x00010C51 File Offset: 0x0000EE51
		public void CloseNamespace()
		{
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x00036C23 File Offset: 0x00034E23
		public void DefineLocalVariable(int index, string name)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.AddLocal(index, name);
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x00036C3B File Offset: 0x00034E3B
		public void DefineCapturedLocal(int scope_id, string name, string captured_name)
		{
			this.file.DefineCapturedVariable(scope_id, name, captured_name, CapturedVariable.CapturedKind.Local);
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x00036C4C File Offset: 0x00034E4C
		public void DefineCapturedParameter(int scope_id, string name, string captured_name)
		{
			this.file.DefineCapturedVariable(scope_id, name, captured_name, CapturedVariable.CapturedKind.Parameter);
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x00036C5D File Offset: 0x00034E5D
		public void DefineCapturedThis(int scope_id, string captured_name)
		{
			this.file.DefineCapturedVariable(scope_id, "this", captured_name, CapturedVariable.CapturedKind.This);
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x00036C72 File Offset: 0x00034E72
		public void DefineCapturedScope(int scope_id, int id, string captured_name)
		{
			this.file.DefineCapturedScope(scope_id, id, captured_name);
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x00036C82 File Offset: 0x00034E82
		public void DefineScopeVariable(int scope, int index)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.AddScopeVariable(scope, index);
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x00036C9A File Offset: 0x00034E9A
		public void MarkSequencePoint(int offset, SourceFileEntry file, int line, int column, bool is_hidden)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.MarkSequencePoint(offset, file, line, column, is_hidden);
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x00036CB8 File Offset: 0x00034EB8
		public SourceMethodBuilder OpenMethod(ICompileUnit file, int ns_id, IMethodDef method)
		{
			SourceMethodBuilder sourceMethodBuilder = new SourceMethodBuilder(file, ns_id, method);
			this.current_method_stack.Push(this.current_method);
			this.current_method = sourceMethodBuilder;
			this.methods.Add(this.current_method);
			return sourceMethodBuilder;
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x00036CF8 File Offset: 0x00034EF8
		public void CloseMethod()
		{
			this.current_method = this.current_method_stack.Pop();
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x00036D0C File Offset: 0x00034F0C
		public SourceFileEntry DefineDocument(string url)
		{
			SourceFileEntry sourceFileEntry = new SourceFileEntry(this.file, url);
			this.sources.Add(sourceFileEntry);
			return sourceFileEntry;
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x00036D34 File Offset: 0x00034F34
		public SourceFileEntry DefineDocument(string url, byte[] guid, byte[] checksum)
		{
			SourceFileEntry sourceFileEntry = new SourceFileEntry(this.file, url, guid, checksum);
			this.sources.Add(sourceFileEntry);
			return sourceFileEntry;
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x00036D60 File Offset: 0x00034F60
		public CompileUnitEntry DefineCompilationUnit(SourceFileEntry source)
		{
			CompileUnitEntry compileUnitEntry = new CompileUnitEntry(this.file, source);
			this.comp_units.Add(compileUnitEntry);
			return compileUnitEntry;
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x00036D87 File Offset: 0x00034F87
		public int DefineNamespace(string name, CompileUnitEntry unit, string[] using_clauses, int parent)
		{
			if (unit == null || using_clauses == null)
			{
				throw new NullReferenceException();
			}
			return unit.DefineNamespace(name, using_clauses, parent);
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x00036D9F File Offset: 0x00034F9F
		public int OpenScope(int start_offset)
		{
			if (this.current_method == null)
			{
				return 0;
			}
			this.current_method.StartBlock(CodeBlockEntry.Type.Lexical, start_offset);
			return 0;
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x00036DB9 File Offset: 0x00034FB9
		public void CloseScope(int end_offset)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.EndBlock(end_offset);
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x00036DD0 File Offset: 0x00034FD0
		public void OpenCompilerGeneratedBlock(int start_offset)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.StartBlock(CodeBlockEntry.Type.CompilerGenerated, start_offset);
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x00036DB9 File Offset: 0x00034FB9
		public void CloseCompilerGeneratedBlock(int end_offset)
		{
			if (this.current_method == null)
			{
				return;
			}
			this.current_method.EndBlock(end_offset);
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x00036DE8 File Offset: 0x00034FE8
		public void StartIteratorBody(int start_offset)
		{
			this.current_method.StartBlock(CodeBlockEntry.Type.IteratorBody, start_offset);
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x00036DF7 File Offset: 0x00034FF7
		public void EndIteratorBody(int end_offset)
		{
			this.current_method.EndBlock(end_offset);
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x00036E05 File Offset: 0x00035005
		public void StartIteratorDispatcher(int start_offset)
		{
			this.current_method.StartBlock(CodeBlockEntry.Type.IteratorDispatcher, start_offset);
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x00036DF7 File Offset: 0x00034FF7
		public void EndIteratorDispatcher(int end_offset)
		{
			this.current_method.EndBlock(end_offset);
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x00036E14 File Offset: 0x00035014
		public void DefineAnonymousScope(int id)
		{
			this.file.DefineAnonymousScope(id);
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x00036E24 File Offset: 0x00035024
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

		// Token: 0x040009E7 RID: 2535
		private List<SourceMethodBuilder> methods;

		// Token: 0x040009E8 RID: 2536
		private List<SourceFileEntry> sources;

		// Token: 0x040009E9 RID: 2537
		private List<CompileUnitEntry> comp_units;

		// Token: 0x040009EA RID: 2538
		protected readonly MonoSymbolFile file;

		// Token: 0x040009EB RID: 2539
		private string filename;

		// Token: 0x040009EC RID: 2540
		private SourceMethodBuilder current_method;

		// Token: 0x040009ED RID: 2541
		private Stack<SourceMethodBuilder> current_method_stack = new Stack<SourceMethodBuilder>();
	}
}
