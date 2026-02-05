using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000B7 RID: 183
	internal class SymWithType
	{
		// Token: 0x06000643 RID: 1603 RVA: 0x0001DECF File Offset: 0x0001C0CF
		public SymWithType()
		{
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0001DED7 File Offset: 0x0001C0D7
		public SymWithType(Symbol sym, AggregateType ats)
		{
			this.Set(sym, ats);
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0001DEE7 File Offset: 0x0001C0E7
		public virtual void Clear()
		{
			this._sym = null;
			this._ats = null;
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x0001DEF7 File Offset: 0x0001C0F7
		public AggregateType Ats
		{
			get
			{
				return this._ats;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x0001DEFF File Offset: 0x0001C0FF
		public Symbol Sym
		{
			get
			{
				return this._sym;
			}
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0001DF07 File Offset: 0x0001C107
		public new AggregateType GetType()
		{
			return this.Ats;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0001DF0F File Offset: 0x0001C10F
		public static bool operator ==(SymWithType swt1, SymWithType swt2)
		{
			if (swt1 == swt2)
			{
				return true;
			}
			if (swt1 == null)
			{
				return swt2._sym == null;
			}
			if (swt2 == null)
			{
				return swt1._sym == null;
			}
			return swt1.Sym == swt2.Sym && swt1.Ats == swt2.Ats;
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x0001DF4F File Offset: 0x0001C14F
		public static bool operator !=(SymWithType swt1, SymWithType swt2)
		{
			return !(swt1 == swt2);
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0001DF5C File Offset: 0x0001C15C
		[ExcludeFromCodeCoverage]
		public override bool Equals(object obj)
		{
			SymWithType symWithType = obj as SymWithType;
			return !(symWithType == null) && this.Sym == symWithType.Sym && this.Ats == symWithType.Ats;
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0001DF99 File Offset: 0x0001C199
		[ExcludeFromCodeCoverage]
		public override int GetHashCode()
		{
			Symbol sym = this.Sym;
			int num = ((sym != null) ? sym.GetHashCode() : 0);
			AggregateType ats = this.Ats;
			return num + ((ats != null) ? ats.GetHashCode() : 0);
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0001DFC0 File Offset: 0x0001C1C0
		public static implicit operator bool(SymWithType swt)
		{
			return swt != null;
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0001DFC9 File Offset: 0x0001C1C9
		public MethodOrPropertySymbol MethProp()
		{
			return this.Sym as MethodOrPropertySymbol;
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0001DFD6 File Offset: 0x0001C1D6
		public MethodSymbol Meth()
		{
			return this.Sym as MethodSymbol;
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0001DFE3 File Offset: 0x0001C1E3
		public PropertySymbol Prop()
		{
			return this.Sym as PropertySymbol;
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0001DFF0 File Offset: 0x0001C1F0
		public FieldSymbol Field()
		{
			return this.Sym as FieldSymbol;
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0001DFFD File Offset: 0x0001C1FD
		public EventSymbol Event()
		{
			return this.Sym as EventSymbol;
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0001E00A File Offset: 0x0001C20A
		public void Set(Symbol sym, AggregateType ats)
		{
			if (sym == null)
			{
				ats = null;
			}
			this._sym = sym;
			this._ats = ats;
		}

		// Token: 0x040005B3 RID: 1459
		private AggregateType _ats;

		// Token: 0x040005B4 RID: 1460
		private Symbol _sym;
	}
}
