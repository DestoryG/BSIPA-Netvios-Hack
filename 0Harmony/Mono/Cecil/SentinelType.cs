using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x0200016C RID: 364
	internal sealed class SentinelType : TypeSpecification
	{
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000B3E RID: 2878 RVA: 0x00010910 File Offset: 0x0000EB10
		// (set) Token: 0x06000B3F RID: 2879 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000B40 RID: 2880 RVA: 0x00010F39 File Offset: 0x0000F139
		public override bool IsSentinel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0002618F File Offset: 0x0002438F
		public SentinelType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.Sentinel;
		}
	}
}
