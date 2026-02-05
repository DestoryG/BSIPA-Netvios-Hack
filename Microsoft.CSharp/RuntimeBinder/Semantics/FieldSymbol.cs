using System;
using System.Reflection;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000063 RID: 99
	internal class FieldSymbol : VariableSymbol
	{
		// Token: 0x06000375 RID: 885 RVA: 0x000165F1 File Offset: 0x000147F1
		public void SetType(CType pType)
		{
			this.type = pType;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000165FA File Offset: 0x000147FA
		public new CType GetType()
		{
			return this.type;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00016602 File Offset: 0x00014802
		public AggregateSymbol getClass()
		{
			return this.parent as AggregateSymbol;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x0001660F File Offset: 0x0001480F
		public EventSymbol getEvent(SymbolLoader symbolLoader)
		{
			return symbolLoader.LookupAggMember(this.name, this.getClass(), symbmask_t.MASK_EventSymbol) as EventSymbol;
		}

		// Token: 0x040004A3 RID: 1187
		public new bool isStatic;

		// Token: 0x040004A4 RID: 1188
		public bool isReadOnly;

		// Token: 0x040004A5 RID: 1189
		public bool isEvent;

		// Token: 0x040004A6 RID: 1190
		public bool isAssigned;

		// Token: 0x040004A7 RID: 1191
		public FieldInfo AssociatedFieldInfo;
	}
}
