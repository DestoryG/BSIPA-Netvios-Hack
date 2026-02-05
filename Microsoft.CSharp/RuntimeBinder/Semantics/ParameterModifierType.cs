using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000A9 RID: 169
	internal sealed class ParameterModifierType : CType
	{
		// Token: 0x06000593 RID: 1427 RVA: 0x0001B7CA File Offset: 0x000199CA
		public CType GetParameterType()
		{
			return this._pParameterType;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x0001B7D2 File Offset: 0x000199D2
		public void SetParameterType(CType pType)
		{
			this._pParameterType = pType;
		}

		// Token: 0x04000583 RID: 1411
		public bool isOut;

		// Token: 0x04000584 RID: 1412
		private CType _pParameterType;
	}
}
