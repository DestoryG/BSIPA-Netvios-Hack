using System;
using System.Collections.Generic;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000017 RID: 23
	public class SourceMethodBuilder
	{
		// Token: 0x060000AB RID: 171 RVA: 0x0000519C File Offset: 0x0000339C
		public SourceMethodBuilder(ICompileUnit comp_unit)
		{
			this._comp_unit = comp_unit;
			this.method_lines = new List<LineNumberEntry>();
		}

		// Token: 0x060000AC RID: 172 RVA: 0x000051B6 File Offset: 0x000033B6
		public SourceMethodBuilder(ICompileUnit comp_unit, int ns_id, IMethodDef method)
			: this(comp_unit)
		{
			this.ns_id = ns_id;
			this.method = method;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000051CD File Offset: 0x000033CD
		public void MarkSequencePoint(int offset, SourceFileEntry file, int line, int column, bool is_hidden)
		{
			this.MarkSequencePoint(offset, file, line, column, -1, -1, is_hidden);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000051E0 File Offset: 0x000033E0
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

		// Token: 0x060000AF RID: 175 RVA: 0x0000526E File Offset: 0x0000346E
		public void StartBlock(CodeBlockEntry.Type type, int start_offset)
		{
			this.StartBlock(type, start_offset, (this._blocks == null) ? 1 : (this._blocks.Count + 1));
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00005290 File Offset: 0x00003490
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

		// Token: 0x060000B1 RID: 177 RVA: 0x000052FC File Offset: 0x000034FC
		public void EndBlock(int end_offset)
		{
			this._block_stack.Pop().Close(end_offset);
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00005310 File Offset: 0x00003510
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

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x0000534B File Offset: 0x0000354B
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

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00005370 File Offset: 0x00003570
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

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x0000538C File Offset: 0x0000358C
		public ICompileUnit SourceFile
		{
			get
			{
				return this._comp_unit;
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00005394 File Offset: 0x00003594
		public void AddLocal(int index, string name)
		{
			if (this._locals == null)
			{
				this._locals = new List<LocalVariableEntry>();
			}
			int num = ((this.CurrentBlock != null) ? this.CurrentBlock.Index : 0);
			this._locals.Add(new LocalVariableEntry(index, name, num));
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x000053DE File Offset: 0x000035DE
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

		// Token: 0x060000B8 RID: 184 RVA: 0x000053FA File Offset: 0x000035FA
		public void AddScopeVariable(int scope, int index)
		{
			if (this._scope_vars == null)
			{
				this._scope_vars = new List<ScopeVariable>();
			}
			this._scope_vars.Add(new ScopeVariable(scope, index));
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005421 File Offset: 0x00003621
		public void DefineMethod(MonoSymbolFile file)
		{
			this.DefineMethod(file, this.method.Token);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005438 File Offset: 0x00003638
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

		// Token: 0x04000088 RID: 136
		private List<LocalVariableEntry> _locals;

		// Token: 0x04000089 RID: 137
		private List<CodeBlockEntry> _blocks;

		// Token: 0x0400008A RID: 138
		private List<ScopeVariable> _scope_vars;

		// Token: 0x0400008B RID: 139
		private Stack<CodeBlockEntry> _block_stack;

		// Token: 0x0400008C RID: 140
		private readonly List<LineNumberEntry> method_lines;

		// Token: 0x0400008D RID: 141
		private readonly ICompileUnit _comp_unit;

		// Token: 0x0400008E RID: 142
		private readonly int ns_id;

		// Token: 0x0400008F RID: 143
		private readonly IMethodDef method;
	}
}
