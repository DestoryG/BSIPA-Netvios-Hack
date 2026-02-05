using System;
using System.Text;

namespace Mono.Cecil.Cil
{
	// Token: 0x020000FE RID: 254
	public sealed class Instruction
	{
		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000A34 RID: 2612 RVA: 0x000211FE File Offset: 0x0001F3FE
		// (set) Token: 0x06000A35 RID: 2613 RVA: 0x00021206 File Offset: 0x0001F406
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

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000A36 RID: 2614 RVA: 0x0002120F File Offset: 0x0001F40F
		// (set) Token: 0x06000A37 RID: 2615 RVA: 0x00021217 File Offset: 0x0001F417
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

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000A38 RID: 2616 RVA: 0x00021220 File Offset: 0x0001F420
		// (set) Token: 0x06000A39 RID: 2617 RVA: 0x00021228 File Offset: 0x0001F428
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

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x00021231 File Offset: 0x0001F431
		// (set) Token: 0x06000A3B RID: 2619 RVA: 0x00021239 File Offset: 0x0001F439
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

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x00021242 File Offset: 0x0001F442
		// (set) Token: 0x06000A3D RID: 2621 RVA: 0x0002124A File Offset: 0x0001F44A
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

		// Token: 0x06000A3E RID: 2622 RVA: 0x00021253 File Offset: 0x0001F453
		internal Instruction(int offset, OpCode opCode)
		{
			this.offset = offset;
			this.opcode = opCode;
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x00021269 File Offset: 0x0001F469
		internal Instruction(OpCode opcode, object operand)
		{
			this.opcode = opcode;
			this.operand = operand;
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00021280 File Offset: 0x0001F480
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

		// Token: 0x06000A41 RID: 2625 RVA: 0x00021324 File Offset: 0x0001F524
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

		// Token: 0x06000A42 RID: 2626 RVA: 0x00021418 File Offset: 0x0001F618
		private static void AppendLabel(StringBuilder builder, Instruction instruction)
		{
			builder.Append("IL_");
			builder.Append(instruction.offset.ToString("x4"));
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0002143D File Offset: 0x0001F63D
		public static Instruction Create(OpCode opcode)
		{
			if (opcode.OperandType != OperandType.InlineNone)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, null);
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0002145B File Offset: 0x0001F65B
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

		// Token: 0x06000A45 RID: 2629 RVA: 0x00021493 File Offset: 0x0001F693
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

		// Token: 0x06000A46 RID: 2630 RVA: 0x000214C0 File Offset: 0x0001F6C0
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

		// Token: 0x06000A47 RID: 2631 RVA: 0x000214F7 File Offset: 0x0001F6F7
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

		// Token: 0x06000A48 RID: 2632 RVA: 0x0002152E File Offset: 0x0001F72E
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

		// Token: 0x06000A49 RID: 2633 RVA: 0x0002155B File Offset: 0x0001F75B
		public static Instruction Create(OpCode opcode, sbyte value)
		{
			if (opcode.OperandType != OperandType.ShortInlineI && opcode != OpCodes.Ldc_I4_S)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0002158C File Offset: 0x0001F78C
		public static Instruction Create(OpCode opcode, byte value)
		{
			if (opcode.OperandType != OperandType.ShortInlineI || opcode == OpCodes.Ldc_I4_S)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x000215BD File Offset: 0x0001F7BD
		public static Instruction Create(OpCode opcode, int value)
		{
			if (opcode.OperandType != OperandType.InlineI)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x000215E0 File Offset: 0x0001F7E0
		public static Instruction Create(OpCode opcode, long value)
		{
			if (opcode.OperandType != OperandType.InlineI8)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x00021603 File Offset: 0x0001F803
		public static Instruction Create(OpCode opcode, float value)
		{
			if (opcode.OperandType != OperandType.ShortInlineR)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x00021627 File Offset: 0x0001F827
		public static Instruction Create(OpCode opcode, double value)
		{
			if (opcode.OperandType != OperandType.InlineR)
			{
				throw new ArgumentException("opcode");
			}
			return new Instruction(opcode, value);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0002164A File Offset: 0x0001F84A
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

		// Token: 0x06000A50 RID: 2640 RVA: 0x00021680 File Offset: 0x0001F880
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

		// Token: 0x06000A51 RID: 2641 RVA: 0x000216AD File Offset: 0x0001F8AD
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

		// Token: 0x06000A52 RID: 2642 RVA: 0x000216E5 File Offset: 0x0001F8E5
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

		// Token: 0x0400053D RID: 1341
		internal int offset;

		// Token: 0x0400053E RID: 1342
		internal OpCode opcode;

		// Token: 0x0400053F RID: 1343
		internal object operand;

		// Token: 0x04000540 RID: 1344
		internal Instruction previous;

		// Token: 0x04000541 RID: 1345
		internal Instruction next;
	}
}
