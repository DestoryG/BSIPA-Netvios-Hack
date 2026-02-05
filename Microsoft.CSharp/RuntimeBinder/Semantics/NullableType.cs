using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000A8 RID: 168
	internal sealed class NullableType : CType
	{
		// Token: 0x0600058D RID: 1421 RVA: 0x0001B744 File Offset: 0x00019944
		public AggregateType GetAts()
		{
			if (this.ats == null)
			{
				AggregateSymbol nullable = this.typeManager.GetNullable();
				CType underlyingType = this.GetUnderlyingType();
				CType[] array = new CType[] { underlyingType };
				TypeArray typeArray = this.symmgr.AllocParams(1, array);
				this.ats = this.typeManager.GetAggregate(nullable, typeArray);
			}
			return this.ats;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001B79E File Offset: 0x0001999E
		public CType GetUnderlyingType()
		{
			return this.UnderlyingType;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001B7A6 File Offset: 0x000199A6
		public override CType StripNubs()
		{
			return this.UnderlyingType;
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001B7AE File Offset: 0x000199AE
		public override CType StripNubs(out bool wasNullable)
		{
			wasNullable = true;
			return this.UnderlyingType;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001B7B9 File Offset: 0x000199B9
		public void SetUnderlyingType(CType pType)
		{
			this.UnderlyingType = pType;
		}

		// Token: 0x0400057F RID: 1407
		private AggregateType ats;

		// Token: 0x04000580 RID: 1408
		public BSYMMGR symmgr;

		// Token: 0x04000581 RID: 1409
		public TypeManager typeManager;

		// Token: 0x04000582 RID: 1410
		public CType UnderlyingType;
	}
}
