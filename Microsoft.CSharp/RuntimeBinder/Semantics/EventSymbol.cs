using System;
using System.Reflection;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000062 RID: 98
	internal sealed class EventSymbol : Symbol
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000372 RID: 882 RVA: 0x000165D8 File Offset: 0x000147D8
		// (set) Token: 0x06000373 RID: 883 RVA: 0x000165E0 File Offset: 0x000147E0
		public bool IsWindowsRuntimeEvent { get; set; }

		// Token: 0x0400049C RID: 1180
		public EventInfo AssociatedEventInfo;

		// Token: 0x0400049D RID: 1181
		public new bool isStatic;

		// Token: 0x0400049E RID: 1182
		public bool isOverride;

		// Token: 0x0400049F RID: 1183
		public CType type;

		// Token: 0x040004A0 RID: 1184
		public MethodSymbol methAdd;

		// Token: 0x040004A1 RID: 1185
		public MethodSymbol methRemove;
	}
}
