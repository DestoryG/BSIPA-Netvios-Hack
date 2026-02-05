using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000120 RID: 288
	public sealed class ImportDebugInformation : DebugInformation
	{
		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x00023F58 File Offset: 0x00022158
		public bool HasTargets
		{
			get
			{
				return !this.targets.IsNullOrEmpty<ImportTarget>();
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000B0D RID: 2829 RVA: 0x00023F68 File Offset: 0x00022168
		public Collection<ImportTarget> Targets
		{
			get
			{
				Collection<ImportTarget> collection;
				if ((collection = this.targets) == null)
				{
					collection = (this.targets = new Collection<ImportTarget>());
				}
				return collection;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000B0E RID: 2830 RVA: 0x00023F8D File Offset: 0x0002218D
		// (set) Token: 0x06000B0F RID: 2831 RVA: 0x00023F95 File Offset: 0x00022195
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

		// Token: 0x06000B10 RID: 2832 RVA: 0x00023F9E File Offset: 0x0002219E
		public ImportDebugInformation()
		{
			this.token = new MetadataToken(TokenType.ImportScope);
		}

		// Token: 0x040006C2 RID: 1730
		internal ImportDebugInformation parent;

		// Token: 0x040006C3 RID: 1731
		internal Collection<ImportTarget> targets;
	}
}
