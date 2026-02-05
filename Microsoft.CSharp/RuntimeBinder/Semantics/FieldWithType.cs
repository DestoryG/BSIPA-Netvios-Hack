using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000BC RID: 188
	internal sealed class FieldWithType : SymWithType
	{
		// Token: 0x0600065B RID: 1627 RVA: 0x0001E08F File Offset: 0x0001C28F
		public FieldWithType(FieldSymbol field, AggregateType ats)
		{
			base.Set(field, ats);
		}
	}
}
