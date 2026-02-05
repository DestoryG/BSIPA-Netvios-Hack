using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x02000164 RID: 356
	internal sealed class ByReferenceType : TypeSpecification
	{
		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x00025E82 File Offset: 0x00024082
		public override string Name
		{
			get
			{
				return base.Name + "&";
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000B16 RID: 2838 RVA: 0x00025E94 File Offset: 0x00024094
		public override string FullName
		{
			get
			{
				return base.FullName + "&";
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x00010910 File Offset: 0x0000EB10
		// (set) Token: 0x06000B18 RID: 2840 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x00010F39 File Offset: 0x0000F139
		public override bool IsByReference
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x00025EA6 File Offset: 0x000240A6
		public ByReferenceType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.ByRef;
		}
	}
}
