using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x0200006A RID: 106
	internal abstract class ParentSymbol : Symbol
	{
		// Token: 0x060003A9 RID: 937 RVA: 0x00016A30 File Offset: 0x00014C30
		public void AddToChildList(Symbol sym)
		{
			if (this._lastChild == null)
			{
				this._lastChild = sym;
				this.firstChild = sym;
			}
			else
			{
				this._lastChild.nextChild = sym;
				this._lastChild = sym;
				sym.nextChild = null;
			}
			sym.parent = this;
		}

		// Token: 0x040004C9 RID: 1225
		public Symbol firstChild;

		// Token: 0x040004CA RID: 1226
		private Symbol _lastChild;
	}
}
