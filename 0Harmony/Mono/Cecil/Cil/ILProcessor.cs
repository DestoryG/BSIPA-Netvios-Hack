using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001C1 RID: 449
	internal sealed class ILProcessor
	{
		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x00030023 File Offset: 0x0002E223
		public MethodBody Body
		{
			get
			{
				return this.body;
			}
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0003002B File Offset: 0x0002E22B
		internal ILProcessor(MethodBody body)
		{
			this.body = body;
			this.instructions = body.Instructions;
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x00030046 File Offset: 0x0002E246
		public Instruction Create(OpCode opcode)
		{
			return Instruction.Create(opcode);
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x0003004E File Offset: 0x0002E24E
		public Instruction Create(OpCode opcode, TypeReference type)
		{
			return Instruction.Create(opcode, type);
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00030057 File Offset: 0x0002E257
		public Instruction Create(OpCode opcode, CallSite site)
		{
			return Instruction.Create(opcode, site);
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00030060 File Offset: 0x0002E260
		public Instruction Create(OpCode opcode, MethodReference method)
		{
			return Instruction.Create(opcode, method);
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00030069 File Offset: 0x0002E269
		public Instruction Create(OpCode opcode, FieldReference field)
		{
			return Instruction.Create(opcode, field);
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x00030072 File Offset: 0x0002E272
		public Instruction Create(OpCode opcode, string value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0003007B File Offset: 0x0002E27B
		public Instruction Create(OpCode opcode, sbyte value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00030084 File Offset: 0x0002E284
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

		// Token: 0x06000DFB RID: 3579 RVA: 0x000300DC File Offset: 0x0002E2DC
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

		// Token: 0x06000DFC RID: 3580 RVA: 0x00030131 File Offset: 0x0002E331
		public Instruction Create(OpCode opcode, long value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0003013A File Offset: 0x0002E33A
		public Instruction Create(OpCode opcode, float value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x00030143 File Offset: 0x0002E343
		public Instruction Create(OpCode opcode, double value)
		{
			return Instruction.Create(opcode, value);
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0003014C File Offset: 0x0002E34C
		public Instruction Create(OpCode opcode, Instruction target)
		{
			return Instruction.Create(opcode, target);
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00030155 File Offset: 0x0002E355
		public Instruction Create(OpCode opcode, Instruction[] targets)
		{
			return Instruction.Create(opcode, targets);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0003015E File Offset: 0x0002E35E
		public Instruction Create(OpCode opcode, VariableDefinition variable)
		{
			return Instruction.Create(opcode, variable);
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x00030167 File Offset: 0x0002E367
		public Instruction Create(OpCode opcode, ParameterDefinition parameter)
		{
			return Instruction.Create(opcode, parameter);
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x00030170 File Offset: 0x0002E370
		public void Emit(OpCode opcode)
		{
			this.Append(this.Create(opcode));
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0003017F File Offset: 0x0002E37F
		public void Emit(OpCode opcode, TypeReference type)
		{
			this.Append(this.Create(opcode, type));
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0003018F File Offset: 0x0002E38F
		public void Emit(OpCode opcode, MethodReference method)
		{
			this.Append(this.Create(opcode, method));
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x0003019F File Offset: 0x0002E39F
		public void Emit(OpCode opcode, CallSite site)
		{
			this.Append(this.Create(opcode, site));
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x000301AF File Offset: 0x0002E3AF
		public void Emit(OpCode opcode, FieldReference field)
		{
			this.Append(this.Create(opcode, field));
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x000301BF File Offset: 0x0002E3BF
		public void Emit(OpCode opcode, string value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x000301CF File Offset: 0x0002E3CF
		public void Emit(OpCode opcode, byte value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x000301DF File Offset: 0x0002E3DF
		public void Emit(OpCode opcode, sbyte value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x000301EF File Offset: 0x0002E3EF
		public void Emit(OpCode opcode, int value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x000301FF File Offset: 0x0002E3FF
		public void Emit(OpCode opcode, long value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x0003020F File Offset: 0x0002E40F
		public void Emit(OpCode opcode, float value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x0003021F File Offset: 0x0002E41F
		public void Emit(OpCode opcode, double value)
		{
			this.Append(this.Create(opcode, value));
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x0003022F File Offset: 0x0002E42F
		public void Emit(OpCode opcode, Instruction target)
		{
			this.Append(this.Create(opcode, target));
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x0003023F File Offset: 0x0002E43F
		public void Emit(OpCode opcode, Instruction[] targets)
		{
			this.Append(this.Create(opcode, targets));
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0003024F File Offset: 0x0002E44F
		public void Emit(OpCode opcode, VariableDefinition variable)
		{
			this.Append(this.Create(opcode, variable));
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x0003025F File Offset: 0x0002E45F
		public void Emit(OpCode opcode, ParameterDefinition parameter)
		{
			this.Append(this.Create(opcode, parameter));
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x00030270 File Offset: 0x0002E470
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

		// Token: 0x06000E14 RID: 3604 RVA: 0x000302C4 File Offset: 0x0002E4C4
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

		// Token: 0x06000E15 RID: 3605 RVA: 0x00030318 File Offset: 0x0002E518
		public void InsertAfter(int index, Instruction instruction)
		{
			if (index < 0 || index >= this.instructions.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			this.instructions.Insert(index + 1, instruction);
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x00030354 File Offset: 0x0002E554
		public void Append(Instruction instruction)
		{
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			this.instructions.Add(instruction);
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00030370 File Offset: 0x0002E570
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

		// Token: 0x06000E18 RID: 3608 RVA: 0x0003039D File Offset: 0x0002E59D
		public void Replace(int index, Instruction instruction)
		{
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			this.InsertAfter(index, instruction);
			this.RemoveAt(index);
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x000303BC File Offset: 0x0002E5BC
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

		// Token: 0x06000E1A RID: 3610 RVA: 0x000303E5 File Offset: 0x0002E5E5
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.instructions.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.instructions.RemoveAt(index);
		}

		// Token: 0x0400079A RID: 1946
		private readonly MethodBody body;

		// Token: 0x0400079B RID: 1947
		private readonly Collection<Instruction> instructions;
	}
}
