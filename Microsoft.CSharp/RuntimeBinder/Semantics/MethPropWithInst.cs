using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000BD RID: 189
	internal class MethPropWithInst : MethPropWithType
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x0001E09F File Offset: 0x0001C29F
		// (set) Token: 0x0600065D RID: 1629 RVA: 0x0001E0A7 File Offset: 0x0001C2A7
		public TypeArray TypeArgs { get; private set; }

		// Token: 0x0600065E RID: 1630 RVA: 0x0001E0B0 File Offset: 0x0001C2B0
		public MethPropWithInst()
		{
			this.Set(null, null, null);
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0001E0C1 File Offset: 0x0001C2C1
		public MethPropWithInst(MethodOrPropertySymbol mps, AggregateType ats)
			: this(mps, ats, null)
		{
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0001E0CC File Offset: 0x0001C2CC
		public MethPropWithInst(MethodOrPropertySymbol mps, AggregateType ats, TypeArray typeArgs)
		{
			this.Set(mps, ats, typeArgs);
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0001E0DD File Offset: 0x0001C2DD
		public override void Clear()
		{
			base.Clear();
			this.TypeArgs = null;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0001E0EC File Offset: 0x0001C2EC
		public void Set(MethodOrPropertySymbol mps, AggregateType ats, TypeArray typeArgs)
		{
			if (mps == null)
			{
				ats = null;
				typeArgs = null;
			}
			base.Set(mps, ats);
			this.TypeArgs = typeArgs;
		}
	}
}
