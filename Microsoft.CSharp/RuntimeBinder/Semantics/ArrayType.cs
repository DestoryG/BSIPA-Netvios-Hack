using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000A4 RID: 164
	internal sealed class ArrayType : CType
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x0001B6CE File Offset: 0x000198CE
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x0001B6D6 File Offset: 0x000198D6
		public bool IsSZArray { get; set; }

		// Token: 0x06000585 RID: 1413 RVA: 0x0001B6DF File Offset: 0x000198DF
		public CType GetElementType()
		{
			return this._pElementType;
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0001B6E7 File Offset: 0x000198E7
		public void SetElementType(CType pType)
		{
			this._pElementType = pType;
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0001B6F0 File Offset: 0x000198F0
		public CType GetBaseElementType()
		{
			CType ctype = this.GetElementType();
			ArrayType arrayType;
			while ((arrayType = ctype as ArrayType) != null)
			{
				ctype = arrayType.GetElementType();
			}
			return ctype;
		}

		// Token: 0x0400057A RID: 1402
		public int rank;

		// Token: 0x0400057C RID: 1404
		private CType _pElementType;
	}
}
