using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001E4 RID: 484
	internal sealed class ImportDebugInformation : DebugInformation
	{
		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x00033118 File Offset: 0x00031318
		public bool HasTargets
		{
			get
			{
				return !this.targets.IsNullOrEmpty<ImportTarget>();
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000EF4 RID: 3828 RVA: 0x00033128 File Offset: 0x00031328
		public Collection<ImportTarget> Targets
		{
			get
			{
				if (this.targets == null)
				{
					Interlocked.CompareExchange<Collection<ImportTarget>>(ref this.targets, new Collection<ImportTarget>(), null);
				}
				return this.targets;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x0003314A File Offset: 0x0003134A
		// (set) Token: 0x06000EF6 RID: 3830 RVA: 0x00033152 File Offset: 0x00031352
		public ImportDebugInformation Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x0003315B File Offset: 0x0003135B
		public ImportDebugInformation()
		{
			this.token = new MetadataToken(TokenType.ImportScope);
		}

		// Token: 0x04000921 RID: 2337
		internal ImportDebugInformation parent;

		// Token: 0x04000922 RID: 2338
		internal Collection<ImportTarget> targets;
	}
}
