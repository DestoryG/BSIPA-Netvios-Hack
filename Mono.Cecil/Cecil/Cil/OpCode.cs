using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000106 RID: 262
	public struct OpCode : IEquatable<OpCode>
	{
		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x00021B05 File Offset: 0x0001FD05
		public string Name
		{
			get
			{
				return OpCodeNames.names[(int)this.Code];
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x00021B13 File Offset: 0x0001FD13
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

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x00021B25 File Offset: 0x0001FD25
		public byte Op1
		{
			get
			{
				return this.op1;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x00021B2D File Offset: 0x0001FD2D
		public byte Op2
		{
			get
			{
				return this.op2;
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x00021B35 File Offset: 0x0001FD35
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

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x00021B5B File Offset: 0x0001FD5B
		public Code Code
		{
			get
			{
				return (Code)this.code;
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x00021B63 File Offset: 0x0001FD63
		public FlowControl FlowControl
		{
			get
			{
				return (FlowControl)this.flow_control;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x00021B6B File Offset: 0x0001FD6B
		public OpCodeType OpCodeType
		{
			get
			{
				return (OpCodeType)this.opcode_type;
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x00021B73 File Offset: 0x0001FD73
		public OperandType OperandType
		{
			get
			{
				return (OperandType)this.operand_type;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x00021B7B File Offset: 0x0001FD7B
		public StackBehaviour StackBehaviourPop
		{
			get
			{
				return (StackBehaviour)this.stack_behavior_pop;
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x00021B83 File Offset: 0x0001FD83
		public StackBehaviour StackBehaviourPush
		{
			get
			{
				return (StackBehaviour)this.stack_behavior_push;
			}
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00021B8C File Offset: 0x0001FD8C
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

		// Token: 0x06000A7D RID: 2685 RVA: 0x00021C53 File Offset: 0x0001FE53
		public override int GetHashCode()
		{
			return (int)this.Value;
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00021C5C File Offset: 0x0001FE5C
		public override bool Equals(object obj)
		{
			if (!(obj is OpCode))
			{
				return false;
			}
			OpCode opCode = (OpCode)obj;
			return this.op1 == opCode.op1 && this.op2 == opCode.op2;
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x00021C98 File Offset: 0x0001FE98
		public bool Equals(OpCode opcode)
		{
			return this.op1 == opcode.op1 && this.op2 == opcode.op2;
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00021C98 File Offset: 0x0001FE98
		public static bool operator ==(OpCode one, OpCode other)
		{
			return one.op1 == other.op1 && one.op2 == other.op2;
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x00021CB8 File Offset: 0x0001FEB8
		public static bool operator !=(OpCode one, OpCode other)
		{
			return one.op1 != other.op1 || one.op2 != other.op2;
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x00021CDB File Offset: 0x0001FEDB
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x04000590 RID: 1424
		private readonly byte op1;

		// Token: 0x04000591 RID: 1425
		private readonly byte op2;

		// Token: 0x04000592 RID: 1426
		private readonly byte code;

		// Token: 0x04000593 RID: 1427
		private readonly byte flow_control;

		// Token: 0x04000594 RID: 1428
		private readonly byte opcode_type;

		// Token: 0x04000595 RID: 1429
		private readonly byte operand_type;

		// Token: 0x04000596 RID: 1430
		private readonly byte stack_behavior_pop;

		// Token: 0x04000597 RID: 1431
		private readonly byte stack_behavior_push;
	}
}
