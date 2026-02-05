using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001E9 RID: 489
	internal sealed class AsyncMethodBodyDebugInformation : CustomDebugInformation
	{
		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000F01 RID: 3841 RVA: 0x000331BB File Offset: 0x000313BB
		// (set) Token: 0x06000F02 RID: 3842 RVA: 0x000331C3 File Offset: 0x000313C3
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

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000F03 RID: 3843 RVA: 0x000331CC File Offset: 0x000313CC
		public Collection<InstructionOffset> Yields
		{
			get
			{
				if (this.yields == null)
				{
					Interlocked.CompareExchange<Collection<InstructionOffset>>(ref this.yields, new Collection<InstructionOffset>(), null);
				}
				return this.yields;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000F04 RID: 3844 RVA: 0x000331EE File Offset: 0x000313EE
		public Collection<InstructionOffset> Resumes
		{
			get
			{
				if (this.resumes == null)
				{
					Interlocked.CompareExchange<Collection<InstructionOffset>>(ref this.resumes, new Collection<InstructionOffset>(), null);
				}
				return this.resumes;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000F05 RID: 3845 RVA: 0x00033210 File Offset: 0x00031410
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

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000F06 RID: 3846 RVA: 0x00033235 File Offset: 0x00031435
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.AsyncMethodBody;
			}
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x00033238 File Offset: 0x00031438
		internal AsyncMethodBodyDebugInformation(int catchHandler)
			: base(AsyncMethodBodyDebugInformation.KindIdentifier)
		{
			this.catch_handler = new InstructionOffset(catchHandler);
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x00033251 File Offset: 0x00031451
		public AsyncMethodBodyDebugInformation(Instruction catchHandler)
			: base(AsyncMethodBodyDebugInformation.KindIdentifier)
		{
			this.catch_handler = new InstructionOffset(catchHandler);
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0003326A File Offset: 0x0003146A
		public AsyncMethodBodyDebugInformation()
			: base(AsyncMethodBodyDebugInformation.KindIdentifier)
		{
			this.catch_handler = new InstructionOffset(-1);
		}

		// Token: 0x0400092D RID: 2349
		internal InstructionOffset catch_handler;

		// Token: 0x0400092E RID: 2350
		internal Collection<InstructionOffset> yields;

		// Token: 0x0400092F RID: 2351
		internal Collection<InstructionOffset> resumes;

		// Token: 0x04000930 RID: 2352
		internal Collection<MethodDefinition> resume_methods;

		// Token: 0x04000931 RID: 2353
		public static Guid KindIdentifier = new Guid("{54FD2AC5-E925-401A-9C2A-F94F171072F8}");
	}
}
