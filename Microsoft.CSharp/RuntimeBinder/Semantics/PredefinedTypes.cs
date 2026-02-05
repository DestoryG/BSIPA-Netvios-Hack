using System;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000AB RID: 171
	internal sealed class PredefinedTypes
	{
		// Token: 0x06000599 RID: 1433 RVA: 0x0001B7FC File Offset: 0x000199FC
		public PredefinedTypes(BSYMMGR symbolManager)
		{
			this._symbolManager = symbolManager;
			this._runtimeBinderSymbolTable = null;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001B812 File Offset: 0x00019A12
		private AggregateSymbol DelayLoadPredefSym(PredefinedType pt)
		{
			return PredefinedTypes.InitializePredefinedType(this._runtimeBinderSymbolTable.GetCTypeFromType(PredefinedTypeFacts.GetAssociatedSystemType(pt)).getAggregate(), pt);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001B830 File Offset: 0x00019A30
		internal static AggregateSymbol InitializePredefinedType(AggregateSymbol sym, PredefinedType pt)
		{
			sym.SetPredefined(true);
			sym.SetPredefType(pt);
			sym.SetSkipUDOps(pt <= PredefinedType.PT_ENUM && pt != PredefinedType.FirstNonSimpleType && pt != PredefinedType.PT_UINTPTR && pt != PredefinedType.PT_TYPE);
			return sym;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0001B861 File Offset: 0x00019A61
		public void Init(SymbolTable symtable)
		{
			this._runtimeBinderSymbolTable = symtable;
			this._predefSyms = new AggregateSymbol[48];
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001B878 File Offset: 0x00019A78
		public AggregateSymbol GetPredefinedAggregate(PredefinedType pt)
		{
			AggregateSymbol aggregateSymbol;
			if ((aggregateSymbol = this._predefSyms[(int)pt]) == null)
			{
				aggregateSymbol = (this._predefSyms[(int)pt] = this.DelayLoadPredefSym(pt));
			}
			return aggregateSymbol;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0001B8A3 File Offset: 0x00019AA3
		private static string GetNiceName(PredefinedType pt)
		{
			return PredefinedTypeFacts.GetNiceName(pt);
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x0001B8AB File Offset: 0x00019AAB
		public static string GetNiceName(AggregateSymbol type)
		{
			if (!type.IsPredefined())
			{
				return null;
			}
			return PredefinedTypes.GetNiceName(type.GetPredefType());
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0001B8C2 File Offset: 0x00019AC2
		public static string GetFullName(PredefinedType pt)
		{
			return PredefinedTypeFacts.GetName(pt);
		}

		// Token: 0x04000586 RID: 1414
		private SymbolTable _runtimeBinderSymbolTable;

		// Token: 0x04000587 RID: 1415
		private readonly BSYMMGR _symbolManager;

		// Token: 0x04000588 RID: 1416
		private AggregateSymbol[] _predefSyms;
	}
}
