using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000077 RID: 119
	internal sealed class TypeParameterSymbol : Symbol
	{
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x00017D1A File Offset: 0x00015F1A
		public bool Invariant
		{
			get
			{
				return !this.Covariant && !this.Contravariant;
			}
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00017D2F File Offset: 0x00015F2F
		public void SetTypeParameterType(TypeParameterType pType)
		{
			this._pTypeParameterType = pType;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00017D38 File Offset: 0x00015F38
		public TypeParameterType GetTypeParameterType()
		{
			return this._pTypeParameterType;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00017D40 File Offset: 0x00015F40
		public bool IsMethodTypeParameter()
		{
			return this._bIsMethodTypeParameter;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00017D48 File Offset: 0x00015F48
		public void SetIsMethodTypeParameter(bool b)
		{
			this._bIsMethodTypeParameter = b;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00017D51 File Offset: 0x00015F51
		public int GetIndexInOwnParameters()
		{
			return this._nIndexInOwnParameters;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00017D59 File Offset: 0x00015F59
		public void SetIndexInOwnParameters(int index)
		{
			this._nIndexInOwnParameters = index;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00017D62 File Offset: 0x00015F62
		public int GetIndexInTotalParameters()
		{
			return this._nIndexInTotalParameters;
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00017D6A File Offset: 0x00015F6A
		public void SetIndexInTotalParameters(int index)
		{
			this._nIndexInTotalParameters = index;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00017D73 File Offset: 0x00015F73
		public void SetBounds(TypeArray pBounds)
		{
			this._pBounds = pBounds;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00017D7C File Offset: 0x00015F7C
		public TypeArray GetBounds()
		{
			return this._pBounds;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00017D84 File Offset: 0x00015F84
		public void SetConstraints(SpecCons constraints)
		{
			this._constraints = constraints;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00017D8D File Offset: 0x00015F8D
		public bool IsValueType()
		{
			return (this._constraints & SpecCons.Val) > SpecCons.None;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00017D9A File Offset: 0x00015F9A
		public bool IsReferenceType()
		{
			return (this._constraints & SpecCons.Ref) > SpecCons.None;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00017DA7 File Offset: 0x00015FA7
		public bool IsNonNullableValueType()
		{
			return (this._constraints & SpecCons.Val) > SpecCons.None;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00017DB4 File Offset: 0x00015FB4
		public bool HasNewConstraint()
		{
			return (this._constraints & SpecCons.New) > SpecCons.None;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00017DC1 File Offset: 0x00015FC1
		public bool HasRefConstraint()
		{
			return (this._constraints & SpecCons.Ref) > SpecCons.None;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00017DCE File Offset: 0x00015FCE
		public bool HasValConstraint()
		{
			return (this._constraints & SpecCons.Val) > SpecCons.None;
		}

		// Token: 0x04000510 RID: 1296
		private bool _bIsMethodTypeParameter;

		// Token: 0x04000511 RID: 1297
		private SpecCons _constraints;

		// Token: 0x04000512 RID: 1298
		private TypeParameterType _pTypeParameterType;

		// Token: 0x04000513 RID: 1299
		private int _nIndexInOwnParameters;

		// Token: 0x04000514 RID: 1300
		private int _nIndexInTotalParameters;

		// Token: 0x04000515 RID: 1301
		private TypeArray _pBounds;

		// Token: 0x04000516 RID: 1302
		public bool Covariant;

		// Token: 0x04000517 RID: 1303
		public bool Contravariant;
	}
}
