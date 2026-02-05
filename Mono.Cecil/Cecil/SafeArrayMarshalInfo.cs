using System;

namespace Mono.Cecil
{
	// Token: 0x0200007C RID: 124
	public sealed class SafeArrayMarshalInfo : MarshalInfo
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x000131E8 File Offset: 0x000113E8
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x000131F0 File Offset: 0x000113F0
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

		// Token: 0x060004DF RID: 1247 RVA: 0x000131F9 File Offset: 0x000113F9
		public SafeArrayMarshalInfo()
			: base(NativeType.SafeArray)
		{
			this.element_type = VariantType.None;
		}

		// Token: 0x040000F5 RID: 245
		internal VariantType element_type;
	}
}
