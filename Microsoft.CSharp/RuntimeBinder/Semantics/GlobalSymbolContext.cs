using System;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200004D RID: 77
	internal sealed class GlobalSymbolContext
	{
		// Token: 0x060002A1 RID: 673 RVA: 0x00012444 File Offset: 0x00010644
		public GlobalSymbolContext(NameManager namemgr)
		{
			this.GlobalSymbols = new BSYMMGR(namemgr);
			this._predefTypes = new PredefinedTypes(this.GlobalSymbols);
			this.TypeManager = new TypeManager(this.GlobalSymbols, this._predefTypes);
			this._nameManager = namemgr;
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x00012492 File Offset: 0x00010692
		public TypeManager TypeManager { get; }

		// Token: 0x060002A3 RID: 675 RVA: 0x0001249A File Offset: 0x0001069A
		public TypeManager GetTypes()
		{
			return this.TypeManager;
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x000124A2 File Offset: 0x000106A2
		private BSYMMGR GlobalSymbols { get; }

		// Token: 0x060002A5 RID: 677 RVA: 0x000124AA File Offset: 0x000106AA
		public BSYMMGR GetGlobalSymbols()
		{
			return this.GlobalSymbols;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x000124B2 File Offset: 0x000106B2
		public NameManager GetNameManager()
		{
			return this._nameManager;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x000124BA File Offset: 0x000106BA
		public PredefinedTypes GetPredefTypes()
		{
			return this._predefTypes;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x000124C2 File Offset: 0x000106C2
		public SymFactory GetGlobalSymbolFactory()
		{
			return this.GetGlobalSymbols().GetSymFactory();
		}

		// Token: 0x040003AF RID: 943
		private readonly PredefinedTypes _predefTypes;

		// Token: 0x040003B0 RID: 944
		private readonly NameManager _nameManager;
	}
}
