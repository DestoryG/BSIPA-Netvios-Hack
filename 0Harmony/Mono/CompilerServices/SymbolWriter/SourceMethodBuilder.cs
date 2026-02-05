using System;
using System.Collections.Generic;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200021A RID: 538
	internal class SourceMethodBuilder
	{
		// Token: 0x06001008 RID: 4104 RVA: 0x00036ECC File Offset: 0x000350CC
		public SourceMethodBuilder(ICompileUnit comp_unit)
		{
			this._comp_unit = comp_unit;
			this.method_lines = new List<LineNumberEntry>();
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x00036EE6 File Offset: 0x000350E6
		public SourceMethodBuilder(ICompileUnit comp_unit, int ns_id, IMethodDef method)
			: this(comp_unit)
		{
			this.ns_id = ns_id;
			this.method = method;
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x00036EFD File Offset: 0x000350FD
		public void MarkSequencePoint(int offset, SourceFileEntry file, int line, int column, bool is_hidden)
		{
			this.MarkSequencePoint(offset, file, line, column, -1, -1, is_hidden);
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x00036F10 File Offset: 0x00035110
		public void MarkSequencePoint(int offset, SourceFileEntry file, int line, int column, int end_line, int end_column, bool is_hidden)
		{
			LineNumberEntry lineNumberEntry = new LineNumberEntry((file != null) ? file.Index : 0, line, column, end_line, end_column, offset, is_hidden);
			if (this.method_lines.Count > 0)
			{
				LineNumberEntry lineNumberEntry2 = this.method_lines[this.method_lines.Count - 1];
				if (lineNumberEntry2.Offset == offset)
				{
					if (LineNumberEntry.LocationComparer.Default.Compare(lineNumberEntry, lineNumberEntry2) > 0)
					{
						this.method_lines[this.method_lines.Count - 1] = lineNumberEntry;
					}
					return;
				}
			}
			this.method_lines.Add(lineNumberEntry);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x00036F9E File Offset: 0x0003519E
		public void StartBlock(CodeBlockEntry.Type type, int start_offset)
		{
			this.StartBlock(type, start_offset, (this._blocks == null) ? 1 : (this._blocks.Count + 1));
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x00036FC0 File Offset: 0x000351C0
		public void StartBlock(CodeBlockEntry.Type type, int start_offset, int scopeIndex)
		{
			if (this._block_stack == null)
			{
				this._block_stack = new Stack<CodeBlockEntry>();
			}
			if (this._blocks == null)
			{
				this._blocks = new List<CodeBlockEntry>();
			}
			int num = ((this.CurrentBlock != null) ? this.CurrentBlock.Index : (-1));
			CodeBlockEntry codeBlockEntry = new CodeBlockEntry(scopeIndex, num, type, start_offset);
			this._block_stack.Push(codeBlockEntry);
			this._blocks.Add(codeBlockEntry);
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0003702C File Offset: 0x0003522C
		public void EndBlock(int end_offset)
		{
			this._block_stack.Pop().Close(end_offset);
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x0600100F RID: 4111 RVA: 0x00037040 File Offset: 0x00035240
		public CodeBlockEntry[] Blocks
		{
			get
			{
				if (this._blocks == null)
				{
					return new CodeBlockEntry[0];
				}
				CodeBlockEntry[] array = new CodeBlockEntry[this._blocks.Count];
				this._blocks.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06001010 RID: 4112 RVA: 0x0003707B File Offset: 0x0003527B
		public CodeBlockEntry CurrentBlock
		{
			get
			{
				if (this._block_stack != null && this._block_stack.Count > 0)
				{
					return this._block_stack.Peek();
				}
				return null;
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06001011 RID: 4113 RVA: 0x000370A0 File Offset: 0x000352A0
		public LocalVariableEntry[] Locals
		{
			get
			{
				if (this._locals == null)
				{
					return new LocalVariableEntry[0];
				}
				return this._locals.ToArray();
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06001012 RID: 4114 RVA: 0x000370BC File Offset: 0x000352BC
		public ICompileUnit SourceFile
		{
			get
			{
				return this._comp_unit;
			}
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x000370C4 File Offset: 0x000352C4
		public void AddLocal(int index, string name)
		{
			if (this._locals == null)
			{
				this._locals = new List<LocalVariableEntry>();
			}
			int num = ((this.CurrentBlock != null) ? this.CurrentBlock.Index : 0);
			this._locals.Add(new LocalVariableEntry(index, name, num));
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06001014 RID: 4116 RVA: 0x0003710E File Offset: 0x0003530E
		public ScopeVariable[] ScopeVariables
		{
			get
			{
				if (this._scope_vars == null)
				{
					return new ScopeVariable[0];
				}
				return this._scope_vars.ToArray();
			}
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0003712A File Offset: 0x0003532A
		public void AddScopeVariable(int scope, int index)
		{
			if (this._scope_vars == null)
			{
				this._scope_vars = new List<ScopeVariable>();
			}
			this._scope_vars.Add(new ScopeVariable(scope, index));
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00037151 File Offset: 0x00035351
		public void DefineMethod(MonoSymbolFile file)
		{
			this.DefineMethod(file, this.method.Token);
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x00037168 File Offset: 0x00035368
		public void DefineMethod(MonoSymbolFile file, int token)
		{
			CodeBlockEntry[] array = this.Blocks;
			if (array.Length != 0)
			{
				List<CodeBlockEntry> list = new List<CodeBlockEntry>(array.Length);
				int num = 0;
				for (int i = 0; i < array.Length; i++)
				{
					num = Math.Max(num, array[i].Index);
				}
				for (int j = 0; j < num; j++)
				{
					int num2 = j + 1;
					if (j < array.Length && array[j].Index == num2)
					{
						list.Add(array[j]);
					}
					else
					{
						bool flag = false;
						for (int k = 0; k < array.Length; k++)
						{
							if (array[k].Index == num2)
							{
								list.Add(array[k]);
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							list.Add(new CodeBlockEntry(num2, -1, CodeBlockEntry.Type.CompilerGenerated, 0));
						}
					}
				}
				array = list.ToArray();
			}
			MethodEntry methodEntry = new MethodEntry(file, this._comp_unit.Entry, token, this.ScopeVariables, this.Locals, this.method_lines.ToArray(), array, null, MethodEntry.Flags.ColumnsInfoIncluded, this.ns_id);
			file.AddMethod(methodEntry);
		}

		// Token: 0x040009EE RID: 2542
		private List<LocalVariableEntry> _locals;

		// Token: 0x040009EF RID: 2543
		private List<CodeBlockEntry> _blocks;

		// Token: 0x040009F0 RID: 2544
		private List<ScopeVariable> _scope_vars;

		// Token: 0x040009F1 RID: 2545
		private Stack<CodeBlockEntry> _block_stack;

		// Token: 0x040009F2 RID: 2546
		private readonly List<LineNumberEntry> method_lines;

		// Token: 0x040009F3 RID: 2547
		private readonly ICompileUnit _comp_unit;

		// Token: 0x040009F4 RID: 2548
		private readonly int ns_id;

		// Token: 0x040009F5 RID: 2549
		private readonly IMethodDef method;
	}
}
