using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000A2 RID: 162
	public sealed class PinnedType : TypeSpecification
	{
		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x000026DB File Offset: 0x000008DB
		// (set) Token: 0x06000707 RID: 1799 RVA: 0x00002C55 File Offset: 0x00000E55
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

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override bool IsPinned
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00016ACD File Offset: 0x00014CCD
		public PinnedType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.Pinned;
		}
	}
}
