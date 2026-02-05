using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000B0 RID: 176
	public sealed class SentinelType : TypeSpecification
	{
		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x000026DB File Offset: 0x000008DB
		// (set) Token: 0x06000781 RID: 1921 RVA: 0x00002C55 File Offset: 0x00000E55
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

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override bool IsSentinel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x00017560 File Offset: 0x00015760
		public SentinelType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.Sentinel;
		}
	}
}
