using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001EA RID: 490
	internal sealed class StateMachineScope
	{
		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000F0B RID: 3851 RVA: 0x00033294 File Offset: 0x00031494
		// (set) Token: 0x06000F0C RID: 3852 RVA: 0x0003329C File Offset: 0x0003149C
		public InstructionOffset Start
		{
			get
			{
				return this.start;
			}
			set
			{
				this.start = value;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000F0D RID: 3853 RVA: 0x000332A5 File Offset: 0x000314A5
		// (set) Token: 0x06000F0E RID: 3854 RVA: 0x000332AD File Offset: 0x000314AD
		public InstructionOffset End
		{
			get
			{
				return this.end;
			}
			set
			{
				this.end = value;
			}
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x000332B6 File Offset: 0x000314B6
		internal StateMachineScope(int start, int end)
		{
			this.start = new InstructionOffset(start);
			this.end = new InstructionOffset(end);
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x000332D8 File Offset: 0x000314D8
		public StateMachineScope(Instruction start, Instruction end)
		{
			this.start = new InstructionOffset(start);
			this.end = ((end != null) ? new InstructionOffset(end) : default(InstructionOffset));
		}

		// Token: 0x04000932 RID: 2354
		internal InstructionOffset start;

		// Token: 0x04000933 RID: 2355
		internal InstructionOffset end;
	}
}
