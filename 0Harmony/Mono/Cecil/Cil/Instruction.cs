using System;
using System.Text;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001C2 RID: 450
	internal sealed class Instruction
	{
		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000E1B RID: 3611 RVA: 0x00030410 File Offset: 0x0002E610
		// (set) Token: 0x06000E1C RID: 3612 RVA: 0x00030418 File Offset: 0x0002E618
		public int Offset
		{
			get
			{
				return this.offset;
			}
			set
			{
				this.offset = value;
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000E1D RID: 3613 RVA: 0x00030421 File Offset: 0x0002E621
		// (set) Token: 0x06000E1E RID: 3614 RVA: 0x00030429 File Offset: 0x0002E629
		public OpCode OpCode
		{
			get
			{
				return this.opcode;
			}
			set
			{
				this.opcode = value;
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000E1F RID: 3615 RVA: 0x00030432 File Offset: 0x0002E632
		// (set) Token: 0x06000E20 RID: 3616 RVA: 0x0003043A File Offset: 0x0002E63A
		public object Operand
		{
			get
			{
				return this.operand;
			}
			set
			{
				this.operand = value;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000E21 RID: 3617 RVA: 0x00030443 File Offset: 0x0002E643
		// (set) Token: 0x06000E22 RID: 3618 RVA: 0x0003044B File Offset: 0x0002E64B
		public Instruction Previous
		{
			get
			{
				return this.previous;
			}
			set
			{
				this.previous = value;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000E23 RID: 3619 RVA: 0x00030454 File Offset: 0x0002E654
		// (set) Token: 0x06000E24 RID: 3620 RVA: 0x0003045C File Offset: 0x0002E65C
		public Instruction Next
		{
			get
			{
				return this.next;
			}
			set
			{
				this.next = value;
			}
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x00030465 File Offset: 0x0002E665
		internal Instruction(int offset, OpCode opCode)
		{
			this.offset = offset;
			this.opcode = opCode;
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0003047B File Offset: 0x0002E67B
		internal Instruction(OpCode opcode, object operand)
		{
			this.opcode = opcode;
			this.operand = operand;
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x00030494 File Offset: 0x0002E694
		public int GetSize()
		{
			int size = this.opcode.Size;
			switch (this.opcode.OperandType)
			{
			case OperandType.InlineBrTarget:
			case OperandType.InlineField:
			case OperandType.InlineI:
			case OperandType.InlineMethod:
			case OperandType.InlineSig:
			case OperandType.InlineString:
			case OperandType.InlineTok:
			case OperandType.InlineType:
			case OperandType.ShortInlineR:
				return size + 4;
			case OperandType.InlineI8:
			case OperandType.InlineR:
				return size + 8;
			case OperandType.InlineSwitch:
				return size + (1 + ((Instruction[])this.operand).Length) * 4;
			case OperandType.InlineVar:
			case OperandType.InlineArg:
				return size + 2;
			case OperandType.ShortInlineBrTarget:
			case OperandType.ShortInlineI:
			case OperandType.ShortInlineVar:
			case OperandType.ShortInlineArg:
				return size + 1;
			}
			return size;
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x00030538 File Offset: 0x0002E738
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Instruction.AppendLabel(stringBuilder, this);
			stringBuilder.Append(':');
			stringBuilder.Append(' ');
			stringBuilder.Append(this.opcode.Name);
			if (this.operand == null)
			{
				return stringBuilder.ToString();
			}
			stringBuilder.Append(' ');
			OperandType operandType = this.opcode.OperandType;
			if (operandType <= OperandType.InlineString)
			{
				if (operandType != OperandType.InlineBrTarget)
				{
					if (operandType != OperandType.InlineString)
					{
						goto IL_00D4;
					}
					stringBuilder.Append('"');
					stringBuilder.Append(this.operand);
					stringBuilder.Append('"');
					goto IL_00E1;
				}
			}
			else
			{
				if (operandType == OperandType.InlineSwitch)
				{
					Instruction[] array = (Instruction[])this.operand;
					for (int i = 0; i < array.Length; i++)
					{
						if (i > 0)
						{
							stringBuilder.Append(',');
						}
						Instruction.AppendLabel(stringBuilder, array[i]);
					}
					goto IL_00E1;
				}
				if (operandType != OperandType.ShortInlineBrTarget)
				{
					goto IL_00D4;
				}
			}
			Instruction.AppendLabel(stringBuilder, (Instruction)this.operand);
			goto IL_00E1;
			IL_00D4:
			stringBuilder.Append(this.operand);
			IL_00E1:
			return stringBuilder.ToString();
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x0003062C File Offset: 0x0002E82C
		private static void AppendLabel(StringBuilder builder, Instruction instruction)
		{
			builder.Append("IL_");
			builder.Append(instruction.offset.ToString("x4"));
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00030651 File Offset: 0x0002E851
		public static Instruction Create(OpCode opcode)
		{
			if (opcode.OperandType != OperandType.InlineNone)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, null);
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x0003066F File Offset: 0x0002E86F
		public static Instruction Create(OpCode opcode, TypeReference type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (opcode.OperandType != OperandType.InlineType && opcode.OperandType != OperandType.InlineTok)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, type);
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x000306A7 File Offset: 0x0002E8A7
		public static Instruction Create(OpCode opcode, CallSite site)
		{
			if (site == null)
			{
				throw new ArgumentNullException("site");
			}
			if (opcode.Code != Code.Calli)
			{
				throw new ArgumentException("code");
			}
			return new Instruction(opcode, site);
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x000306D4 File Offset: 0x0002E8D4
		public static Instruction Create(OpCode opcode, MethodReference method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			if (opcode.OperandType != OperandType.InlineMethod && opcode.OperandType != OperandType.InlineTok)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, method);
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x0003070B File Offset: 0x0002E90B
		public static Instruction Create(OpCode opcode, FieldReference field)
		{
			if (field == null)
			{
				throw new ArgumentNullException("field");
			}
			if (opcode.OperandType != OperandType.InlineField && opcode.OperandType != OperandType.InlineTok)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, field);
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x00030742 File Offset: 0x0002E942
		public static Instruction Create(OpCode opcode, string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (opcode.OperandType != OperandType.InlineString)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x0003076F File Offset: 0x0002E96F
		public static Instruction Create(OpCode opcode, sbyte value)
		{
			if (opcode.OperandType != OperandType.ShortInlineI && opcode != OpCodes.Ldc_I4_S)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x000307A0 File Offset: 0x0002E9A0
		public static Instruction Create(OpCode opcode, byte value)
		{
			if (opcode.OperandType != OperandType.ShortInlineI || opcode == OpCodes.Ldc_I4_S)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x000307D1 File Offset: 0x0002E9D1
		public static Instruction Create(OpCode opcode, int value)
		{
			if (opcode.OperandType != OperandType.InlineI)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x000307F4 File Offset: 0x0002E9F4
		public static Instruction Create(OpCode opcode, long value)
		{
			if (opcode.OperandType != OperandType.InlineI8)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00030817 File Offset: 0x0002EA17
		public static Instruction Create(OpCode opcode, float value)
		{
			if (opcode.OperandType != OperandType.ShortInlineR)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x0003083B File Offset: 0x0002EA3B
		public static Instruction Create(OpCode opcode, double value)
		{
			if (opcode.OperandType != OperandType.InlineR)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x0003085E File Offset: 0x0002EA5E
		public static Instruction Create(OpCode opcode, Instruction target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			if (opcode.OperandType != OperandType.InlineBrTarget && opcode.OperandType != OperandType.ShortInlineBrTarget)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, target);
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x00030894 File Offset: 0x0002EA94
		public static Instruction Create(OpCode opcode, Instruction[] targets)
		{
			if (targets == null)
			{
				throw new ArgumentNullException("targets");
			}
			if (opcode.OperandType != OperandType.InlineSwitch)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, targets);
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x000308C1 File Offset: 0x0002EAC1
		public static Instruction Create(OpCode opcode, VariableDefinition variable)
		{
			if (variable == null)
			{
				throw new ArgumentNullException("variable");
			}
			if (opcode.OperandType != OperandType.ShortInlineVar && opcode.OperandType != OperandType.InlineVar)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, variable);
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x000308F9 File Offset: 0x0002EAF9
		public static Instruction Create(OpCode opcode, ParameterDefinition parameter)
		{
			if (parameter == null)
			{
				throw new ArgumentNullException("parameter");
			}
			if (opcode.OperandType != OperandType.ShortInlineArg && opcode.OperandType != OperandType.InlineArg)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, parameter);
		}

		// Token: 0x0400079C RID: 1948
		internal int offset;

		// Token: 0x0400079D RID: 1949
		internal OpCode opcode;

		// Token: 0x0400079E RID: 1950
		internal object operand;

		// Token: 0x0400079F RID: 1951
		internal Instruction previous;

		// Token: 0x040007A0 RID: 1952
		internal Instruction next;
	}
}
