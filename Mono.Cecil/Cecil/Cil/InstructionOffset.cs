using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000118 RID: 280
	public struct InstructionOffset
	{
		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x00023C4C File Offset: 0x00021E4C
		public int Offset
		{
			get
			{
				if (this.instruction != null)
				{
					return this.instruction.Offset;
				}
				if (this.offset != null)
				{
					return this.offset.Value;
				}
				throw new NotSupportedException();
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x00023C94 File Offset: 0x00021E94
		public bool IsEndOfMethod
		{
			get
			{
				return this.instruction == null && this.offset == null;
			}
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00023CBC File Offset: 0x00021EBC
		public InstructionOffset(Instruction instruction)
		{
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			this.instruction = instruction;
			this.offset = null;
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00023CDF File Offset: 0x00021EDF
		public InstructionOffset(int offset)
		{
			this.instruction = null;
			this.offset = new int?(offset);
		}

		// Token: 0x040006A4 RID: 1700
		private readonly Instruction instruction;

		// Token: 0x040006A5 RID: 1701
		private readonly int? offset;
	}
}
