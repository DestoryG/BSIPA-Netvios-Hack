using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000AA RID: 170
	internal sealed class PointerType : CType
	{
		// Token: 0x06000596 RID: 1430 RVA: 0x0001B7E3 File Offset: 0x000199E3
		public CType GetReferentType()
		{
			return this._pReferentType;
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001B7EB File Offset: 0x000199EB
		public void SetReferentType(CType pType)
		{
			this._pReferentType = pType;
		}

		// Token: 0x04000585 RID: 1413
		private CType _pReferentType;
	}
}
