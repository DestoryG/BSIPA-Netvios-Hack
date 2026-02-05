using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200015F RID: 351
	internal sealed class PointerType : TypeSpecification
	{
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x000259A5 File Offset: 0x00023BA5
		public override string Name
		{
			get
			{
				return base.Name + "*";
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x000259B7 File Offset: 0x00023BB7
		public override string FullName
		{
			get
			{
				return base.FullName + "*";
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000AE8 RID: 2792 RVA: 0x00010910 File Offset: 0x0000EB10
		// (set) Token: 0x06000AE9 RID: 2793 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x00010F39 File Offset: 0x0000F139
		public override bool IsPointer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x000259C9 File Offset: 0x00023BC9
		public PointerType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.Ptr;
		}
	}
}
