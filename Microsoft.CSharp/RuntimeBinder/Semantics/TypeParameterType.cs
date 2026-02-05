using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000B2 RID: 178
	internal sealed class TypeParameterType : CType
	{
		// Token: 0x0600061B RID: 1563 RVA: 0x0001DB18 File Offset: 0x0001BD18
		public TypeParameterSymbol GetTypeParameterSymbol()
		{
			return this._pTypeParameterSymbol;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x0001DB20 File Offset: 0x0001BD20
		public void SetTypeParameterSymbol(TypeParameterSymbol pTypePArameterSymbol)
		{
			this._pTypeParameterSymbol = pTypePArameterSymbol;
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0001DB29 File Offset: 0x0001BD29
		public ParentSymbol GetOwningSymbol()
		{
			return this._pTypeParameterSymbol.parent;
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x0001DB36 File Offset: 0x0001BD36
		public bool Covariant
		{
			get
			{
				return this._pTypeParameterSymbol.Covariant;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x0001DB43 File Offset: 0x0001BD43
		public bool Invariant
		{
			get
			{
				return this._pTypeParameterSymbol.Invariant;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x0001DB50 File Offset: 0x0001BD50
		public bool Contravariant
		{
			get
			{
				return this._pTypeParameterSymbol.Contravariant;
			}
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0001DB5D File Offset: 0x0001BD5D
		public bool IsValueType()
		{
			return this._pTypeParameterSymbol.IsValueType();
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0001DB6A File Offset: 0x0001BD6A
		public bool IsReferenceType()
		{
			return this._pTypeParameterSymbol.IsReferenceType();
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0001DB77 File Offset: 0x0001BD77
		public bool IsNonNullableValueType()
		{
			return this._pTypeParameterSymbol.IsNonNullableValueType();
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0001DB84 File Offset: 0x0001BD84
		public bool HasNewConstraint()
		{
			return this._pTypeParameterSymbol.HasNewConstraint();
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x0001DB91 File Offset: 0x0001BD91
		public bool HasRefConstraint()
		{
			return this._pTypeParameterSymbol.HasRefConstraint();
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0001DB9E File Offset: 0x0001BD9E
		public bool HasValConstraint()
		{
			return this._pTypeParameterSymbol.HasValConstraint();
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0001DBAB File Offset: 0x0001BDAB
		public bool IsMethodTypeParameter()
		{
			return this._pTypeParameterSymbol.IsMethodTypeParameter();
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0001DBB8 File Offset: 0x0001BDB8
		public int GetIndexInOwnParameters()
		{
			return this._pTypeParameterSymbol.GetIndexInOwnParameters();
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0001DBC5 File Offset: 0x0001BDC5
		public int GetIndexInTotalParameters()
		{
			return this._pTypeParameterSymbol.GetIndexInTotalParameters();
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0001DBD2 File Offset: 0x0001BDD2
		public TypeArray GetBounds()
		{
			return this._pTypeParameterSymbol.GetBounds();
		}

		// Token: 0x040005A9 RID: 1449
		private TypeParameterSymbol _pTypeParameterSymbol;
	}
}
