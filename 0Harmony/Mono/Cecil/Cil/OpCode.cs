using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001CA RID: 458
	internal struct OpCode : IEquatable<OpCode>
	{
		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000E58 RID: 3672 RVA: 0x00030D11 File Offset: 0x0002EF11
		public string Name
		{
			get
			{
				return OpCodeNames.names[(int)this.Code];
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000E59 RID: 3673 RVA: 0x00030D1F File Offset: 0x0002EF1F
		public int Size
		{
			get
			{
				if (this.op1 != 255)
				{
					return 2;
				}
				return 1;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000E5A RID: 3674 RVA: 0x00030D31 File Offset: 0x0002EF31
		public byte Op1
		{
			get
			{
				return this.op1;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000E5B RID: 3675 RVA: 0x00030D39 File Offset: 0x0002EF39
		public byte Op2
		{
			get
			{
				return this.op2;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000E5C RID: 3676 RVA: 0x00030D41 File Offset: 0x0002EF41
		public short Value
		{
			get
			{
				if (this.op1 != 255)
				{
					return (short)(((int)this.op1 << 8) | (int)this.op2);
				}
				return (short)this.op2;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000E5D RID: 3677 RVA: 0x00030D67 File Offset: 0x0002EF67
		public Code Code
		{
			get
			{
				return (Code)this.code;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000E5E RID: 3678 RVA: 0x00030D6F File Offset: 0x0002EF6F
		public FlowControl FlowControl
		{
			get
			{
				return (FlowControl)this.flow_control;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000E5F RID: 3679 RVA: 0x00030D77 File Offset: 0x0002EF77
		public OpCodeType OpCodeType
		{
			get
			{
				return (OpCodeType)this.opcode_type;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000E60 RID: 3680 RVA: 0x00030D7F File Offset: 0x0002EF7F
		public OperandType OperandType
		{
			get
			{
				return (OperandType)this.operand_type;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000E61 RID: 3681 RVA: 0x00030D87 File Offset: 0x0002EF87
		public StackBehaviour StackBehaviourPop
		{
			get
			{
				return (StackBehaviour)this.stack_behavior_pop;
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000E62 RID: 3682 RVA: 0x00030D8F File Offset: 0x0002EF8F
		public StackBehaviour StackBehaviourPush
		{
			get
			{
				return (StackBehaviour)this.stack_behavior_push;
			}
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x00030D98 File Offset: 0x0002EF98
		internal OpCode(int x, int y)
		{
			this.op1 = (byte)(x & 255);
			this.op2 = (byte)((x >> 8) & 255);
			this.code = (byte)((x >> 16) & 255);
			this.flow_control = (byte)((x >> 24) & 255);
			this.opcode_type = (byte)(y & 255);
			this.operand_type = (byte)((y >> 8) & 255);
			this.stack_behavior_pop = (byte)((y >> 16) & 255);
			this.stack_behavior_push = (byte)((y >> 24) & 255);
			if (this.op1 == 255)
			{
				OpCodes.OneByteOpCode[(int)this.op2] = this;
				return;
			}
			OpCodes.TwoBytesOpCode[(int)this.op2] = this;
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x00030E5F File Offset: 0x0002F05F
		public override int GetHashCode()
		{
			return (int)this.Value;
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x00030E68 File Offset: 0x0002F068
		public override bool Equals(object obj)
		{
			if (!(obj is OpCode))
			{
				return false;
			}
			OpCode opCode = (OpCode)obj;
			return this.op1 == opCode.op1 && this.op2 == opCode.op2;
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x00030EA4 File Offset: 0x0002F0A4
		public bool Equals(OpCode opcode)
		{
			return this.op1 == opcode.op1 && this.op2 == opcode.op2;
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x00030EA4 File Offset: 0x0002F0A4
		public static bool operator ==(OpCode one, OpCode other)
		{
			return one.op1 == other.op1 && one.op2 == other.op2;
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x00030EC4 File Offset: 0x0002F0C4
		public static bool operator !=(OpCode one, OpCode other)
		{
			return one.op1 != other.op1 || one.op2 != other.op2;
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x00030EE7 File Offset: 0x0002F0E7
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x040007EF RID: 2031
		private readonly byte op1;

		// Token: 0x040007F0 RID: 2032
		private readonly byte op2;

		// Token: 0x040007F1 RID: 2033
		private readonly byte code;

		// Token: 0x040007F2 RID: 2034
		private readonly byte flow_control;

		// Token: 0x040007F3 RID: 2035
		private readonly byte opcode_type;

		// Token: 0x040007F4 RID: 2036
		private readonly byte operand_type;

		// Token: 0x040007F5 RID: 2037
		private readonly byte stack_behavior_pop;

		// Token: 0x040007F6 RID: 2038
		private readonly byte stack_behavior_push;
	}
}
