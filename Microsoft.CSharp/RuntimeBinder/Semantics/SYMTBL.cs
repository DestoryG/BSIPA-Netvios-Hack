using System;
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder.Syntax;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000076 RID: 118
	internal sealed class SYMTBL
	{
		// Token: 0x0600040D RID: 1037 RVA: 0x00017C40 File Offset: 0x00015E40
		public SYMTBL()
		{
			this._dictionary = new Dictionary<SYMTBL.Key, Symbol>();
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00017C54 File Offset: 0x00015E54
		public Symbol LookupSym(Name name, ParentSymbol parent, symbmask_t kindmask)
		{
			SYMTBL.Key key = new SYMTBL.Key(name, parent);
			Symbol symbol;
			if (this._dictionary.TryGetValue(key, out symbol))
			{
				return SYMTBL.FindCorrectKind(symbol, kindmask);
			}
			return null;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00017C82 File Offset: 0x00015E82
		public void InsertChild(ParentSymbol parent, Symbol child)
		{
			child.parent = parent;
			this.InsertChildNoGrow(child);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00017C94 File Offset: 0x00015E94
		private void InsertChildNoGrow(Symbol child)
		{
			SYMKIND kind = child.getKind();
			if (kind == SYMKIND.SK_LocalVariableSymbol || kind == SYMKIND.SK_Scope)
			{
				return;
			}
			SYMTBL.Key key = new SYMTBL.Key(child.name, child.parent);
			Symbol nextSameName;
			if (this._dictionary.TryGetValue(key, out nextSameName))
			{
				while (((nextSameName != null) ? nextSameName.nextSameName : null) != null)
				{
					nextSameName = nextSameName.nextSameName;
				}
				nextSameName.nextSameName = child;
				return;
			}
			this._dictionary.Add(key, child);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00017D00 File Offset: 0x00015F00
		private static Symbol FindCorrectKind(Symbol sym, symbmask_t kindmask)
		{
			while ((kindmask & sym.mask()) == (symbmask_t)0L)
			{
				sym = sym.nextSameName;
				if (sym == null)
				{
					return null;
				}
			}
			return sym;
		}

		// Token: 0x0400050F RID: 1295
		private readonly Dictionary<SYMTBL.Key, Symbol> _dictionary;

		// Token: 0x020000EF RID: 239
		private sealed class Key : IEquatable<SYMTBL.Key>
		{
			// Token: 0x0600074B RID: 1867 RVA: 0x0002397F File Offset: 0x00021B7F
			public Key(Name name, ParentSymbol parent)
			{
				this._name = name;
				this._parent = parent;
			}

			// Token: 0x0600074C RID: 1868 RVA: 0x00023995 File Offset: 0x00021B95
			public bool Equals(SYMTBL.Key other)
			{
				return other != null && this._name.Equals(other._name) && this._parent.Equals(other._parent);
			}

			// Token: 0x0600074D RID: 1869 RVA: 0x000239C0 File Offset: 0x00021BC0
			public override bool Equals(object obj)
			{
				return this.Equals(obj as SYMTBL.Key);
			}

			// Token: 0x0600074E RID: 1870 RVA: 0x000239CE File Offset: 0x00021BCE
			public override int GetHashCode()
			{
				return this._name.GetHashCode() ^ this._parent.GetHashCode();
			}

			// Token: 0x04000712 RID: 1810
			private readonly Name _name;

			// Token: 0x04000713 RID: 1811
			private readonly ParentSymbol _parent;
		}
	}
}
