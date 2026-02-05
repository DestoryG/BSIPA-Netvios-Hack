using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace HarmonyLib
{
	// Token: 0x02000058 RID: 88
	public class CodeInstruction
	{
		// Token: 0x0600017E RID: 382 RVA: 0x000098C8 File Offset: 0x00007AC8
		public CodeInstruction(OpCode opcode, object operand = null)
		{
			this.opcode = opcode;
			this.operand = operand;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x000098F4 File Offset: 0x00007AF4
		public CodeInstruction(CodeInstruction instruction)
		{
			this.opcode = instruction.opcode;
			this.operand = instruction.operand;
			this.labels = instruction.labels.ToList<Label>();
			this.blocks = instruction.blocks.ToList<ExceptionBlock>();
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00009957 File Offset: 0x00007B57
		public CodeInstruction Clone()
		{
			return new CodeInstruction(this)
			{
				labels = new List<Label>(),
				blocks = new List<ExceptionBlock>()
			};
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00009975 File Offset: 0x00007B75
		public CodeInstruction Clone(OpCode opcode)
		{
			CodeInstruction codeInstruction = this.Clone();
			codeInstruction.opcode = opcode;
			return codeInstruction;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00009984 File Offset: 0x00007B84
		public CodeInstruction Clone(object operand)
		{
			CodeInstruction codeInstruction = this.Clone();
			codeInstruction.operand = operand;
			return codeInstruction;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00009994 File Offset: 0x00007B94
		public override string ToString()
		{
			List<string> list = new List<string>();
			foreach (Label label in this.labels)
			{
				list.Add(string.Format("Label{0}", label.GetHashCode()));
			}
			foreach (ExceptionBlock exceptionBlock in this.blocks)
			{
				list.Add("EX_" + exceptionBlock.blockType.ToString().Replace("Block", ""));
			}
			string text = ((list.Count > 0) ? (" [" + string.Join(", ", list.ToArray()) + "]") : "");
			string text2 = Emitter.FormatArgument(this.operand, null);
			if (text2.Length > 0)
			{
				text2 = " " + text2;
			}
			OpCode opCode = this.opcode;
			return opCode.ToString() + text2 + text;
		}

		// Token: 0x040000F0 RID: 240
		public OpCode opcode;

		// Token: 0x040000F1 RID: 241
		public object operand;

		// Token: 0x040000F2 RID: 242
		public List<Label> labels = new List<Label>();

		// Token: 0x040000F3 RID: 243
		public List<ExceptionBlock> blocks = new List<ExceptionBlock>();
	}
}
