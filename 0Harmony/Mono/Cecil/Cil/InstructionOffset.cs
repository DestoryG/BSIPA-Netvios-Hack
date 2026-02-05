using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001DC RID: 476
	internal struct InstructionOffset
	{
		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000ECC RID: 3788 RVA: 0x00032E44 File Offset: 0x00031044
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

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000ECD RID: 3789 RVA: 0x00032E78 File Offset: 0x00031078
		public bool IsEndOfMethod
		{
			get
			{
				return this.instruction == null && this.offset == null;
			}
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x00032E92 File Offset: 0x00031092
		public InstructionOffset(Instruction instruction)
		{
			if (instruction == null)
			{
				throw new ArgumentNullException("instruction");
			}
			this.instruction = instruction;
			this.offset = null;
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x00032EB5 File Offset: 0x000310B5
		public InstructionOffset(int offset)
		{
			this.instruction = null;
			this.offset = new int?(offset);
		}

		// Token: 0x04000903 RID: 2307
		private readonly Instruction instruction;

		// Token: 0x04000904 RID: 2308
		private readonly int? offset;
	}
}
