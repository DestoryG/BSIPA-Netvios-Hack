using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020000FD RID: 253
	public sealed class ILProcessor
	{
		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000A0D RID: 2573 RVA: 0x00020E97 File Offset: 0x0001F097
		public MethodBody Body
		{
			get
			{
				return this.body;
			}
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x00020E9F File Offset: 0x0001F09F
		internal ILProcessor(MethodBody body)
		{
			this.body = body;
			this.instructions = body.Instructions;
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x00020EBA File Offset: 0x0001F0BA
		public Instruction Create(OpCode opcode)
		{
			return Instruction.Create(opcode);
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x00020EC2 File Offset: 0x0001F0C2
		public Instruction Create(OpCode opcode, TypeReference type)
		{
			return Instruction.Create(opcode, type);
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x00020ECB File Offset: 0x0001F0CB
		public Instruction Create(OpCode opcode, CallSite site)
		{
			return Instruction.Create(opcode, site);
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x00020ED4 File Offset: 0x0001F0D4
		public Instruction Create(OpCode opcode, MethodReference method)
		{
			return Instruction.Create(opcode, method);
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x00020EDD File Offset: 0x0001F0DD
		public Instruction Create(OpCode opcode, FieldReference field)
		{
			return Instruction.Create(opcode, field);
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x00020EE6 File Offset: 0x0001F0E6
		public Instruction Create(OpCode opcode, string value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x00020EEF File Offset: 0x0001F0EF
		public Instruction Create(OpCode opcode, sbyte value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x00020EF8 File Offset: 0x0001F0F8
		public Instruction Create(OpCode opcode, byte value)
		{
			if (opcode.OperandType == OperandType.ShortInlineVar)
			{
				return Instruction.Create(opcode, this.body.Variables[(int)value]);
			}
			if (opcode.OperandType == OperandType.ShortInlineArg)
			{
				return Instruction.Create(opcode, this.body.GetParameter((int)value));
			}
			return Instruction.Create(opcode, value);
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x00020F50 File Offset: 0x0001F150
		public Instruction Create(OpCode opcode, int value)
		{
			if (opcode.OperandType == OperandType.InlineVar)
			{
				return Instruction.Create(opcode, this.body.Variables[value]);
			}
			if (opcode.OperandType == OperandType.InlineArg)
			{
				return Instruction.Create(opcode, this.body.GetParameter(value));
			}
			return Instruction.Create(opcode, value);
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x00020FA5 File Offset: 0x0001F1A5
		public Instruction Create(OpCode opcode, long value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x00020FAE File Offset: 0x0001F1AE
		public Instruction Create(OpCode opcode, float value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x00020FB7 File Offset: 0x0001F1B7
		public Instruction Create(OpCode opcode, double value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x00020FC0 File Offset: 0x0001F1C0
		public Instruction Create(OpCode opcode, Instruction target)
		{
			return Instruction.Create(opcode, target);
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x00020FC9 File Offset: 0x0001F1C9
		public Instruction Create(OpCode opcode, Instruction[] targets)
		{
			return Instruction.Create(opcode, targets);
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x00020FD2 File Offset: 0x0001F1D2
		public Instruction Create(OpCode opcode, VariableDefinition variable)
		{
			return Instruction.Create(opcode, variable);
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x00020FDB File Offset: 0x0001F1DB
		public Instruction Create(OpCode opcode, ParameterDefinition parameter)
		{
			return Instruction.Create(opcode, parameter);
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x00020FE4 File Offset: 0x0001F1E4
		public void Emit(OpCode opcode)
		{
			this.Append(this.Create(opcode));
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x00020FF3 File Offset: 0x0001F1F3
		public void Emit(OpCode opcode, TypeReference type)
		{
			this.Append(this.Create(opcode, type));
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x00021003 File Offset: 0x0001F203
		public void Emit(OpCode opcode, MethodReference method)
		{
			this.Append(this.Create(opcode, method));
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00021013 File Offset: 0x0001F213
		public void Emit(OpCode opcode, CallSite site)
		{
			this.Append(this.Create(opcode, site));
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00021023 File Offset: 0x0001F223
		public void Emit(OpCode opcode, FieldReference field)
		{
			this.Append(this.Create(opcode, field));
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x00021033 File Offset: 0x0001F233
		public void Emit(OpCode opcode, string value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x00021043 File Offset: 0x0001F243
		public void Emit(OpCode opcode, byte value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x00021053 File Offset: 0x0001F253
		public void Emit(OpCode opcode, sbyte value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x00021063 File Offset: 0x0001F263
		public void Emit(OpCode opcode, int value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x00021073 File Offset: 0x0001F273
		public void Emit(OpCode opcode, long value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x00021083 File Offset: 0x0001F283
		public void Emit(OpCode opcode, float value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x00021093 File Offset: 0x0001F293
		public void Emit(OpCode opcode, double value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x000210A3 File Offset: 0x0001F2A3
		public void Emit(OpCode opcode, Instruction target)
		{
			this.Append(this.Create(opcode, target));
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x000210B3 File Offset: 0x0001F2B3
		public void Emit(OpCode opcode, Instruction[] targets)
		{
			this.Append(this.Create(opcode, targets));
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x000210C3 File Offset: 0x0001F2C3
		public void Emit(OpCode opcode, VariableDefinition variable)
		{
			this.Append(this.Create(opcode, variable));
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x000210D3 File Offset: 0x0001F2D3
		public void Emit(OpCode opcode, ParameterDefinition parameter)
		{
			this.Append(this.Create(opcode, parameter));
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x000210E4 File Offset: 0x0001F2E4
		public void InsertBefore(Instruction target, Instruction instruction)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			int num = this.instructions.IndexOf(target);
			if (num == -1)
			{
				throw new ArgumentOutOfRangeException("target");
			}
			this.instructions.Insert(num, instruction);
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x00021138 File Offset: 0x0001F338
		public void InsertAfter(Instruction target, Instruction instruction)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			int num = this.instructions.IndexOf(target);
			if (num == -1)
			{
				throw new ArgumentOutOfRangeException("target");
			}
			this.instructions.Insert(num + 1, instruction);
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0002118C File Offset: 0x0001F38C
		public void Append(Instruction instruction)
		{
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			this.instructions.Add(instruction);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x000211A8 File Offset: 0x0001F3A8
		public void Replace(Instruction target, Instruction instruction)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			this.InsertAfter(target, instruction);
			this.Remove(target);
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x000211D5 File Offset: 0x0001F3D5
		public void Remove(Instruction instruction)
		{
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			if (!this.instructions.Remove(instruction))
			{
				throw new ArgumentOutOfRangeException("instruction");
			}
		}

		// Token: 0x0400053B RID: 1339
		private readonly MethodBody body;

		// Token: 0x0400053C RID: 1340
		private readonly Collection<Instruction> instructions;
	}
}
