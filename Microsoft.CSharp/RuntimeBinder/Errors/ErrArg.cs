using System;
using Microsoft.CSharp.RuntimeBinder.Semantics;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Errors
{
	// Token: 0x020000C5 RID: 197
	internal class ErrArg
	{
		// Token: 0x0600066A RID: 1642 RVA: 0x0001E7F6 File Offset: 0x0001C9F6
		public ErrArg()
		{
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x0001E7FE File Offset: 0x0001C9FE
		public ErrArg(int n)
		{
			this.eak = ErrArgKind.Int;
			this.eaf = ErrArgFlags.None;
			this.n = n;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0001E81B File Offset: 0x0001CA1B
		public ErrArg(Name name)
		{
			this.eak = ErrArgKind.Name;
			this.eaf = ErrArgFlags.None;
			this.name = name;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0001E838 File Offset: 0x0001CA38
		public ErrArg(string psz)
		{
			this.eak = ErrArgKind.Str;
			this.eaf = ErrArgFlags.None;
			this.psz = psz;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0001E855 File Offset: 0x0001CA55
		public ErrArg(CType pType)
			: this(pType, ErrArgFlags.None)
		{
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0001E85F File Offset: 0x0001CA5F
		public ErrArg(CType pType, ErrArgFlags eaf)
		{
			this.eak = ErrArgKind.Type;
			this.eaf = eaf;
			this.pType = pType;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0001E87C File Offset: 0x0001CA7C
		public ErrArg(Symbol pSym)
			: this(pSym, ErrArgFlags.None)
		{
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0001E886 File Offset: 0x0001CA86
		private ErrArg(Symbol pSym, ErrArgFlags eaf)
		{
			this.eak = ErrArgKind.Sym;
			this.eaf = eaf;
			this.sym = pSym;
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0001E8A4 File Offset: 0x0001CAA4
		public ErrArg(SymWithType swt)
		{
			this.eak = ErrArgKind.SymWithType;
			this.eaf = ErrArgFlags.None;
			this.swtMemo = new SymWithTypeMemo();
			this.swtMemo.sym = swt.Sym;
			this.swtMemo.ats = swt.Ats;
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0001E8F4 File Offset: 0x0001CAF4
		public ErrArg(MethPropWithInst mpwi)
		{
			this.eak = ErrArgKind.MethWithInst;
			this.eaf = ErrArgFlags.None;
			this.mpwiMemo = new MethPropWithInstMemo();
			this.mpwiMemo.sym = mpwi.Sym;
			this.mpwiMemo.ats = mpwi.Ats;
			this.mpwiMemo.typeArgs = mpwi.TypeArgs;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001E954 File Offset: 0x0001CB54
		public static implicit operator ErrArg(int n)
		{
			return new ErrArg(n);
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0001E95C File Offset: 0x0001CB5C
		public static implicit operator ErrArg(CType type)
		{
			return new ErrArg(type);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0001E964 File Offset: 0x0001CB64
		public static implicit operator ErrArg(string psz)
		{
			return new ErrArg(psz);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001E96C File Offset: 0x0001CB6C
		public static implicit operator ErrArg(Name name)
		{
			return new ErrArg(name);
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001E974 File Offset: 0x0001CB74
		public static implicit operator ErrArg(Symbol pSym)
		{
			return new ErrArg(pSym);
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001E97C File Offset: 0x0001CB7C
		public static implicit operator ErrArg(SymWithType swt)
		{
			return new ErrArg(swt);
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001E984 File Offset: 0x0001CB84
		public static implicit operator ErrArg(MethPropWithInst mpwi)
		{
			return new ErrArg(mpwi);
		}

		// Token: 0x0400061B RID: 1563
		public ErrArgKind eak;

		// Token: 0x0400061C RID: 1564
		public ErrArgFlags eaf;

		// Token: 0x0400061D RID: 1565
		internal MessageID ids;

		// Token: 0x0400061E RID: 1566
		internal int n;

		// Token: 0x0400061F RID: 1567
		internal SYMKIND sk;

		// Token: 0x04000620 RID: 1568
		internal Name name;

		// Token: 0x04000621 RID: 1569
		internal Symbol sym;

		// Token: 0x04000622 RID: 1570
		internal string psz;

		// Token: 0x04000623 RID: 1571
		internal CType pType;

		// Token: 0x04000624 RID: 1572
		internal MethPropWithInstMemo mpwiMemo;

		// Token: 0x04000625 RID: 1573
		internal SymWithTypeMemo swtMemo;
	}
}
