using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000126 RID: 294
	public sealed class StateMachineScope
	{
		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000B24 RID: 2852 RVA: 0x000240E4 File Offset: 0x000222E4
		// (set) Token: 0x06000B25 RID: 2853 RVA: 0x000240EC File Offset: 0x000222EC
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

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000B26 RID: 2854 RVA: 0x000240F5 File Offset: 0x000222F5
		// (set) Token: 0x06000B27 RID: 2855 RVA: 0x000240FD File Offset: 0x000222FD
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

		// Token: 0x06000B28 RID: 2856 RVA: 0x00024106 File Offset: 0x00022306
		internal StateMachineScope(int start, int end)
		{
			this.start = new InstructionOffset(start);
			this.end = new InstructionOffset(end);
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x00024128 File Offset: 0x00022328
		public StateMachineScope(Instruction start, Instruction end)
		{
			this.start = new InstructionOffset(start);
			this.end = ((end != null) ? new InstructionOffset(end) : default(InstructionOffset));
		}

		// Token: 0x040006D3 RID: 1747
		internal InstructionOffset start;

		// Token: 0x040006D4 RID: 1748
		internal InstructionOffset end;
	}
}
