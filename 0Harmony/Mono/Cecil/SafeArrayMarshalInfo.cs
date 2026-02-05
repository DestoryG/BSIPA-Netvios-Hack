using System;

namespace Mono.Cecil
{
	// Token: 0x02000133 RID: 307
	internal sealed class SafeArrayMarshalInfo : MarshalInfo
	{
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x00021A14 File Offset: 0x0001FC14
		// (set) Token: 0x06000870 RID: 2160 RVA: 0x00021A1C File Offset: 0x0001FC1C
		public VariantType ElementType
		{
			get
			{
				return this.element_type;
			}
			set
			{
				this.element_type = value;
			}
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00021A25 File Offset: 0x0001FC25
		public SafeArrayMarshalInfo()
			: base(NativeType.SafeArray)
		{
			this.element_type = VariantType.None;
		}

		// Token: 0x0400030F RID: 783
		internal VariantType element_type;
	}
}
