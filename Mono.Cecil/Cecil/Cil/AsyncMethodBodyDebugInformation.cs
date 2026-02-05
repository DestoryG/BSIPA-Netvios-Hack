using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000125 RID: 293
	public sealed class AsyncMethodBodyDebugInformation : CustomDebugInformation
	{
		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000B1A RID: 2842 RVA: 0x00023FFE File Offset: 0x000221FE
		// (set) Token: 0x06000B1B RID: 2843 RVA: 0x00024006 File Offset: 0x00022206
		public InstructionOffset CatchHandler
		{
			get
			{
				return this.catch_handler;
			}
			set
			{
				this.catch_handler = value;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000B1C RID: 2844 RVA: 0x00024010 File Offset: 0x00022210
		public Collection<InstructionOffset> Yields
		{
			get
			{
				Collection<InstructionOffset> collection;
				if ((collection = this.yields) == null)
				{
					collection = (this.yields = new Collection<InstructionOffset>());
				}
				return collection;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x00024038 File Offset: 0x00022238
		public Collection<InstructionOffset> Resumes
		{
			get
			{
				Collection<InstructionOffset> collection;
				if ((collection = this.resumes) == null)
				{
					collection = (this.resumes = new Collection<InstructionOffset>());
				}
				return collection;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000B1E RID: 2846 RVA: 0x00024060 File Offset: 0x00022260
		public Collection<MethodDefinition> ResumeMethods
		{
			get
			{
				Collection<MethodDefinition> collection;
				if ((collection = this.resume_methods) == null)
				{
					collection = (this.resume_methods = new Collection<MethodDefinition>());
				}
				return collection;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000B1F RID: 2847 RVA: 0x00024085 File Offset: 0x00022285
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.AsyncMethodBody;
			}
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x00024088 File Offset: 0x00022288
		internal AsyncMethodBodyDebugInformation(int catchHandler)
			: base(AsyncMethodBodyDebugInformation.KindIdentifier)
		{
			this.catch_handler = new InstructionOffset(catchHandler);
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x000240A1 File Offset: 0x000222A1
		public AsyncMethodBodyDebugInformation(Instruction catchHandler)
			: base(AsyncMethodBodyDebugInformation.KindIdentifier)
		{
			this.catch_handler = new InstructionOffset(catchHandler);
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x000240BA File Offset: 0x000222BA
		public AsyncMethodBodyDebugInformation()
			: base(AsyncMethodBodyDebugInformation.KindIdentifier)
		{
			this.catch_handler = new InstructionOffset(-1);
		}

		// Token: 0x040006CE RID: 1742
		internal InstructionOffset catch_handler;

		// Token: 0x040006CF RID: 1743
		internal Collection<InstructionOffset> yields;

		// Token: 0x040006D0 RID: 1744
		internal Collection<InstructionOffset> resumes;

		// Token: 0x040006D1 RID: 1745
		internal Collection<MethodDefinition> resume_methods;

		// Token: 0x040006D2 RID: 1746
		public static Guid KindIdentifier = new Guid("{54FD2AC5-E925-401A-9C2A-F94F171072F8}");
	}
}
