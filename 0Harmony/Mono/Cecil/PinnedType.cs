using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200015C RID: 348
	internal sealed class PinnedType : TypeSpecification
	{
		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000ABD RID: 2749 RVA: 0x00010910 File Offset: 0x0000EB10
		// (set) Token: 0x06000ABE RID: 2750 RVA: 0x00010FA6 File Offset: 0x0000F1A6
		public override bool IsValueType
		{
			get
			{
				return false;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x00010F39 File Offset: 0x0000F139
		public override bool IsPinned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0002569D File Offset: 0x0002389D
		public PinnedType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.Pinned;
		}
	}
}
